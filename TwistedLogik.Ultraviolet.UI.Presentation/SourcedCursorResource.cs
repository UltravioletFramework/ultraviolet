using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the underlying cursor resource of a <see cref="SourcedCursor"/> structure.
    /// </summary>
    public sealed class SourcedCursorResource
    {
        /// <summary>
        /// Initializes the <see cref="SourcedCursorResource"/> type.
        /// </summary>
        static SourcedCursorResource()
        {
            ObjectResolver.RegisterValueResolver<SourcedCursorResource>(SourcedCursorResolver);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SourcedCursorResource"/> class.
        /// </summary>
        /// <param name="cursorCollectionID">The asset identifier of the cursor collection that contains the cursor.</param>
        /// <param name="cursorName">The name of the cursor within its cursor collection.</param>
        public SourcedCursorResource(AssetID cursorCollectionID, String cursorName)
        {
            this.cursorCollectionID = cursorCollectionID;
            this.cursorName = cursorName;
        }

        /// <summary>
        /// Loads the image's texture resource from the specified content manager.
        /// </summary>
        /// <param name="content">The content manager with which to load the image's texture resource.</param>
        public void Load(ContentManager content)
        {
            Contract.Require(content, "content");

            if (!cursorCollectionID.IsValid || String.IsNullOrEmpty(cursorName))
                return;

            cursorCollection = content.Load<CursorCollection>(CursorCollectionID);
            cursor = cursorCollection[cursorName];
        }

        /// <summary>
        /// Gets or sets the asset identifier of the cursor collection which contains the cursor resource.
        /// </summary>
        public AssetID CursorCollectionID
        {
            get { return cursorCollectionID; }
            set
            {
                if (!cursorCollectionID.Equals(value))
                {
                    cursorCollectionID = value;
                    cursorCollection = null;
                    cursor = null;
                }
            }
        }

        /// <summary>
        /// Gets the name of the cursor resource.
        /// </summary>
        public String CursorName
        {
            get { return cursorName; }
        }

        /// <summary>
        /// Gets the cursor collection resource.
        /// </summary>
        public CursorCollection CursorCollection
        {
            get { return cursorCollection; }
        }

        /// <summary>
        /// Gets the cursor resource.
        /// </summary>
        public Cursor Cursor
        {
            get { return cursor; }
        }

        /// <summary>
        /// Gets a value indicating whether this object represents a valid image.
        /// </summary>
        public Boolean IsValid
        {
            get { return cursorCollectionID.IsValid; }
        }

        /// <summary>
        /// Gets a value indicating whether the image's texture resource has been loaded.
        /// </summary>
        public Boolean IsLoaded
        {
            get { return cursor != null; }
        }

        /// <summary>
        /// Resolves a string into an instance of the <see cref="SourcedCursorResource"/> class.
        /// </summary>
        /// <param name="value">The string value to resolve.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>The resolved object.</returns>
        private static Object SourcedCursorResolver(String value, IFormatProvider provider)
        {
            var components = value.Trim().Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (components.Length != 2)
                throw new FormatException();
            
            var cursorCollection = components[0];
            var cursorCollectionAssetID = AssetID.Parse(cursorCollection);
            var cursorName = components[1];

            return new SourcedCursorResource(cursorCollectionAssetID, cursorName);
        }

        // Property values.
        private AssetID cursorCollectionID;
        private String cursorName;
        private CursorCollection cursorCollection;
        private Cursor cursor;
    }
}
