using SpecBind.ActionPipeline;
using SpecBind.Configuration;
using SpecBind.Context;
using SpecBind.PropertyHandlers;
using SpecBind.Control;

namespace SpecBind.Actions
{
    /// <summary>
    /// A pre-action that highlights controls when enabled.
    /// </summary>
    public sealed class HighlightLocatorAction : ILocatorAction
    {
        /// <summary>
        /// The highlight mode tag constant.
        /// </summary>
        public const string HighlightMode = "Highlight";

        private readonly IContext context;
        private readonly ISettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightLocatorAction" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        public HighlightLocatorAction(IContext context, ISettings settings)
        {
            this.context = context;
            this.settings = settings;
        }

        /// <summary>
        /// Called when an control is about to be located.
        /// </summary>
        /// <param name="key">The control key.</param>
        public void OnLocate(string key)
        {
            // Pre-locate isn't used here
        }

        /// <summary>
        /// Called when a control is located.
        /// </summary>
        /// <param name="key">The control key.</param>
        /// <param name="item">The item if located; otherwise <c>null</c>.</param>
        public void OnLocateComplete(string key, IPropertyData item)
        {
            if (item == null || !this.HighlightModeEnabled())
            {
                return;
            }

            item.Highlight();
        }

        /// <summary>
        /// Checks to see if highlighting mode is enabled
        /// </summary>
        /// <returns><c>true</c> if the mode is enabled; otherwise <c>false</c>.</returns>
        private bool HighlightModeEnabled()
        {
            return this.settings.HighlightModeEnabled
                   || this.context.FeatureContainsTag(HighlightMode)
                   || this.context.ContainsTag(HighlightMode);
        }
    }
}
