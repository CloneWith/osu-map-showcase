// Copyright (c) ppy Pty Ltd <contact@ppy.sh> & CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.MapShowcase.Screens
{
    public partial class ShowcaseCountdownOverlay(double duration = 2000) : ShowcaseDelayedResumeOverlay(duration)
    {
        protected override void LoadComplete()
        {
            base.LoadComplete();

            // Manually change the state of the overlay.
            // Otherwise, the countdown won't be shown properly.
            Show();
            PopIn();
        }
    }
}
