// Copyright (c) CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Overlays;
using osu.Game.Overlays.Notifications;
using osu.Game.Rulesets.Mods;
using osu.Game.Scoring;
using osu.Game.Screens.Select;
using osu.Game.Screens.OnlinePlay;

namespace osu.Game.Rulesets.MapShowcase.Screens
{
    public partial class ShowcaseSongSelect : SongSelect, ISongSelect, ISubScreenWithTitle
    {
        public string ShortTitle => @"Item Select";

        public LocalisableString LocalisableTitle => MapShowcaseStrings.ItemSelect;

        /// <summary>
        /// Triggered when a beatmap item or score is selected.
        /// </summary>
        public event Action<SelectResult, BeatmapInfo, RulesetInfo, ScoreInfo?, IReadOnlyList<Mod>>? OnSelect;

        [Resolved]
        private INotificationOverlay? notificationOverlay { get; set; }

        private SelectResult status = SelectResult.None;

        private readonly ShowcaseBeatmap? oldItem;

        public ShowcaseSongSelect(ShowcaseBeatmap? oldItem = null)
        {
            this.oldItem = oldItem;

            Padding = new MarginPadding { Horizontal = HORIZONTAL_OVERFLOW_PADDING };
            TopPadding = Header.HEIGHT - 10;
        }

        protected override void OnStart()
        {
            updateSelectResult();

            status |= SelectResult.ScoreUpdated;

            OnSelect?.Invoke(status, Beatmap.Value.BeatmapInfo, Ruleset.Value, null, Mods.Value);
            this.Exit();
        }

        void ISongSelect.PresentScore(ScoreInfo score, ScorePresentType presentType)
        {
            if (score.BeatmapInfo == null)
                return;

            // The "Passed" property may not reflect the actual state.
            if (!score.Passed || score.Rank is ScoreRank.F)
            {
                notificationOverlay?.Post(new SimpleErrorNotification
                {
                    Text = MapShowcaseStrings.FailedScorePrompt,
                });

                return;
            }

            updateSelectResult();
            status |= SelectResult.ScoreUpdated;

            OnSelect?.Invoke(status, Beatmap.Value.BeatmapInfo, Ruleset.Value, score, Mods.Value);
            this.Exit();
        }

        private void updateSelectResult()
        {
            if (oldItem?.BeatmapHash != Beatmap.Value.BeatmapInfo.Hash)
                status |= SelectResult.BeatmapUpdated;

            if (oldItem?.RulesetId != Ruleset.Value.OnlineID)
                status |= SelectResult.RulesetUpdated;

            if (oldItem?.RequiredMods.SequenceEqual(Mods.Value) != true)
                status |= SelectResult.ModUpdated;
        }
    }

    [Flags]
    public enum SelectResult
    {
        /// <summary>
        /// No change was made to the original selection.
        /// </summary>
        None = 0,

        /// <summary>
        /// Only the selected mod has been updated.
        /// This should only happen when no score was selected.
        /// </summary>
        ModUpdated = 2 << 1,

        /// <summary>
        /// The score has been updated.
        /// </summary>
        ScoreUpdated = 2 << 2,

        /// <summary>
        /// The ruleset has been changed.
        /// </summary>
        RulesetUpdated = 2 << 3,

        /// <summary>
        /// The beatmap has been reselected.
        /// </summary>
        BeatmapUpdated = 2 << 4,
    }
}
