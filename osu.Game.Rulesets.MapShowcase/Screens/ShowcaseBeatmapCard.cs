// Copyright (c) ppy Pty Ltd <contact@ppy.sh> & CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Allocation;
using osu.Game.Graphics;
using osu.Game.Graphics.Backgrounds;
using osu.Game.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Overlays;
using osu.Game.Rulesets.MapShowcase.Graphics;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.MapShowcase.Screens
{
    /// <summary>
    /// The card of the transient beatmap information display, kept on its face-down side.
    /// </summary>
    public partial class ShowcaseBeatmapCard : CompositeDrawable
    {
        public static readonly Vector2 SIZE = new Vector2(180, 300);

        public const float CORNER_RADIUS = 6;

        /// <summary>
        /// The size of the chess piece at the centre of the card.
        /// </summary>
        private const float chess_icon_size = 64;

        /// <summary>
        /// The gap between the chess piece and the mod text below it.
        /// </summary>
        private const float mod_text_spacing = 12;

        private readonly ShowcaseBeatmap beatmap;
        private readonly Sprite cardBackground;

        [BackgroundDependencyLoader]
        private void load(BeatmapManager beatmapManager)
        {
            cardBackground.Texture = beatmapManager.GetWorkingBeatmap(beatmap.BeatmapInfo).GetBackground();
        }

        public ShowcaseBeatmapCard(ShowcaseBeatmap showcaseBeatmap, OverlayColourScheme? colourScheme = null)
        {
            beatmap = showcaseBeatmap;
            var colourProvider = new OverlayColourProvider(colourScheme ?? OverlayColourScheme.Blue);

            string mod = showcaseBeatmap.ModString.Value;
            string modIndex = showcaseBeatmap.ModIndex.Value;

            Size = SIZE;

            InternalChild = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children =
                [
                    new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Masking = true,
                        CornerRadius = CORNER_RADIUS,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        EdgeEffect = new EdgeEffectParameters
                        {
                            Type = EdgeEffectType.Shadow,
                            Radius = 5,
                            Colour = Color4.Black.Opacity(0.1f),
                        },
                    },
                    new Container
                    {
                        Name = @"Card back",
                        RelativeSizeAxes = Axes.Both,
                        Masking = true,
                        CornerRadius = CORNER_RADIUS,
                        Children =
                        [
                            cardBackground = new Sprite
                            {
                                RelativeSizeAxes = Axes.Both,
                                FillMode = FillMode.Fill,
                            },
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = ColourInfo.GradientVertical(colourProvider.Background3.Opacity(0.45f), colourProvider.Background4.Opacity(0.45f)),
                            },
                            new TrianglesV2
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = colourProvider.Background4,
                                SpawnRatio = 1.2f,
                                Velocity = 0.1f,
                            },
                            new FillFlowContainer
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Direction = FillDirection.Vertical,
                                AutoSizeAxes = Axes.Both,
                                Spacing = new Vector2(0, mod_text_spacing),
                                Children =
                                [
                                    new ShowcaseChessIcon(mod, modIndex)
                                    {
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        Size = new Vector2(chess_icon_size),
                                    },
                                    new TruncatingSpriteText
                                    {
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        Text = $"{mod}{modIndex}",
                                        MaxWidth = SIZE.X - 40,
                                        Font = OsuFont.GetFont(size: 32, weight: FontWeight.Bold),
                                        Colour = ModColours.FromModString(mod).Accent,
                                    },
                                ],
                            },
                        ],
                    },
                ],
            };
        }
    }
}
