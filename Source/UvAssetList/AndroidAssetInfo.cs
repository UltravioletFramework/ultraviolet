using System;

namespace UvAssetList
{
    /// <summary>
    /// Represents the metadata for an Android Asset which is included in a cached asset list.
    /// </summary>
    public struct AndroidAssetInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidAssetInfo"/> structure.
        /// </summary>
        /// <param name="name">The asset's name.</param>
        /// <param name="file">A value indicating whether the asset is a file.</param>
        public AndroidAssetInfo(String name, Boolean file)
        {
            this.Name = name;
            this.IsFile = file;
        }

        /// <summary>
        /// Gets the asset's name.
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Gets a value indicating whether the asset is a file.
        /// </summary>
        public Boolean IsFile { get; }

        /// <summary>
        /// Gets a value indicating whether the asset is a directory.
        /// </summary>
        public Boolean IsDirectory => !IsFile;
    }
}
