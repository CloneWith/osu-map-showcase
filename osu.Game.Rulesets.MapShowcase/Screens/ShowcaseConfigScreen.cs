// Copyright (c) CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Framework.Screens;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Cursor;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Overlays;
using osu.Game.Overlays.Dialog;
using osu.Game.Overlays.Notifications;
using osu.Game.Overlays.Toolbar;
using osu.Game.Scoring;
using osu.Game.Screens.Footer;
using osu.Game.Rulesets.MapShowcase.Utils;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.MapShowcase.Screens
{
    public partial class ShowcaseConfigScreen : OsuScreen, ISubScreenWithTitle, IKeyBindingHandler<PlatformAction>, IKeyBindingHandler<MapShowcaseAction>
    {
        public string ShortTitle => @"Configuration";

        public LocalisableString LocalisableTitle => MapShowcaseStrings.Configuration;

        [Cached]
        private OverlayColourProvider colourProvider = new OverlayColourProvider(OverlayColourScheme.Blue);

        [Resolved]
        private ScoreManager scoreManager { get; set; } = null!;

        [Resolved]
        private ShowcaseStorage storage { get; set; } = null!;

        [Resolved]
        private IDialogOverlay? dialogOverlay { get; set; }

        [Resolved]
        private INotificationOverlay? notificationOverlay { get; set; }

        private const float sizing_duration = 200;

        #region Drawable variables

        private FillFlowContainer tournamentInfoSection = null!;
        private FillFlowContainer settingsSection = null!;
        private ShowcaseBeatmapEditor beatmapSection = null!;

        private ToolbarRulesetSelector rulesetSelector = null!;
        private FormTextBox tournamentNameInput = null!;
        private FormTextBox roundNameInput = null!;
        private FormEnumDropdown<OverlayColourScheme> colourSchemeDropdown = null!;
        private FormCheckBox useCustomIntroSwitch = null!;
        private FillFlowContainer introEditor = null!;

        private FooterButtonSave saveButton = null!;

        #endregion

        private readonly Bindable<ShowcaseConfig> currentProfile = new Bindable<ShowcaseConfig>();
        private readonly Bindable<ShowcaseConfigTab> currentTab = new Bindable<ShowcaseConfigTab>();

        public ShowcaseConfigScreen(ShowcaseConfig config)
        {
            Alpha = 0;

            currentProfile.Value = config;
            FooterContent.Value = ScreenFooterContent.Argon(leftButtons: CreateFooterButtons);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            currentProfile.Value.IntroBeatmap.Value ??= new ShowcaseBeatmap();

            #region Sections

            tournamentInfoSection = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                AutoSizeEasing = Easing.OutQuint,
                AutoSizeDuration = sizing_duration,
                Spacing = new Vector2(10),
                Direction = FillDirection.Vertical,
                Children =
                [
                    new SectionHeader(MapShowcaseStrings.DefaultRuleset),
                    new OsuScrollContainer(Direction.Horizontal)
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 40,
                        Margin = new MarginPadding { Vertical = -10 },
                        ScrollbarVisible = false,
                        Child = rulesetSelector = new ToolbarRulesetSelector
                        {
                            Current = currentProfile.Value.FallbackRuleset,
                        },
                    },
                    new SectionHeader(MapShowcaseStrings.TournamentInfoHeader),
                    tournamentNameInput = new FormTextBox
                    {
                        Caption = MapShowcaseStrings.TournamentName,
                        PlaceholderText = MapShowcaseStrings.TournamentNamePlaceholder,
                        HintText = MapShowcaseStrings.TournamentNameDescription,
                        Current = currentProfile.Value.TournamentName,
                        TabbableContentContainer = this,
                    },
                    roundNameInput = new FormTextBox
                    {
                        Caption = MapShowcaseStrings.TournamentRound,
                        PlaceholderText = MapShowcaseStrings.TournamentRoundPlaceholder,
                        HintText = MapShowcaseStrings.TournamentRoundDescription,
                        Current = currentProfile.Value.RoundName,
                        TabbableContentContainer = this,
                    },
                    new FormTextBox
                    {
                        Caption = MapShowcaseStrings.DateAndTime,
                        PlaceholderText = "2024/11/4 5:14:19:191 UTC+8",
                        HintText = MapShowcaseStrings.DateAndTimeDescription,
                        Current = currentProfile.Value.DateTime,
                        TabbableContentContainer = this,
                    },
                    new FormTextBox
                    {
                        Caption = MapShowcaseStrings.Comment,
                        PlaceholderText = "Welcome to osu!",
                        HintText = MapShowcaseStrings.IntroCommentDescription,
                        Current = currentProfile.Value.Comment,
                        TabbableContentContainer = this,
                    },
                ],
            };
            settingsSection = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                AutoSizeEasing = Easing.OutQuint,
                AutoSizeDuration = sizing_duration,
                Spacing = new Vector2(10),
                Direction = FillDirection.Vertical,
                Children =
                [
                    new SectionHeader(MapShowcaseStrings.ShowcaseSettingsHeader),
                    colourSchemeDropdown = new FormEnumDropdown<OverlayColourScheme>
                    {
                        Caption = MapShowcaseStrings.ColourScheme,
                        HintText = MapShowcaseStrings.ColourSchemeDescription,
                        Current = currentProfile.Value.ColourScheme,
                    },
                    new FormEnumDropdown<ShowcaseLayout>
                    {
                        Caption = MapShowcaseStrings.InterfaceLayout,
                        HintText = MapShowcaseStrings.InterfaceLayoutDescription,
                        Current = currentProfile.Value.Layout,
                    },
                    new FormSliderBar<float>
                    {
                        Caption = MapShowcaseStrings.AspectRatio,
                        HintText = MapShowcaseStrings.AspectRatioDescription,
                        Current = currentProfile.Value.AspectRatio,
                        TransferValueOnCommit = true,
                        TabbableContentContainer = this,
                    },
                    new FormSliderBar<int>
                    {
                        Caption = MapShowcaseStrings.TransformDuration,
                        HintText = MapShowcaseStrings.TransformDurationDescription,
                        Current = currentProfile.Value.TransformDuration,
                        LabelFormat = v => @$"{v}ms",
                        TransferValueOnCommit = true,
                        TabbableContentContainer = this,
                    },
                    new FormSliderBar<int>
                    {
                        Caption = MapShowcaseStrings.StartCountdownDuration,
                        HintText = MapShowcaseStrings.StartCountdownDurationDescription,
                        Current = currentProfile.Value.StartCountdown,
                        LabelFormat = v => @$"{v}ms",
                        TransferValueOnCommit = true,
                        TabbableContentContainer = this,
                    },
                    new FormSliderBar<int>
                    {
                        Caption = MapShowcaseStrings.FirstBeatmapDelay,
                        HintText = MapShowcaseStrings.FirstBeatmapDelayDescription,
                        Current = currentProfile.Value.FirstBeatmapDelay,
                        LabelFormat = v => @$"{v}s",
                        TransferValueOnCommit = true,
                        TabbableContentContainer = this,
                    },
                    new FormTextBox
                    {
                        Caption = MapShowcaseStrings.OutroTitle,
                        PlaceholderText = @"Thanks for watching!",
                        Current = currentProfile.Value.OutroTitle,
                        TabbableContentContainer = this,
                    },
                    new FormTextBox
                    {
                        Caption = MapShowcaseStrings.OutroSubtitle,
                        PlaceholderText = @"Take care of yourself, and be well.",
                        Current = currentProfile.Value.OutroSubtitle,
                        TabbableContentContainer = this,
                    },
                ],
            };
            beatmapSection = new ShowcaseBeatmapEditor
            {
                Config = { BindTarget = currentProfile },
            };
            introEditor = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                AutoSizeEasing = Easing.OutQuint,
                AutoSizeDuration = sizing_duration,
                Spacing = new Vector2(10),
                Direction = FillDirection.Vertical,
                Children =
                [
                    new SectionHeader(MapShowcaseStrings.IntroBeatmapHeader),
                    useCustomIntroSwitch = new FormCheckBox
                    {
                        Caption = MapShowcaseStrings.UseCustomIntroBeatmap,
                        HintText = MapShowcaseStrings.UseCustomIntroBeatmapDescription,
                        Current = currentProfile.Value.UseCustomIntroBeatmap,
                    },
                    new DrawableShowcaseBeatmapItem(currentProfile.Value.IntroBeatmap.Value, currentProfile.Value)
                    {
                        AllowEditingDetails = false,
                        AllowDeletion = false,
                        ShowItemOwner = false,
                    },
                ],
            };

            #endregion

            #region Basic Layout

            InternalChildren =
            [
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black.Opacity(0.6f),
                },
                new GridContainer
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding { Bottom = ScreenFooter.HEIGHT },
                    Width = 0.8f,
                    RowDimensions =
                    [
                        new Dimension(GridSizeMode.Relative, 0.1f),
                        new Dimension(),
                    ],
                    Content = new[]
                    {
                        new Drawable[]
                        {
                            new OsuTabControl<ShowcaseConfigTab>
                            {
                                Name = @"Top Tab Control",
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                RelativeSizeAxes = Axes.Both,
                                Current = { BindTarget = currentTab },
                            },
                        },
                        new Drawable[]
                        {
                            new OsuContextMenuContainer
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children =
                                [
                                    new OsuScrollContainer
                                    {
                                        Name = @"Setup flow",
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        RelativeSizeAxes = Axes.Both,
                                        ScrollbarOverlapsContent = false,
                                        Child = new FillFlowContainer
                                        {
                                            RelativeSizeAxes = Axes.X,
                                            AutoSizeAxes = Axes.Y,
                                            Spacing = new Vector2(10),
                                            Direction = FillDirection.Full,
                                            Children =
                                            [
                                                new GridContainer
                                                {
                                                    RelativeSizeAxes = Axes.X,
                                                    AutoSizeAxes = Axes.Y,
                                                    RowDimensions =
                                                    [
                                                        new Dimension(GridSizeMode.AutoSize),
                                                    ],
                                                    ColumnDimensions =
                                                    [
                                                        new Dimension(),
                                                        // Add a 10px gap between two columns
                                                        new Dimension(GridSizeMode.Absolute, 10),
                                                        new Dimension(),
                                                    ],
                                                    Content = new[]
                                                    {
                                                        new[]
                                                        {
                                                            tournamentInfoSection,
                                                            Empty(),
                                                            settingsSection,
                                                        },
                                                    },
                                                },
                                                introEditor,
                                            ],
                                        },
                                    },
                                    new Container
                                    {
                                        Name = @"Beatmap flow",
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        RelativeSizeAxes = Axes.Both,
                                        Child = beatmapSection,
                                    },
                                ],
                            },
                        },
                    },
                },
            ];

            #endregion

        }

        public IReadOnlyList<ScreenFooterButton> CreateFooterButtons() =>
        [
            new FooterButtonNewShowcaseItem
            {
                Action = () =>
                {
                    var select = new ShowcaseSongSelect();
                    select.OnSelect += (_, beatmapInfo, rulesetInfo, scoreInfo, mods) =>
                    {
                        var newBeatmap = new ShowcaseBeatmap(beatmapInfo)
                        {
                            RulesetId = rulesetInfo.OnlineID,
                            ScoreHash = scoreInfo?.Hash ?? string.Empty,
                        };

                        newBeatmap.RequiredMods.AddRange(mods);
                        currentProfile.Value.Beatmaps.Add(newBeatmap);
                    };

                    this.Push(select);
                },
            },
            saveButton = new FooterButtonSave
            {
                Action = () =>
                {
                    if (checkConfig(false))
                        storage.SaveChanges(currentProfile.Value);
                },
            },
            new FooterButtonStartShowcase
            {
                // The start hotkey is a ruleset binding now, so the footer button can no longer advertise it
                // (ScreenFooterButton.Hotkey is typed as GlobalAction). It is handled in OnPressed below instead.
                Action = startShowcase,
            },
            new FooterButtonOpenExternally
            {
                Action = () => storage.PresentFileExternally(currentProfile.Value.Filename.Value),
            },
        ];

        protected override void LoadComplete()
        {
            base.LoadComplete();

            currentTab.BindValueChanged(currentTabChanged, true);
            colourSchemeDropdown.Current.BindValueChanged(e => colourProvider.ChangeColourScheme(e.NewValue), true);

            this.FadeInFromZero(500, Easing.OutQuint);
        }

        /// <summary>
        /// Check all necessary fields to ensure that the profile can be saved and used properly.
        /// In case an issue is found, a notification about it would be sent.
        /// </summary>
        /// <returns>True if valid, false otherwise</returns>
        private bool checkConfig(bool checkBeatmaps = true)
        {
            bool isValid = rulesetSelector.Current.Value != null
                           && tournamentNameInput.Current.Value != null
                           && roundNameInput.Current.Value != null
                           && tournamentNameInput.Current.Value.Trim() != string.Empty
                           && roundNameInput.Current.Value.Trim() != string.Empty;

            if (!isValid)
            {
                notificationOverlay?.Post(new SimpleErrorNotification
                {
                    Text = MapShowcaseStrings.ProfileErrorDialogText,
                });

                return false;
            }

            if (!currentProfile.Value.Filename.Value.IsSafeForFilename(out LocalisableString error, 50))
            {
                notificationOverlay?.Post(new SimpleErrorNotification
                {
                    Text = error,
                });

                return false;
            }

            if (checkBeatmaps && currentProfile.Value.Beatmaps.Count == 0)
            {
                notificationOverlay?.Post(new SimpleErrorNotification
                {
                    Text = MapShowcaseStrings.EmptyBeatmapListDialogText,
                });

                return false;
            }

            if (useCustomIntroSwitch.Current.Value && !currentProfile.Value.IntroBeatmap.Value.IsValid())
            {
                notificationOverlay?.Post(new SimpleErrorNotification
                {
                    Text = MapShowcaseStrings.NullIntroMapDialogText,
                });

                return false;
            }

            return isValid;
        }

        private bool checkAndFetchScores()
        {
            // Re-resolve every entry against the local database.
            currentProfile.Value.Beatmaps.ForEach(ensureScoreResolved);

            // The custom intro beatmap is a standalone entry, so it would otherwise never be checked.
            if (useCustomIntroSwitch.Current.Value)
                ensureScoreResolved(currentProfile.Value.IntroBeatmap.Value);

            return missingScoreCount == 0;
        }

        private void ensureScoreResolved(ShowcaseBeatmap beatmap)
        {
            if (beatmap.ShowcaseScore != null && !string.IsNullOrEmpty(beatmap.ShowcaseScore.Hash))
                return;

            beatmap.ShowcaseScore = beatmap.ResolveScoreInfo(scoreManager);
        }

        private int missingScoreCount
        {
            get
            {
                int missing = currentProfile.Value.Beatmaps.Count(b => b.ShowcaseScore == null);

                if (useCustomIntroSwitch.Current.Value && currentProfile.Value.IntroBeatmap.Value.ShowcaseScore == null)
                    missing++;

                return missing;
            }
        }

        private void startShowcase()
        {
            Action launchAction = () => this.Push(new ShowcaseViewScreen(currentProfile.Value));

            if (!checkConfig())
                return;

            if (!checkAndFetchScores())
            {
                dialogOverlay?.Push(new ScoreMissingDialog(missingScoreCount, launchAction));
            }
            else
            {
                launchAction.Invoke();
            }
        }

        private void currentTabChanged(ValueChangedEvent<ShowcaseConfigTab> e)
        {
            // <comment>
            // The old implementation mechanism will destroy and rebuild the component tree on each switch
            // Causes a lot of unnecessary overhead and more importantly it does not work well with GridContainers
            // they tried to reuse the components and it violates osu!framework's component ownership
            // So I decided to use the Alpha property to control the visibility of the components :)
            // </comment>
            bool generalVisible = currentTab.Value == ShowcaseConfigTab.General;
            tournamentInfoSection.Alpha = generalVisible ? 1 : 0;
            settingsSection.Alpha = generalVisible ? 1 : 0;
            introEditor.Alpha = generalVisible ? 1 : 0;

            beatmapSection.Alpha = currentTab.Value == ShowcaseConfigTab.Beatmaps ? 1 : 0;
        }

        private bool exitConfirmed;

        public override bool OnExiting(ScreenExitEvent e)
        {
            if (!exitConfirmed && dialogOverlay != null)
            {
                if (dialogOverlay.CurrentDialog is ConfirmDialog confirmDialog)
                    confirmDialog.PerformOkAction();
                else
                {
                    dialogOverlay.Push(new ConfirmDialog(MapShowcaseStrings.ExitScreenDialogTitle, () =>
                    {
                        exitConfirmed = true;
                        if (this.IsCurrentScreen())
                            this.Exit();
                    }));
                }

                return true;
            }

            return base.OnExiting(e);
        }

        public bool OnPressed(KeyBindingPressEvent<PlatformAction> e)
        {
            switch (e.Action)
            {
                case PlatformAction.Save:
                    return e.Repeat || saveButton.TriggerClick();

                default:
                    return false;
            }
        }

        public void OnReleased(KeyBindingReleaseEvent<PlatformAction> e)
        {
        }

        public bool OnPressed(KeyBindingPressEvent<MapShowcaseAction> e)
        {
            switch (e.Action)
            {
                case MapShowcaseAction.StartShowcase:
                    if (e.Repeat)
                        return false;

                    startShowcase();
                    return true;

                default:
                    return false;
            }
        }

        public void OnReleased(KeyBindingReleaseEvent<MapShowcaseAction> e)
        {
        }
    }

    public enum ShowcaseConfigTab
    {
        General,
        Beatmaps,
    }
}
