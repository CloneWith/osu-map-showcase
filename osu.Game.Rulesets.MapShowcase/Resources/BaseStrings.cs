// Copyright (c) ppy Pty Ltd <contact@ppy.sh> & CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;

namespace osu.Game.Rulesets.MapShowcase.Resources
{
    /// <summary>
    /// Generic interface strings.
    /// </summary>
    public static class BaseStrings
    {
        private const string prefix = @"osu.Game.Rulesets.MapShowcase.Resources.Localisation.Base";

        /// <summary>
        /// "Add New"
        /// </summary>
        public static LocalisableString AddNew => new TranslatableString(getKey(@"add_new"), @"Add New");

        /// <summary>
        /// "Refresh"
        /// </summary>
        public static LocalisableString Refresh => new TranslatableString(getKey(@"refresh"), @"Refresh");

        /// <summary>
        /// "Remove"
        /// </summary>
        public static LocalisableString Remove => new TranslatableString(getKey(@"remove"), @"Remove");

        private static string getKey(string key) => $@"{prefix}:{key}";
    }
}
