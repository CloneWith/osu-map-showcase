// Copyright (c) CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Graphics.Containers;

namespace osu.Game.Rulesets.MapShowcase.Graphics
{
    /// <summary>
    /// A general chess icon display for usages outside the normal board.
    /// </summary>
    public partial class ShowcaseChessIcon : ConstrainedIconContainer
    {
        /// <summary>
        /// The name of the chess's mod.
        /// </summary>
        public string ModName { get; }

        /// <summary>
        /// The index of the chess in its mod category.
        /// </summary>
        public string ModIndex { get; }

        [Resolved]
        private TextureStore textures { get; set; } = null!;

        private ModColourScheme colourScheme = ModColours.EMPTY;
        private bool isTieBreaker;

        /// <summary>
        /// Constructs a chess icon.
        /// </summary>
        /// <param name="mod">the mod name of the icon</param>
        /// <param name="index">the mod index</param>
        public ShowcaseChessIcon(string mod, string index)
        {
            ModName = mod;
            ModIndex = index;
        }

        /// <summary>
        /// Constructs a chess icon with no mod information.
        /// </summary>
        public ShowcaseChessIcon()
            : this(string.Empty, string.Empty)
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            isTieBreaker = ModName.Equals(@"TB", StringComparison.OrdinalIgnoreCase);

            colourScheme = isTieBreaker ? ModColours.TIE_BREAKER : ModColours.FromModString(ModName);

            // Use win icon for TB maps, subject to change
            Texture? chessIcon;

            if (isTieBreaker)
                chessIcon = textures.Get(@"Board/chess-win");
            else
            {
                chessIcon = textures.Get(@$"Board/{ModName}{ModIndex}")
                            ?? textures.Get(@$"Board/{ModName}");
            }

            InternalChildren =
            [
                new Sprite
                {
                    Name = @"Chess icon",
                    RelativeSizeAxes = Axes.Both,
                    Texture = chessIcon,
                    FillMode = FillMode.Fit,
                    Colour = colourScheme.Accent,
                },
            ];
        }
    }
}
