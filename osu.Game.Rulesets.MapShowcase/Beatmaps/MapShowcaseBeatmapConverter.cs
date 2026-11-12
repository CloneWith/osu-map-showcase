// Copyright (c) ppy Pty Ltd <contact@ppy.sh> & CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Threading;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.MapShowcase.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osuTK;

namespace osu.Game.Rulesets.MapShowcase.Beatmaps
{
    public class MapShowcaseBeatmapConverter(IBeatmap beatmap, Ruleset ruleset) : BeatmapConverter<MapShowcaseHitObject>(beatmap, ruleset)
    {
        public override bool CanConvert() => true;

        protected override IEnumerable<MapShowcaseHitObject> ConvertHitObject(HitObject original, IBeatmap beatmap, CancellationToken cancellationToken)
        {
            yield return new MapShowcaseHitObject
            {
                Samples = original.Samples,
                StartTime = original.StartTime,
                Position = (original as IHasPosition)?.Position ?? Vector2.Zero,
            };
        }
    }
}
