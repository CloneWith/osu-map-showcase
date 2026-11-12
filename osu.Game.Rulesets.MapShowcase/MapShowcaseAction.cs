// Copyright (c) CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;

namespace osu.Game.Rulesets.MapShowcase
{
    public enum MapShowcaseAction
    {
        [LocalisableDescription(typeof(MapShowcaseStrings), nameof(MapShowcaseStrings.StartShowcase))]
        StartShowcase,

        [LocalisableDescription(typeof(MapShowcaseStrings), nameof(MapShowcaseStrings.ForceQuit))]
        ForceQuit,

        [LocalisableDescription(typeof(MapShowcaseStrings), nameof(MapShowcaseStrings.GoToPrevious))]
        PreviousBeatmap,

        [LocalisableDescription(typeof(MapShowcaseStrings), nameof(MapShowcaseStrings.GoToNext))]
        NextBeatmap,

        [LocalisableDescription(typeof(MapShowcaseStrings), nameof(MapShowcaseStrings.ReplayCurrent))]
        ReplayCurrent,

        [LocalisableDescription(typeof(MapShowcaseStrings), nameof(MapShowcaseStrings.ToggleAuto))]
        ToggleAuto,
    }
}
