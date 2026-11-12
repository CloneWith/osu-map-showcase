// Copyright (c) CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.MapShowcase.Configuration;
using osu.Game.Screens.Menu;
using osu.Game.Screens.Select;

namespace osu.Game.Rulesets.MapShowcase.UI
{
    public partial class MapShowcaseSettingsSubsection(Ruleset ruleset) : RulesetSettingsSubsection(ruleset)
    {
        [Resolved(CanBeNull = true)]
        private IPerformFromScreenRunner? performer { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            if (Config is not MapShowcaseConfigManager config)
                return;

            Children =
            [
                new SettingsButton
                {
                    Text = MapShowcaseStrings.OpenShowcase,
                    Action = () =>
                    {
                        performer?.PerformFromScreen(screen => screen.Push(new ShowcaseScreen()), valid_screens);
                    },
                },
                new OsuTextFlowContainer(t =>
                {
                    t.Font = t.Font.With(size: 12);
                    t.Colour = OsuColour.Gray(0.6f);
                })
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Margin = new MarginPadding
                    {
                        Horizontal = SettingsPanel.CONTENT_MARGINS,
                        Top = 4,
                    },
                    Text = MapShowcaseStrings.OpenShowcaseDescription.ToString(),
                },
                new SettingsCheckbox
                {
                    LabelText = MapShowcaseStrings.IntroOverlayShowOnSelection,
                    Current = config.GetBindable<bool>(MapShowcaseSetting.ShowIntroOverlay),
                    Margin = new MarginPadding { Top = 10 },
                },
            ];
        }

        private static readonly Type[] valid_screens =
        [
            typeof(MainMenu),
            typeof(SongSelect),
        ];
    }
}
