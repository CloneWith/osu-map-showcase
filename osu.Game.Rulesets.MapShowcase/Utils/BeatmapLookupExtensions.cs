// Copyright (c) CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;

namespace osu.Game.Rulesets.MapShowcase.Utils
{
    /// <summary>
    /// Helpers for finding the local beatmap a showcase entry refers to.
    /// </summary>
    public static class BeatmapLookupExtensions
    {
        /// <param name="showcaseBeatmap">The showcase entry to look up.</param>
        extension(ShowcaseBeatmap showcaseBeatmap)
        {
            /// <summary>
            /// Find the database-backed beatmap info of a showcase entry.
            /// </summary>
            /// <returns>The matching local beatmap, or <c>null</c> if the beatmap is not available locally.</returns>
            public BeatmapInfo? FindLocalBeatmapInfo(BeatmapManager beatmapManager)
            {
                string hash = showcaseBeatmap.BeatmapHash;

                // BeatmapInfo.Hash used to be the MD5 of the .osu file and is now its SHA-2, so profiles written by
                // either version of the game may hold either value. Both are checked to keep existing profiles working.
                if (!string.IsNullOrEmpty(hash))
                {
                    BeatmapInfo? byHash = beatmapManager.QueryBeatmap(b => b.Hash == hash)
                                           ?? beatmapManager.QueryBeatmap(b => b.MD5Hash == hash);

                    if (byHash != null)
                        return byHash;
                }

                if (showcaseBeatmap.BeatmapId > 0)
                    return beatmapManager.QueryOnlineBeatmapId(showcaseBeatmap.BeatmapId);

                return null;
            }

            /// <summary>
            /// Retrieve the working beatmap of a showcase entry from the local library.
            /// </summary>
            /// <returns>The working beatmap, or <c>null</c> if the beatmap is not available locally.</returns>
            public WorkingBeatmap? GetLocalWorkingBeatmap(BeatmapManager beatmapManager)
            {
                BeatmapInfo? beatmapInfo = showcaseBeatmap.FindLocalBeatmapInfo(beatmapManager);

                // The refetch guarantees that the returned beatmap info carries its beatmap set along with the files.
                return beatmapInfo == null ? null : beatmapManager.GetWorkingBeatmap(beatmapInfo, true);
            }
        }
    }
}
