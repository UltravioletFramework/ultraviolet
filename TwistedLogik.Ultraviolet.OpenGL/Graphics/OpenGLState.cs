using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using TwistedLogik.Gluon;
using TwistedLogik.Nucleus.Collections;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents a collection of states to apply to the OpenGL context.
    /// </summary>
    internal sealed class OpenGLState : IDisposable
    {
        /// <summary>
        /// Represents the type of a <see cref="OpenGLState"/> object, which determines how it modifies the OpenGL context.
        /// </summary>
        private enum OpenGLStateType
        {
            None,
            BindTexture2D,
            BindVertexArrayObject,
            BindArrayBuffer,
            BindElementArrayBuffer,
            BindFramebuffer,
            UseProgram,
            CreateTexture2D,
            CreateArrayBuffer,
            CreateElementArrayBuffer,
            CreateFramebuffer,
        }

        /// <summary>
        /// Initializes the <see cref="OpenGLState"/> type.
        /// </summary>
        static OpenGLState()
        {
            glCachedIntegers = typeof(OpenGLState).GetFields(BindingFlags.NonPublic | BindingFlags.Static)
                .Where(x => x.FieldType == typeof(OpenGLStateInteger))
                .Select(x => (OpenGLStateInteger)x.GetValue(null)).ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLState"/> class.
        /// </summary>
        private OpenGLState()
        { }

        /// <summary>
        /// Resets the values that are stored in the OpenGL state cache to their defaults.
        /// </summary>
        public static void ResetCache()
        {
            foreach (var glCachedInteger in glCachedIntegers)
            {
                glCachedInteger.Reset();
            }
        }

        /// <summary>
        /// Verifies that the values that are stored in the OpenGL state cache accurately
        /// reflect the current state of the OpenGL context.
        /// </summary>
        [Conditional("VERIFY_OPENGL_CACHE")]
        public static void VerifyCache()
        {
            foreach (var value in glCachedIntegers)
            {
                value.Verify();
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which binds a 2D texture to the context.
        /// </summary>
        /// <param name="texture">The texture to bind to the context.</param>
        /// <param name="force">A value indicating whether to force-bind the texture, even if DSA is available.</param>
        public static OpenGLState ScopedBindTexture2D(UInt32 texture, Boolean force = false)
        {
            var state = pool.Retrieve();

            state.stateType                = OpenGLStateType.BindTexture2D;
            state.disposed                 = false;
            state.forced                   = force;
            state.newGL_TEXTURE_BINDING_2D = texture;

            state.Apply();

            return state;
        }

        /// <summary>
        /// Immediately binds a 2D texture to the OpenGL context and updates the state cache.
        /// </summary>
        /// <param name="texture">The texture to bind to the context.</param>
        public static void Texture2DImmediate(UInt32 texture)
        {
            gl.BindTexture(gl.GL_TEXTURE_2D, texture);
            gl.ThrowIfError();

            OpenGLState.GL_TEXTURE_BINDING_2D.Update(texture);
            OpenGLState.VerifyCache();
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which binds a vertex array object to the context.
        /// </summary>
        /// <param name="vertexArrayObject">The vertex array object to bind to the context.</param>
        /// <param name="arrayBuffer">The vertex array's associated array buffer.</param>
        /// <param name="elementArrayBuffer">The vertex array's associated element array buffer.</param>
        /// <param name="force">A value indicating whether to force-bind the vertex array object, even if DSA is available.</param>
        public static OpenGLState ScopedBindVertexArrayObject(UInt32 vertexArrayObject, UInt32 arrayBuffer, UInt32 elementArrayBuffer, Boolean force = false)
        {
            var state = pool.Retrieve();

            state.stateType                          = OpenGLStateType.BindVertexArrayObject;
            state.disposed                           = false;
            state.forced                             = force;
            state.newGL_VERTEX_ARRAY_BINDING         = vertexArrayObject;
            state.newGL_ARRAY_BUFFER_BINDING         = arrayBuffer;
            state.newGL_ELEMENT_ARRAY_BUFFER_BINDING = elementArrayBuffer;

            state.Apply();

            return state;
        }

        /// <summary>
        /// Immediately binds a vertex array object to the OpenGL context and updates the state cache.
        /// </summary>
        /// <param name="vertexArrayObject">The vertex array object to bind to the context.</param>
        /// <param name="arrayBuffer">The vertex array's associated array buffer.</param>
        /// <param name="elementArrayBuffer">The vertex array's associated element array buffer.</param>
        public static void BindVertexArrayObject(UInt32 vertexArrayObject, UInt32 arrayBuffer, UInt32 elementArrayBuffer)
        {
            gl.BindVertexArray(vertexArrayObject);
            gl.ThrowIfError();

            OpenGLState.GL_VERTEX_ARRAY_BINDING.Update(vertexArrayObject);
            OpenGLState.GL_ARRAY_BUFFER_BINDING.Update(arrayBuffer);
            OpenGLState.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(elementArrayBuffer);
            OpenGLState.VerifyCache();
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which binds an array buffer to the context.
        /// </summary>
        /// <param name="arrayBuffer">The array buffer to bind to the OpenGL context.</param>
        /// <param name="force">A value indicating whether to force-bind the array buffer, even if DSA is available.</param>
        public static OpenGLState ScopedBindArrayBuffer(UInt32 arrayBuffer, Boolean force = false)
        {
            var state = pool.Retrieve();

            state.stateType                  = OpenGLStateType.BindArrayBuffer;
            state.disposed                   = false;
            state.forced                     = force;
            state.newGL_ARRAY_BUFFER_BINDING = arrayBuffer;

            state.Apply();

            return state;
        }

        /// <summary>
        /// Immediately binds an array buffer to the OpenGL context and updates the state cache.
        /// </summary>
        /// <param name="arrayBuffer">The array buffer to bind to the OpenGL context.</param>
        public static void BindArrayBuffer(UInt32 arrayBuffer)
        {
            gl.BindBuffer(gl.GL_ARRAY_BUFFER, arrayBuffer);
            gl.ThrowIfError();

            OpenGLState.GL_ARRAY_BUFFER_BINDING.Update(arrayBuffer);
            OpenGLState.VerifyCache();
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which binds an element array buffer to the context.
        /// </summary>
        /// <param name="elementArrayBuffer">The element array buffer to bind to the OpenGL context.</param>
        /// <param name="force">A value indicating whether to force-bind the array buffer, even if DSA is available.</param>
        public static OpenGLState ScopedBindElementArrayBuffer(UInt32 elementArrayBuffer, Boolean force = false)
        {
            var state = pool.Retrieve();

            state.stateType                          = OpenGLStateType.BindElementArrayBuffer;
            state.disposed                           = false;
            state.forced                             = force;
            state.newGL_ELEMENT_ARRAY_BUFFER_BINDING = elementArrayBuffer;

            state.Apply();

            return state;
        }

        /// <summary>
        /// Immediately binds an element array buffer to the OpenGL context and updates the state cache.
        /// </summary>
        /// <param name="elementArrayBuffer">The element array buffer to bind to the OpenGL context.</param>
        public static void BindElementArrayBuffer(UInt32 elementArrayBuffer)
        {
            gl.BindBuffer(gl.GL_ELEMENT_ARRAY_BUFFER, elementArrayBuffer);
            gl.ThrowIfError();

            OpenGLState.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(elementArrayBuffer);
            OpenGLState.VerifyCache();
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which binds a framebuffer to the context.
        /// </summary>
        /// <param name="framebuffer">The framebuffer to bind to the OpenGL context.</param>
        /// <param name="force">A value indicating whether to force-bind the array buffer, even if DSA is available.</param>
        public static OpenGLState ScopedBindFramebuffer(UInt32 framebuffer, Boolean force = false)
        {
            var state = pool.Retrieve();

            state.stateType                 = OpenGLStateType.BindFramebuffer;
            state.disposed                  = false;
            state.forced                    = force;
            state.newGL_FRAMEBUFFER_BINDING = framebuffer;

            state.Apply();

            return state;
        }

        /// <summary>
        /// Immediately binds a framebuffer to the OpenGL context and updates the state cache.
        /// </summary>
        /// <param name="framebuffer">The framebuffer to bind to the OpenGL context.</param>
        public static void BindFramebuffer(UInt32 framebuffer)
        {
            gl.BindFramebuffer(gl.GL_FRAMEBUFFER, framebuffer);
            gl.ThrowIfError();

            OpenGLState.GL_FRAMEBUFFER_BINDING.Update(framebuffer);
            OpenGLState.VerifyCache();
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which activates a shader program.
        /// </summary>
        /// <param name="program">The program to bind to the OpenGL context.</param>
        public static OpenGLState ScopedUseProgram(UInt32 program)
        {
            var state = pool.Retrieve();

            state.stateType             = OpenGLStateType.UseProgram;
            state.disposed              = false;
            state.newGL_CURRENT_PROGRAM = program;

            state.Apply();

            return state;
        }

        /// <summary>
        /// Immediately activates a shader program and updates the state cache.
        /// </summary>
        /// <param name="program">The program to bind to the OpenGL context.</param>
        public static void UseProgram(UInt32 program)
        {
            gl.UseProgram(program);
            gl.ThrowIfError();

            OpenGLState.GL_CURRENT_PROGRAM.Update(program);
            OpenGLState.VerifyCache();
        }

        /// <summary>
        /// Immediately creates an array buffer and updates the state cache.
        /// </summary>
        /// <param name="buffer">The identifier of the buffer that was created.</param>
        public static void CreateArrayBuffer(out UInt32 buffer)
        {
            if (gl.IsARBDirectStateAccessAvailable)
            {
                buffer = gl.CreateBuffer();
            }
            else
            {
                buffer = gl.GenBuffer();
                gl.BindBuffer(gl.GL_ARRAY_BUFFER, buffer);

                OpenGLState.GL_ARRAY_BUFFER_BINDING.Update(buffer);
                OpenGLState.VerifyCache();
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which creates an array buffer.
        /// </summary>
        /// <param name="buffer">The identifier of the buffer that was created.</param>
        public static OpenGLState ScopedCreateArrayBuffer(out UInt32 buffer)
        {
            var state = pool.Retrieve();

            state.stateType             = OpenGLStateType.CreateArrayBuffer;
            state.disposed              = false;

            state.Apply_CreateArrayBuffer(out buffer);

            return state;
        }

        /// <summary>
        /// Immediately creates an element array buffer and updates the state cache.
        /// </summary>
        /// <param name="buffer">The identifier of the buffer that was created.</param>
        public static void CreateElementArrayBuffer(out UInt32 buffer)
        {
            if (gl.IsARBDirectStateAccessAvailable)
            {
                buffer = gl.CreateBuffer();
            }
            else
            {
                buffer = gl.GenBuffer();
                gl.BindBuffer(gl.GL_ELEMENT_ARRAY_BUFFER, buffer);

                OpenGLState.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(buffer);
                OpenGLState.VerifyCache();
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which creates an element array buffer.
        /// </summary>
        /// <param name="buffer">The identifier of the buffer that was created.</param>
        public static OpenGLState ScopedCreateElementArrayBuffer(out UInt32 buffer)
        {
            var state = pool.Retrieve();

            state.stateType             = OpenGLStateType.CreateElementArrayBuffer;
            state.disposed              = false;

            state.Apply_CreateElementArrayBuffer(out buffer);

            return state;
        }

        /// <summary>
        /// Immediately creates a 2D texture and updates the state cache.
        /// </summary>
        /// <param name="texture">The identifier of the texture that was created.</param>
        public static void CreateTexture2D(out UInt32 texture)
        {
            if (gl.IsARBDirectStateAccessAvailable)
            {
                texture = gl.CreateTexture(gl.GL_TEXTURE_2D);
            }
            else
            {
                texture = gl.GenTexture();
                gl.BindTexture(gl.GL_TEXTURE_2D, texture);

                OpenGLState.GL_TEXTURE_BINDING_2D.Update(texture);
                OpenGLState.VerifyCache();
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which creates a 2D texture.
        /// </summary>
        /// <param name="texture">The identifier of the texture that was created.</param>
        public static OpenGLState ScopedCreateTexture2D(out UInt32 texture)
        {
            var state = pool.Retrieve();

            state.stateType             = OpenGLStateType.CreateTexture2D;
            state.disposed              = false;

            state.Apply_CreateTexture2D(out texture);

            return state;
        }

        /// <summary>
        /// Immediately creates a framebuffer and updates the state cache.
        /// </summary>
        /// <param name="framebuffer">The identifier of the framebuffer that was created.</param>
        public static void CreateFramebuffer(out UInt32 framebuffer)
        {
            if (gl.IsARBDirectStateAccessAvailable)
            {
                framebuffer = gl.CreateFramebuffer();
            }
            else
            {
                framebuffer = gl.GenFramebuffer();
                gl.BindFramebuffer(gl.GL_FRAMEBUFFER, framebuffer);

                OpenGLState.GL_FRAMEBUFFER_BINDING.Update(framebuffer);
                OpenGLState.VerifyCache();
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which creates a framebuffer.
        /// </summary>
        /// <param name="framebuffer">The identifier of the framebuffer that was created.</param>
        public static OpenGLState ScopedCreateFramebuffer(out UInt32 framebuffer)
        {
            var state = pool.Retrieve();

            state.stateType             = OpenGLStateType.CreateFramebuffer;
            state.disposed              = false;

            state.Apply_CreateFramebuffer(out framebuffer);

            return state;
        }

        /// <summary>
        /// Indicates that the specified vertex array object has been deleted and updates the OpenGL state accordingly.
        /// </summary>
        /// <param name="vertexArrayObject">The vertex array object to delete.</param>
        /// <param name="arrayBuffer">The array buffer to delete.</param>
        /// <param name="elementArrayBuffer">The element array buffer to delete.</param>
        public static void DeleteVertexArrayObject(UInt32 vertexArrayObject, UInt32 arrayBuffer, UInt32 elementArrayBuffer)
        {
            if (OpenGLState.GL_VERTEX_ARRAY_BINDING == vertexArrayObject)
            {
                OpenGLState.GL_VERTEX_ARRAY_BINDING.Update(0);
                OpenGLState.GL_ARRAY_BUFFER_BINDING.Update(0);
                OpenGLState.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(0);

                OpenGLState.VerifyCache();
            }
        }

        /// <summary>
        /// Applies the state object's values to the OpenGL context.
        /// </summary>
        public void Apply()
        {
            switch (stateType)
            {
                case OpenGLStateType.None:
                    break;

                case OpenGLStateType.BindTexture2D:
                    Apply_BindTexture2D();
                    break;

                case OpenGLStateType.BindVertexArrayObject:
                    Apply_BindVertexArrayObject();
                    break;

                case OpenGLStateType.BindArrayBuffer:
                    Apply_BindArrayBuffer();
                    break;

                case OpenGLStateType.BindElementArrayBuffer:
                    Apply_BindElementArrayBuffer();
                    break;

                case OpenGLStateType.BindFramebuffer:
                    Apply_BindFramebuffer();
                    break;

                case OpenGLStateType.UseProgram:
                    Apply_UseProgram();
                    break;

                default:
                    throw new InvalidOperationException();
            }

            OpenGLState.VerifyCache();
        }

        private void Apply_BindTexture2D()
        {
            if (forced || !gl.IsDirectStateAccessAvailable)
            {
                gl.BindTexture(gl.GL_TEXTURE_2D, newGL_TEXTURE_BINDING_2D);
                gl.ThrowIfError();

                oldGL_TEXTURE_BINDING_2D = OpenGLState.GL_TEXTURE_BINDING_2D.Update(newGL_TEXTURE_BINDING_2D);
            }
        }

        private void Apply_BindVertexArrayObject()
        {
            if (forced || !gl.IsDirectStateAccessAvailable)
            {
                gl.BindVertexArray(newGL_VERTEX_ARRAY_BINDING);
                gl.ThrowIfError();

                oldGL_VERTEX_ARRAY_BINDING         = OpenGLState.GL_VERTEX_ARRAY_BINDING.Update(newGL_VERTEX_ARRAY_BINDING);
                oldGL_ARRAY_BUFFER_BINDING         = OpenGLState.GL_ARRAY_BUFFER_BINDING.Update(newGL_ARRAY_BUFFER_BINDING);
                oldGL_ELEMENT_ARRAY_BUFFER_BINDING = OpenGLState.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(newGL_ELEMENT_ARRAY_BUFFER_BINDING);
            }
        }

        private void Apply_BindArrayBuffer()
        {
            if (forced || !gl.IsDirectStateAccessAvailable)
            {
                gl.BindBuffer(gl.GL_ARRAY_BUFFER, newGL_ARRAY_BUFFER_BINDING);
                gl.ThrowIfError();

                oldGL_ARRAY_BUFFER_BINDING = OpenGLState.GL_ARRAY_BUFFER_BINDING.Update(newGL_ARRAY_BUFFER_BINDING);
            }
        }

        private void Apply_BindElementArrayBuffer()
        {
            if (forced || !gl.IsDirectStateAccessAvailable)
            {
                gl.BindBuffer(gl.GL_ELEMENT_ARRAY_BUFFER, newGL_ELEMENT_ARRAY_BUFFER_BINDING);
                gl.ThrowIfError();

                oldGL_ELEMENT_ARRAY_BUFFER_BINDING = OpenGLState.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(newGL_ELEMENT_ARRAY_BUFFER_BINDING);
            }
        }

        private void Apply_BindFramebuffer()
        {
            if (forced || !gl.IsDirectStateAccessAvailable)
            {
                gl.BindFramebuffer(gl.GL_FRAMEBUFFER, newGL_FRAMEBUFFER_BINDING);
                gl.ThrowIfError();

                oldGL_FRAMEBUFFER_BINDING = OpenGLState.GL_FRAMEBUFFER_BINDING.Update(newGL_FRAMEBUFFER_BINDING);
            }
        }

        private void Apply_UseProgram()
        {
            gl.UseProgram(newGL_CURRENT_PROGRAM);
            gl.ThrowIfError();

            oldGL_CURRENT_PROGRAM = OpenGLState.GL_CURRENT_PROGRAM.Update(newGL_CURRENT_PROGRAM);
        }

        private void Apply_CreateElementArrayBuffer(out UInt32 buffer)
        {
            if (gl.IsARBDirectStateAccessAvailable)
            {
                buffer = gl.CreateBuffer();
                gl.ThrowIfError();
            }
            else
            {
                buffer = gl.GenBuffer();
                gl.ThrowIfError();

                gl.BindBuffer(gl.GL_ELEMENT_ARRAY_BUFFER, buffer);
                gl.ThrowIfError();

                newGL_ELEMENT_ARRAY_BUFFER_BINDING = buffer;
                oldGL_ELEMENT_ARRAY_BUFFER_BINDING = OpenGLState.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(buffer);
            }
        }

        private void Apply_CreateArrayBuffer(out UInt32 buffer)
        {
            if (gl.IsARBDirectStateAccessAvailable)
            {
                buffer = gl.CreateBuffer();
                gl.ThrowIfError();
            }
            else
            {
                buffer = gl.GenBuffer();
                gl.ThrowIfError();

                gl.BindBuffer(gl.GL_ARRAY_BUFFER, buffer);
                gl.ThrowIfError();

                newGL_ARRAY_BUFFER_BINDING = buffer;
                oldGL_ARRAY_BUFFER_BINDING = OpenGLState.GL_ARRAY_BUFFER_BINDING.Update(buffer);
            }
        }

        private void Apply_CreateTexture2D(out UInt32 texture)
        {
            if (gl.IsARBDirectStateAccessAvailable)
            {
                texture = gl.CreateTexture(gl.GL_TEXTURE_2D);
                gl.ThrowIfError();
            }
            else
            {
                texture = gl.GenTexture();
                gl.ThrowIfError();

                gl.BindTexture(gl.GL_TEXTURE_2D, texture);
                gl.ThrowIfError();

                newGL_TEXTURE_BINDING_2D = texture;
                oldGL_TEXTURE_BINDING_2D = OpenGLState.GL_TEXTURE_BINDING_2D.Update(texture);
            }
        }

        private void Apply_CreateFramebuffer(out UInt32 framebuffer)
        {
            if (gl.IsARBDirectStateAccessAvailable)
            {
                framebuffer = gl.CreateFramebuffer();
            }
            else
            {
                framebuffer = gl.GenFramebuffer();
                gl.ThrowIfError();

                gl.BindFramebuffer(gl.GL_FRAMEBUFFER, framebuffer);
                gl.ThrowIfError();

                newGL_FRAMEBUFFER_BINDING = framebuffer;
                oldGL_FRAMEBUFFER_BINDING = OpenGLState.GL_FRAMEBUFFER_BINDING.Update(framebuffer);
            }
        }

        /// <summary>
        /// Resets the OpenGL context to its values prior to this object's application.
        /// </summary>
        public void Dispose()
        {
            if (disposed)
                return;

            switch (stateType)
            {
                case OpenGLStateType.None:
                    break;

                case OpenGLStateType.BindTexture2D:
                    Dispose_BindTexture2D();
                    break;

                case OpenGLStateType.BindVertexArrayObject:
                    Dispose_BindVertexArrayObject();
                    break;

                case OpenGLStateType.BindArrayBuffer:
                    Dispose_BindArrayBuffer();
                    break;

                case OpenGLStateType.BindElementArrayBuffer:
                    Dispose_BindElementArrayBuffer();
                    break;

                case OpenGLStateType.BindFramebuffer:
                    Dispose_BindFramebuffer();
                    break;

                case OpenGLStateType.UseProgram:
                    Dispose_UseProgram();
                    break;

                case OpenGLStateType.CreateTexture2D:
                    Dispose_CreateTexture2D();
                    break;

                case OpenGLStateType.CreateArrayBuffer:
                    Dispose_CreateArrayBuffer();
                    break;

                case OpenGLStateType.CreateElementArrayBuffer:
                    Dispose_CreateElementArrayBuffer();
                    break;

                case OpenGLStateType.CreateFramebuffer:
                    Dispose_CreateFramebuffer();
                    break;

                default:
                    throw new InvalidOperationException();
            }

            OpenGLState.VerifyCache();

            disposed = true;
            pool.Release(this);
        }

        private void Dispose_BindTexture2D()
        {
            if (forced || !gl.IsDirectStateAccessAvailable)
            {
                gl.BindTexture(gl.GL_TEXTURE_2D, oldGL_TEXTURE_BINDING_2D);
                gl.ThrowIfError();

                OpenGLState.GL_TEXTURE_BINDING_2D.Update(oldGL_TEXTURE_BINDING_2D);
            }
        }

        private void Dispose_BindVertexArrayObject()
        {
            if (forced || !gl.IsDirectStateAccessAvailable)
            {
                gl.BindVertexArray(oldGL_VERTEX_ARRAY_BINDING);
                gl.ThrowIfError();

                OpenGLState.GL_VERTEX_ARRAY_BINDING.Update(oldGL_VERTEX_ARRAY_BINDING);
                OpenGLState.GL_ARRAY_BUFFER_BINDING.Update(oldGL_ARRAY_BUFFER_BINDING);
                OpenGLState.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(oldGL_ELEMENT_ARRAY_BUFFER_BINDING);
            }
        }

        private void Dispose_BindArrayBuffer()
        {
            if (forced || !gl.IsDirectStateAccessAvailable)
            {
                gl.BindBuffer(gl.GL_ARRAY_BUFFER, oldGL_ARRAY_BUFFER_BINDING);
                gl.ThrowIfError();

                OpenGLState.GL_ARRAY_BUFFER_BINDING.Update(oldGL_ARRAY_BUFFER_BINDING);
            }
        }

        private void Dispose_BindElementArrayBuffer()
        {
            if (forced || !gl.IsDirectStateAccessAvailable)
            {
                gl.BindBuffer(gl.GL_ELEMENT_ARRAY_BUFFER, oldGL_ELEMENT_ARRAY_BUFFER_BINDING);
                gl.ThrowIfError();

                OpenGLState.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(oldGL_ELEMENT_ARRAY_BUFFER_BINDING);
            }
        }

        private void Dispose_BindFramebuffer()
        {
            if (forced || !gl.IsDirectStateAccessAvailable)
            {
                gl.BindFramebuffer(gl.GL_FRAMEBUFFER, oldGL_FRAMEBUFFER_BINDING);
                gl.ThrowIfError();

                OpenGLState.GL_FRAMEBUFFER_BINDING.Update(oldGL_FRAMEBUFFER_BINDING);
            }
        }

        private void Dispose_UseProgram()
        {
            gl.UseProgram(oldGL_CURRENT_PROGRAM);
            gl.ThrowIfError();

            OpenGLState.GL_CURRENT_PROGRAM.Update(oldGL_CURRENT_PROGRAM);
        }

        private void Dispose_CreateArrayBuffer()
        {
            if (!gl.IsARBDirectStateAccessAvailable)
            {
                gl.BindBuffer(gl.GL_ARRAY_BUFFER, oldGL_ARRAY_BUFFER_BINDING);
                gl.ThrowIfError();

                OpenGLState.GL_ARRAY_BUFFER_BINDING.Update(oldGL_ARRAY_BUFFER_BINDING);
            }
        }

        private void Dispose_CreateElementArrayBuffer()
        {
            if (!gl.IsARBDirectStateAccessAvailable)
            {
                gl.BindBuffer(gl.GL_ELEMENT_ARRAY_BUFFER, oldGL_ELEMENT_ARRAY_BUFFER_BINDING);
                gl.ThrowIfError();

                OpenGLState.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(oldGL_ELEMENT_ARRAY_BUFFER_BINDING);
            }
        }

        private void Dispose_CreateTexture2D()
        {
            if (!gl.IsARBDirectStateAccessAvailable)
            {
                gl.BindTexture(gl.GL_TEXTURE_2D, oldGL_TEXTURE_BINDING_2D);
                gl.ThrowIfError();

                OpenGLState.GL_TEXTURE_BINDING_2D.Update(oldGL_TEXTURE_BINDING_2D);
            }
        }

        private void Dispose_CreateFramebuffer()
        {
            if (!gl.IsARBDirectStateAccessAvailable)
            {
                gl.BindFramebuffer(gl.GL_FRAMEBUFFER, oldGL_FRAMEBUFFER_BINDING);
                gl.ThrowIfError();

                OpenGLState.GL_FRAMEBUFFER_BINDING.Update(oldGL_FRAMEBUFFER_BINDING);
            }
        }

        /// <summary>
        /// Gets the cached value of GL_TEXTURE_BINDING_2D.
        /// </summary>
        public static OpenGLStateInteger GL_TEXTURE_BINDING_2D { get { return glTextureBinding2D; } }

        /// <summary>
        /// Gets the cached value of GL_VERTEX_ARRAY_BINDING.
        /// </summary>
        public static OpenGLStateInteger GL_VERTEX_ARRAY_BINDING { get { return glVertexArrayBinding; } }

        /// <summary>
        /// Gets the cached value of GL_ARRAY_BUFFER_BINDING.
        /// </summary>
        public static OpenGLStateInteger GL_ARRAY_BUFFER_BINDING { get { return glArrayBufferBinding; } }

        /// <summary>
        /// Gets the cached value of GL_ELEMENT_ARRAY_BUFFER_BINDING.
        /// </summary>
        public static OpenGLStateInteger GL_ELEMENT_ARRAY_BUFFER_BINDING { get { return glElementArrayBufferBinding; } }

        /// <summary>
        /// Gets the cached value of GL_FRAMEBUFFER_BINDING.
        /// </summary>
        public static OpenGLStateInteger GL_FRAMEBUFFER_BINDING { get { return glFramebufferBinding; } }

        /// <summary>
        /// Gets the cached value of GL_CURRENT_PROGRAM.
        /// </summary>
        public static OpenGLStateInteger GL_CURRENT_PROGRAM { get { return glCurrentProgram; } }

        // State values.
        private OpenGLStateType stateType;
        private Boolean disposed;
        private Boolean forced;

        private UInt32 newGL_TEXTURE_BINDING_2D;
        private UInt32 newGL_VERTEX_ARRAY_BINDING;
        private UInt32 newGL_ARRAY_BUFFER_BINDING;
        private UInt32 newGL_ELEMENT_ARRAY_BUFFER_BINDING;
        private UInt32 newGL_FRAMEBUFFER_BINDING;
        private UInt32 newGL_CURRENT_PROGRAM;

        private UInt32 oldGL_TEXTURE_BINDING_2D;
        private UInt32 oldGL_VERTEX_ARRAY_BINDING;
        private UInt32 oldGL_ARRAY_BUFFER_BINDING;
        private UInt32 oldGL_ELEMENT_ARRAY_BUFFER_BINDING;
        private UInt32 oldGL_FRAMEBUFFER_BINDING;
        private UInt32 oldGL_CURRENT_PROGRAM;

        // Cached OpenGL state values.
        private static readonly OpenGLStateInteger[] glCachedIntegers;
        private static readonly OpenGLStateInteger glTextureBinding2D          = new OpenGLStateInteger("GL_TEXTURE_BINDING_2D", gl.GL_TEXTURE_BINDING_2D);
        private static readonly OpenGLStateInteger glVertexArrayBinding        = new OpenGLStateInteger("GL_VERTEX_ARRAY_BINDING", gl.GL_VERTEX_ARRAY_BINDING);
        private static readonly OpenGLStateInteger glArrayBufferBinding        = new OpenGLStateInteger("GL_ARRAY_BUFFER_BINDING", gl.GL_ARRAY_BUFFER_BINDING);
        private static readonly OpenGLStateInteger glElementArrayBufferBinding = new OpenGLStateInteger("GL_ELEMENT_ARRAY_BUFFER_BINDING", gl.GL_ELEMENT_ARRAY_BUFFER_BINDING);
        private static readonly OpenGLStateInteger glFramebufferBinding        = new OpenGLStateInteger("GL_FRAMEBUFFER_BINDING", gl.GL_FRAMEBUFFER_BINDING);
        private static readonly OpenGLStateInteger glCurrentProgram            = new OpenGLStateInteger("GL_CURRENT_PROGRAM", gl.GL_CURRENT_PROGRAM);

        // The pool of state objects.
        private static readonly IPool<OpenGLState> pool = new ExpandingPool<OpenGLState>(1, () => new OpenGLState());
    }
}
