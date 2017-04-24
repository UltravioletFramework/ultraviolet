using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Input
{
    public abstract partial class InputActionCollection
    {
        /// <summary>
        /// Represents a group of input bindings that use the same primary buttons.
        /// </summary>
        private class InputBindingGroup
        {
            /// <summary>
            /// Represents a comparer for sorting input bindings by priority.
            /// </summary>
            private class InputBindingComparer : IComparer<InputBinding>
            {
                /// <summary>
                /// Compares two input bindings by priority.
                /// </summary>
                /// <param name="x">The first input binding to compare.</param>
                /// <param name="y">The second input binding to compare.</param>
                /// <returns>A value representing the relative order of the compared input bindings.</returns>
                public Int32 Compare(InputBinding x, InputBinding y)
                {
                    return y.Priority.CompareTo(x.Priority);
                }
            }

            /// <summary>
            /// Adds a binding to the group.
            /// </summary>
            /// <param name="binding">The <see cref="InputBinding"/> to add to the group.</param>
            /// <returns><see langword="true"/> if the binding was added to the group; otherwise, <see langword="false"/>.</returns>
            public Boolean Add(InputBinding binding)
            {
                Contract.Require(binding, nameof(binding));

                if (bindings.Count == 0 || bindings[0].UsesSamePrimaryButtons(binding))
                {
                    bindings.Add(binding);
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Removes a binding from the group.
            /// </summary>
            /// <param name="binding">The <see cref="InputBinding"/> to remove from the group.</param>
            /// <returns><see langword="true"/> if the binding was removed from the group; otherwise, <see langword="false"/>.</returns>
            public Boolean Remove(InputBinding binding)
            {
                return bindings.Remove(binding);
            }

            /// <summary>
            /// Clears the binding group.
            /// </summary>
            public void Clear()
            {
                bindings.Clear();
            }

            /// <summary>
            /// Updates the bindings in the group.
            /// </summary>
            public void Update()
            {
                if (pressed != null)
                {
                    pressed.Update();
                    if (!pressed.IsDown())
                    {
                        pressed = null;
                    }
                }
                else
                {
                    bindings.Sort(comparer);
                    foreach (var binding in bindings)
                    {
                        binding.Update();
                        if (binding.IsPressed())
                        {
                            pressed = binding;
                            break;
                        }
                    }
                }
            }

            /// <summary>
            /// Gets a value indicating whether the group is empty.
            /// </summary>
            public Boolean Empty
            {
                get { return bindings.Count == 0; }
            }

            // The group's list of bindings.
            private readonly InputBindingComparer comparer = new InputBindingComparer();
            private readonly List<InputBinding> bindings = new List<InputBinding>();
            private InputBinding pressed;
        }
    }
}
