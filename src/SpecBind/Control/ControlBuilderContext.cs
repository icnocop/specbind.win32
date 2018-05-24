namespace SpecBind.Control
{
    public class ControlBuilderContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlBuilderContext" /> class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="parentElement">The parent element.</param>
        /// <param name="window">The window.</param>
        public ControlBuilderContext(ExpressionData application, ExpressionData parentElement, ExpressionData window)
        {
            this.Application = application;
            this.Window = window;
            this.ParentControl = parentElement;
        }

        /// <summary>
        /// Gets the application expression data.
        /// </summary>
        /// <value>The application expression data.</value>
        public ExpressionData Application { get; private set; }

        /// <summary>
        /// Gets the window expression data.
        /// </summary>
        /// <value>The window expression data.</value>
        public ExpressionData Window { get; private set; }

        /// <summary>
        /// Gets the root locator expression data.
        /// </summary>
        /// <value>The root locator expression data.</value>
        public ExpressionData RootLocator { get; private set; }

        /// <summary>
        /// Gets the parent control expression data.
        /// </summary>
        /// <value>The parent control expression data.</value>
        public ExpressionData ParentControl { get; private set; }

        /// <summary>
        /// Gets or sets the current property element being built.
        /// </summary>
        /// <value>The current property element.</value>
        public ExpressionData CurrentControl { get; set; }

        /// <summary>
        /// Creates the child context.
        /// </summary>
        /// <param name="childContext">The new child context element.</param>
        /// <returns>The created child context.</returns>
        public ControlBuilderContext CreateChildContext(ExpressionData childContext)
        {
            return new ControlBuilderContext(this.Application, this.Window, childContext)
            {
                CurrentControl = null,
                RootLocator = this.RootLocator ?? this.ParentControl
            };
        }
    }
}
