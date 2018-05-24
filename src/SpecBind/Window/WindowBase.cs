using System;
using System.Collections.Generic;
using SpecBind.PropertyHandlers;
using System.Linq;
using SpecBind.Actions;
using System.Reflection;
using System.Linq.Expressions;
using SpecBind.Control;

namespace SpecBind.Window
{
    public abstract class WindowBase<TParent, TWindow, TControl> : IWindowControlHandler<TControl>
        where TParent : class
        where TWindow : class
        where TControl : class
    {
        private readonly Dictionary<string, IPropertyData> properties;
        private readonly TParent parent;
        private readonly TParent window;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowBase{TWindowBase, TControl}" /> class.
        /// </summary>
        /// <param name="window">The window.</param>
        protected WindowBase(TParent parent, TParent window, Type windowType, IWindow parentWindow)
        {
            this.parent = parent;
            this.window = window;
            this.WindowType = windowType;
            this.ParentWindow = parentWindow;
            this.properties = new Dictionary<string, IPropertyData>(StringComparer.InvariantCultureIgnoreCase);
            this.GetProperties();
        }

        /// <summary>
        /// Gets the type of the window.
        /// </summary>
        /// <value>
        /// The type of the window.
        /// </value>
        public virtual Type WindowType { get; private set; }

        public IWindow ParentWindow { get; set; }

        /// <summary>
        /// Gets the native window object.
        /// </summary>
        /// <typeparam name="TNativeWindow">The type of the window.</typeparam>
        /// <returns>
        /// The native window object.
        /// </returns>
        public virtual TNativeWindow GetNativeWindow<TNativeWindow>() where TNativeWindow : class
        {
            return this.window as TNativeWindow;
        }

        /// <summary>
        /// Gets the property names.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// A list of matching properties.
        /// </returns>
        public IEnumerable<string> GetPropertyNames(Func<IPropertyData, bool> filter)
        {
            return this.properties.Values.Where(filter).Select(p => p.Name);
        }

        /// <summary>
        /// Highlights this instance.
        /// </summary>
        public virtual void Highlight()
        {
        }

        /// <summary>
        /// Tries the get the control.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="propertyData">
        /// The property data.
        /// </param>
        /// <returns>
        /// <c>true</c> if the control exists; otherwise <c>false</c>.
        /// </returns>
        public bool TryGetControl(string key, out IPropertyData propertyData, Func<IPropertyData, bool> condition)
        {
            IPropertyData item;
            if (this.properties.TryGetValue(key, out item) && condition(item))
            {
                propertyData = item;
                return true;
            }

            propertyData = null;
            return false;
        }

        /// <summary>
        /// Tries the get the control.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="propertyData">
        /// The property data.
        /// </param>
        /// <returns>
        /// <c>true</c> if the control exists; otherwise <c>false</c>.
        /// </returns>
        public bool TryGetProperty(string key, out IPropertyData propertyData)
        {
            IPropertyData item;
            if (this.properties.TryGetValue(key, out item))
            {
                propertyData = item;
                return true;
            }

            propertyData = null;
            return false;
        }

        /// <summary>
        /// Waits for the window to become active based on some user content.
        /// </summary>
        public void WaitForWindowToBeActive()
        {
            var nativeWindow = this.GetNativeWindow<IActiveCheck>();
            if (nativeWindow == null)
            {
                return;
            }

            nativeWindow.WaitForActive();
        }

        /// <summary>
        /// Controls the enabled check.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns><c>true</c> if the control is enabled.</returns>
        public abstract bool ControlEnabledCheck(TControl control);

        /// <summary>
        /// Gets the control exists check function.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>
        /// True if the control exists; otherwise false.
        /// </returns>
        public abstract bool ControlExistsCheck(TControl control);

        /// <summary>
        /// Gets the control not-exists check function.
        /// Unlike ControlExistsCheck, this doesn't let the web driver wait first for the control to exist.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>
        /// True if the control doesn't exist; otherwise false.
        /// </returns>
        public abstract bool ControlNotExistsCheck(TControl control);

        /// <summary>
        /// Gets the control attribute value.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>The attribute value.</returns>
        public abstract string GetControlAttributeValue(TControl control, string attributeName);

        /// <summary>
        /// Gets the control text.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>The control's text.</returns>
        public abstract string GetControlText(TControl control);

        /// <summary>
        /// Gets the window from control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>The window control.</returns>
        public abstract IWindow GetWindowFromControl(TControl control);

        /// <summary>
        /// Clicks the control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>
        /// True if the click is successful.
        /// </returns>
        public abstract bool ClickControl(TControl control);

        /// <summary>
        /// Gets the clears method.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns>
        ///  The function used to clear the data.
        /// </returns>
        public abstract Action<TControl> GetClearMethod(Type propertyType);

        /// <summary>
        /// Gets the window fill method.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns>
        /// The function used to fill the data.
        /// </returns>
        public abstract Action<TControl, string> GetWindowFillMethod(Type propertyType);

        /// <summary>
        /// Highlights the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        public virtual void Highlight(TControl control)
        {
        }

        /// <summary>
        /// Waits for control condition to be met.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="waitCondition">The wait condition.</param>
        /// <param name="timeout">The time to wait before failing.</param>
        /// <returns><c>true</c> if the condition is met, <c>false</c> otherwise.</returns>
        public abstract bool WaitForControl(TControl control, WaitConditions waitCondition, TimeSpan? timeout);

        /// <summary>
        /// Checks to see if the current type matches the base type of the system to not reflect base properties.
        /// </summary>
        /// <param name="propertyInfo">Type of the window.</param>
        /// <returns><c>true</c> if the type is the base class, otherwise <c>false</c>.</returns>
        protected virtual bool TypeIsNotBaseClass(PropertyInfo propertyInfo)
        {
            var type = propertyInfo.DeclaringType;
            return typeof(TParent) != type && typeof(TParent).IsAssignableFrom(type);
        }

        /// <summary>
        /// Checks to see if the property type is supported.
        /// </summary>
        /// <param name="type">The type being checked.</param>
        /// <returns><c>true</c> if the type is supported, <c>false</c> otherwise.</returns>
        protected virtual bool SupportedPropertyType(Type type)
        {
            return type.IsClass;
        }

        /// <summary>
        /// Adds the control property.
        /// </summary>
        /// <param name="windowType">Type of the window.</param>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>The created control locator function.</returns>
        private static Func<IWindow, Func<TControl, bool>, bool> AddControlProperty(Type windowType, PropertyInfo propertyInfo)
        {
            Expression<Func<IWindow, TParent>> nativeWindowFunc = p => p.GetNativeWindow<TParent>();
            var windowArgument = Expression.Parameter(typeof(IWindow), "window");
            var actionFunc = Expression.Parameter(typeof(Func<TControl, bool>), "actionFunc");

            var nativeWindowVariable = Expression.Variable(typeof(TParent), "nativeWindowType");
            var propertyVariable = Expression.Variable(windowType, "windowItem");

            var methodCall = Expression.Block(
                new[] { nativeWindowVariable, propertyVariable },
                Expression.Assign(nativeWindowVariable, Expression.Invoke(nativeWindowFunc, windowArgument)),
                Expression.Assign(propertyVariable, Expression.Convert(nativeWindowVariable, windowType)),
                Expression.Invoke(actionFunc, Expression.Property(propertyVariable, propertyInfo)));

            return Expression.Lambda<Func<IWindow, Func<TControl, bool>, bool>>(methodCall, windowArgument, actionFunc).Compile();
        }

        /// <summary>
        /// Adds the control property.
        /// </summary>
        /// <param name="controlDescription">The control description.</param>
        /// <returns>The value function.</returns>
        private static Func<IWindow, Func<TControl, bool>, bool> AddValueProperty(ControlDescription controlDescription)
        {
            var windowArgument = Expression.Parameter(typeof(IWindow), "window");
            var actionFunc = Expression.Parameter(typeof(Func<TControl, bool>), "actionFunc");

            return
                Expression.Lambda<Func<IWindow, Func<TControl, bool>, bool>>(
                                    Expression.Invoke(actionFunc, Expression.Convert(Expression.Constant(controlDescription.Value, controlDescription.PropertyType), typeof(TControl))),
                                    windowArgument,
                                    actionFunc)
                          .Compile();
        }

        /// <summary>
        /// Adds the control property.
        /// </summary>
        /// <param name="windowType">Type of the window.</param>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>A tuple containing the get and set expressions.</returns>
        private static Tuple<Func<IWindow, Func<object, bool>, bool>, Action<IWindow, object>> AddWindowProperty(Type windowType, PropertyInfo propertyInfo)
        {
            Expression<Func<IWindow, TParent>> nativeWindowFunc = p => p.GetNativeWindow<TParent>();
            var windowArgument = Expression.Parameter(typeof(IWindow), "window");
            var actionFunc = Expression.Parameter(typeof(Func<object, bool>), "actionFunc");

            var nativeWindowVariable = Expression.Variable(typeof(TParent), "nativeWindowType");
            var propertyVariable = Expression.Variable(windowType, "windowItem");

            var getMethodCall = Expression.Block(
                new[] { nativeWindowVariable, propertyVariable },
                Expression.Assign(nativeWindowVariable, Expression.Invoke(nativeWindowFunc, windowArgument)),
                Expression.Assign(propertyVariable, Expression.Convert(nativeWindowVariable, windowType)),
                Expression.Invoke(actionFunc, Expression.Property(propertyVariable, propertyInfo)));

            var getExpression = Expression.Lambda<Func<IWindow, Func<object, bool>, bool>>(getMethodCall, windowArgument, actionFunc).Compile();

            Action<IWindow, object> setExpression = null;
            if (propertyInfo.CanWrite && propertyInfo.GetSetMethod() != null)
            {
                var setValue = Expression.Variable(typeof(object));
                var setMethodCall = Expression.Block(
                    new[] { nativeWindowVariable, propertyVariable },
                    Expression.Assign(nativeWindowVariable, Expression.Invoke(nativeWindowFunc, windowArgument)),
                    Expression.Assign(propertyVariable, Expression.Convert(nativeWindowVariable, windowType)),
                    Expression.Assign(
                        Expression.Property(propertyVariable, propertyInfo),
                        Expression.Convert(setValue, propertyInfo.PropertyType)));

                setExpression = Expression.Lambda<Action<IWindow, object>>(setMethodCall, windowArgument, setValue).Compile();
            }

            return new Tuple<Func<IWindow, Func<object, bool>, bool>, Action<IWindow, object>>(getExpression, setExpression);
        }

        /// <summary>
        /// Gets the window control.
        /// </summary>
        /// <returns>The window control.</returns>
        private ControlPropertyData<TControl> GetWindowControl()
        {
            //Note the word 'window' as a property representing the control
            return new ControlPropertyData<TControl>(this, "Window", this.WindowType, (p, func) => func(this.GetNativeWindow<TControl>()));
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        private void GetProperties()
        {
            const BindingFlags Flags = BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;

            var windowType = this.WindowType;

            var control = this.GetWindowControl();
            this.properties.Add(control.Name, control);

            var locatorControl = this.GetNativeWindow<IControlProvider>();
            if (locatorControl != null)
            {
                foreach (var property in locatorControl.GetControls())
                {
                    var propertyData = new ControlPropertyData<TControl>(
                                            this, property.PropertyName, property.PropertyType, AddValueProperty(property));

                    this.properties.Add(propertyData.Name, propertyData);
                }

                return;
            }

            foreach (var propertyInfo in windowType.GetProperties(Flags).Where(
                                                p => p.CanRead && (this.SupportedPropertyType(p.PropertyType)) && this.TypeIsNotBaseClass(p)))
            {
                IPropertyData propertyData;

                if (typeof(TWindow).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    IWindowControlHandler<TWindow> controlHandler = CreateNativeWindow(
                        this.window,
                        propertyInfo.PropertyType,
                        false);

                    var expressions = AddWindowProperty(windowType, propertyInfo);

                    propertyData = new WindowPropertyData<TWindow>(
                        controlHandler,
                        propertyInfo.Name,
                        propertyInfo.PropertyType,
                        expressions.Item1,
                        expressions.Item2);
                }
                else if (typeof(TControl).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    var controlHandler = AddControlProperty(windowType, propertyInfo);
                    propertyData = new ControlPropertyData<TControl>(
                        this,
                        propertyInfo.Name,
                        propertyInfo.PropertyType,
                        controlHandler);
                }
                else
                {
                    throw new NotSupportedException($"Property: {propertyInfo}");
                }

                this.properties.Add(propertyData.Name, propertyData);
            }
        }

        public void FindWindow(Type windowType, out IWindow window)
        {
            window = this.Window(windowType);
        }

        /// <summary>
        /// Gets the window instance from the application.
        /// </summary>
        /// <param name="windowType">Thye type of window.</param>
        /// <returns>The window object.</returns>
        public IWindow Window(Type windowType)
        {
            return this.CreateNativeWindow(this.window, windowType, true);
        }

        /// <summary>
        /// Creates the native window.
        /// </summary>
        /// <param name="windowType">Type of the window.</param>
        /// <param name="verifyPageValidity">if set to <c>true</c> verify the window's validity.</param>
        /// <returns>A window interface.</returns>
        protected abstract IWindowControlHandler<TWindow> CreateNativeWindow(TParent parent, Type windowType, bool verifyWindowValidity);
    }
}