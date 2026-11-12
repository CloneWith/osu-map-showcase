// Copyright (c) CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;

namespace osu.Game.Rulesets.MapShowcase.Screens
{
    /// <summary>
    /// A screen that reports a (short) title to the screen stack above it.
    /// </summary>
    public interface ISubScreenWithTitle : IOsuScreen
    {
        string Title { get; }

        string ShortTitle { get; }

        LocalisableString LocalisableTitle => default;

        bool ShowHeaderLine => true;
    }
}
