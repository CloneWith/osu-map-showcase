// Copyright (c) ppy Pty Ltd <contact@ppy.sh> & CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Input.StateChanges;
using osu.Framework.Utils;
using osu.Game.Replays;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.MapShowcase.Replays
{
    public class MapShowcaseFramedReplayInputHandler(Replay replay) : FramedReplayInputHandler<MapShowcaseReplayFrame>(replay)
    {
        protected override bool IsImportant(MapShowcaseReplayFrame frame) => frame.Actions.Count != 0;

        protected override void CollectReplayInputs(List<IInput> inputs)
        {
            var position = Interpolation.ValueAt(CurrentTime, StartFrame.Position, EndFrame.Position, StartFrame.Time, EndFrame.Time);

            inputs.Add(new MousePositionAbsoluteInput
            {
                Position = GamefieldToScreenSpace(position),
            });
            inputs.Add(new ReplayState<MapShowcaseAction>
            {
                PressedActions = CurrentFrame?.Actions ?? new List<MapShowcaseAction>(),
            });
        }
    }
}
