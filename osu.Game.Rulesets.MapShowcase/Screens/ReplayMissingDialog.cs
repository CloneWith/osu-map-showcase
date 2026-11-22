using System;
using osu.Game.Overlays.Dialog;
using CommonStrings = osu.Game.Resources.Localisation.Web.CommonStrings;

namespace osu.Game.Rulesets.MapShowcase.Screens
{
    public partial class ReplayMissingDialog : PopupDialog
    {
        public ReplayMissingDialog(Action download, Action alwaysDownload)
        {
            HeaderText = MapShowcaseStrings.ReplayMissingTitle;
            BodyText = MapShowcaseStrings.ReplayMissingText;
            Buttons =
            [
                new PopupDialogOkButton { Text = MapShowcaseStrings.DownloadReplay, Action = download },
                new PopupDialogOkButton { Text = MapShowcaseStrings.AlwaysDownloadReplays, Action = alwaysDownload },
                new PopupDialogCancelButton { Text = CommonStrings.ButtonsCancel },
            ];
        }
    }
}
