using System;
using SpecBind.Validation;
using SpecBind.Window;

namespace SpecBind.PropertyHandlers
{
    internal class WindowPropertyData<TControl> : PropertyDataBase<TControl>
        where TControl : class
    {
        private readonly Func<IWindow, Func<object, bool>, bool> action;

        public WindowPropertyData(
            IWindowControlHandler<TControl> controlHandler,
            string name,
            Type propertyType,
            Func<IWindow, Func<object, bool>, bool> action,
            Action<IWindow, object> setAction)
            : base(controlHandler, name, propertyType)
        {
            this.action = action;
            this.IsWindow = true;
        }

        /// <summary>
        /// Highlights this instance.
        /// </summary>
        public override void Highlight()
        {
            this.ControlHandler.Highlight();
        }

        public override bool ValidateItem(ItemValidation validation, out string actualValue)
        {
            throw new NotImplementedException();
        }
    }
}
