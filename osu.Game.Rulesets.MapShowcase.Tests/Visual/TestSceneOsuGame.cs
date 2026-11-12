// Copyright (c) CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;
using osu.Game.Tests.Visual;
using osuTK.Graphics;

namespace osu.Game.Rulesets.MapShowcase.Tests.Visual
{
    [TestFixture]
    [Explicit]
    public partial class TestSceneOsuGame : OsuTestScene
    {
        private TestGame game = null!;

        [BackgroundDependencyLoader]
        private void load()
        {
            Child = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Black,
            };

            AddGame(game = new TestGame());
        }

        [SetUpSteps]
        public void SetUpSteps()
        {
            AddUntilStep("wait for game", () => game.IsLoaded);
        }

        [Test]
        public void TestRulesetIsInstalled()
        {
            // The client only offers a ruleset it managed to load from disk and whose API version it accepts,
            // so this asserts the built assembly is actually usable as a ruleset.
            AddAssert("showcase ruleset is available", () => game.RulesetStore.GetRuleset(MapShowcaseRuleset.SHORT_NAME) != null);
        }

        private partial class TestGame : OsuGame
        {
            public new RealmRulesetStore RulesetStore => base.RulesetStore;
        }
    }
}
