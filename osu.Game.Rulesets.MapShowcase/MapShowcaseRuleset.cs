// Copyright (c) ppy Pty Ltd <contact@ppy.sh> & CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Graphics;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Configuration;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.MapShowcase.Beatmaps;
using osu.Game.Rulesets.MapShowcase.Configuration;
using osu.Game.Rulesets.MapShowcase.Mods;
using osu.Game.Rulesets.MapShowcase.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.MapShowcase
{
    public partial class MapShowcaseRuleset : Ruleset
    {
        public const string SHORT_NAME = @"showcase";

        public override string Description => @"Mappool showcase";

        public override string ShortName => SHORT_NAME;

        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod>? mods = null) =>
            new DrawableMapShowcaseRuleset(this, beatmap, mods);

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) =>
            new MapShowcaseBeatmapConverter(beatmap, this);

        public override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap) =>
            new MapShowcaseDifficultyCalculator(RulesetInfo, beatmap);

        public override IEnumerable<Mod> GetModsFor(ModType type) => type switch
        {
            ModType.Automation => [new MapShowcaseModAutoplay()],
            _ => [],
        };

        public override RulesetSettingsSubsection CreateSettings() => new MapShowcaseSettingsSubsection(this);

        public override IRulesetConfigManager CreateConfig(SettingsStore? settings) => new MapShowcaseConfigManager(settings, RulesetInfo);

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0) =>
        [
            new KeyBinding(new[] { InputKey.Control, InputKey.Enter }, MapShowcaseAction.StartShowcase),
            new KeyBinding(new[] { InputKey.Control, InputKey.Alt, InputKey.Q }, MapShowcaseAction.ForceQuit),
            new KeyBinding(new[] { InputKey.Control, InputKey.Alt, InputKey.Left }, MapShowcaseAction.PreviousBeatmap),
            new KeyBinding(new[] { InputKey.Control, InputKey.Alt, InputKey.Right }, MapShowcaseAction.NextBeatmap),
            new KeyBinding(new[] { InputKey.Control, InputKey.Alt, InputKey.R }, MapShowcaseAction.ReplayCurrent),
            new KeyBinding(new[] { InputKey.Control, InputKey.A }, MapShowcaseAction.ToggleAuto),
        ];

        public override Drawable CreateIcon() => new Icon();

        public partial class Icon : CompositeDrawable
        {
            public Icon()
            {
                RelativeSizeAxes = Axes.Both;
                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;

                InternalChild = new SpriteIcon
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Icon = OsuIcon.Ranking,
                };
            }

            [Resolved(CanBeNull = true)]
            private OsuGame? game { get; set; }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                if (game != null)
                    MapShowcaseSession.EnsureInitialised(game);
            }
        }

        // Leave this line intact. It will bake the correct version into the ruleset on each build/release.
        public override string RulesetAPIVersionSupported => CURRENT_RULESET_API_VERSION;
    }
}
