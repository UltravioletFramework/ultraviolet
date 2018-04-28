using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a collection of column definitions belonging to an instance of the <see cref="Grid"/> container class.
    /// </summary>
    public sealed partial class ColumnDefinitionCollection : IDefinitionBaseCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnDefinitionCollection"/> class.
        /// </summary>
        /// <param name="grid">The <see cref="Grid"/> that owns the collection.</param>
        internal ColumnDefinitionCollection(Grid grid)
        {
            Contract.Require(grid, nameof(grid));

            this.grid           = grid;
            this.implicitColumn = new ColumnDefinition() { Grid = grid, Width = new GridLength(1.0, GridUnitType.Star) };
            this.implicitStorage.Add(implicitColumn);
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
        /// Adds a column to the collection.
        /// </summary>
        /// <param name="definition">The column definition to add to the collection.</param>
        public void Add(ColumnDefinition definition)
        {
            Contract.Require(definition, nameof(definition));

            if (definition.Grid != null)
                definition.Grid.ColumnDefinitions.Remove(definition);

            definition.Grid = grid;
            storage.Add(definition);

            OnModified();
        }

        /// <summary>
        /// Adds a collection of columns to the collection.
        /// </summary>
        /// <param name="definitions">The collection of columns to add to the collection.</param>
        public void AddRange(IEnumerable<ColumnDefinition> definitions)
        {
            Contract.Require(definitions, nameof(definitions));

            var modified = false;

            foreach (var definition in definitions)
            {
                modified = true;

                if (definition.Grid != null)
                    definition.Grid.ColumnDefinitions.Remove(definition);

                definition.Grid = grid;
                storage.Add(definition);
            }

            if (modified)
                OnModified();
        }

        /// <summary>
        /// Removes a column from the collection.
        /// </summary>
        /// <param name="definition">The column to remove from the collection.</param>
        /// <returns><see langword="true"/> if the specified column was removed from the collection; otherwise, <see langword="false"/>.</returns>
        public Boolean Remove(ColumnDefinition definition)
        {
            Contract.Require(definition, nameof(definition));

            if (storage.Remove(definition))
            {
                definition.Grid = null;

                OnModified();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified column.
        /// </summary>
        /// <param name="definition">The column to evaluate.</param>
        /// <returns><see langword="true"/> if the collection contains the specified column; otherwise, <see langword="false"/>.</returns>
        public Boolean Contains(ColumnDefinition definition)
        {
            Contract.Require(definition, nameof(definition));

            if (storage.Count == 0)
            {
                return definition == implicitColumn;
            }
            return storage.Contains(definition);
        }

        /// <summary>
        /// Gets the column definition at the specified index within the collection.
        /// </summary>
        /// <param name="ix">The index of the column definition to retrieve.</param>
        /// <returns>The column definition at the specified index within the collection.</returns>
        public ColumnDefinition this[Int32 ix]
        {
            get 
            {
                if (storage.Count == 0)
                {
                    if (ix != 0)
                        throw new ArgumentOutOfRangeException("ix");

                    return implicitColumn;
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
            grid.OnColumnsModified();
        }

        // State values.
        private readonly Grid grid;
        private readonly List<ColumnDefinition> storage = new List<ColumnDefinition>();
        private readonly List<ColumnDefinition> implicitStorage = new List<ColumnDefinition>();
        private readonly ColumnDefinition implicitColumn;
    }
}
