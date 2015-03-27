using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a collection of row definitions belonging to an instance of the <see cref="Grid"/> container class.
    /// </summary>
    public sealed partial class RowDefinitionCollection : IDefinitionBaseCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RowDefinitionCollection"/> class.
        /// </summary>
        /// <param name="grid">The <see cref="Grid"/> that owns the collection.</param>
        internal RowDefinitionCollection(Grid grid)
        {
            Contract.Require(grid, "grid");

            this.grid        = grid;
            this.implicitRow = new RowDefinition() { Grid = grid, Height = new GridLength(1, GridUnitType.Star) };
            this.implicitStorage.Add(implicitRow);
        }

        /// <summary>
        /// Clears the collection.
        /// </summary>
        public void Clear()
        {
            if (storage.Count > 0)
            {
                foreach (var item in storage)
                {
                    item.Grid = null;
                }
                storage.Clear();

                OnModified();
            }
        }

        /// <summary>
        /// Adds a row to the collection.
        /// </summary>
        /// <param name="definition">The row definition to add to the collection.</param>
        public void Add(RowDefinition definition)
        {
            Contract.Require(definition, "definition");

            if (definition.Grid != null)
                definition.Grid.RowDefinitions.Remove(definition);

            definition.Grid = grid;
            storage.Add(definition);

            OnModified();
        }

        /// <summary>
        /// Removes a row from the collection.
        /// </summary>
        /// <param name="definition">The row to remove from the collection.</param>
        /// <returns><c>true</c> if the specified row was removed from the collection; otherwise, <c>false</c>.</returns>
        public Boolean Remove(RowDefinition definition)
        {
            Contract.Require(definition, "definition");

            if (storage.Remove(definition))
            {
                definition.Grid = null;

                OnModified();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified row.
        /// </summary>
        /// <param name="definition">The row to evaluate.</param>
        /// <returns><c>true</c> if the collection contains the specified row; otherwise, <c>false</c>.</returns>
        public Boolean Contains(RowDefinition definition)
        {
            Contract.Require(definition, "definition");

            if (storage.Count == 0)
            {
                return definition == implicitRow;
            }
            return storage.Contains(definition);
        }

        /// <summary>
        /// Gets the row definition at the specified index within the collection.
        /// </summary>
        /// <param name="ix">The index of the row definition to retrieve.</param>
        /// <returns>The row definition at the specified index within the collection.</returns>
        public RowDefinition this[Int32 ix]
        {
            get 
            {
                if (storage.Count == 0)
                {
                    if (ix != 0)
                        throw new ArgumentOutOfRangeException("ix");

                    return implicitRow;
                }
                return storage[ix]; 
            }
        }

        /// <inheritdoc/>
        DefinitionBase IDefinitionBaseCollection.this[Int32 ix]
        {
            get { return this[ix]; }
        }
        
        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public Int32 Count
        {
            get 
            {
                if (storage.Count == 0)
                {
                    return 1;
                }
                return storage.Count; 
            }
        }

        /// <inheritdoc/>
        Int32 IDefinitionBaseCollection.Count
        {
            get { return this.Count; }
        }

        /// <summary>
        /// Called when the collection is modified.
        /// </summary>
        private void OnModified()
        {
            grid.OnRowsModified();
        }

        // State values.
        private readonly Grid grid;
        private readonly List<RowDefinition> storage = new List<RowDefinition>();
        private readonly List<RowDefinition> implicitStorage = new List<RowDefinition>();
        private readonly RowDefinition implicitRow;
    }
}
