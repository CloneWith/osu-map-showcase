// Copyright (c) CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.MapShowcase.Graphics
{
    /// <summary>
    /// Represents a color theme with a few variants.
    /// </summary>
    public interface IShowcaseColour
    {
        Colour4 Regular { get; }
        Colour4 Dark { get; }
        Colour4 Darker { get; }
        Colour4 Darkest { get; }
        Colour4 Light { get; }
        Colour4 Lighter { get; }
        Colour4 Lightest { get; }
    }

    /// <summary>
    /// Colour schemes shared by the showcase's interface elements.
    /// </summary>
    public static class ShowcaseColours
    {
        private sealed class ShowcaseColour(Colour4 regular, Colour4 dark, Colour4 darker, Colour4 darkest, Colour4 light, Colour4 lighter, Colour4 lightest)
            : IShowcaseColour
        {
            public ShowcaseColour(string regular, string dark, string darker, string darkest, string light, string lighter, string lightest)
                : this(Colour4.FromHex(regular), Colour4.FromHex(dark), Colour4.FromHex(darker), Colour4.FromHex(darkest), Colour4.FromHex(light), Colour4.FromHex(lighter), Colour4.FromHex(lightest))
            {
            }

            public Colour4 Regular { get; } = regular;
            public Colour4 Dark { get; } = dark;
            public Colour4 Darker { get; } = darker;
            public Colour4 Darkest { get; } = darkest;
            public Colour4 Light { get; } = light;
            public Colour4 Lighter { get; } = lighter;
            public Colour4 Lightest { get; } = lightest;
        }

        public static IShowcaseColour SeaBlue { get; } = new ShowcaseColour(@"#269FFE", @"#0992FE", @"#018BF7", @"#0172CB", @"#43ACFE", @"#52B2FE", @"#7DC6FE");
        public static IShowcaseColour SunshineYellow { get; } = new ShowcaseColour(@"#F29F00", @"#F18F00", @"#ED8300", @"#E87300", @"#F4B03B", @"#F7C775", @"#FADDAC");
        public static IShowcaseColour FlandreRed { get; } = new ShowcaseColour(@"#D05677", @"#C94562", @"#B9415E", @"#A33C58", @"#D96E8D", @"#E394AC", @"#EEBDCD");
        public static IShowcaseColour DeepPurple { get; } = new ShowcaseColour(@"#673AB7", @"#5E35B1", @"#512DA8", @"#4527A0", @"#7E57C2", @"#9575CD", @"#B39DDB");
        public static IShowcaseColour LightGreen { get; } = new ShowcaseColour(@"#8BC34A", @"#7CB342", @"#689F38", @"#558B2F", @"#9CCC65", @"#AED581", @"#C5E1A5");

        /// <summary>
        /// Derive a full <see cref="IShowcaseColour"/> scheme from a single theme colour.
        /// </summary>
        /// <param name="regular">the theme colour, used as the <see cref="IShowcaseColour.Regular"/> shade</param>
        public static IShowcaseColour FromThemeColour(Colour4 regular) => new ShowcaseColour(
            regular,
            deriveShade(regular, -0.05f),
            deriveShade(regular, -0.09f),
            deriveShade(regular, -0.15f),
            deriveShade(regular, +0.07f),
            deriveShade(regular, +0.15f),
            deriveShade(regular, +0.23f)
        );

        private static Colour4 deriveShade(Colour4 colour, float lightnessDelta)
        {
            var hsl = colour.ToHSL();
            return Colour4.FromHSL(hsl.X, hsl.Y, Math.Clamp(hsl.Z + lightnessDelta, 0f, 1f));
        }
    }
}
