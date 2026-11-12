// Copyright (c) CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.MapShowcase.Screens
{
    public partial class ShowcaseBeatmapMissingScreen(ShowcaseBeatmap beatmap) : OsuScreen
    {
        public override string Title => @"Beatmap missing!";

        private OsuTextFlowContainer textFlow = null!;

        [BackgroundDependencyLoader]
        private void load()
        {
            Alpha = 0;

            InternalChildren =
            [
                new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Width = 500,
                    Height = 500,
                    Masking = true,
                    CornerRadius = 10,
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Colour = Color4.Black.Opacity(0.5f),
                        Radius = 10,
                    },
                    Children =
                    [
                        new Box
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4Extensions.FromHex("#262626"),
                        },
                        new FillFlowContainer
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(10),
                            Children =
                            [
                                new FillFlowContainer
                                {
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    AutoSizeAxes = Axes.Both,
                                    Direction = FillDirection.Horizontal,
                                    Spacing = new Vector2(10),
                                    Children =
                                    [
                                        new SpriteIcon
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Icon = OsuIcon.Beatmap,
                                            Size = new Vector2(50),
                                        },
                                        new SpriteIcon
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Icon = OsuIcon.Search,
                                            Size = new Vector2(50),
                                        },
                                        new SpriteIcon
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Icon = FontAwesome.Solid.QuestionCircle,
                                            Size = new Vector2(50),
                                        },
                                    ],
                                },
                                new OsuSpriteText
                                {
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Font = OsuFont.Style.Title,
                                    Text = MapShowcaseStrings.BeatmapMissingScreenTitle,
                                },
                                textFlow = new OsuTextFlowContainer
                                {
                                    AlwaysPresent = true,
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Padding = new MarginPadding { Top = 20, Horizontal = 50 },
                                    TextAnchor = Anchor.TopCentre,
                                    FirstLineIndent = 0.5f,
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                },
                            ],
                        },
                    ],
                },
            ];

            textFlow.AddText(MapShowcaseStrings.BeatmapMissingScreenFirst);
            textFlow.NewLine();
            textFlow.AddText(MapShowcaseStrings.BeatmapMissingScreenSecond);
            textFlow.NewParagraph();
            textFlow.AddText(MapShowcaseStrings.BeatmapMissingScreenThird);

            textFlow.NewParagraph();
            textFlow.AddText($"Online ID: {beatmap.BeatmapId}");
            textFlow.NewLine();
            textFlow.AddText($"Hash: {beatmap.BeatmapHash}");
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            this.FadeIn(500, Easing.OutQuint).Delay(2500).FadeOut(500, Easing.OutQuint);
        }
    }
}
