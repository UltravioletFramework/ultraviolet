using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the EffectPass class.
    /// </summary>
    public sealed class OpenGLEffectPass : EffectPass
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLEffectPass class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The effect pass' name.</param>
        /// <param name="programs">The effect pass' collection of shader programs.</param>
        public OpenGLEffectPass(UltravioletContext uv, String name, ICollection<OpenGLShaderProgram> programs)
            : base(uv)
        {
            Contract.RequireNotEmpty(programs, "programs");

            this.name = name ?? String.Empty;
            this.programs = new OpenGLShaderProgramCollection(programs);
        }

        /// <summary>
        /// Applies the effect pass state to the device.
        /// </summary>
        public override void Apply()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var program = programs[programIndex];
            var programID = program.OpenGLName;

            OpenGLState.UseProgram(programID);

            foreach (var uniform in program.Uniforms)
            {
                uniform.Apply();
            }
        }

        /// <summary>
        /// Gets the effect pass's name.
        /// </summary>
        public override String Name
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return name;
            }
        }

        /// <summary>
        /// Gets or sets the effect pass' current program index.
        /// </summary>
        public Int32 ProgramIndex
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return programIndex;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);
                Contract.EnsureRange(value >= 0 && value < ProgramCount, "value");

                programIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets the effect pass' program count.
        /// </summary>
        public Int32 ProgramCount
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return programs.Count;
            }
        }

        /// <summary>
        /// Gets the effect pass' collection of shader programs.
        /// </summary>
        public OpenGLShaderProgramCollection Programs
        {
            get 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return programs;
            }
        }

        // Property values.
        private readonly String name;
        private readonly OpenGLShaderProgramCollection programs;
        private Int32 programIndex;
    }
}
