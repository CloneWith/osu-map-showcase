// Copyright (c) ppy Pty Ltd <contact@ppy.sh> & CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Dialog;

namespace osu.Game.Rulesets.MapShowcase.Screens
{
    public partial class ProfileDeleteConfirmationDialog : DangerousActionDialog
    {
        public ProfileDeleteConfirmationDialog(Action? onConfirm)
        {
            HeaderText = MapShowcaseStrings.DeleteProfileDialogHeader;
            Icon = FontAwesome.Solid.Trash;

            DangerousAction = onConfirm;
        }
    }
}
