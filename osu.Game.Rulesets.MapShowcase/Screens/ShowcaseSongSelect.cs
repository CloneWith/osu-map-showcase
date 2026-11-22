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
using osu.Game.Rulesets.MapShowcase.Configuration;
using osu.Game.Rulesets.Mods;
using osu.Game.Scoring;
using osu.Game.Online;
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

        [Resolved]
        private IDialogOverlay? dialogOverlay { get; set; }

        [Resolved]
        private ScoreManager scoreManager { get; set; } = null!;

        [Resolved]
        private ScoreModelDownloader scoreDownloader { get; set; } = null!;

        [Resolved]
        private IRulesetConfigCache rulesetConfigCache { get; set; } = null!;

        [Resolved]
        private BeatmapManager beatmapManager { get; set; } = null!;

        [Resolved]
        private RulesetStore rulesetStore { get; set; } = null!;

        private SelectResult status = SelectResult.None;

        private readonly ShowcaseBeatmap? oldItem;
        private readonly BeatmapInfo? initialBeatmap;

        public ShowcaseSongSelect(ShowcaseBeatmap? oldItem = null, BeatmapInfo? initialBeatmap = null)
        {
            this.oldItem = oldItem;
            this.initialBeatmap = initialBeatmap;

            Padding = new MarginPadding { Horizontal = HORIZONTAL_OVERFLOW_PADDING };
            TopPadding = Header.HEIGHT - 10;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            if (initialBeatmap == null || oldItem == null)
                return;

            var ruleset = rulesetStore.GetRuleset(oldItem.RulesetId);
            if (ruleset != null)
                ((IHandlePresentBeatmap)this).PresentBeatmap(beatmapManager.GetWorkingBeatmap(initialBeatmap), ruleset);
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

            var config = rulesetConfigCache.GetConfigFor(new MapShowcaseRuleset()) as MapShowcaseConfigManager;

            if (score.HasOnlineReplay && scoreManager.GetScore(score)?.Replay.Frames.Count is not > 0)
            {
                void beginDownload()
                {
                    var tracker = new ScoreDownloadTracker(score);
                    AddInternal(tracker);
                    tracker.State.BindValueChanged(state =>
                    {
                        if (state.NewValue != DownloadState.LocallyAvailable)
                            return;

                        tracker.RemoveAndDisposeImmediately();
                        commitScoreSelection(score);
                    });
                    scoreDownloader.Download(score);
                }

                if (config?.GetBindable<bool>(MapShowcaseSetting.AutomaticallyDownloadReplays).Value == true)
                    beginDownload();
                else
                    dialogOverlay?.Push(new ReplayMissingDialog(beginDownload, () =>
                    {
                        if (config != null)
                            config.GetBindable<bool>(MapShowcaseSetting.AutomaticallyDownloadReplays).Value = true;
                        beginDownload();
                    }));

                return;
            }

            commitScoreSelection(score);
        }

        private void commitScoreSelection(ScoreInfo score)
        {
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
