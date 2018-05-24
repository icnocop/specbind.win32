using SpecBind.Application;
using SpecBind.Control;
using SpecBind.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SpecBind.Control
{
    public abstract class WindowBuilderBase<TApplication, TParent, TOutput, TControl>
        where TOutput : class
    {
        private readonly MethodInfo assignMethodInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowBuilderBase{TParent, TOutput, TElement}"/> class.
        /// </summary>
        protected WindowBuilderBase()
        {
            this.assignMethodInfo = new Action<TControl, ControlLocatorAttribute, object[]>(this.AssignControlAttributes).GetMethodInfo();
        }

        /// <summary>
        /// Gets a value indicating whether to allow an empty constructor for a window object.
        /// </summary>
        /// <value><c>true</c> if an empty constructor should be allowed; otherwise, <c>false</c>.</value>
        protected virtual bool AllowEmptyConstructor
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Assigns the control attributes.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="nativeAttributes">The native attributes.</param>
        protected abstract void AssignControlAttributes(TControl control, ControlLocatorAttribute attribute, object[] nativeAttributes);

        /// <summary>
        /// Assigns the window control attributes.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="locatorAttribute">The locator attribute.</param>
        protected virtual void AssignWindowControlAttributes(TOutput control, ControlLocatorAttribute locatorAttribute)
        {

        }

        /// <summary>
        /// Gets the custom driver specific attributes for a given type.
        /// </summary>
        /// <param name="propertyInfo">Type of the item.</param>
        /// <returns>The collection of custom attributes.</returns>
        protected virtual object[] GetCustomAttributes(PropertyInfo propertyInfo)
        {
            return new object[0];
        }

        /// <summary>
        /// Checks to see if the control is the same type as the base class and performs the appropriate actions.
        /// </summary>
        /// <param name="control">The control.</param>
        protected virtual void CheckWindowIsBaseClass(TOutput control)
        {
        }

        protected Func<TParent, TApplication, Action<TOutput>, TOutput> CreateControlInternal(Type controlType)
        {
            var expression = this.CreateNewItemExpression(controlType);
            return expression.Compile();
        }

        /// <summary>
        /// Creates the constructor exception to be thrown if the type cannot be created.
        /// </summary>
        /// <param name="propertyName">Name of the property being created; otherwise <c>null</c>.</param>
        /// <param name="expectedControlType">Expected type of the control.</param>
        /// <returns>The exception that will be thrown to the user.</returns>
        protected virtual Exception CreateConstructorException(string propertyName, Type expectedControlType)
        {
            string message;
            if (!string.IsNullOrEmpty(propertyName))
            {
                message =
                    string.Format(
                        "Property '{0}' of type '{1}' has an invalid constructor. Elements need to inherit the base constructor that accepts a {2} parameter.",
                        propertyName,
                        expectedControlType.Name,
                        typeof(TParent).Name);
            }
            else
            {
                message = string.Format(
                    "Constructor on type '{0}' must have a single argument of type {1}.",
                    expectedControlType.Name,
                    typeof(TParent).Name);
            }

            return new InvalidOperationException(message);
        }

        /// <summary>
        /// Gets the constructor parameter for the given type.
        /// </summary>
        /// <param name="parameterType">Type of the parameter to fill.</param>
        /// <param name="parentArgument">The parent argument.</param>
        /// <param name="rootLocator">The root locator argument if different from the parent.</param>
        /// <returns>The constructor information that matches.</returns>
        protected virtual Expression FillConstructorParameter(Type parameterType, ExpressionData parentArgument, ExpressionData rootLocator)
        {
            return typeof(TParent).IsAssignableFrom(parameterType) ? parentArgument.Expression : null;
        }

        /// <summary>
        /// Gets the type of the property proxy.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns>The property type.</returns>
        protected virtual Type GetPropertyProxyType(Type propertyType)
        {
            return propertyType;
        }

        /// <summary>
        /// Assigns the attributes for the window based on its metadata.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="customAction">The custom action.</param>
        private void AssignWindowAttributes(TOutput control, Action<TOutput> customAction)
        {
            this.CheckWindowIsBaseClass(control);

            if (customAction != null)
            {
                customAction.Invoke(control);
            }

            var controlType = control.GetType();
            ControlLocatorAttribute locatorAttribute;
            if (controlType.TryGetAttribute(out locatorAttribute))
            {
                this.AssignWindowControlAttributes(control, locatorAttribute);
            }
        }

        /// <summary>
        /// Creates the new item expression that creates the object and initial mapping.
        /// </summary>
        /// <param name="controlType">Type of the control.</param>
        /// <returns>The initial creation lambda expression.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if the constructor is invalid.</exception>
        private Expression<Func<TParent, TApplication, Action<TOutput>, TOutput>> CreateNewItemExpression(Type controlType)
        {
            var applicationParameter = Expression.Parameter(typeof(TApplication), "application");
            var applicationArgument = new ExpressionData(applicationParameter, typeof(TApplication), "application");

            var parentParameter = Expression.Parameter(typeof(TParent), "parent");
            var parentArgument = new ExpressionData(parentParameter, typeof(TParent), "rootContext");

            var controlVariable = Expression.Variable(controlType);
            var controlData = new ExpressionData(controlVariable, controlType, controlType.Name);

            var context = new ControlBuilderContext(applicationArgument, parentArgument, controlData);

            var constructor = this.GetConstructor(controlType, context);
            if (constructor == null)
            {
                throw this.CreateConstructorException(null, controlType);
            }

            var actionParameter = Expression.Parameter(typeof(Action<TOutput>), "action");

            // Spin though properties and make an initializer for anything we can set that has an attribute
            var pageMethodInfo = new Action<TOutput, Action<TOutput>>(this.AssignWindowAttributes).GetMethodInfo();
            var expressions = new List<Expression>
            {
                Expression.Assign(controlVariable, Expression.New(constructor.Item1, constructor.Item2)),
                Expression.Call(
                    Expression.Constant(this),
                    pageMethodInfo,
                    Expression.Convert(controlVariable, typeof(TOutput)),
                    actionParameter)
            };

            this.MapObjectProperties(expressions, controlType, context);
            expressions.Add(controlVariable);

            var methodCall = Expression.Block(new[] { controlVariable }, expressions);
            return Expression.Lambda<Func<TParent, TApplication, Action<TOutput>, TOutput>>(methodCall, parentParameter, applicationParameter, actionParameter);
        }

        /// <summary>
        /// Gets the constructor.
        /// </summary>
        /// <param name="itemType">Type of the item.</param>
        /// <param name="context">The window builder context.</param>
        /// <returns>The constructor information that matches.</returns>
        private Tuple<ConstructorInfo, IEnumerable<Expression>> GetConstructor(Type itemType, ControlBuilderContext context)
        {
            ConstructorInfo emptyConstructor = null;
            foreach (var constructorInfo in itemType
                .GetConstructors(BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .OrderByDescending(c => c.GetParameters().Length))
            {

                var parameters = constructorInfo.GetParameters();
                if (parameters.Length == 0)
                {
                    emptyConstructor = constructorInfo;
                    continue;
                }

                var slots = new Expression[parameters.Length];
                slots.Initialize();

                for (var i = 0; i < parameters.Length; i++)
                {
                    var parameter = parameters[i];
                    var parameterType = parameter.ParameterType;

                    if (context.Window.Type.IsAssignableFrom(parameterType))
                    {
                        slots[i] = Expression.Convert(context.Window.Expression, context.Window.Type);
                    }
                    //else if (context.Application.Type.IsAssignableFrom(parameterType))
                    //{
                    //    slots[i] = Expression.Convert(context.Application.Expression, context.Application.Type);
                    //}
                    else
                    {
                        slots[i] = this.FillConstructorParameter(parameterType, context.ParentControl, context.RootLocator);
                    }
                }

                if (slots.All(s => s != null))
                {
                    return new Tuple<ConstructorInfo, IEnumerable<Expression>>(constructorInfo, slots);
                }
            }

            return this.AllowEmptyConstructor && emptyConstructor != null
                ? new Tuple<ConstructorInfo, IEnumerable<Expression>>(emptyConstructor, new List<Expression>(0))
                : null;
        }

        /// <summary>
        /// Maps the object properties for the given object.
        /// </summary>
        /// <param name="expressions">The parent expression list.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="context">The window builder context.</param>
        private void MapObjectProperties(ICollection<Expression> expressions, Type objectType, ControlBuilderContext context)
        {
            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var propertyInfo in objectType.GetProperties().Where(p =>
                p.SetMethod != null && p.CanWrite && p.PropertyType.IsSupportedPropertyType(typeof(TControl))))
            {
                var propertyType = propertyInfo.PropertyType;
                var attribute = propertyInfo.GetCustomAttributes(typeof(ControlLocatorAttribute), false).FirstOrDefault() as ControlLocatorAttribute;
                var customAttributes = this.GetCustomAttributes(propertyInfo);
                if (attribute == null && customAttributes.Length == 0)
                {
                    continue;
                }

                // Final Properties
                var itemVariable = Expression.Variable(propertyType);
                context.CurrentControl = new ExpressionData(itemVariable, propertyType, propertyInfo.Name);

                var variableList = new List<ParameterExpression> { itemVariable };
                var propertyExpressions = new List<Expression>();

                // New up property and then check it for inner properties.
                var childContext = context.CreateChildContext(context.CurrentControl);

                propertyExpressions.AddRange(this.CreateWinControlObject(childContext, attribute, customAttributes));
                this.MapObjectProperties(propertyExpressions, propertyType, childContext);

                propertyExpressions.Add(Expression.Assign(Expression.Property(context.Window.Expression, propertyInfo), itemVariable));
                expressions.Add(Expression.Block(variableList, propertyExpressions));
            }
        }

        /// <summary>
        /// Creates the HTML object for each property that is part of the parent.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="nativeAttributes">The native attributes.</param>
        /// <returns>The expressions needed to create the list</returns>
        private IEnumerable<Expression> CreateWinControlObject(ControlBuilderContext context, ControlLocatorAttribute attribute, IEnumerable nativeAttributes)
        {
            var buildElement = context.CurrentControl ?? context.Window;
            var objectType = this.GetPropertyProxyType(buildElement.Type);

            var propConstructor = this.GetConstructor(objectType, context);
            if (propConstructor == null)
            {
                throw this.CreateConstructorException(buildElement.Name, objectType);
            }

            var itemVariable = buildElement.Expression;
            return new[]
            {
                (Expression)Expression.Assign(itemVariable, Expression.New(propConstructor.Item1, propConstructor.Item2)),
                Expression.Call(
                    Expression.Constant(this),
                    this.assignMethodInfo,
                    Expression.Convert(itemVariable, typeof(TControl)),
                    Expression.Constant(attribute, typeof(ControlLocatorAttribute)),
                    Expression.Constant(nativeAttributes, typeof(object[])))
            };
        }
    }
}
