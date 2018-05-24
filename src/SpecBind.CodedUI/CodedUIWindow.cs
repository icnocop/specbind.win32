using System;
using SpecBind.Window;
using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
using SpecBind.Actions;
using Microsoft.VisualStudio.TestTools.UITesting;
using System.Drawing;
using System.Windows.Input;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using System.Collections.Generic;

namespace SpecBind.CodedUI
{
    public class CodedUIWindow<TApplication> : WindowBase<UITestControl, WinWindow, UITestControl>
    {
        private readonly Dictionary<Type, Func<UITestControl, UITestControl, Action<WinControl>, WinWindow>> windowCache
            = new Dictionary<Type, Func<UITestControl, UITestControl, Action<WinControl>, WinWindow>>();

        private readonly UITestControl application;
        private readonly UITestControl parent;
        private readonly UITestControl window;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodedUIWindow{TWindow}"/> class.
        /// </summary>
        /// <param name="window">
        /// The window.
        /// </param>
        public CodedUIWindow(
            UITestControl parent,
            UITestControl application,
            Type windowType,
            IWindow parentWindow,
            UITestControl window)
            : base(parent, window, windowType, parentWindow)
        {
            this.parent = parent;
            this.application = application;
            this.window = window;
        }

        /// <summary>
        /// Highlights this instance.
        /// </summary>
        public override void Highlight()
        {
            try
            {
                this.window.DrawHighlight();
            }
            catch (UITestControlNotAvailableException ex)
            {
                throw new ControlNotAvailableException(ex);
            }
        }

        /// <summary>
        /// Highlights the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        public override void Highlight(UITestControl control)
        {
            try
            {
                control.DrawHighlight();
            }
            catch (UITestControlNotAvailableException ex)
            {
                throw new ControlNotAvailableException(ex);
            }
        }

        /// <summary>
        /// Waits for control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="waitCondition">The wait condition.</param>
        /// <param name="timeout">The timeout to wait before failing.</param>
        /// <returns><c>true</c> if the condition is met, <c>false</c> otherwise.</returns>
        public override bool WaitForControl(UITestControl control, WaitConditions waitCondition, TimeSpan? timeout)
        {
            var milliseconds = (int)timeout.GetValueOrDefault(TimeSpan.FromSeconds(10)).TotalMilliseconds;
            switch (waitCondition)
            {
                case WaitConditions.Exists:
                    return control.WaitForControlExist(milliseconds);
                case WaitConditions.NotExists:
                    return control.WaitForControlNotExist(milliseconds);
                case WaitConditions.Enabled:
                    return control.WaitForControlCondition(e => e.Enabled, milliseconds);
                case WaitConditions.NotEnabled:
                    return control.WaitForControlCondition(e => !e.Enabled, milliseconds);
                case WaitConditions.NotMoving:
                    control.WaitForControlExist(milliseconds);
                    return control.WaitForControlCondition(e => !this.Moving(e), milliseconds);
            }

            return true;
        }

        /// <summary>
        /// Checks to see if the control is enabled.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns><c>true</c> if the control is enabled; otherwise <c>false</c></returns>
        public override bool ControlEnabledCheck(UITestControl control)
        {
            return control.Exists && control.Enabled;
        }

        /// <summary>
        /// Checks to see if the control exists.
        /// Waits the appropriate timeout if necessary.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns><c>true</c> if the control exists; otherwise <c>false</c></returns>
        public override bool ControlExistsCheck(UITestControl control)
        {
            if (control.Exists)
            {
                return true;
            }

            control.Find();
            return true;
        }

        /// <summary>
        /// Checks to see if the control doesn't exist.
        /// Unlike ControlExistsCheck, this doesn't let the web driver wait first for the control to exist.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns><c>true</c> if the control doesn't exist; otherwise <c>false</c></returns>
        public override bool ControlNotExistsCheck(UITestControl control)
        {
            if (control == null)
            {
                return true;
            }

            return !control.Exists;
        }

        /// <summary>
        /// Gets the control attribute value.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>The control attribute value.</returns>
        public override string GetControlAttributeValue(UITestControl control, string attributeName)
        {
            var value = control.GetProperty(attributeName);
            return value != null ? value.ToString() : string.Empty;
        }

        /// <summary>
        /// Gets the control text.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>The control text.</returns>
        public override string GetControlText(UITestControl control)
        {
            var comboBoxControl = control as WinComboBox;
            if (comboBoxControl != null)
            {
                return comboBoxControl.SelectedItem;
            }

            var checkBoxControl = control as WinCheckBox;
            if (checkBoxControl != null)
            {
                return checkBoxControl.Checked ? "true" : "false";
            }

            var inputControl = control as WinEdit;
            if (inputControl != null)
            {
                return inputControl.Text;
            }

            return null;
        }

        /// <summary>
        /// Gets the window from control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>The window interface.</returns>
        public override IWindow GetWindowFromControl(UITestControl control)
        {
            return this;
        }

        /// <summary>
        /// Clicks the control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns><c>true</c> unless there is an error.</returns>
        public override bool ClickControl(UITestControl control)
        {
            this.WaitForControl(control, WaitConditions.NotMoving, timeout: null);

            Point point;
            if (control.TryGetClickablePoint(out point))
            {
                control.EnsureClickable();
            }

            Mouse.Click(control);
            return true;
        }

        /// <summary>
        /// Gets the clear method for the control.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns>
        /// The function used to clear the data.
        /// </returns>
        public override Action<UITestControl> GetClearMethod(Type propertyType)
        {
            var fillMethod = this.GetWindowFillMethod(propertyType);
            return c => fillMethod(c, string.Empty);
        }

        /// <summary>
        /// Gets the window fill method.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns>The action used to fill the window field.</returns>
        public override Action<UITestControl, string> GetWindowFillMethod(Type propertyType)
        {
            if (typeof(WinEdit).IsAssignableFrom(propertyType))
            {
                return (control, s) =>
                {
                    var editControl = control as WinEdit;

                    if (editControl.IsPassword)
                    {
                        editControl.Password = Playback.EncryptText(s);
                        return;
                    }

                    if (!string.IsNullOrEmpty(editControl.Text))
                    {
                        editControl.Text = string.Empty;
                    }

                    if (string.IsNullOrEmpty(s))
                    {
                        return;
                    }

                    try
                    {
                        Keyboard.SendKeys(control, s, ModifierKeys.None);
                    }
                    catch (PlaybackFailureException ex)
                    {
                        if (ex.Message.Contains("SendKeys"))
                        {
                            // Fallback strategy of setting the text directly if sendkeys doesn't work
                            editControl.Text = s;
                        }
                        else
                        {
                            throw;
                        }
                    }
                };
            }

            if (typeof(WinComboBox).IsAssignableFrom(propertyType))
            {
                return (control, s) =>
                {
                    var combo = control as WinComboBox;
                    combo.SetFocus();
                    combo.SelectedItem = s;
                };
            }

            if (typeof(WinRadioButton).IsAssignableFrom(propertyType))
            {
                return (control, s) =>
                {
                    var radioButton = control as WinRadioButton;
                    bool boolValue;
                    if (bool.TryParse(s, out boolValue))
                    {
                        radioButton.Selected = boolValue;
                    }
                };
            }

            if (typeof(WinCheckBox).IsAssignableFrom(propertyType))
            {
                return (control, s) =>
                {
                    var radioButton = control as WinCheckBox;
                    bool boolValue;
                    if (bool.TryParse(s, out boolValue))
                    {
                        radioButton.Checked = boolValue;
                    }
                };
            }

            if (typeof(WinCustom).IsAssignableFrom(propertyType))
            {
                return (control, s) => Keyboard.SendKeys(control, s, ModifierKeys.None);
            }

            return null;
        }

        /// <summary>
        /// Determines if an control is currently moving (e.g. due to animation).
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns><c>true</c> if the control's Location is changing, <c>false</c> otherwise.</returns>
        protected virtual bool Moving(UITestControl control)
        {
            var firstLeft = control.Left;
            var firstTop = control.Top;
            Thread.Sleep(200);
            var secondLeft = control.Left;
            var secondTop = control.Top;
            var moved = !(secondLeft.Equals(firstLeft) && secondTop.Equals(firstTop));
            return moved;
        }

        protected override IWindowControlHandler<WinWindow> CreateNativeWindow(UITestControl parent, Type windowType, bool verifyWindowValidity)
        {
            var nativeWindow = this.CreateNativeWindow(parent, windowType);

            if (verifyWindowValidity)
            {
                nativeWindow.Find();
            }

            return new CodedUIWindow<UITestControl>(parent, this.application, windowType, this, nativeWindow);
        }

        /// <summary>
        /// Creates the native window.
        /// </summary>
        /// <param name="windowType">Type of the window.</param>
        /// <returns>The window.</returns>
        private WinWindow CreateNativeWindow(UITestControl parent, Type windowType)
        {
            Func<UITestControl, UITestControl, Action<WinControl>, WinWindow> function;
            if (!this.windowCache.TryGetValue(windowType, out function))
            {
                function = WindowBuilder<UITestControl, WinWindow>.CreateControl(windowType);
                this.windowCache.Add(windowType, function);
            }

            return function(parent, this.application, null);
        }
    }
}
