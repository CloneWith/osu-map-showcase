// Copyright (c) CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.MapShowcase.Configuration;
using osu.Game.Screens.Menu;
using osu.Game.Screens.Select;
using osu.Game.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.MapShowcase.UI
{
    public partial class MapShowcaseSettingsSubsection(Ruleset ruleset) : RulesetSettingsSubsection(ruleset)
    {
        [Resolved(CanBeNull = true)]
        private IPerformFromScreenRunner? performer { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            if (Config is not MapShowcaseConfigManager)
                return;

            Children =
            [
                new SettingsItemV2(new FormCheckBox
                {
                    Caption = MapShowcaseStrings.AutomaticallyDownloadReplays,
                    Current = ((MapShowcaseConfigManager)Config).GetBindable<bool>(MapShowcaseSetting.AutomaticallyDownloadReplays),
                })
                {
                    Margin = new MarginPadding { Bottom = 8 },
                },
                new SettingsButton
                {
                    Text = MapShowcaseStrings.OpenShowcase,
                    Action = () =>
                    {
                        performer?.PerformFromScreen(screen => screen.Push(new ShowcaseScreen()), valid_screens);
                    },
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
