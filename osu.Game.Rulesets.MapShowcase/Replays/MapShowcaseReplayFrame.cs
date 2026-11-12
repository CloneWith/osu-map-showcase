// Copyright (c) ppy Pty Ltd <contact@ppy.sh> & CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Replays;
using osuTK;

namespace osu.Game.Rulesets.MapShowcase.Replays
{
    public class MapShowcaseReplayFrame : ReplayFrame
    {
        public List<MapShowcaseAction> Actions = [];
        public Vector2 Position;

        public MapShowcaseReplayFrame(MapShowcaseAction? button = null)
        {
            if (button.HasValue)
                Actions.Add(button.Value);
        }

        public override bool IsEquivalentTo(ReplayFrame other)
            => other is MapShowcaseReplayFrame showcaseFrame && Math.Abs(Time - showcaseFrame.Time) < double.Epsilon && Position == showcaseFrame.Position && Actions.SequenceEqual(showcaseFrame.Actions);
    }
}
