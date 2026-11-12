// Copyright (c) ppy Pty Ltd <contact@ppy.sh> & CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets.MapShowcase.Objects;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.MapShowcase.Replays
{
    public class MapShowcaseAutoGenerator(IBeatmap beatmap) : AutoGenerator<MapShowcaseReplayFrame>(beatmap)
    {
        public new Beatmap<MapShowcaseHitObject> Beatmap => (Beatmap<MapShowcaseHitObject>)base.Beatmap;

        protected override void GenerateFrames()
        {
            Frames.Add(new MapShowcaseReplayFrame());

            foreach (MapShowcaseHitObject hitObject in Beatmap.HitObjects)
            {
                Frames.Add(new MapShowcaseReplayFrame
                {
                    Time = hitObject.StartTime,
                    Position = hitObject.Position,
                });
            }
        }
    }
}
