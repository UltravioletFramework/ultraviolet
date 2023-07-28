using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Ultraviolet.Core;
using Ultraviolet.Core.Xml;

namespace Ultraviolet.Input
{
    /// <summary>
    /// Represents a collection of named input actions.
    /// </summary>
    public abstract partial class InputActionCollection : UltravioletResource, IEnumerable<KeyValuePair<String, InputAction>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputActionCollection"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        protected InputActionCollection(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates an <see cref="UltravioletSingleton{T}"/> which encapsulates an instance of the specified input action collection type.
        /// </summary>
        /// <typeparam name="T">The type of input action collection for which to create a singleton.</typeparam>
        /// <returns>An <see cref="UltravioletSingleton{T}"/> which encapsulates an instance of the specified input action collection type.</returns>
        public static UltravioletSingleton<T> CreateSingleton<T>() where T : InputActionCollection
        {
            var ctor = typeof(T).GetConstructor(new[] { typeof(UltravioletContext) });
            if (ctor == null)
                throw new InvalidOperationException(UltravioletStrings.NoValidConstructor.Format(typeof(T).Name));

            return new UltravioletSingleton<T>(uv =>
            {
                var input = uv.GetInput();
                if (input.Disposed)
                    return default(T);

                var instance = (T)ctor.Invoke(new object[] { uv });
                instance.CreateActions();
                input.Updating += (s, t) => 
                { 
                    instance.Update(); 
                };
                return instance;
            });
        }

        /// <summary>
        /// Removes any bindings which conflict with the specified binding.
        /// </summary>
        /// <param name="binding">The input binding for which to unbind conflicts.</param>
        /// <param name="predicate">A predicate specifying which input actions to unbind.If <see langword="null"/>, 
        /// all potential conflicts are unbound.</param>
        /// <returns>A collection of input actions which were affected by this operation.</returns>
        public IEnumerable<KeyValuePair<String, InputAction>> UnbindConflicts(InputBinding binding, Predicate<InputAction> predicate = null)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var conflicts = actions
                .Where(x =>
                    x.Value.Primary != null && x.Value.Primary.UsesSameButtons(binding) ||
                    x.Value.Secondary != null && x.Value.Secondary.UsesSameButtons(binding))
                .Where(x =>
                    predicate == null || predicate(x.Value))
                .ToList();

            foreach (var conflict in conflicts)
            {
                if (conflict.Value.Primary != null && conflict.Value.Primary.UsesSameButtons(binding))
                    conflict.Value.Primary = null;

                if (conflict.Value.Secondary != null && conflict.Value.Secondary.UsesSameButtons(binding))
                    conflict.Value.Secondary = null;
            }

            return conflicts;
        }

        /// <summary>
        /// Updates the collection's input bindings.
        /// </summary>
        public void Update()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            foreach (var group in groups)
                group.Update();

            foreach (var action in actions)
                action.Value.Update();
        }

        /// <summary>
        /// Saves the collection's input actions to the specified path.
        /// </summary>
        /// <param name="path">The path to the file to save.</param>
        public void Save(String path)
        {
            Contract.RequireNotEmpty(path, nameof(path));
            Contract.EnsureNotDisposed(this, Disposed);

            SerializeToXml().Save(path);
        }

        /// <summary>
        /// Saves the collection's input actions to the specified stream.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to which to save the input actions.</param>
        public void Save(Stream stream)
        {
            Contract.Require(stream, nameof(stream));
            Contract.EnsureNotDisposed(this, Disposed);

            SerializeToXml().Save(stream);
        }

        /// <summary>
        /// Loads the collection's input actions from the specified path.
        /// </summary>
        /// <param name="path">The path to the file to load.</param>
        /// <param name="throwIfNotFound">A value indicating whether to throw an exception if the specified file is not found.</param>
        public void Load(String path, Boolean throwIfNotFound = true)
        {
            Contract.RequireNotEmpty(path, nameof(path));
            Contract.EnsureNotDisposed(this, Disposed);

            try
            {
                var xml = XDocument.Load(path);
                DeserializeFromXml(xml);
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException || e is DirectoryNotFoundException)
                {
                    if (!throwIfNotFound)
                    {
                        return;
                    }
                }
                throw;
            }
        }

        /// <summary>
        /// Loads the collection's input actions from the specified stream.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> from which to load the input actions.</param>
        public void Load(Stream stream)
        {
            Contract.Require(stream, nameof(stream));

            var xml = XDocument.Load(stream);
            DeserializeFromXml(xml);
        }

        /// <summary>
        /// Creates the collection's action set.
        /// </summary>
        public void CreateActions()
        {
            Contract.EnsureNot(actionsCreated, UltravioletStrings.InputActionCollectionAlreadyCreated);

            OnCreatingActions();
            OnResetting();

            actionsCreated = true;
        }

        /// <summary>
        /// Resets the collection to its default state.
        /// </summary>
        public void ResetToDefaults()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            foreach (var kvp in this)
            {
                kvp.Value.Primary = null;
                kvp.Value.Secondary = null;
            }
            OnResetting();
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
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        IEnumerator<KeyValuePair<String, InputAction>> IEnumerable<KeyValuePair<String, InputAction>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public Dictionary<String, InputAction>.Enumerator GetEnumerator()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return actions.GetEnumerator();
        }

        /// <summary>
        /// Registers an input binding with the collection.
        /// </summary>
        /// <param name="binding">The input binding to register.</param>
        internal void RegisterBinding(InputBinding binding)
        {
            Contract.Require(binding, nameof(binding));
            Contract.EnsureNotDisposed(this, Disposed);

            foreach (var g in groups)
            {
                if (g.Add(binding))
                    return;
            }

            var group = new InputBindingGroup();
            group.Add(binding);
            groups.Add(group);
        }

        /// <summary>
        /// Unregisters an input binding from the collection.
        /// </summary>
        /// <param name="binding">The input binding to unregister.</param>
        internal void UnregisterBinding(InputBinding binding)
        {
            Contract.Require(binding, nameof(binding));
            Contract.EnsureNotDisposed(this, Disposed);

            foreach (var g in groups)
            {
                if (g.Remove(binding))
                {
                    if (g.Empty)
                    {
                        groups.Remove(g);
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// Called when the collection is creating its actions.
        /// </summary>
        protected virtual void OnCreatingActions()
        {

        }

        /// <summary>
        /// Called when the collection is being reset to its default values.
        /// </summary>
        protected virtual void OnResetting()
        {

        }

        /// <summary>
        /// Called when a set of input bindings is about to be loaded.
        /// </summary>
        protected virtual void OnLoading()
        {

        }

        /// <summary>
        /// Called after a set of input bindings has been loaded.
        /// </summary>
        protected virtual void OnLoaded()
        {

        }

        /// <summary>
        /// Creates and registers a new input action.
        /// </summary>
        /// <param name="name">The unique name of the input action.</param>
        /// <returns>The input action that was created.</returns>
        protected InputAction CreateAction(String name)
        {
            Contract.RequireNotEmpty(name, nameof(name));

            if (actions.ContainsKey(name))
                throw new InvalidOperationException(UltravioletStrings.InputActionAlreadyExists.Format(name));

            var action = new InputAction(this);
            actions[name] = action;
            return action;
        }

        /// <summary>
        /// Creates a new keyboard binding.
        /// </summary>
        /// <param name="key">The binding's primary key.</param>
        /// <param name="control">A value indicating whether the binding requires the Control modifier.</param>
        /// <param name="alt">A value indicating whether the binding requires the Alt modifier.</param>
        /// <param name="shift">A value indicating whether the binding requires the Shift modifier.</param>
        /// <returns>The binding that was created.</returns>
        protected InputBinding CreateKeyboardBinding(Key key, Boolean control = false, Boolean alt = false, Boolean shift = false)
        {
            return Ultraviolet.GetInput().IsKeyboardSupported() ? new KeyboardInputBinding(Ultraviolet, key, control, alt, shift) : null;
        }

        /// <summary>
        /// Creates a new mouse binding.
        /// </summary>
        /// <param name="button">The binding's primary button.</param>
        /// <param name="control">A value indicating whether the binding requires the Control modifier.</param>
        /// <param name="alt">A value indicating whether the binding requires the Alt modifier.</param>
        /// <param name="shift">A value indicating whether the binding requires the Shift modifier.</param>
        /// <returns>The binding that was created.</returns>
        protected InputBinding CreateMouseBinding(MouseButton button, Boolean control = false, Boolean alt = false, Boolean shift = false)
        {
            return Ultraviolet.GetInput().IsMouseSupported() ? new MouseInputBinding(Ultraviolet, button, control, alt, shift) : null;
        }

        /// <summary>
        /// Creates a new game pad binding.
        /// </summary>
        /// <param name="playerIndex">The index of the player for which to create the binding.</param>
        /// <param name="button">The binding's primary button.</param>
        /// <returns>The binding that was created.</returns>
        protected InputBinding CreateGamePadBinding(Int32 playerIndex, GamePadButton button)
        {
            return Ultraviolet.GetInput().IsGamePadSupported() ? new GamePadInputBinding(Ultraviolet, playerIndex, button) : null;
        }

        /// <summary>
        /// Serializes the input actions into an XML document.
        /// </summary>
        /// <returns>An XML document that represents the input actions.</returns>
        private XDocument SerializeToXml()
        {
            return new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), new XElement("Actions", new XAttribute("Version", "2.0"),
                from a in actions
                select new XElement("Action", new XAttribute("Name", a.Key),
                    a.Value.Primary == null ? null : a.Value.Primary.ToXml("Primary"),
                    a.Value.Secondary == null ? null : a.Value.Secondary.ToXml("Secondary"))));
        }

        /// <summary>
        /// Deserializes the input actions from the specified XML file.
        /// </summary>
        /// <param name="xml">The XML file from which to deserialize the input actions.</param>
        private void DeserializeFromXml(XDocument xml)
        {
            OnLoading();

            var version = Version.Parse((String)xml.Root.Attribute("Version") ?? "1.0");

            foreach (var kvp in this)
            {
                var element = xml.Root.Elements("Action").Where(x => x.AttributeValueString("Name") == kvp.Key).SingleOrDefault();
                if (element != null)
                {
                    var primary = (element == null) ? null : element.Element("Primary");
                    var secondary = (element == null) ? null : element.Element("Secondary");

                    kvp.Value.Primary = CreateBindingFromXml(primary, version);
                    kvp.Value.Secondary = CreateBindingFromXml(secondary, version);
                }
            }

            OnLoaded();
        }

        /// <summary>
        /// Creates an input binding from the specified XML element.
        /// </summary>
        /// <param name="element">The XML element that represents the input binding to create.</param>
        /// <param name="version">The version of the file that is being loaded.</param>
        /// <returns>The input binding that was created.</returns>
        private InputBinding CreateBindingFromXml(XElement element, Version version)
        {
            if (element == null)
                return null;

            var typeName = element.AttributeValueString("Type");
            if (String.IsNullOrEmpty(typeName))
                throw new InvalidOperationException();

            // For legacy compatibility, map from old namespaces to new namespaves.
            if (version.Major < 2)
                typeName = typeName.Substring("TwistedLogik.".Length);

            var type = Type.GetType(typeName);
            var ctor = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(UltravioletContext), typeof(XElement) }, null);
            if (ctor == null)
                throw new InvalidOperationException(UltravioletStrings.NoValidConstructor.Format(typeName));

            return (InputBinding)ctor.Invoke(new object[] { Ultraviolet, element });
        }

        // The list of registered input bindings.
        private readonly Dictionary<String, InputAction> actions = new Dictionary<String, InputAction>();
        private readonly List<InputBindingGroup> groups = new List<InputBindingGroup>();

        // State values.
        private Boolean actionsCreated;
    }
}
