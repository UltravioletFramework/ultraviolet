using System;
using System.Collections.Generic;
using System.Threading;
using Ultraviolet.Core;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents an object which can asynchronously load content assets on a background thread.
    /// </summary>
    public sealed class AsynchronousContentLoader
    {
        /// <summary>
        /// Resets the content loader.
        /// </summary>
        public void Reset()
        {
            Contract.EnsureNot(IsLoading, UltravioletStrings.ContentLoaderAlreadyLoading);

            this.IsLoaded = false;
            this.steps.Clear();
        }

        /// <summary>
        /// Adds a step to the content loader.
        /// </summary>
        /// <param name="step">The content loading step to add to the loader.</param>
        public void AddStep(Action step)
        {
            Contract.Require(step, nameof(step));
            Contract.EnsureNot(IsLoading, UltravioletStrings.ContentLoaderAlreadyLoading);

            AddStepInternal(step);
        }

        /// <summary>
        /// Adds a step to the content loader.
        /// </summary>
        /// <param name="step">The content loading step to add to the loader.</param>
        public void AddStep(Action<ContentManager> step)
        {
            Contract.Require(step, nameof(step));
            Contract.EnsureNot(IsLoading, UltravioletStrings.ContentLoaderAlreadyLoading);

            AddStepInternal(() => { step(content); });
        }

        /// <summary>
        /// Adds a step to the content loader which loads the specified content manifest
        /// into the content manager's asset cache.
        /// </summary>
        /// <param name="manifest">The content manifest to load.</param>
        public void AddStep(ContentManifest manifest)
        {
            Contract.Require(manifest, nameof(manifest));

            AddStepInternal(() => { content.Load(manifest); });
        }

        /// <summary>
        /// Adds a step which delays for the specified number of milliseconds.
        /// </summary>
        /// <param name="milliseconds">The number of milliseconds to delay.</param>
        public void AddDelay(Int32 milliseconds)
        {
            Contract.EnsureNot(IsLoading, UltravioletStrings.ContentLoaderAlreadyLoading);

            const Int32 MaxMillisecondsPerStep = 250;
            while (milliseconds > 0)
            {
                var millisecondsThisStep = Math.Min(MaxMillisecondsPerStep, milliseconds);
                AddStepInternal(() => Thread.Sleep(millisecondsThisStep));
                milliseconds -= millisecondsThisStep;
            }
        }

        /// <summary>
        /// Adds a step which performs a garbage collection.
        /// </summary>
        public void AddGarbageCollection()
        {
            Contract.EnsureNot(IsLoading, UltravioletStrings.ContentLoaderAlreadyLoading);

            AddStepInternal(() => GC.Collect());
        }

        /// <summary>
        /// Adds a step which performs a garbage collection on the specified generation.
        /// </summary>
        /// <param name="generation">The generation on which to perform a collection.</param>
        public void AddGarbageCollection(Int32 generation)
        {
            Contract.EnsureNot(IsLoading, UltravioletStrings.ContentLoaderAlreadyLoading);

            AddStepInternal(() => GC.Collect(generation));
        }

        /// <summary>
        /// Asynchronously performs all of the steps that have been added to the loader.
        /// </summary>
        /// <param name="content">The content manager with which to load content.</param>
        /// <param name="onLoaded">An action to invoke when loading is complete.</param>
        /// <param name="onFaulted">An action to invoke when the loader faults due to an exception.</param>
        public void Load(ContentManager content, Action onLoaded = null, Action<Exception> onFaulted = null)
        {
            Contract.Ensure(content != null, UltravioletStrings.NoContentManagerSpecified);
            Contract.EnsureNot(IsLoading, UltravioletStrings.ContentLoaderAlreadyLoading);

            this.IsLoading = true;
            this.IsLoaded = false;
            this.IsFaulted = false;
            this.Exception = null;

            if (steps.Count == 0 || this.IsLoaded)
            {
                this.IsLoaded = true;
                this.IsLoading = false;
                this.IsFaulted = false;
                this.Exception = null;

                onLoaded?.Invoke();
            }
            else
            {
                this.content = content;
                this.content.Ultraviolet.SpawnTask((ct) =>
                {
                    try
                    {
                        foreach (var step in this.steps)
                        {
                            step();
                            ct.ThrowIfCancellationRequested();
                        }

                        this.IsLoaded = true;
                        this.IsLoading = false;
                        this.IsFaulted = false;
                        this.content = null;

                        onLoaded?.Invoke();
                    }
                    catch (Exception e)
                    {
                        this.Exception = e;

                        this.IsLoaded = true;
                        this.IsLoading = false;
                        this.IsFaulted = true;
                        this.content = null;

                        onFaulted?.Invoke(e);
                    }
                });
            }
        }

        /// <summary>
        /// Gets the exception that caused the loader to fault, if the loader has faulted.
        /// </summary>
        public Exception Exception
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the content loader has faulted due to an exception
        /// being thrown in a background thread.
        /// </summary>
        public Boolean IsFaulted
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the content loader is in the process of loading content.
        /// </summary>
        public Boolean IsLoading
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the loader's content is completely loaded.
        /// </summary>
        public Boolean IsLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Adds a step to the content loader.
        /// </summary>
        /// <param name="step">The content loading step.</param>
        private void AddStepInternal(Action step)
        {
            steps.Add(step);
        }

        // State values.
        private readonly List<Action> steps = new List<Action>();
        private ContentManager content;
    }
}
