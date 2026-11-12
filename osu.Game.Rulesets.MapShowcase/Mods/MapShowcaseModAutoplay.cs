// Copyright (c) ppy Pty Ltd <contact@ppy.sh> & CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.MapShowcase.Replays;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.MapShowcase.Mods
{
    public class MapShowcaseModAutoplay : ModAutoplay
    {
        public override ModReplayData CreateReplayData(IBeatmap beatmap, IReadOnlyList<Mod> mods)
            => new ModReplayData(new MapShowcaseAutoGenerator(beatmap).Generate(), new ModCreatedUser { Username = "sample" });
    }
}
