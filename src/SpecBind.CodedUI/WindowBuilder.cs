using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
using SpecBind.Application;
using SpecBind.Control;
using System;
using System.Collections.Generic;

namespace SpecBind.CodedUI
{
    public static class WindowBuilder
    {
        public static void AssignControlAttributes(UITestControl control, ControlLocatorAttribute attribute)
        {
            SetProperty(control.WindowTitles, attribute.Title);

            SetProperty(control.SearchProperties, UITestControl.PropertyNames.Name, attribute.Name, PropertyExpressionOperator.EqualTo);
            SetProperty(control.SearchProperties, UITestControl.PropertyNames.Name, attribute.NameContains, PropertyExpressionOperator.Contains);
            SetProperty(control.SearchProperties, UITestControl.PropertyNames.ClassName, attribute.ClassName, PropertyExpressionOperator.EqualTo);
            SetProperty(control.SearchProperties, UITestControl.PropertyNames.ClassName, attribute.ClassNameContains, PropertyExpressionOperator.Contains);
        }

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="propertyExpressionOperator">The property expression operator.</param>
        private static void SetProperty(PropertyExpressionCollection collection, string key, string value, PropertyExpressionOperator propertyExpressionOperator)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                collection.Add(new PropertyExpression(key, value, propertyExpressionOperator));
            }
        }

        private static void SetProperty<T>(Func<bool> conditionFunc, ICollection<T> collection, T value)
        {
            if (!(value == null) && (conditionFunc == null || conditionFunc()))
            {
                collection.Add(value);
            }
        }

        private static void SetProperty<T>(ICollection<T> collection, T value)
        {
            if (!(value == null))
            {
                collection.Add(value);
            }
        }

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <param name="conditionFunc">The condition function.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        private static void SetProperty(Func<bool> conditionFunc, PropertyExpressionCollection collection, string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && (conditionFunc == null || conditionFunc()))
            {
                collection[key] = value;
            }
        }
    }

    public class WindowBuilder<TApplication, TOutput> : WindowBuilderBase<TApplication, UITestControl, TOutput, WinControl>
        where TOutput : WinControl
    {
        /// <summary>
        /// Creates the window.
        /// </summary>
        /// <param name="controlType">Type of the control.</param>
        /// <returns>The window builder function.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if the constructor is invalid.</exception>
        public static Func<UITestControl, TApplication, Action<WinControl>, TOutput> CreateControl(Type controlType)
        {
            var builder = new WindowBuilder<TApplication, TOutput>();
            return builder.CreateControlInternal(controlType);
        }

        protected override void AssignControlAttributes(WinControl control, ControlLocatorAttribute attribute, object[] nativeAttributes)
        {
            WindowBuilder.AssignControlAttributes(control, attribute);
        }
    }
}
