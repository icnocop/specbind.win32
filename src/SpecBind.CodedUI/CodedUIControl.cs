using System;
using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
using SpecBind.Control;
using SpecBind.Actions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using System.Windows.Input;
using System.Drawing;
using System.Threading;

namespace SpecBind.CodedUI
{
    public class CodedUIControl<TControl> : ControlBase<TControl, UITestControl>
        where TControl : class
    {
        public CodedUIControl(UITestControl parent, TControl control)
            : base(parent, control)
        {
        }

        /// <summary>
        /// Highlights this instance.
        /// </summary>
        public override void Highlight()
        {
            this.GetNativeControl<UITestControl>().DrawHighlight();
        }

        /// <summary>
        /// Highlights the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        public override void Highlight(UITestControl control)
        {
            control.DrawHighlight();
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
        /// Unlike ControlExistsCheck, this doesn't wait for the control to exist.
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

            var textControl = control as WinText;
            if (textControl != null)
            {
                return textControl.DisplayText;
            }

            return null;
        }

        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>The control.</returns>
        public override IControl GetControl(UITestControl control)
        {
            return new CodedUIControl<UITestControl>(this.Parent, control);
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
                        var editControl = (WinEdit)control;

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
                        var combo = (WinComboBox)control;
                        combo.SetFocus();
                        combo.SelectedItem = s;
                    };
            }

            if (typeof(WinRadioButton).IsAssignableFrom(propertyType))
            {
                return (control, s) =>
                {
                    var radioButton = (WinRadioButton)control;
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
                    var radioButton = (WinCheckBox)control;
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
    }
}