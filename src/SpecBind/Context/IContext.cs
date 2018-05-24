namespace SpecBind.Context
{
    public interface IContext
    {
		/// <summary>
		/// Determines whether the current scenario contains the specified tag.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <returns>
		///   <c>true</c> the current scenario contains the specified tag; otherwise, <c>false</c>.
		/// </returns>
		bool ContainsTag(string tag);

        /// <summary>
        /// Determines whether the current scenario's feature contains the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns><c>true</c> the current feature contains the specified tag; otherwise, <c>false</c>.</returns>
        bool FeatureContainsTag(string tag);

        /// <summary>Sets the value.</summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="key">The key.</param>
        void SetValue<T>(T value, string key);

        /// <summary>Gets the value.</summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The value if located.</returns>
        T GetValue<T>(string key);
    }
}