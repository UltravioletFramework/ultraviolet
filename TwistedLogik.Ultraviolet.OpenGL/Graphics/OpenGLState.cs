using System;
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
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLState"/> class.
        /// </summary>
        private OpenGLState()
        { }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which binds a 2D texture to the context.
        /// </summary>
        public static OpenGLState BindTexture2D(UInt32 GL_TEXTURE_BINDING_2D, Boolean force = false)
        {
            var state = pool.Retrieve();

            state.StateType             = OpenGLStateType.BindTexture2D;
            state.Disposed              = false;
            state.Forced                = force;
            state.GL_TEXTURE_BINDING_2D = GL_TEXTURE_BINDING_2D;

            state.Apply();

            return state;
        }

        /// <summary>
        /// Immediately binds a 2D texture to the OpenGL context and updates the cache.
        /// </summary>
        public static void BindTexture2DImmediate(UInt32 GL_TEXTURE_BINDING_2D)
        {
            gl.BindTexture(gl.GL_TEXTURE_2D, GL_TEXTURE_BINDING_2D);
            gl.ThrowIfError();

            OpenGLCache.GL_TEXTURE_BINDING_2D.Update(GL_TEXTURE_BINDING_2D);
            OpenGLCache.Verify();
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which binds a vertex array object to the context.
        /// </summary>
        public static OpenGLState BindVertexArrayObject(UInt32 GL_VERTEX_ARRAY_BINDING, UInt32 GL_ARRAY_BUFFER_BINDING, UInt32 GL_ELEMENT_ARRAY_BUFFER_BINDING, Boolean force = false)
        {
            var state = pool.Retrieve();

            state.StateType                       = OpenGLStateType.BindVertexArrayObject;
            state.Disposed                        = false;
            state.Forced                          = force;
            state.GL_VERTEX_ARRAY_BINDING         = GL_VERTEX_ARRAY_BINDING;
            state.GL_ARRAY_BUFFER_BINDING         = GL_ARRAY_BUFFER_BINDING;
            state.GL_ELEMENT_ARRAY_BUFFER_BINDING = GL_ELEMENT_ARRAY_BUFFER_BINDING;

            state.Apply();

            return state;
        }

        /// <summary>
        /// Immediately binds a vertex array object to the OpenGL context and updates the cache.
        /// </summary>
        public static void BindVertexArrayObjectImmediate(UInt32 GL_VERTEX_ARRAY_BINDING, UInt32 GL_ARRAY_BUFFER_BINDING, UInt32 GL_ELEMENT_ARRAY_BUFFER_BINDING)
        {
            gl.BindVertexArray(GL_VERTEX_ARRAY_BINDING);
            gl.ThrowIfError();

            OpenGLCache.GL_VERTEX_ARRAY_BINDING.Update(GL_VERTEX_ARRAY_BINDING);
            OpenGLCache.GL_ARRAY_BUFFER_BINDING.Update(GL_ARRAY_BUFFER_BINDING);
            OpenGLCache.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(GL_ELEMENT_ARRAY_BUFFER_BINDING);
            OpenGLCache.Verify();
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which binds an array buffer to the context.
        /// </summary>
        public static OpenGLState BindArrayBuffer(UInt32 GL_ARRAY_BUFFER_BINDING, Boolean force = false)
        {
            var state = pool.Retrieve();

            state.StateType               = OpenGLStateType.BindArrayBuffer;
            state.Disposed                = false;
            state.Forced                  = force;
            state.GL_ARRAY_BUFFER_BINDING = GL_ARRAY_BUFFER_BINDING;

            state.Apply();

            return state;
        }

        /// <summary>
        /// Immediately binds an array buffer to the OpenGL context and updates the cache.
        /// </summary>
        public static void BindArrayBufferImmediate(UInt32 GL_ARRAY_BUFFER_BINDING)
        {
            gl.BindBuffer(gl.GL_ARRAY_BUFFER, GL_ARRAY_BUFFER_BINDING);
            gl.ThrowIfError();

            OpenGLCache.GL_ARRAY_BUFFER_BINDING.Update(GL_ARRAY_BUFFER_BINDING);
            OpenGLCache.Verify();
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which binds an element array buffer to the context.
        /// </summary>
        public static OpenGLState BindElementArrayBuffer(UInt32 GL_ELEMENT_ARRAY_BUFFER_BINDING, Boolean force = false)
        {
            var state = pool.Retrieve();

            state.StateType                       = OpenGLStateType.BindElementArrayBuffer;
            state.Disposed                        = false;
            state.Forced                          = force;
            state.GL_ELEMENT_ARRAY_BUFFER_BINDING = GL_ELEMENT_ARRAY_BUFFER_BINDING;

            state.Apply();

            return state;
        }

        /// <summary>
        /// Immediately binds an element array buffer to the OpenGL context and updates the cache.
        /// </summary>
        public static void BindElementArrayBufferImmediate(UInt32 GL_ELEMENT_ARRAY_BUFFER_BINDING)
        {
            gl.BindBuffer(gl.GL_ELEMENT_ARRAY_BUFFER, GL_ELEMENT_ARRAY_BUFFER_BINDING);
            gl.ThrowIfError();

            OpenGLCache.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(GL_ELEMENT_ARRAY_BUFFER_BINDING);
            OpenGLCache.Verify();
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which binds a framebuffer to the context.
        /// </summary>
        public static OpenGLState BindFramebuffer(UInt32 GL_FRAMEBUFFER_BINDING, Boolean force = false)
        {
            var state = pool.Retrieve();

            state.StateType              = OpenGLStateType.BindFramebuffer;
            state.Disposed               = false;
            state.Forced                 = force;
            state.GL_FRAMEBUFFER_BINDING = GL_FRAMEBUFFER_BINDING;

            state.Apply();

            return state;
        }

        /// <summary>
        /// Immediately binds a framebuffer to the OpenGL context and updates the cache.
        /// </summary>
        public static void BindFramebufferImmediate(UInt32 GL_FRAMEBUFFER_BINDING)
        {
            gl.BindFramebuffer(gl.GL_FRAMEBUFFER, GL_FRAMEBUFFER_BINDING);
            gl.ThrowIfError();

            OpenGLCache.GL_FRAMEBUFFER_BINDING.Update(GL_FRAMEBUFFER_BINDING);
            OpenGLCache.Verify();
        }

        /// <summary>
        /// Applies the state object's values to the OpenGL context.
        /// </summary>
        public void Apply()
        {
            switch (StateType)
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

                default:
                    throw new InvalidOperationException();
            }

            OpenGLCache.Verify();
        }

        private void Apply_BindTexture2D()
        {
            if (Forced || !gl.IsDirectStateAccessAvailable)
            {
                gl.BindTexture(gl.GL_TEXTURE_2D, GL_TEXTURE_BINDING_2D);
                gl.ThrowIfError();

                Previous_GL_TEXTURE_BINDING_2D = OpenGLCache.GL_TEXTURE_BINDING_2D.Update(GL_TEXTURE_BINDING_2D);
            }
        }

        private void Apply_BindVertexArrayObject()
        {
            if (Forced || !gl.IsDirectStateAccessAvailable)
            {
                gl.BindVertexArray(GL_VERTEX_ARRAY_BINDING);
                gl.ThrowIfError();

                Previous_GL_VERTEX_ARRAY_BINDING         = OpenGLCache.GL_VERTEX_ARRAY_BINDING.Update(GL_VERTEX_ARRAY_BINDING);
                Previous_GL_ARRAY_BUFFER_BINDING         = OpenGLCache.GL_ARRAY_BUFFER_BINDING.Update(GL_ARRAY_BUFFER_BINDING);
                Previous_GL_ELEMENT_ARRAY_BUFFER_BINDING = OpenGLCache.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(GL_ELEMENT_ARRAY_BUFFER_BINDING);
            }
        }

        private void Apply_BindArrayBuffer()
        {
            if (Forced || !gl.IsDirectStateAccessAvailable)
            {
                gl.BindBuffer(gl.GL_ARRAY_BUFFER, GL_ARRAY_BUFFER_BINDING);
                gl.ThrowIfError();

                Previous_GL_ARRAY_BUFFER_BINDING = OpenGLCache.GL_ARRAY_BUFFER_BINDING.Update(GL_ARRAY_BUFFER_BINDING);
            }
        }

        private void Apply_BindElementArrayBuffer()
        {
            if (Forced || !gl.IsDirectStateAccessAvailable)
            {
                gl.BindBuffer(gl.GL_ELEMENT_ARRAY_BUFFER, GL_ELEMENT_ARRAY_BUFFER_BINDING);
                gl.ThrowIfError();

                Previous_GL_ELEMENT_ARRAY_BUFFER_BINDING = OpenGLCache.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(GL_ELEMENT_ARRAY_BUFFER_BINDING);
            }
        }

        private void Apply_BindFramebuffer()
        {
            if (Forced || !gl.IsDirectStateAccessAvailable)
            {
                gl.BindFramebuffer(gl.GL_FRAMEBUFFER, GL_FRAMEBUFFER_BINDING);
                gl.ThrowIfError();

                Previous_GL_FRAMEBUFFER_BINDING = OpenGLCache.GL_FRAMEBUFFER_BINDING.Update(GL_FRAMEBUFFER_BINDING);
            }
        }

        /// <summary>
        /// Resets the OpenGL context to its values prior to this object's application.
        /// </summary>
        public void Dispose()
        {
            if (Disposed)
                return;

            switch (StateType)
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

                default:
                    throw new InvalidOperationException();
            }

            OpenGLCache.Verify();

            Disposed = true;
            pool.Release(this);
        }

        private void Dispose_BindTexture2D()
        {
            if (Forced || !gl.IsDirectStateAccessAvailable)
            {
                gl.BindTexture(gl.GL_TEXTURE_2D, Previous_GL_TEXTURE_BINDING_2D);
                gl.ThrowIfError();

                OpenGLCache.GL_TEXTURE_BINDING_2D.Update(Previous_GL_TEXTURE_BINDING_2D);
            }
        }

        private void Dispose_BindVertexArrayObject()
        {
            if (Forced || !gl.IsDirectStateAccessAvailable)
            {
                gl.BindVertexArray(Previous_GL_VERTEX_ARRAY_BINDING);
                gl.ThrowIfError();

                OpenGLCache.GL_VERTEX_ARRAY_BINDING.Update(Previous_GL_VERTEX_ARRAY_BINDING);
                OpenGLCache.GL_ARRAY_BUFFER_BINDING.Update(Previous_GL_ARRAY_BUFFER_BINDING);
                OpenGLCache.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(Previous_GL_ELEMENT_ARRAY_BUFFER_BINDING);
            }
        }

        private void Dispose_BindArrayBuffer()
        {
            if (Forced || !gl.IsDirectStateAccessAvailable)
            {
                gl.BindBuffer(gl.GL_ARRAY_BUFFER, Previous_GL_ARRAY_BUFFER_BINDING);
                gl.ThrowIfError();

                OpenGLCache.GL_ARRAY_BUFFER_BINDING.Update(Previous_GL_ARRAY_BUFFER_BINDING);
            }
        }

        private void Dispose_BindElementArrayBuffer()
        {
            if (Forced || !gl.IsDirectStateAccessAvailable)
            {
                gl.BindBuffer(gl.GL_ELEMENT_ARRAY_BUFFER, Previous_GL_ELEMENT_ARRAY_BUFFER_BINDING);
                gl.ThrowIfError();

                OpenGLCache.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(Previous_GL_ELEMENT_ARRAY_BUFFER_BINDING);
            }
        }

        private void Dispose_BindFramebuffer()
        {
            if (Forced || !gl.IsDirectStateAccessAvailable)
            {
                gl.BindFramebuffer(gl.GL_FRAMEBUFFER, Previous_GL_FRAMEBUFFER_BINDING);
                gl.ThrowIfError();

                OpenGLCache.GL_FRAMEBUFFER_BINDING.Update(Previous_GL_FRAMEBUFFER_BINDING);
            }
        }

        public UInt32 GL_TEXTURE_BINDING_2D { get; private set; }
        public UInt32 GL_VERTEX_ARRAY_BINDING { get; private set; }
        public UInt32 GL_ARRAY_BUFFER_BINDING { get; private set; }
        public UInt32 GL_ELEMENT_ARRAY_BUFFER_BINDING { get; private set; }
        public UInt32 GL_FRAMEBUFFER_BINDING { get; private set; }

        private UInt32 Previous_GL_TEXTURE_BINDING_2D { get; set; }
        private UInt32 Previous_GL_VERTEX_ARRAY_BINDING { get; set; }
        private UInt32 Previous_GL_ARRAY_BUFFER_BINDING { get; set; }
        private UInt32 Previous_GL_ELEMENT_ARRAY_BUFFER_BINDING { get; set; }
        private UInt32 Previous_GL_FRAMEBUFFER_BINDING { get; set; }

        private OpenGLStateType StateType { get; set; }
        private Boolean Disposed { get; set; }
        private Boolean Forced { get; set; }

        // The pool of state objects.
        private static readonly IPool<OpenGLState> pool = new ExpandingPool<OpenGLState>(1, () => new OpenGLState());
    }
}
