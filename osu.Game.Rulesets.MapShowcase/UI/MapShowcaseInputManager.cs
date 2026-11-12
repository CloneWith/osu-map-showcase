// Copyright (c) ppy Pty Ltd <contact@ppy.sh> & CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.MapShowcase.UI
{
    public partial class MapShowcaseInputManager(RulesetInfo ruleset)
        : RulesetInputManager<MapShowcaseAction>(ruleset, 0, SimultaneousBindingMode.None);
}
