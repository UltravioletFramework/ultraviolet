using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// Represents a registry of data-driven objects of a particular type.
    /// </summary>
    [CLSCompliant(false)]
    public abstract class DataObjectRegistry<T> : IDataObjectRegistry, IEnumerable<KeyValuePair<String, T>> where T : DataObject
    {
        /// <summary>
        /// Resets the object registry to its default state.
        /// </summary>
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

        /// <summary>
        /// Loads the object keys.
        /// </summary>
        public void LoadKeys()
        {
            Contract.EnsureNot(loadedKeys, NucleusStrings.DataObjectRegistryAlreadyLoadedKeys);

            LoadKeysInternal();
            loadedKeys = true;
        }

        /// <summary>
        /// Loads the registry's objects.
        /// </summary>
        public void LoadObjects()
        {
            Contract.Ensure(loadedKeys, NucleusStrings.DataObjectRegistryMustLoadKeys);
            Contract.EnsureNot(loadedObjects, NucleusStrings.DataObjectRegistryAlreadyLoadedObjects);

            LoadObjectsInternal();
            loadedObjects = true;
        }

        /// <summary>
        /// Resolves an object key to a global identifier.
        /// </summary>
        /// <param name="key">The object key to resolve.</param>
        /// <returns>The global identifier associated with the specified object key, or null if no such key exists.</returns>
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

        /// <summary>
        /// Gets the data object's reference resolution name, which is used to resolve
        /// references with the format @rrname:KEY
        /// </summary>
        public abstract String ReferenceResolutionName
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
        /// Called when the registry is being reset.
        /// </summary>
        protected virtual void OnResetting()
        {

        }

        /// <summary>
        /// Loads the registry's keys.
        /// </summary>
        protected abstract void LoadKeysInternal();

        /// <summary>
        /// Loads the registry's objects.
        /// </summary>
        protected abstract void LoadObjectsInternal();

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
        /// <param name="key">The object's unique key.</param>
        /// <param name="obj">The object to register.</param>
        protected void Register(String key, T obj)
        {
            Contract.RequireNotEmpty(key, "key");
            Contract.Require(obj, "obj");

            if (index > UInt16.MaxValue)
                throw new InvalidOperationException(NucleusStrings.DataObjectRegistryCapacityExceeded);

            if (objectsByGlobalID.ContainsKey(obj.GlobalID))
                throw new InvalidOperationException(NucleusStrings.DataObjectRegistryAlreadyContainsID.Format(obj.GlobalID));

            obj.LocalID = (UInt16)index;
            index++;

            keys[key]                       = obj.GlobalID;
            objectsByKey[key]               = obj;
            objectsByLocalID[obj.LocalID]   = obj;
            objectsByGlobalID[obj.GlobalID] = obj;
        }

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
