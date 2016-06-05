using System;
using System.Collections.Generic;
using System.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of EffectImplementation.
    /// </summary>
    public class OpenGLEffectImplementation : EffectImplementation
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLEffect class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="techniques">The effect's techniques.</param>
        /// <param name="parameters">The effect's list of expected parameters, or <see langword="null"/> to
        /// determine the parameters by querying shader uniforms.</param>
        public OpenGLEffectImplementation(UltravioletContext uv,
            IEnumerable<OpenGLEffectTechnique> techniques, IEnumerable<String> parameters = null)
            : base(uv)
        {
            Contract.RequireNotEmpty(techniques, nameof(techniques));

            this.techniques = new OpenGLEffectTechniqueCollection(techniques);
            this.currentTechnique = this.techniques.First();

            this.parameters = CreateEffectParameters(techniques, parameters);
        }

        /// <summary>
        /// Gets the effect's collection of parameters.
        /// </summary>
        public override EffectParameterCollection Parameters
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return parameters;
            }
        }

        /// <summary>
        /// Gets the effect's collection of techniques.
        /// </summary>
        public override EffectTechniqueCollection Techniques
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return techniques;
            }
        }

        /// <summary>
        /// Gets the effect's current technique.
        /// </summary>
        public override EffectTechnique CurrentTechnique
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return this.currentTechnique;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);
                Contract.Require(value, nameof(value));

                if (!techniques.Contains(value))
                    throw new ArgumentException(OpenGLStrings.TechniqueBelongsToDifferentEffect);

                this.currentTechnique = value;
            }
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed; false if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                foreach (var technique in techniques)
                    technique.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Creates an effect parameter collection from the specified collection of techniques.
        /// </summary>
        /// <param name="techniques">The collection of techniques from which to create an effect parameter collection.</param>
        /// <param name="parameters">The collection of expected parameter names, or <see langword="null"/> to query
        /// effect parameters from shader uniforms.</param>
        /// <returns>The effect parameter collection that was created.</returns>
        private OpenGLEffectParameterCollection CreateEffectParameters(
            IEnumerable<OpenGLEffectTechnique> techniques, IEnumerable<String> parameters)
        {
            var paramlist = new Dictionary<String, OpenGLEffectParameter>();

            var uniforms =
                from tech in techniques
                from pass in tech.Passes
                from prog in ((OpenGLEffectPass)pass).Programs
                from unif in prog.Uniforms
                let name = unif.Name
                let nameSanitized = name.EndsWith("[0]") ? name.Substring(0, name.Length - "[0]".Length) : name
                where parameters == null || parameters.Contains(nameSanitized)
                group unif by nameSanitized into g
                select g;

            var mismatches = uniforms.Where(x => x.Select(y => y.Type).Distinct().Count() > 1);
            if (mismatches.Any())
                throw new InvalidOperationException(OpenGLStrings.EffectUniformTypeMismatch.Format(mismatches.First().Key));

            foreach (var kvp in uniforms)
            {
                var name = kvp.Key;
                var type = kvp.Select(x => x.Type).First();
                var parameter = new OpenGLEffectParameter(Ultraviolet, name, type);
                paramlist[name] = parameter;

                foreach (var uniform in kvp)
                {
                    uniform.SetDataSource(parameter.Data);
                }
            }

            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    if (!paramlist.ContainsKey(p))
                        throw new InvalidOperationException(OpenGLStrings.EffectParameterCannotFindUniform.Format(p));        
                }
            }

            return new OpenGLEffectParameterCollection(paramlist.Values);
        }

        // Property values.
        private readonly OpenGLEffectParameterCollection parameters;
        private readonly OpenGLEffectTechniqueCollection techniques;
        private EffectTechnique currentTechnique;
    }
}
