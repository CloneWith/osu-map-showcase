// Copyright (c) ppy Pty Ltd <contact@ppy.sh> & CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Graphics;
using osu.Game.Overlays.Dialog;
using CommonStrings = osu.Game.Resources.Localisation.Web.CommonStrings;

namespace osu.Game.Rulesets.MapShowcase.Screens
{
    public partial class ScoreMissingDialog : ConfirmDialog
    {
        public ScoreMissingDialog(int missingNumber, Action onConfirm)
            : base("", onConfirm)
        {
            HeaderText = MapShowcaseStrings.ScoreMissingDialogTitle;
            BodyText = MapShowcaseStrings.ScoreMissingDialogText(missingNumber);

            Icon = OsuIcon.Search;

            Buttons =
            [
                new PopupDialogOkButton
                {
                    Text = MapShowcaseStrings.ScoreMissingProceed,
                    Action = onConfirm,
                },
                new PopupDialogCancelButton
                {
                    Text = CommonStrings.ButtonsCancel,
                },
            ];
        }
    }
}
