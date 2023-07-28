using System.Collections.Generic;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents an effect pass' collection of shader programs.
    /// </summary>
    public sealed class OpenGLShaderProgramCollection : UltravioletCollection<OpenGLShaderProgram>
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLShaderProgramCollection class.
        /// </summary>
        public OpenGLShaderProgramCollection()
        {

        }

        /// <summary>
        /// Initializes a new instance of the OpenGLShaderProgramCollection class.
        /// </summary>
        /// <param name="programs">The collection whose elements are copied to this collection.</param>
        public OpenGLShaderProgramCollection(IEnumerable<OpenGLShaderProgram> programs)
        {
            foreach (var program in programs)
            {
                AddInternal(program);
            }
        }
    }
}
