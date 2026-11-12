// Copyright (c) ppy Pty Ltd <contact@ppy.sh> & CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.MapShowcase.Objects;
using osu.Game.Rulesets.MapShowcase.Objects.Drawables;
using osu.Game.Rulesets.MapShowcase.Replays;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.MapShowcase.UI
{
    [Cached]
    public partial class DrawableMapShowcaseRuleset(MapShowcaseRuleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod>? mods = null)
        : DrawableRuleset<MapShowcaseHitObject>(ruleset, beatmap, mods)
    {
        protected override Playfield CreatePlayfield() => new MapShowcasePlayfield();

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new MapShowcaseFramedReplayInputHandler(replay);

        public override DrawableHitObject<MapShowcaseHitObject> CreateDrawableRepresentation(MapShowcaseHitObject h) => new DrawableMapShowcaseHitObject(h);

        protected override PassThroughInputManager CreateInputManager() => new MapShowcaseInputManager(Ruleset.RulesetInfo);
    }
}
