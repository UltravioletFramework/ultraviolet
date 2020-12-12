using System;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet.Graphics;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL implementation of EffectImplementation.
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
            IEnumerable<OpenGLEffectTechnique> techniques, HashSet<String> parameters = null)
            : base(uv)
        {
            Contract.RequireNotEmpty(techniques, nameof(techniques));

            if (parameters == null)
                parameters = new HashSet<String>();

            var cameraHints = new Dictionary<String, String>();

            foreach (var technique in techniques)
            {
                technique.EffectImplementation = this;
                foreach (var pass in technique.Passes)
                {
                    var glpass = (OpenGLEffectPass)pass;
                    glpass.EffectImplementation = this;

                    foreach (var program in glpass.Programs)
                    {
                        var vertShader = program.VertexShader;
                        if (vertShader != null)
                        {
                            foreach (var hint in vertShader.ShaderSourceMetadata.ParameterHints)
                                parameters.Add(hint);

                            foreach (var hint in vertShader.ShaderSourceMetadata.CameraHints)
                                cameraHints[hint.Key] = hint.Value;
                        }

                        var fragShader = program.FragmentShader;
                        if (fragShader != null)
                        {
                            foreach (var hint in fragShader.ShaderSourceMetadata.ParameterHints)
                                parameters.Add(hint);

                            foreach (var hint in fragShader.ShaderSourceMetadata.CameraHints)
                                cameraHints[hint.Key] = hint.Value;
                        }
                    }
                }
            }

            this.techniques = new OpenGLEffectTechniqueCollection(techniques);
            this.currentTechnique = this.techniques.First();

            this.parameters = CreateEffectParameters(techniques, parameters, cameraHints);
        }

        /// <summary>
        /// Gets the effect's collection of parameters.
        /// </summary>
        public override EffectParameterCollection Parameters => parameters;

        /// <summary>
        /// Gets the effect's collection of techniques.
        /// </summary>
        public override EffectTechniqueCollection Techniques => techniques;

        /// <summary>
        /// Gets the effect's current technique.
        /// </summary>
        public override EffectTechnique CurrentTechnique
        {
            get => this.currentTechnique;
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
        /// Calls the <see cref="Effect.OnApply()"/> method for the implementation's owning effect.
        /// </summary>
        protected internal void OnApply()
        {
            OnApplyInternal();
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
        private OpenGLEffectParameterCollection CreateEffectParameters(
            IEnumerable<OpenGLEffectTechnique> techniques, HashSet<String> parameters, Dictionary<String, String> cameraHints)
        {
            var paramlist = new Dictionary<String, OpenGLEffectParameter>();

            var uniforms1 =
                from tech in techniques
                from pass in tech.Passes
                from prog in ((OpenGLEffectPass)pass).Programs
                from unif in prog.Uniforms
                select unif.Name;

            var uniforms =
                from tech in techniques
                from pass in tech.Passes
                from prog in ((OpenGLEffectPass)pass).Programs
                from unif in prog.Uniforms
                let name = unif.Name
                let nameSanitized = name.EndsWith("[0]") ? name.Substring(0, name.Length - "[0]".Length) : name
                where parameters == null || parameters.Count == 0 || parameters.Contains(nameSanitized)
                group unif by nameSanitized into g
                select g;

            var mismatches = uniforms.Where(x => x.Select(y => y.Type).Distinct().Count() > 1);
            if (mismatches.Any())
                throw new InvalidOperationException(OpenGLStrings.EffectUniformTypeMismatch.Format(mismatches.First().Key));

            foreach (var kvp in uniforms)
            {
                var uniformName = kvp.Key;
                var uniformData = kvp.OrderByDescending(x => x.SizeInBytes).First();

                var type = uniformData.Type;
                var sizeInBytes = uniformData.SizeInBytes;
                var parameter = new OpenGLEffectParameter(Ultraviolet, uniformName, type, sizeInBytes);
                paramlist[uniformName] = parameter;

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

            return new OpenGLEffectParameterCollection(paramlist.Values, cameraHints);
        }

        // Property values.
        private readonly OpenGLEffectParameterCollection parameters;
        private readonly OpenGLEffectTechniqueCollection techniques;
        private EffectTechnique currentTechnique;
    }
}
