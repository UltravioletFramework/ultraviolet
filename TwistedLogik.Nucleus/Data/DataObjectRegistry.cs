using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// Represents a registry of data-driven objects of a particular type.
    /// </summary>
    /// <typeparam name="T">The type of data object managed by the registry.</typeparam>
    [CLSCompliant(false)]
    public abstract partial class DataObjectRegistry<T> : IDataObjectRegistry where T : DataObject
    {
        /// <inheritdoc/>
        void IDataObjectRegistry.Register() => OnRegistered();

        /// <inheritdoc/>
        void IDataObjectRegistry.Unregister() => OnUnregistered();

        /// <inheritdoc/>
        public void SetSourceFiles(IEnumerable<String> files)
        {
            this.registrySourceFiles.Clear();
            this.registrySourceFiles.AddRange(files ?? Enumerable.Empty<String>());
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
            Contract.RequireNotEmpty(key, nameof(key));

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
            Contract.RequireNotEmpty(id, nameof(id));

            return GetObject(new Guid(id));
        }

        /// <summary>
        /// Gets the object with the specified key.
        /// </summary>
        /// <param name="key">The key of the object to retrieve.</param>
        /// <returns>The object with the specified key.</returns>
        public T GetObjectByKey(String key)
        {
            Contract.RequireNotEmpty(key, nameof(key));

            var id = ResolveObjectKey(key);
            if (id.HasValue)
            {
                return GetObject(id.Value);
            }
            return null;
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
        /// Loads the registry's keys.
        /// </summary>
        protected virtual void LoadKeysInternal()
        {
            foreach (var registrySourceFile in registrySourceFiles)
            {
                var extension = Path.GetExtension(registrySourceFile)?.ToLower();
                if (extension == ".json")
                {
                    throw new NotImplementedException();
                }
                else
                {
                    var xml = XDocument.Load(registrySourceFile).Root;
                    LoadKeysFromData(DataElementName, xml);
                }
            }
        }

        /// <summary>
        /// Loads the registry's objects.
        /// </summary>
        protected virtual void LoadObjectsInternal()
        {
            foreach (var registrySourceFile in registrySourceFiles)
            {
                var objects = Path.GetExtension(registrySourceFile)?.ToLower() == ".json" ?
                    LoadDefinitionsFromJson(registrySourceFile) :
                    LoadDefinitionsFromXml(registrySourceFile);

                foreach (var obj in objects)
                {
                    String error;
                    if (!ValidateObject(obj, out error))
                    {
                        throw new InvalidOperationException(
                            NucleusStrings.DataObjectFailedValidation.Format(typeof(T).Name, obj.GlobalID, error));
                    }
                }

                foreach (var definition in objects)
                    Register(definition);
            }
        }

        /// <summary>
        /// Validates the specified data object.
        /// </summary>
        /// <param name="dataObject">The data object to validate.</param>
        /// <param name="error">A string describing the validation error that occurred, if any.</param>
        /// <returns><see langword="true"/> if a validation error occurred; otherwise, <see langword="false"/>.</returns>
        protected virtual Boolean ValidateObject(T dataObject, out String error)
        {
            error = null;
            return true;
        }

        /// <summary>
        /// Loads a set of object definitions from the specified XML file.
        /// </summary>
        /// <param name="file">The file from which to load the data objects.</param>
        /// <returns>The collection of objects that were loaded.</returns>
        protected virtual IEnumerable<T> LoadDefinitionsFromXml(String file)
        {
            Contract.RequireNotEmpty(file, nameof(file));

            return ObjectLoader.LoadDefinitions<T>(XDocument.Load(file), DataElementName, DefaultObjectClass);
        }

        /// <summary>
        /// Loads a set of object definitions from the specified JSON file.
        /// </summary>
        /// <param name="file">The file from which to load the data objects.</param>
        /// <returns>The collection of objects that were loaded.</returns>
        protected virtual IEnumerable<T> LoadDefinitionsFromJson(String file)
        {
            Contract.RequireNotEmpty(file, nameof(file));

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
        protected void LoadKeysFromData(String element, XElement data) =>
            LoadKeysFromData(element, data, keys);

        /// <summary>
        /// Loads a set of object keys from the specified data definition file into the specified key registry.
        /// </summary>
        /// <param name="element">The name of the element from which to load keys.</param>
        /// <param name="data">The root element of the data definition file from which to load keys.</param>
        /// <param name="keys">The key registry into which to load keys.</param>
        protected void LoadKeysFromData(String element, XElement data, Dictionary<String, Guid> keys)
        {
            Contract.Require(data, nameof(data));
            Contract.Require(keys, nameof(keys));

            var items = from item in data.Elements(element)
                        let itemID = (String)item.Attribute("ID")
                        let itemKey = (String)item.Attribute("Key")
                        select new
                        {
                            ID = new Guid(itemID),
                            Key = itemKey
                        };

            foreach (var item in items)
            {
                if (item.Key == null || keys.ContainsKey(item.Key))
                    throw new InvalidOperationException(NucleusStrings.DataObjectInvalidKey.Format(item.Key ?? "(null)", item.ID));

                keys[item.Key] = item.ID;
            }
        }

        /// <summary>
        /// Loads a set of object keys from the specified data definition file into the specified key registry.
        /// </summary>
        /// <param name="element">The name of the element from which to load keys.</param>
        /// <param name="data">The root element of the data definition file from which to load keys.</param>
        protected void LoadKeysFromData(String element, JObject data) =>
            LoadKeysFromData(element, data, keys);

        /// <summary>
        /// Loads a set of object keys from the specified data definition file into the specified key registry.
        /// </summary>
        /// <param name="element">The name of the element from which to load keys.</param>
        /// <param name="data">The root element of the data definition file from which to load keys.</param>
        /// <param name="keys">The key registry into which to load keys.</param>
        protected void LoadKeysFromData(String element, JObject data, Dictionary<String, Guid> keys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds an object to the registry.
        /// </summary>
        /// <param name="obj">The object to register.</param>
        protected void Register(T obj)
        {
            Contract.Require(obj, nameof(obj));

            if (index > UInt16.MaxValue)
                throw new InvalidOperationException(NucleusStrings.DataObjectRegistryCapacityExceeded);

            if (objectsByGlobalID.ContainsKey(obj.GlobalID))
                throw new InvalidOperationException(NucleusStrings.DataObjectRegistryAlreadyContainsID.Format(obj.GlobalID));

            obj.LocalID = (UInt16)index;
            index++;

            keys[obj.Key] = obj.GlobalID;
            objectsByKey[obj.Key] = obj;
            objectsByLocalID[obj.LocalID] = obj;
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
