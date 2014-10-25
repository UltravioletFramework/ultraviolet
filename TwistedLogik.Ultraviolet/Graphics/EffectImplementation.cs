
namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="EffectImplementation"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <returns>The instance of <see cref="EffectImplementation"/> that was created.</returns>
    public delegate EffectImplementation EffectImplementationFactory(UltravioletContext uv);

    /// <summary>
    /// Represents a shader effect's underlying implementation.
    /// </summary>
    public abstract class EffectImplementation : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectImplementation"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        protected EffectImplementation(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Gets the effect's collection of parameters.
        /// </summary>
        public abstract EffectParameterCollection Parameters
        {
            get;
        }

        /// <summary>
        /// Gets the effect's collection of techniques.
        /// </summary>
        public abstract EffectTechniqueCollection Techniques
        {
            get;
        }

        /// <summary>
        /// Gets or sets the effect's current technique.
        /// </summary>
        public abstract EffectTechnique CurrentTechnique
        {
            get;
            set;
        }
    }
}
