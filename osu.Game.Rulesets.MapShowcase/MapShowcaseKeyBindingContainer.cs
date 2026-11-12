// Copyright (c) CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Input.Bindings;
using osu.Game.Input.Bindings;

namespace osu.Game.Rulesets.MapShowcase
{
    public partial class MapShowcaseKeyBindingContainer(RulesetInfo ruleset)
        : DatabasedKeyBindingContainer<MapShowcaseAction>(ruleset, 0, SimultaneousBindingMode.None, KeyCombinationMatchingMode.Modifiers);
}
