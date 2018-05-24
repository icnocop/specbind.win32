using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Microsoft.VisualStudio.TestTools.UITesting;
using TechTalk.SpecFlow;

namespace SpecBind.CodedUI.IntegrationTests.Steps
{
    [Binding]
    public sealed class PlaybackOptionSteps
    {
        [Before]
        public void Before()
        {
            Playback.PlaybackSettings.AlwaysSearchControls = false; // false
            Playback.PlaybackSettings.DelayBetweenActions = 0; // 100
            Playback.PlaybackSettings.MatchExactHierarchy = true; // false
            Playback.PlaybackSettings.MaximumRetryCount = 0; // 1
            Playback.PlaybackSettings.SearchInMinimizedWindows = false; // true
            Playback.PlaybackSettings.SearchTimeout = 1; // 120000
            Playback.PlaybackSettings.ShouldSearchFailFast = true; // true
            Playback.PlaybackSettings.SkipSetPropertyVerification = true; // false
            Playback.PlaybackSettings.SmartMatchOptions = SmartMatchOptions.None; // TopLevelWindow | Control
            Playback.PlaybackSettings.ThinkTimeMultiplier = 0; // 1
            Playback.PlaybackSettings.WaitForReadyLevel = WaitForReadyLevel.Disabled; // UIThreadOnly
            Playback.PlaybackSettings.WaitForReadyTimeout = 1; // 60000
        }
    }
}
