using System;
using System.Collections.Generic;
using TwistedLogik.Ultraviolet.UI.Presentation.Elements;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    partial class UIViewLoader
    {
        /// <summary>
        /// Represents a context within which the view loader instantiates new controls. This context
        /// is used primarily to influence how expressions are bound.
        /// </summary>
        private class InstantiationContext
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="InstantiationContext"/> class.
            /// </summary>
            /// <param name="viewModelType">The type of view model to which the view is bound.</param>
            public InstantiationContext(Type viewModelType)
            {
                this.viewModelType = viewModelType;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="InstantiationContext"/> class.
            /// </summary>
            /// <param name="viewModelType">The type of view model to which the view is bound.</param>
            /// <param name="userControl">The current user control.</param>
            /// <param name="initialBindingContext">The initial binding context.</param>
            public InstantiationContext(Type viewModelType, UserControl userControl, String initialBindingContext)
            {
                this.userControl   = userControl;
                this.viewModelType = viewModelType;

                if (!String.IsNullOrEmpty(initialBindingContext))
                {
                    PushBindingContext(initialBindingContext);
                }
            }

            /// <summary>
            /// Pushes a binding context expression onto the binding context stack and
            /// updates the value of the <see cref="BindingContext"/> property.
            /// </summary>
            /// <param name="bindingContext">The binding context expression to push onto the stack.</param>
            public void PushBindingContext(String bindingContext)
            {
                bindingContextStack.Push(bindingContext);
                GenerateBindingContext();
            }

            /// <summary>
            /// Pops a binding context expression off of the binding context stack
            /// and updates the value of the <see cref="BindingContext"/> property.
            /// </summary>
            public void PopBindingContext()
            {
                bindingContextStack.Pop();
                GenerateBindingContext();
            }

            /// <summary>
            /// Gets or sets the current user control. Controls which are instantiated while this property is
            /// not <c>null</c> are considered to be components of the user control and will have their events
            /// bound to the <see cref="UserControl"/> object in question, rather than the view model.
            /// </summary>
            public UserControl UserControl
            {
                get { return userControl; }
                set { userControl = value; }
            }

            /// <summary>
            /// Gets or sets the current binding context. The binding context is prepended to all binding
            /// expressions within the instantiation context.
            /// </summary>
            public String BindingContext
            {
                get { return bindingContext; }
            }

            /// <summary>
            /// Gets the binding context that was most recently pushed onto the context stack.
            /// </summary>
            public String MostRecentBindingContext
            {
                get 
                {
                    if (bindingContextStack.Count > 0)
                    {
                        return bindingContextStack.Peek();
                    }
                    return null;
                }
            }

            /// <summary>
            /// Gets the type of view model to which the view is bound.
            /// </summary>
            public Type ViewModelType
            {
                get { return viewModelType; }
            }

            /// <summary>
            /// Regenerates the value of the <see cref="BindingContext"/> property from the
            /// current binding context stack.
            /// </summary>
            private void GenerateBindingContext()
            {
                var exp = default(String);
                foreach (var context in bindingContextStack)
                {
                    exp = BindingExpressions.Combine(context, exp);
                }
                bindingContext = exp;
            }

            // Property values.
            private UserControl userControl;
            private String bindingContext;
            private Type viewModelType;

            // State values.
            private readonly Stack<String> bindingContextStack = new Stack<String>();
        }
    }
}
