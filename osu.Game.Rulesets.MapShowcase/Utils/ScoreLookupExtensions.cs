// Copyright (c) CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Logging;
using osu.Game.Scoring;

namespace osu.Game.Rulesets.MapShowcase.Utils
{
    /// <summary>
    /// Helpers for finding the local replay a showcase entry refers to.
    /// </summary>
    public static class ScoreLookupExtensions
    {
        /// <param name="showcaseBeatmap">The showcase entry to look up.</param>
        extension(ShowcaseBeatmap showcaseBeatmap)
        {
            /// <summary>
            /// Find the databased score of a showcase entry.
            /// </summary>
            /// <returns>The local score, or <c>null</c> if it is not available locally.</returns>
            public ScoreInfo? ResolveScoreInfo(ScoreManager scoreManager)
                => showcaseBeatmap.GetLocalScore(scoreManager)?.ScoreInfo;

            /// <summary>
            /// Find the local replay of a showcase entry.
            /// </summary>
            /// <returns>The local score, or <c>null</c> if no playable replay could be found.</returns>
            public Score? GetLocalScore(ScoreManager scoreManager)
            {
                // A score picked during this session is already detached and can be used directly, but it may still be an
                // online-only score (no local .osr), which is why every candidate is validated below.
                if (showcaseBeatmap.ShowcaseScore != null && tryFetch(showcaseBeatmap.ShowcaseScore, scoreManager) is { } fromMemory)
                    return fromMemory;

                if (!string.IsNullOrEmpty(showcaseBeatmap.ScoreHash) && tryFetch(new ScoreInfo { Hash = showcaseBeatmap.ScoreHash }, scoreManager) is { } byHash)
                    return byHash;

                if (showcaseBeatmap.ScoreOnlineId > 0 && tryFetch(new ScoreInfo { OnlineID = showcaseBeatmap.ScoreOnlineId }, scoreManager) is { } byOnlineId)
                    return byOnlineId;

                return null;
            }
        }

        /// <summary>
        /// Query a score from the local database and verify that it actually holds replay frames.
        /// </summary>
        private static Score? tryFetch(IScoreInfo scoreInfo, ScoreManager scoreManager)
        {
            try
            {
                Score? score = scoreManager.GetScore(scoreInfo);

                if (score?.Replay.Frames.Count > 0)
                    return score;

                Logger.Log($"Showcase score lookup for \"{scoreInfo}\" yielded no replay frames.", LoggingTarget.Database);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Failed to load showcase score \"{scoreInfo}\".");
            }

            return null;
        }
    }
}
