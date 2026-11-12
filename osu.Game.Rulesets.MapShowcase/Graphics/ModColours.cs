// Copyright (c) CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.MapShowcase.Graphics
{
    /// <summary>
    /// All mod colour schemes used by chess pieces.
    /// </summary>
    public static class ModColours
    {
        public static readonly ModColourScheme NO_MOD = new ModColourScheme(
            Colour4.White, Colour4.FromHex(@"#545454"),
            Colour4.FromHex(@"#666666"), Colour4.FromHex(@"#595959"));

        public static readonly ModColourScheme HARD_ROCK = new ModColourScheme(
            Colour4.FromHex(@"#FF5733"), Colour4.FromHex(@"#541D10"),
            Colour4.FromHex(@"#662314"), Colour4.FromHex(@"#591E11"));

        public static readonly ModColourScheme HIDDEN = new ModColourScheme(
            Colour4.FromHex(@"#FFEB3B"), Colour4.FromHex(@"#544D14"),
            Colour4.FromHex(@"#665E18"), Colour4.FromHex(@"#595215"));

        public static readonly ModColourScheme DOUBLE_TIME = new ModColourScheme(
            Colour4.FromHex(@"#9D73FF"), Colour4.FromHex(@"#342654"),
            Colour4.FromHex(@"#3F2E66"), Colour4.FromHex(@"#372859"));

        public static readonly ModColourScheme FREE_MOD = new ModColourScheme(
            Colour4.FromHex(@"#43CF7C"), Colour4.FromHex(@"#164429"),
            Colour4.FromHex(@"#1B5332"), Colour4.FromHex(@"#17482C"));

        public static readonly ModColourScheme TIE_BREAKER = new ModColourScheme(
            Colour4.FromHex(@"#FFA500"), Colour4.FromHex(@"#714800"),
            Colour4.FromHex(@"#4C3000"), Colour4.FromHex(@"#986000"));

        /// <summary>
        /// The fallback color scheme for empty and unavailable chess pieces.
        /// </summary>
        public static readonly ModColourScheme EMPTY = new ModColourScheme(
            Colour4.White, Colour4.FromHex(@"#BDBDBD"),
            Colour4.FromHex(@"#E5E5E5"), Colour4.FromHex(@"#C7C7C7"));

        /// <summary>
        /// Get the corresponding <see cref="ModColourScheme"/> based on a given string.
        /// </summary>
        /// <param name="mod">the given mod string</param>
        /// <returns>a color scheme of the mod, <see cref="EMPTY"/> if unavailable</returns>
        public static ModColourScheme FromModString(string? mod) => mod?.Trim().ToUpperInvariant() switch
        {
            @"NM" => NO_MOD,
            @"HR" => HARD_ROCK,
            @"HD" => HIDDEN,
            @"DT" => DOUBLE_TIME,
            @"FM" => FREE_MOD,
            @"TB" => TIE_BREAKER,
            _ => EMPTY,
        };
    }

    /// <summary>
    /// A set of colors for a specific mod.
    /// </summary>
    public class ModColourScheme(Colour4 accent, Colour4 background, Colour4 triangleLight, Colour4 triangleDark)
    {
        public readonly Colour4 Accent = accent;
        public readonly Colour4 Background = background;
        public readonly Colour4 TriangleLight = triangleLight;
        public readonly Colour4 TriangleDark = triangleDark;

        /// <summary>
        /// Derive a <see cref="ModColourScheme"/> from a single theme colour, used as the <see cref="Accent"/>.
        /// </summary>
        /// <param name="accent">the theme colour</param>
        public static ModColourScheme FromThemeColour(Colour4 accent)
        {
            var hsl = accent.ToHSL();
            float hue = hsl.X;
            float saturation = Math.Clamp(hsl.Y * 0.5f, 0.1f, 0.65f);

            return new ModColourScheme(
                accent,
                Colour4.FromHSL(hue, saturation, 0.20f),
                Colour4.FromHSL(hue, saturation, 0.25f),
                Colour4.FromHSL(hue, saturation, 0.22f)
            );
        }
    }
}
