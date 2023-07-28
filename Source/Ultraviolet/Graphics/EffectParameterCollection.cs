using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a collection of <see cref="EffectParameter"/> instances.
    /// </summary>
    public abstract class EffectParameterCollection : UltravioletNamedCollection<EffectParameter>, IEnumerable<EffectParameter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectParameterCollection"/> class.
        /// </summary>
        /// <param name="parameters">The set of parameters to add to the collection.</param>
        /// <param name="cameraParameterHints">A dictionary which associates camera parameter hints with effect parameters.</param>
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        protected EffectParameterCollection(IEnumerable<EffectParameter> parameters, IDictionary<String, String> cameraParameterHints = null)
        {
            Contract.Require(parameters, nameof(parameters));

            foreach (var parameter in parameters)
                AddInternal(parameter);

            if (cameraParameterHints != null)
            {
                builtInCameraParameters = new EffectParameter[Enum.GetValues(typeof(CameraParameter)).Length];
                customCameraParameters = new Dictionary<String, EffectParameter>();

                foreach (var hint in cameraParameterHints)
                {
                    switch (hint.Key)
                    {
                        case nameof(CameraParameter.World):
                            builtInCameraParameters[(Int32)CameraParameter.World] = this[hint.Value];
                            break;

                        case nameof(CameraParameter.View):
                            builtInCameraParameters[(Int32)CameraParameter.View] = this[hint.Value];
                            break;

                        case nameof(CameraParameter.Projection):
                            builtInCameraParameters[(Int32)CameraParameter.Projection] = this[hint.Value];
                            break;

                        case nameof(CameraParameter.ViewProj):
                            builtInCameraParameters[(Int32)CameraParameter.ViewProj] = this[hint.Value];
                            break;

                        case nameof(CameraParameter.WorldViewProj):
                            builtInCameraParameters[(Int32)CameraParameter.WorldViewProj] = this[hint.Value];
                            break;

                        default:
                            customCameraParameters[hint.Key] = this[hint.Value];
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public new List<EffectParameter>.Enumerator GetEnumerator()
        {
            return ((UltravioletCollection<EffectParameter>)this).GetEnumerator();
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
        IEnumerator<EffectParameter> IEnumerable<EffectParameter>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Retrieves the <see cref="EffectParameter"/> which implements the specified built-in camera parameter, 
        /// assuming this effect supports that parameter.
        /// </summary>
        /// <param name="parameter">A <see cref="CameraParameter"/> value for which to retrieve a corresponding effect parameter.</param>
        /// <returns>The <see cref="EffectParameter"/> that implements the specified built-in camera parameter, 
        /// or <see langword="null"/> if the effect does not support that parameter.</returns>
        public EffectParameter GetCameraParameter(CameraParameter parameter)
        {
            return builtInCameraParameters?[(Int32)parameter];
        }

        /// <summary>
        /// Retrieves the <see cref="EffectParameter"/> which implements the specified custom camera parameter, 
        /// assuming this effect supports that parameter.
        /// </summary>
        /// <param name="parameter">A <see cref="CameraParameter"/> value for which to retrieve a corresponding effect parameter.</param>
        /// <returns>The <see cref="EffectParameter"/> that implements the specified custom camera parameter, 
        /// or <see langword="null"/> if the effect does not support that parameter.</returns>
        public EffectParameter GetCameraParameter(String parameter)
        {
            Contract.Require(parameter, nameof(parameter));

            if (customCameraParameters == null)
                return null;

            customCameraParameters.TryGetValue(parameter, out var result);
            return result;
        }

        /// <summary>
        /// Gets the specified item's name.
        /// </summary>
        /// <param name="item">The item for which to retrieve a name.</param>
        /// <returns>The specified item's name.</returns>
        protected sealed override String GetName(EffectParameter item)
        {
            return item.Name;
        }

        /// <summary>
        /// Clears the collection.
        /// </summary>
        protected sealed override void ClearInternal()
        {
            base.ClearInternal();
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">The item to add to the collection.</param>
        protected sealed override void AddInternal(EffectParameter item)
        {
            base.AddInternal(item);
        }

        /// <summary>
        /// Removes an item from the collection.
        /// </summary>
        /// <param name="item">The item to remove from the collection.</param>
        /// <returns><see langword="true"/> if the item was removed from the collection; otherwise, <see langword="false"/>.</returns>
        protected sealed override Boolean RemoveInternal(EffectParameter item)
        {
            return base.RemoveInternal(item);
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified item.
        /// </summary>
        /// <param name="item">The item to evaluate.</param>
        /// <returns><see langword="true"/> if the collection contains the specified item; otherwise, <see langword="false"/>.</returns>
        protected sealed override Boolean ContainsInternal(EffectParameter item)
        {
            return base.ContainsInternal(item);
        }

        // Camera parameters.
        private readonly EffectParameter[] builtInCameraParameters;
        private readonly Dictionary<String, EffectParameter> customCameraParameters;
    }
}
