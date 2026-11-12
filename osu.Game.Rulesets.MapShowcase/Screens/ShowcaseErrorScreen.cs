// Copyright (c) CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
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
using osu.Game.Localisation;
using osu.Game.Rulesets.MapShowcase.Graphics;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.MapShowcase.Screens
{
    public partial class ShowcaseErrorScreen(Exception? exception = null, Action? backAction = null) : OsuScreen
    {
        public override string Title => @"Error!";

        private OsuTextFlowContainer textFlow = null!;

        [BackgroundDependencyLoader]
        private void load()
        {
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
                                new SpriteIcon
                                {
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Icon = FontAwesome.Solid.Bomb,
                                    Size = new Vector2(50),
                                },
                                new OsuSpriteText
                                {
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Font = OsuFont.Style.Title,
                                    Text = MapShowcaseStrings.ErrorScreenTitle,
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
                                new ClickTwiceButton
                                {
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Action = backAction,
                                    IdleIcon = FontAwesome.Regular.ArrowAltCircleLeft,
                                    Text = CommonStrings.Exit,
                                },
                            ],
                        },
                    ],
                },
            ];

            textFlow.AddText(MapShowcaseStrings.ErrorScreenFirst);
            textFlow.NewLine();
            textFlow.AddText(MapShowcaseStrings.ErrorScreenSecond);

            if (exception != null)
            {
                textFlow.NewParagraph();
                textFlow.AddText(MapShowcaseStrings.ErrorScreenException);
                textFlow.NewLine();
                textFlow.AddText(exception.Message);
                textFlow.NewParagraph();
                textFlow.AddText(MapShowcaseStrings.ErrorScreenExceptionDetail);
            }

            textFlow.NewParagraph();
            textFlow.AddText("");
        }
    }
}
