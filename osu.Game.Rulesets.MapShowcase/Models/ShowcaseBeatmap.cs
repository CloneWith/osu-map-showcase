// Copyright (c) ppy Pty Ltd <contact@ppy.sh> & CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Scoring;

namespace osu.Game.Rulesets.MapShowcase.Models
{
    [Serializable]
    public class ShowcaseBeatmap
    {
        // The BeatmapInfo structure itself introduces looped reference, so ignoring it.
        [JsonIgnore]
        public BeatmapInfo BeatmapInfo = new BeatmapInfo();

        public int BeatmapId;
        public int RulesetId;
        public Bindable<string> ModString = new Bindable<string>(string.Empty);
        public Bindable<string> ModIndex = new Bindable<string>(string.Empty);
        public Bindable<string> CreditUserIds = new Bindable<string>();

        [JsonIgnore]
        public BindableList<Mod> RequiredMods = new BindableList<Mod>();

        public Bindable<ShowcaseUser> Selector = new Bindable<ShowcaseUser>();
        public BindableBool IsOriginal = new BindableBool();
        public Bindable<string> DiffField = new Bindable<string>();
        public Bindable<string> BeatmapComment = new Bindable<string>();

        [JsonIgnore]
        public ScoreInfo? ShowcaseScore;

        public string BeatmapHash = string.Empty;
        public string ScoreHash = string.Empty;

        /// <summary>
        /// The online ID of the associated score.
        /// </summary>
        /// <remarks>
        /// Scores picked from the online leaderboard carry no local hash, so this is the only way to re-associate them
        /// with a local replay once it has been downloaded.
        /// </remarks>
        public long ScoreOnlineId = -1;

        /// <summary>
        /// Whether the information of the map should be shown actively in the mappool and during showcase.
        /// </summary>
        /// <remarks>Only automatically applies to TB maps for now.</remarks>
        [JsonIgnore]
        public bool IsSecretMap => ModString.Value.Equals(@"TB", StringComparison.OrdinalIgnoreCase);

        public ShowcaseBeatmap()
        {
        }

        public ShowcaseBeatmap(BeatmapInfo? beatmapInfo)
        {
            BeatmapInfo = beatmapInfo ?? new BeatmapInfo();
            BeatmapId = BeatmapInfo.OnlineID;
            BeatmapHash = BeatmapInfo.Hash;
        }

        public bool IsValid()
        {
            return BeatmapHash != string.Empty;
        }
    }
}
