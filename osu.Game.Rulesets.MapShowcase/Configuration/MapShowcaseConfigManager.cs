// Copyright (c) CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Configuration;
using osu.Game.Rulesets.Configuration;

namespace osu.Game.Rulesets.MapShowcase.Configuration
{
    /// <summary>
    /// Settings owned by the showcase ruleset, stored alongside the ruleset's key bindings.
    /// </summary>
    public class MapShowcaseConfigManager(SettingsStore? settings, RulesetInfo ruleset, int? variant = null)
        : RulesetConfigManager<MapShowcaseSetting>(settings!, ruleset, variant)
    {
        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();

            SetDefault(MapShowcaseSetting.ShowIntroOverlay, true);
        }
    }

    public enum MapShowcaseSetting
    {
        /// <summary>
        /// Whether the informational overlay is shown whenever this ruleset becomes the selected ruleset.
        /// </summary>
        ShowIntroOverlay,
    }
}
