using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// Represents a registry of data-driven objects of a particular type.
    /// </summary>
    /// <typeparam name="T">The type of data object managed by the registry.</typeparam>
    [CLSCompliant(false)]
    public abstract class DataObjectRegistry<T> : IDataObjectRegistry, IEnumerable<KeyValuePair<String, T>> where T : DataObject
    {
        /// <inheritdoc/>
        void IDataObjectRegistry.Register()
        {
            OnRegistered();
        }

        /// <inheritdoc/>
        void IDataObjectRegistry.Unregister()
        {
            OnUnregistered();
        }

        /// <inheritdoc/>
        public void SetSourceFiles(IEnumerable<String> files)
        {
            this.registrySourceFiles.Clear();

            if (files != null)
            {
                this.registrySourceFiles.AddRange(files);
            }
        }

        /// <inheritdoc/>
        public void Clear()
        {
            OnResetting();

            keys.Clear();

            objectsByKey.Clear();
            objectsByGlobalID.Clear();
            objectsByLocalID.Clear();

            loadedKeys = false;
            loadedObjects = false;

            index = 1;
        }

        /// <inheritdoc/>
        public void LoadKeys()
        {
            Contract.EnsureNot(loadedKeys, NucleusStrings.DataObjectRegistryAlreadyLoadedKeys);

            LoadKeysInternal();
            loadedKeys = true;
        }

        /// <inheritdoc/>
        public void LoadObjects()
        {
            Contract.Ensure(loadedKeys, NucleusStrings.DataObjectRegistryMustLoadKeys);
            Contract.EnsureNot(loadedObjects, NucleusStrings.DataObjectRegistryAlreadyLoadedObjects);

            LoadObjectsInternal();
            loadedObjects = true;
        }

        /// <inheritdoc/>
        public Guid? ResolveObjectKey(String key)
        {
            Guid id;
            if (keys.TryGetValue(key, out id))
            {
                return id;
            }
            return null;
        }

        /// <summary>
        /// Gets the object with the specified local identifier.
        /// </summary>
        /// <param name="id">The local identifier of the object to retrieve.</param>
        /// <returns>The object with the specified identifier.</returns>
        public T GetObject(UInt16 id)
        {
            T result;
            if (objectsByLocalID.TryGetValue(id, out result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Gets the object with the specified global identifier.
        /// </summary>
        /// <param name="id">The global identifier of the object to retrieve.</param>
        /// <returns>The object with the specified identifier.</returns>
        public T GetObject(Guid id)
        {
            T result;
            if (objectsByGlobalID.TryGetValue(id, out result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Gets the object with the specified global identifier.
        /// </summary>
        /// <param name="id">The global identifier of the object to retrieve.</param>
        /// <returns>The object with the specified identifier.</returns>
        public T GetObject(ResolvedDataObjectReference id)
        {
            T result;
            if (objectsByGlobalID.TryGetValue(id.Value, out result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Gets the object with the specified global identifier.
        /// </summary>
        /// <param name="id">The global identifier of the object to retrieve.</param>
        /// <returns>The object with the specified identifier.</returns>
        public T GetObject(String id)
        {
            return GetObject(new Guid(id));
        }

        /// <summary>
        /// Gets the object with the specified key.
        /// </summary>
        /// <param name="key">The key of the object to retrieve.</param>
        /// <returns>The object with the specified key.</returns>
        public T GetObjectByKey(String key)
        {
            var id = ResolveObjectKey(key);
            if (id.HasValue)
            {
                return GetObject(id.Value);
            }
            return null;
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public Dictionary<String, T>.Enumerator GetEnumerator()
        {
            return objectsByKey.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        IEnumerator<KeyValuePair<String, T>> IEnumerable<KeyValuePair<String, T>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        public abstract String ReferenceResolutionName
        {
            get;
        }

        /// <inheritdoc/>
        public abstract String DataElementName
        {
            get;
        }

        /// <summary>
        /// Gets the number of registered objects.
        /// </summary>
        public Int32 Count
        {
            get { return objectsByGlobalID.Count; }
        }

        /// <summary>
        /// Called when the registry is added to the global collection of registries.
        /// </summary>
        protected virtual void OnRegistered()
        {

        }

        /// <summary>
        /// Called when the registry is removed from the global collection of registries.
        /// </summary>
        protected virtual void OnUnregistered()
        {

        }

        /// <summary>
        /// Called when the registry is being reset.
        /// </summary>
        protected virtual void OnResetting()
        {

        }

        /// <summary>
        /// Loads a data element from the specified file.
        /// </summary>
        /// <param name="file">The file from which to load the data element.</param>
        /// <returns>The data element that was loaded.</returns>
        protected virtual DataElement LoadDataElement(String file)
        {
            return DataElement.CreateFromFile(file);
        }

        /// <summary>
        /// Loads the registry's keys.
        /// </summary>
        protected virtual void LoadKeysInternal()
        {
            foreach (var file in registrySourceFiles)
            {
                LoadKeysFromData(DataElementName, LoadDataElementFromFile(file));
            }
        }

        /// <summary>
        /// Loads the registry's objects.
        /// </summary>
        protected virtual void LoadObjectsInternal()
        {
            foreach (var file in registrySourceFiles)
            {
                var error   = String.Empty;
                var objects = Path.GetExtension(file) == "json" ? LoadDefinitionsFromJson(file) : LoadDefinitionsFromXml(file);
                foreach (var obj in objects)
                {
                    if (!ValidateObject(obj, out error))
                    {
                        throw new InvalidOperationException(
                            NucleusStrings.DataObjectFailedValidation.Format(typeof(T).Name, obj.GlobalID, error));
                    }
                }

                foreach (var definition in objects)
                {
                    Register(definition);
                }
            }
        }

        /// <summary>
        /// Validates the specified data object.
        /// </summary>
        /// <param name="dataObject">The data object to validate.</param>
        /// <param name="error">A string describing the validation error that occurred, if any.</param>
        /// <returns><c>true</c> if a validation error occurred; otherwise, <c>false</c>.</returns>
        protected virtual Boolean ValidateObject(T dataObject, out String error)
        {
            error = null;
            return true;
        }

        /// <summary>
        /// Loads a <see cref="DataElement"/> from the specified file.
        /// </summary>
        /// <param name="file">The file from which to load the data element.</param>
        /// <returns>The <see cref="DataElement"/> that was loaded from the specified file.</returns>
        protected virtual DataElement LoadDataElementFromFile(String file)
        {
            return DataElement.CreateFromFile(file);
        }

        /// <summary>
        /// Loads a set of object definitions from the specified XML file.
        /// </summary>
        /// <param name="file">The file from which to load the data objects.</param>
        /// <returns>The collection of objects that were loaded.</returns>
        protected virtual IEnumerable<T> LoadDefinitionsFromXml(String file)
        {
            return ObjectLoader.LoadDefinitions<T>(XDocument.Load(file), DataElementName, DefaultObjectClass);
        }

        /// <summary>
        /// Loads a set of object definitions from the specified JSON file.
        /// </summary>
        /// <param name="file">The file from which to load the data objects.</param>
        /// <returns>The collection of objects that were loaded.</returns>
        protected virtual IEnumerable<T> LoadDefinitionsFromJson(String file)
        {
            using (var sreader = new StreamReader(file))
            using (var jreader = new JsonTextReader(sreader))
            {
                return ObjectLoader.LoadDefinitions<T>(JObject.Load(jreader), DataElementName, DefaultObjectClass);
            }
        }

        /// <summary>
        /// Loads a set of object keys from the specified data definition file into the specified key registry.
        /// </summary>
        /// <param name="element">The name of the element from which to load keys.</param>
        /// <param name="data">The root element of the data definition file from which to load keys.</param>
        protected void LoadKeysFromData(String element, DataElement data)
        {
            LoadKeysFromData(element, data, keys);
        }

        /// <summary>
        /// Loads a set of object keys from the specified data definition file into the specified key registry.
        /// </summary>
        /// <param name="element">The name of the element from which to load keys.</param>
        /// <param name="data">The root element of the data definition file from which to load keys.</param>
        /// <param name="keys">The key registry into which to load keys.</param>
        protected void LoadKeysFromData(String element, DataElement data, Dictionary<String, Guid> keys)
        {
            Contract.Require(keys, "keys");

            var items = (from item in data.Elements(element)
                         select new
                         {
                             ID = new Guid(item.Attribute("ID").Value),
                             Key = item.Attribute("Key").Value,
                         });

            foreach (var item in items)
            {
                if (item.Key == null || keys.ContainsKey(item.Key))
                {
                    throw new InvalidOperationException(NucleusStrings.DataObjectInvalidKey.Format(item.Key ?? "(null)", item.ID));
                }
                keys[item.Key] = item.ID;
            }
        }

        /// <summary>
        /// Adds an object to the registry.
        /// </summary>
        /// <param name="obj">The object to register.</param>
        protected void Register(T obj)
        {
            Contract.Require(obj, "obj");

            if (index > UInt16.MaxValue)
                throw new InvalidOperationException(NucleusStrings.DataObjectRegistryCapacityExceeded);

            if (objectsByGlobalID.ContainsKey(obj.GlobalID))
                throw new InvalidOperationException(NucleusStrings.DataObjectRegistryAlreadyContainsID.Format(obj.GlobalID));

            obj.LocalID = (UInt16)index;
            index++;

            keys[obj.Key]                   = obj.GlobalID;
            objectsByKey[obj.Key]           = obj;
            objectsByLocalID[obj.LocalID]   = obj;
            objectsByGlobalID[obj.GlobalID] = obj;
        }

        /// <summary>
        /// Gets the default class for objects loaded by this registry.
        /// </summary>
        protected virtual Type DefaultObjectClass
        {
            get { return typeof(T); }
        }

        // The object registry's source files.
        private readonly List<String> registrySourceFiles = new List<String>();

        // The object registry.
        private readonly Dictionary<String, Guid> keys = new Dictionary<String, Guid>();
        private readonly Dictionary<String, T> objectsByKey = new Dictionary<String, T>();
        private readonly Dictionary<Guid, T> objectsByGlobalID = new Dictionary<Guid, T>();
        private readonly Dictionary<UInt16, T> objectsByLocalID = new Dictionary<UInt16, T>();
        private Int32 index = 1;

        // Registry load state.
        private Boolean loadedKeys;
        private Boolean loadedObjects;
    }
}
