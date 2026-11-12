// Copyright (c) CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Overlays;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.MapShowcase.Configuration;
using osu.Game.Screens.Menu;
using osu.Game.Screens.Select;
using osuTK;

namespace osu.Game.Rulesets.MapShowcase.Overlays
{
    public partial class MapShowcaseIntroOverlay : OsuFocusedOverlayContainer
    {
        public const float TRANSITION_DURATION = 300;

        [Resolved]
        private OsuGame game { get; set; } = null!;

        [Resolved]
        private RulesetStore rulesets { get; set; } = null!;

        [Resolved(CanBeNull = true)]
        private IRulesetConfigCache? configCache { get; set; }

        [Resolved(CanBeNull = true)]
        private IPerformFromScreenRunner? performer { get; set; }

        private readonly OverlayColourProvider colourProvider = new OverlayColourProvider(OverlayColourScheme.Blue);

        /// <summary>
        /// A copy of the current screen's selected ruleset.
        /// </summary>
        private readonly Bindable<RulesetInfo> trackedRuleset = new Bindable<RulesetInfo>();

        private readonly Bindable<bool> showOnSelection = new Bindable<bool>(true);

        public MapShowcaseIntroOverlay()
        {
            RelativeSizeAxes = Axes.Both;
            Scale = new Vector2(0.98f);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RulesetInfo? showcaseRuleset = rulesets.GetRuleset(MapShowcaseRuleset.SHORT_NAME);

            if (showcaseRuleset != null && configCache?.GetConfigFor(showcaseRuleset.CreateInstance()) is MapShowcaseConfigManager config)
                showOnSelection.BindTo(config.GetBindable<bool>(MapShowcaseSetting.ShowIntroOverlay));

            Child = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Width = 560,
                AutoSizeAxes = Axes.Y,
                Masking = true,
                CornerRadius = 10,
                Children =
                [
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = colourProvider.Background4,
                    },
                    new Box
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 5,
                        Colour = colourProvider.Colour1,
                    },
                    new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Padding = new MarginPadding
                        {
                            Horizontal = 30,
                            Top = 30,
                            Bottom = 20,
                        },
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(0, 10),
                        Children =
                        [
                            new FillFlowContainer
                            {
                                AutoSizeAxes = Axes.Both,
                                Direction = FillDirection.Horizontal,
                                Spacing = new Vector2(10, 0),
                                Children =
                                [
                                    new OsuSpriteText
                                    {
                                        Anchor = Anchor.BottomLeft,
                                        Origin = Anchor.BottomLeft,
                                        Text = MapShowcaseStrings.ShowcaseTitle,
                                        Font = OsuFont.Torus.With(size: 28, weight: FontWeight.SemiBold),
                                    },
                                    new OsuSpriteText
                                    {
                                        Anchor = Anchor.BottomLeft,
                                        Origin = Anchor.BottomLeft,
                                        Text = $@"v{typeof(MapShowcaseRuleset).Assembly.GetName().Version?.ToString(3)}",
                                        Font = OsuFont.GetFont(size: 13),
                                        Colour = OsuColour.Gray(0.6f),
                                    },
                                ],
                            },
                            new OsuTextFlowContainer(t =>
                            {
                                t.Font = OsuFont.GetFont(size: 15);
                                t.Colour = OsuColour.Gray(0.75f);
                            })
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Text = MapShowcaseStrings.IntroOverlayDescription.ToString(),
                            },
                            new SettingsCheckbox
                            {
                                LabelText = MapShowcaseStrings.IntroOverlayShowOnSelection,
                                Current = showOnSelection,
                                Margin = new MarginPadding { Vertical = 10 },
                            },
                            new SettingsButton
                            {
                                Text = MapShowcaseStrings.OpenShowcase,
                                Action = openShowcase,
                            },
                        ],
                    },
                ],
            };
        }

        private void openShowcase()
        {
            Hide();
            performer?.PerformFromScreen(screen => screen.Push(new ShowcaseScreen()), valid_screens);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            trackedRuleset.BindValueChanged(rulesetChanged);

            attachToScreenStack();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (game.ScreenStack != null)
            {
                game.ScreenStack.ScreenPushed -= onScreenChanged;
                game.ScreenStack.ScreenExited -= onScreenChanged;
            }

            base.Dispose(isDisposing);
        }

        /// <summary>
        /// Follows screen changes so the selected ruleset can be observed wherever the user is.
        /// </summary>
        private void attachToScreenStack()
        {
            if (game.ScreenStack == null)
                return;

            game.ScreenStack.ScreenPushed += onScreenChanged;
            game.ScreenStack.ScreenExited += onScreenChanged;

            trackCurrentScreen();
        }

        private void onScreenChanged(IScreen lastScreen, IScreen newScreen) => trackCurrentScreen();

        private void trackCurrentScreen()
        {
            trackedRuleset.UnbindAll();

            if (game.ScreenStack?.CurrentScreen is IOsuScreen osuScreen and not ShowcaseScreen)
                trackedRuleset.BindTo(osuScreen.Ruleset);
        }

        private void rulesetChanged(ValueChangedEvent<RulesetInfo> ruleset)
        {
            // Only react to the user switching to this ruleset.
            if (ruleset.NewValue?.ShortName != MapShowcaseRuleset.SHORT_NAME || ruleset.OldValue == null)
                return;

            if (showOnSelection.Value)
                Show();
        }

        protected override void PopIn()
        {
            this.FadeIn(TRANSITION_DURATION, Easing.OutQuint)
                .ScaleTo(1, 400, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            this.FadeOut(TRANSITION_DURATION, Easing.OutQuint)
                .ScaleTo(0.98f, TRANSITION_DURATION, Easing.OutQuint);
        }

        private static readonly Type[] valid_screens =
        [
            typeof(MainMenu),
            typeof(SongSelect),
        ];
    }
}
