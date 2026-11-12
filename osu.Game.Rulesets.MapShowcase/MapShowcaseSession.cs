// Copyright (c) CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Game.Rulesets.MapShowcase.Overlays;

namespace osu.Game.Rulesets.MapShowcase
{
    /// <summary>
    /// Hooks the pieces of the showcase that are not part of a ruleset's normal lifecycle into the game.
    /// </summary>
    public static class MapShowcaseSession
    {
        private static int initialisedFor = -1;

        /// <summary>
        /// The storage showcase profiles are read from and written to.
        /// </summary>
        public static ShowcaseStorage? Storage { get; private set; }

        /// <summary>
        /// Registers the showcase with <paramref name="game"/>, once per game instance.
        /// </summary>
        public static void EnsureInitialised(OsuGame game)
        {
            int session = game.GetHashCode();

            if (initialisedFor == session)
                return;

            initialisedFor = session;

            try
            {
                if (game.Dependencies is not DependencyContainer dependencies)
                    return;

                Storage = findExistingStorage(dependencies) ?? createStorage(dependencies);

                game.Add(new MapShowcaseIntroOverlay());
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to initialise the map showcase session.");
            }
        }

        private static ShowcaseStorage? findExistingStorage(DependencyContainer dependencies)
        {
            try
            {
                return dependencies.Get<ShowcaseStorage>();
            }
            catch (DependencyNotRegisteredException)
            {
                return null;
            }
        }

        private static ShowcaseStorage createStorage(DependencyContainer dependencies)
        {
            var storage = new ShowcaseStorage(dependencies.Get<Storage>());

            dependencies.CacheAs(storage);
            dependencies.Get<TextureStore>().AddTextureSource(new TextureLoaderStore(new StorageBackedResourceStore(storage)));

            return storage;
        }
    }
}
