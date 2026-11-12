// Copyright (c) ppy Pty Ltd <contact@ppy.sh> & CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using osu.Framework.Localisation;

namespace osu.Game.Rulesets.MapShowcase.Utils
{
    /// <summary>
    /// String helpers the showcase needs that the public game assembly does not provide.
    /// </summary>
    public static class StringExtensions
    {
        /// <param name="text">The string to be truncated.</param>
        extension(string text)
        {
            /// <summary>
            /// Truncate a string in the middle, keeping its head and tail.
            /// </summary>
            /// <param name="maxLength">The maximum string length including ellipsis.</param>
            /// <returns>A truncated string.</returns>
            public string TruncateMiddleWithEllipsis(int maxLength)
            {
                if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
                    return text;

                if (maxLength <= 3)
                    return text[..maxLength];

                int charsToKeep = maxLength - 3;
                int leftPart = charsToKeep / 2;
                int rightPart = charsToKeep - leftPart;

                return string.Concat(text.AsSpan(0, leftPart), "...", text.AsSpan(text.Length - rightPart));
            }

            public bool IsSafeForFilename(int maxLength = 255)
                => text.IsSafeForFilename(out _, maxLength);

            /// <summary>
            /// Check if the given string is safe to be used as a filename.
            /// </summary>
            /// <param name="errorMessage">The issue found in the string as a <see cref="LocalisableString"/>.</param>
            /// <param name="maxLength">The maximum length of the string.</param>
            /// <returns>True if safe to use, otherwise false.</returns>
            public bool IsSafeForFilename(out LocalisableString errorMessage, int maxLength = 255)
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    errorMessage = StringCheckStrings.EmptyName;
                    return false;
                }

                if (text.Length > maxLength)
                {
                    errorMessage = StringCheckStrings.MaximumLengthExceeded(maxLength);
                    return false;
                }

                Regex invalidCharsPattern = new Regex(@"[\\/:*?""<>|]");

                if (invalidCharsPattern.IsMatch(text))
                {
                    errorMessage = StringCheckStrings.InvalidCharsFound;
                    return false;
                }

                foreach (UnicodeCategory category in text.Select(char.GetUnicodeCategory))
                {
                    if (category is UnicodeCategory.OtherSymbol or UnicodeCategory.MathSymbol or UnicodeCategory.CurrencySymbol
                        or UnicodeCategory.ModifierSymbol or UnicodeCategory.NonSpacingMark or UnicodeCategory.EnclosingMark
                        or UnicodeCategory.SpacingCombiningMark or UnicodeCategory.Surrogate)
                    {
                        errorMessage = StringCheckStrings.SpecialCharsFound;
                        return false;
                    }
                }

                errorMessage = string.Empty;
                return true;
            }
        }
    }
}
