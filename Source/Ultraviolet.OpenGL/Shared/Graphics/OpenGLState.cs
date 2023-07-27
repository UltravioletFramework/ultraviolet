using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Ultraviolet.Core.Collections;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;
using Ultraviolet.OpenGL.Graphics.Caching;

namespace Ultraviolet.OpenGL.Graphics
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
            ActiveTexture,
            BindTexture2D,
            BindTexture3D,
            BindVertexArrayObject,
            BindArrayBuffer,
            BindElementArrayBuffer,
            BindFramebuffer,
            BindRenderbuffer,
            UseProgram,
            CreateTexture2D,
            CreateTexture3D,
            CreateArrayBuffer,
            CreateElementArrayBuffer,
            CreateFramebuffer,
            CreateRenderbuffer,
        }

        /// <summary>
        /// Initializes the <see cref="OpenGLState"/> type.
        /// </summary>
        static OpenGLState()
        {
            glCachedIntegers = typeof(OpenGLState).GetFields(BindingFlags.NonPublic | BindingFlags.Static)
                .Where(x => x.FieldType == typeof(OpenGLStateInteger))
                .Select(x => (OpenGLStateInteger)x.GetValue(null)).ToArray();

            GL_FRAMEBUFFER_BINDING.Update(GL.DefaultFramebuffer);
            GL_RENDERBUFFER_BINDING.Update(GL.DefaultRenderbuffer);

            VerifyCache();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLState"/> class.
        /// </summary>
        private OpenGLState()
        { }

        /// <summary>
        /// Resets the values that are stored in the OpenGL state cache to their defaults.
        /// </summary>
        public static unsafe void ResetCache()
        {
            foreach (var glCachedInteger in glCachedIntegers)
                glCachedInteger.Reset();

            glTextureBinding2DByTextureUnit.Clear();
            glTextureBinding3DByTextureUnit.Clear();

            GL_FRAMEBUFFER_BINDING.Update(GL.DefaultFramebuffer);
            GL_RENDERBUFFER_BINDING.Update(GL.DefaultRenderbuffer);

            cachedClearColor = CachedClearColor.FromDevice();
            cachedClearDepth = CachedClearDepth.FromDevice();
            cachedClearStencil = CachedClearStencil.FromDevice();
            cachedColorMask = CachedColorMask.FromDevice();
            cachedDepthTestEnabled = CachedCapability.FromDevice(GL.GL_DEPTH_TEST);
            cachedDepthMask = CachedDepthMask.FromDevice();
            cachedDepthFunc = CachedDepthFunc.FromDevice();
            cachedStencilTestEnabled = CachedCapability.FromDevice(GL.GL_STENCIL_TEST);
            cachedStencilFuncFront = CachedStencilFunc.FromDevice(GL.GL_FRONT);
            cachedStencilFuncBack = CachedStencilFunc.FromDevice(GL.GL_BACK);
            cachedStencilOpFront = CachedStencilOp.FromDevice(GL.GL_FRONT);
            cachedStencilOpBack = CachedStencilOp.FromDevice(GL.GL_BACK);
            cachedBlendEnabled = CachedCapability.FromDevice(GL.GL_BLEND);
            cachedBlendColor = CachedBlendColor.FromDevice();
            cachedBlendEquation = CachedBlendEquation.FromDevice();
            cachedBlendFunction = CachedBlendFunction.FromDevice();
            cachedCullingEnabled = CachedCapability.FromDevice(GL.GL_CULL_FACE);
            cachedCulledFace = CachedCulledFace.FromDevice();
            cachedFrontFace = CachedFrontFace.FromDevice();
            cachedPolygonOffsetFillEnabled = CachedCapability.FromDevice(GL.GL_POLYGON_OFFSET_FILL);
            cachedPolygonOffset = CachedPolygonOffset.FromDevice();
            cachedPolygonMode = CachedPolygonMode.FromDevice();
            cachedScissorTestEnabled = CachedCapability.FromDevice(GL.GL_SCISSOR_TEST);
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
        /// Creates an instance of <see cref="OpenGLState"/> which sets the active texture unit.
        /// </summary>
        /// <param name="texture">The texture unit to activate.</param>
        public static OpenGLState ScopedActiveTexture(UInt32 texture)
        {
            if (GL_ACTIVE_TEXTURE == texture)
                return null;

            var state = pool.Retrieve();

            state.stateType = OpenGLStateType.ActiveTexture;
            state.disposed = false;
            state.newGL_ACTIVE_TEXTURE = texture;

            state.Apply();

            return state;
        }

        /// <summary>
        /// Immediately sets the active texture unit and updates the state cache.
        /// </summary>
        /// <param name="texture">The texture unit to activate.</param>
        public static void ActiveTexture(UInt32 texture)
        {
            if (GL_ACTIVE_TEXTURE == texture)
                return;

            GL.ActiveTexture(texture);
            GL.ThrowIfError();

            glTextureBinding2DByTextureUnit.TryGetValue(texture, out var tb2d);
            glTextureBinding3DByTextureUnit.TryGetValue(texture, out var tb3d);

            GL_ACTIVE_TEXTURE.Update(texture);
            GL_TEXTURE_BINDING_2D.Update(tb2d);
            GL_TEXTURE_BINDING_3D.Update(tb3d);

            VerifyCache();
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which binds a 2D texture to the context.
        /// </summary>
        /// <param name="texture">The texture to bind to the context.</param>
        /// <param name="force">A value indicating whether to force-bind the texture, even if DSA is available.</param>
        public static OpenGLState ScopedBindTexture2D(UInt32 texture, Boolean force = false)
        {
            if (GL_TEXTURE_BINDING_2D == texture)
                return null;

            var state = pool.Retrieve();

            state.stateType = OpenGLStateType.BindTexture2D;
            state.disposed = false;
            state.forced = force;
            state.newGL_TEXTURE_BINDING_2D = texture;

            state.Apply();

            return state;
        }

        /// <summary>
        /// Immediately binds a 2D texture to the OpenGL context and updates the state cache.
        /// </summary>
        /// <param name="texture">The texture to bind to the context.</param>
        public static void BindTexture2D(UInt32 texture)
        {
            if (GL_TEXTURE_BINDING_2D == texture)
                return;

            GL.BindTexture(GL.GL_TEXTURE_2D, texture);
            GL.ThrowIfError();

            GL_TEXTURE_BINDING_2D.Update(texture);
            glTextureBinding2DByTextureUnit[GL_ACTIVE_TEXTURE] = texture;
            glTextureBinding3DByTextureUnit[GL_ACTIVE_TEXTURE] = 0;

            VerifyCache();
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which binds a 3D texture to the context.
        /// </summary>
        /// <param name="texture">The texture to bind to the context.</param>
        /// <param name="force">A value indicating whether to force-bind the texture, even if DSA is available.</param>
        public static OpenGLState ScopedBindTexture3D(UInt32 texture, Boolean force = false)
        {
            if (GL_TEXTURE_BINDING_3D == texture)
                return null;

            var state = pool.Retrieve();

            state.stateType = OpenGLStateType.BindTexture3D;
            state.disposed = false;
            state.forced = force;
            state.newGL_TEXTURE_BINDING_3D = texture;

            state.Apply();

            return state;
        }

        /// <summary>
        /// Immediately binds a 3D texture to the OpenGL context and updates the state cache.
        /// </summary>
        /// <param name="texture">The texture to bind to the context.</param>
        public static void BindTexture3D(UInt32 texture)
        {
            if (GL_TEXTURE_BINDING_3D == texture)
                return;

            GL.BindTexture(GL.GL_TEXTURE_3D, texture);
            GL.ThrowIfError();

            GL_TEXTURE_BINDING_3D.Update(texture);
            glTextureBinding3DByTextureUnit[GL_ACTIVE_TEXTURE] = texture;

            VerifyCache();
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which binds a vertex array object to the context.
        /// </summary>
        /// <param name="vertexArrayObject">The vertex array object to bind to the context.</param>
        /// <param name="elementArrayBuffer">The vertex array's associated element array buffer.</param>
        /// <param name="force">A value indicating whether to force-bind the vertex array object, even if DSA is available.</param>
        public static OpenGLState ScopedBindVertexArrayObject(UInt32 vertexArrayObject, UInt32 elementArrayBuffer, Boolean force = false)
        {
            if (GL_VERTEX_ARRAY_BINDING == vertexArrayObject &&
                GL_ELEMENT_ARRAY_BUFFER_BINDING == elementArrayBuffer)
            {
                return null;
            }

            var state = pool.Retrieve();

            state.stateType = OpenGLStateType.BindVertexArrayObject;
            state.disposed = false;
            state.forced = force;
            state.newGL_VERTEX_ARRAY_BINDING = vertexArrayObject;
            state.newGL_ELEMENT_ARRAY_BUFFER_BINDING = elementArrayBuffer;

            state.Apply();

            return state;
        }

        /// <summary>
        /// Immediately binds a vertex array object to the OpenGL context and updates the state cache.
        /// </summary>
        /// <param name="vertexArrayObject">The vertex array object to bind to the context.</param>
        /// <param name="elementArrayBuffer">The vertex array's associated element array buffer.</param>
        public static void BindVertexArrayObject(UInt32 vertexArrayObject, UInt32 elementArrayBuffer)
        {
            if (GL_VERTEX_ARRAY_BINDING == vertexArrayObject &&
                GL_ELEMENT_ARRAY_BUFFER_BINDING == elementArrayBuffer)
            {
                return;
            }

            GL.BindVertexArray(vertexArrayObject);
            GL.ThrowIfError();

            GL_VERTEX_ARRAY_BINDING.Update(vertexArrayObject);
            GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(elementArrayBuffer);
            VerifyCache();
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which binds an array buffer to the context.
        /// </summary>
        /// <param name="arrayBuffer">The array buffer to bind to the OpenGL context.</param>
        /// <param name="force">A value indicating whether to force-bind the array buffer, even if DSA is available.</param>
        public static OpenGLState ScopedBindArrayBuffer(UInt32 arrayBuffer, Boolean force = false)
        {
            if (GL_ARRAY_BUFFER_BINDING == arrayBuffer)
                return null;

            var state = pool.Retrieve();

            state.stateType = OpenGLStateType.BindArrayBuffer;
            state.disposed = false;
            state.forced = force;
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
            if (GL.GL_ARRAY_BUFFER_BINDING == arrayBuffer)
                return;

            GL.BindBuffer(GL.GL_ARRAY_BUFFER, arrayBuffer);
            GL.ThrowIfError();

            GL_ARRAY_BUFFER_BINDING.Update(arrayBuffer);
            VerifyCache();
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which binds an element array buffer to the context.
        /// </summary>
        /// <param name="elementArrayBuffer">The element array buffer to bind to the OpenGL context.</param>
        /// <param name="force">A value indicating whether to force-bind the array buffer, even if DSA is available.</param>
        public static OpenGLState ScopedBindElementArrayBuffer(UInt32 elementArrayBuffer, Boolean force = false)
        {
            if (GL_ELEMENT_ARRAY_BUFFER_BINDING == elementArrayBuffer)
                return null;

            var state = pool.Retrieve();

            state.stateType = OpenGLStateType.BindElementArrayBuffer;
            state.disposed = false;
            state.forced = force;
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
            if (GL.GL_ELEMENT_ARRAY_BUFFER_BINDING == elementArrayBuffer)
                return;

            GL.BindBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, elementArrayBuffer);
            GL.ThrowIfError();

            GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(elementArrayBuffer);
            VerifyCache();
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which binds a framebuffer to the context.
        /// </summary>
        /// <param name="framebuffer">The framebuffer to bind to the OpenGL context.</param>
        /// <param name="force">A value indicating whether to force-bind the framebuffer, even if DSA is available.</param>
        public static OpenGLState ScopedBindFramebuffer(UInt32 framebuffer, Boolean force = false)
        {
            if (GL_FRAMEBUFFER_BINDING == framebuffer)
                return null;

            var state = pool.Retrieve();

            state.stateType = OpenGLStateType.BindFramebuffer;
            state.disposed = false;
            state.forced = force;
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
            if (GL.GL_FRAMEBUFFER_BINDING == framebuffer)
                return;

            GL.BindFramebuffer(GL.GL_FRAMEBUFFER, framebuffer);
            GL.ThrowIfError();

            GL_FRAMEBUFFER_BINDING.Update(framebuffer);
            VerifyCache();
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which binds a renderbuffer to the context.
        /// </summary>
        /// <param name="renderbuffer">The renderbuffer to bind to the OpenGL context.</param>
        /// <param name="force">A value indicating whether to force-bind the renderbuffer, even if DSA is available.</param>
        public static OpenGLState ScopedBindRenderbuffer(UInt32 renderbuffer, Boolean force = false)
        {
            if (GL_RENDERBUFFER_BINDING == renderbuffer)
                return null;

            var state = pool.Retrieve();

            state.stateType = OpenGLStateType.BindRenderbuffer;
            state.disposed = false;
            state.forced = force;
            state.newGL_RENDERBUFFER_BINDING = renderbuffer;

            state.Apply();

            return state;
        }

        /// <summary>
        /// Immediately binds a renderbuffer to the OpenGL context and updates the state cache.
        /// </summary>
        /// <param name="renderbuffer">The renderbuffer to bind to the OpenGL context.</param>
        public static void BindRenderbuffer(UInt32 renderbuffer)
        {
            if (GL_RENDERBUFFER_BINDING == renderbuffer)
                return;

            GL.BindRenderbuffer(GL.GL_RENDERBUFFER, renderbuffer);
            GL.ThrowIfError();

            GL_RENDERBUFFER_BINDING.Update(renderbuffer);
            VerifyCache();
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which activates a shader program.
        /// </summary>
        /// <param name="program">The program to bind to the OpenGL context.</param>
        public static OpenGLState ScopedUseProgram(OpenGLShaderProgram program)
        {
            var oglname = program?.OpenGLName ?? 0;
            if (GL_CURRENT_PROGRAM == oglname)
                return null;

            var state = pool.Retrieve();

            state.stateType = OpenGLStateType.UseProgram;
            state.disposed = false;
            state.newGL_CURRENT_PROGRAM = oglname;
            state.newCurrentProgram = program;

            state.Apply();

            return state;
        }

        /// <summary>
        /// Immediately activates a shader program and updates the state cache.
        /// </summary>
        /// <param name="program">The program to bind to the OpenGL context.</param>
        public static void UseProgram(OpenGLShaderProgram program)
        {
            var oglname = program?.OpenGLName ?? 0;
            if (GL_CURRENT_PROGRAM == oglname)
                return;

            GL.UseProgram(oglname);
            GL.ThrowIfError();

            currentProgram = program;

            GL_CURRENT_PROGRAM.Update(oglname);
            VerifyCache();
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which creates an array buffer.
        /// </summary>
        /// <param name="buffer">The identifier of the buffer that was created.</param>
        public static OpenGLState ScopedCreateArrayBuffer(out UInt32 buffer)
        {
            var state = pool.Retrieve();

            state.stateType = OpenGLStateType.CreateArrayBuffer;
            state.disposed = false;

            state.Apply_CreateArrayBuffer(out buffer);

            return state;
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which creates an element array buffer.
        /// </summary>
        /// <param name="buffer">The identifier of the buffer that was created.</param>
        public static OpenGLState ScopedCreateElementArrayBuffer(out UInt32 buffer)
        {
            var state = pool.Retrieve();

            state.stateType = OpenGLStateType.CreateElementArrayBuffer;
            state.disposed = false;

            state.Apply_CreateElementArrayBuffer(out buffer);

            return state;
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which creates a 2D texture.
        /// </summary>
        /// <param name="texture">The identifier of the texture that was created.</param>
        public static OpenGLState ScopedCreateTexture2D(out UInt32 texture)
        {
            var state = pool.Retrieve();

            state.stateType = OpenGLStateType.CreateTexture2D;
            state.disposed = false;

            state.Apply_CreateTexture2D(out texture);

            return state;
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which creates a 3D texture.
        /// </summary>
        /// <param name="texture">The identifier of the texture that was created.</param>
        public static OpenGLState ScopedCreateTexture3D(out UInt32 texture)
        {
            var state = pool.Retrieve();

            state.stateType = OpenGLStateType.CreateTexture3D;
            state.disposed = false;

            state.Apply_CreateTexture3D(out texture);

            return state;
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which creates a framebuffer.
        /// </summary>
        /// <param name="framebuffer">The identifier of the framebuffer that was created.</param>
        public static OpenGLState ScopedCreateFramebuffer(out UInt32 framebuffer)
        {
            var state = pool.Retrieve();

            state.stateType = OpenGLStateType.CreateFramebuffer;
            state.disposed = false;

            state.Apply_CreateFramebuffer(out framebuffer);

            return state;
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenGLState"/> which creates a renderbuffer.
        /// </summary>
        /// <param name="renderbuffer">The identifier of the renderbuffer that was created.</param>
        public static OpenGLState ScopedCreateRenderbuffer(out UInt32 renderbuffer)
        {
            var state = pool.Retrieve();

            state.stateType = OpenGLStateType.CreateRenderbuffer;
            state.disposed = false;

            state.Apply_CreateRenderbuffer(out renderbuffer);

            return state;
        }

        /// <summary>
        /// Indicates that the specified texture has been deleted and updates the OpenGL state accordingly.
        /// </summary>
        /// <param name="texture">The texture to delete.</param>
        public static void DeleteTexture2D(UInt32 texture)
        {
            if (GL_TEXTURE_BINDING_2D == texture)
            {
                GL_TEXTURE_BINDING_2D.Update(0);
                glTextureBinding2DByTextureUnit[GL_ACTIVE_TEXTURE] = 0;

                VerifyCache();
            }
        }

        /// <summary>
        /// Indicates that the specified texture has been deleted and updates the OpenGL state accordingly.
        /// </summary>
        /// <param name="texture">The texture to delete.</param>
        public static void DeleteTexture3D(UInt32 texture)
        {
            if (GL_TEXTURE_BINDING_3D == texture)
            {
                GL_TEXTURE_BINDING_3D.Update(0);
                glTextureBinding3DByTextureUnit[GL_ACTIVE_TEXTURE] = 0;

                VerifyCache();
            }
        }

        /// <summary>
        /// Indicates that the specified vertex array object has been deleted and updates the OpenGL state accordingly.
        /// </summary>
        /// <param name="vertexArrayObject">The vertex array object to delete.</param>
        /// <param name="elementArrayBuffer">The element array buffer to delete.</param>
        public static void DeleteVertexArrayObject(UInt32 vertexArrayObject, UInt32 elementArrayBuffer)
        {
            if (GL_VERTEX_ARRAY_BINDING == vertexArrayObject)
            {
                GL_VERTEX_ARRAY_BINDING.Update(0);
                GL_ARRAY_BUFFER_BINDING.Update(0);
                GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(0);
                VerifyCache();
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

                case OpenGLStateType.ActiveTexture:
                    Apply_ActiveTexture();
                    break;

                case OpenGLStateType.BindTexture2D:
                    Apply_BindTexture2D();
                    break;

                case OpenGLStateType.BindTexture3D:
                    Apply_BindTexture3D();
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

                case OpenGLStateType.BindRenderbuffer:
                    Apply_BindRenderbuffer();
                    break;

                case OpenGLStateType.UseProgram:
                    Apply_UseProgram();
                    break;

                default:
                    throw new InvalidOperationException();
            }

            OpenGLState.VerifyCache();
        }

        private void Apply_ActiveTexture()
        {
            GL.ActiveTexture(newGL_ACTIVE_TEXTURE);
            GL.ThrowIfError();

            glTextureBinding2DByTextureUnit.TryGetValue(newGL_ACTIVE_TEXTURE, out var tb2d);
            glTextureBinding3DByTextureUnit.TryGetValue(newGL_ACTIVE_TEXTURE, out var tb3d);

            oldGL_ACTIVE_TEXTURE = OpenGLState.GL_ACTIVE_TEXTURE.Update(newGL_ACTIVE_TEXTURE);
            oldGL_TEXTURE_BINDING_2D = GL_TEXTURE_BINDING_2D.Update(tb2d);
            oldGL_TEXTURE_BINDING_3D = GL_TEXTURE_BINDING_3D.Update(tb3d);
        }

        private void Apply_BindTexture2D()
        {
            if (forced || !GL.IsDirectStateAccessAvailable)
            {
                GL.BindTexture(GL.GL_TEXTURE_2D, newGL_TEXTURE_BINDING_2D);
                GL.ThrowIfError();

                oldGL_TEXTURE_BINDING_2D = GL_TEXTURE_BINDING_2D.Update(newGL_TEXTURE_BINDING_2D);
                glTextureBinding2DByTextureUnit[GL_ACTIVE_TEXTURE] = newGL_TEXTURE_BINDING_2D;
            }
        }

        private void Apply_BindTexture3D()
        {
            if (forced || !GL.IsDirectStateAccessAvailable)
            {
                GL.BindTexture(GL.GL_TEXTURE_3D, newGL_TEXTURE_BINDING_3D);
                GL.ThrowIfError();

                oldGL_TEXTURE_BINDING_3D = GL_TEXTURE_BINDING_3D.Update(newGL_TEXTURE_BINDING_3D);
                glTextureBinding3DByTextureUnit[GL_ACTIVE_TEXTURE] = newGL_TEXTURE_BINDING_3D;
            }
        }

        private void Apply_BindVertexArrayObject()
        {
            if (forced || !GL.IsDirectStateAccessAvailable)
            {
                GL.BindVertexArray(newGL_VERTEX_ARRAY_BINDING);
                GL.ThrowIfError();

                oldGL_VERTEX_ARRAY_BINDING = OpenGLState.GL_VERTEX_ARRAY_BINDING.Update(newGL_VERTEX_ARRAY_BINDING);
                oldGL_ELEMENT_ARRAY_BUFFER_BINDING = OpenGLState.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(newGL_ELEMENT_ARRAY_BUFFER_BINDING);
            }
        }

        private void Apply_BindArrayBuffer()
        {
            if (forced || !GL.IsDirectStateAccessAvailable)
            {
                GL.BindBuffer(GL.GL_ARRAY_BUFFER, newGL_ARRAY_BUFFER_BINDING);
                GL.ThrowIfError();

                oldGL_ARRAY_BUFFER_BINDING = OpenGLState.GL_ARRAY_BUFFER_BINDING.Update(newGL_ARRAY_BUFFER_BINDING);
            }
        }

        private void Apply_BindElementArrayBuffer()
        {
            if (forced || !GL.IsDirectStateAccessAvailable)
            {
                GL.BindBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, newGL_ELEMENT_ARRAY_BUFFER_BINDING);
                GL.ThrowIfError();

                oldGL_ELEMENT_ARRAY_BUFFER_BINDING = OpenGLState.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(newGL_ELEMENT_ARRAY_BUFFER_BINDING);
            }
        }

        private void Apply_BindFramebuffer()
        {
            if (forced || !GL.IsDirectStateAccessAvailable)
            {
                GL.BindFramebuffer(GL.GL_FRAMEBUFFER, newGL_FRAMEBUFFER_BINDING);
                GL.ThrowIfError();

                oldGL_FRAMEBUFFER_BINDING = OpenGLState.GL_FRAMEBUFFER_BINDING.Update(newGL_FRAMEBUFFER_BINDING);
            }
        }

        private void Apply_BindRenderbuffer()
        {
            if (forced || !GL.IsDirectStateAccessAvailable)
            {
                GL.BindRenderbuffer(GL.GL_RENDERBUFFER, newGL_RENDERBUFFER_BINDING);
                GL.ThrowIfError();

                oldGL_RENDERBUFFER_BINDING = GL_RENDERBUFFER_BINDING.Update(newGL_RENDERBUFFER_BINDING);
            }
        }

        private void Apply_UseProgram()
        {
            GL.UseProgram(newGL_CURRENT_PROGRAM);
            GL.ThrowIfError();

            oldCurrentProgram = currentProgram;
            currentProgram = newCurrentProgram;

            oldGL_CURRENT_PROGRAM = OpenGLState.GL_CURRENT_PROGRAM.Update(newGL_CURRENT_PROGRAM);
        }

        private void Apply_CreateElementArrayBuffer(out UInt32 buffer)
        {
            if (GL.IsDirectStateAccessCreateAvailable)
            {
                buffer = GL.CreateBuffer();
                GL.ThrowIfError();
            }
            else
            {
                buffer = GL.GenBuffer();
                GL.ThrowIfError();

                if (!GL.IsDirectStateAccessAvailable)
                {
                    GL.BindBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, buffer);
                    GL.ThrowIfError();

                    newGL_ELEMENT_ARRAY_BUFFER_BINDING = buffer;
                    oldGL_ELEMENT_ARRAY_BUFFER_BINDING = OpenGLState.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(buffer);
                }
            }
        }

        private void Apply_CreateArrayBuffer(out UInt32 buffer)
        {
            if (GL.IsDirectStateAccessCreateAvailable)
            {
                buffer = GL.CreateBuffer();
                GL.ThrowIfError();
            }
            else
            {
                buffer = GL.GenBuffer();
                GL.ThrowIfError();

                if (!GL.IsDirectStateAccessAvailable)
                {
                    GL.BindBuffer(GL.GL_ARRAY_BUFFER, buffer);
                    GL.ThrowIfError();

                    newGL_ARRAY_BUFFER_BINDING = buffer;
                    oldGL_ARRAY_BUFFER_BINDING = OpenGLState.GL_ARRAY_BUFFER_BINDING.Update(buffer);
                }
            }
        }

        private void Apply_CreateTexture2D(out UInt32 texture)
        {
            if (GL.IsDirectStateAccessCreateAvailable)
            {
                texture = GL.CreateTexture(GL.GL_TEXTURE_2D);
                GL.ThrowIfError();
            }
            else
            {
                texture = GL.GenTexture();
                GL.ThrowIfError();

                if (!GL.IsDirectStateAccessAvailable)
                {
                    GL.BindTexture(GL.GL_TEXTURE_2D, texture);
                    GL.ThrowIfError();

                    newGL_TEXTURE_BINDING_2D = texture;
                    oldGL_TEXTURE_BINDING_2D = GL_TEXTURE_BINDING_2D.Update(texture);
                    glTextureBinding2DByTextureUnit[GL_ACTIVE_TEXTURE] = texture;
                }
            }
        }

        private void Apply_CreateTexture3D(out UInt32 texture)
        {
            if (GL.IsDirectStateAccessCreateAvailable)
            {
                texture = GL.CreateTexture(GL.GL_TEXTURE_3D);
                GL.ThrowIfError();
            }
            else
            {
                texture = GL.GenTexture();
                GL.ThrowIfError();

                if (!GL.IsDirectStateAccessAvailable)
                {
                    GL.BindTexture(GL.GL_TEXTURE_3D, texture);
                    GL.ThrowIfError();

                    newGL_TEXTURE_BINDING_3D = texture;
                    oldGL_TEXTURE_BINDING_3D = GL_TEXTURE_BINDING_3D.Update(texture);
                    glTextureBinding3DByTextureUnit[GL_ACTIVE_TEXTURE] = texture;
                }
            }
        }

        private void Apply_CreateFramebuffer(out UInt32 framebuffer)
        {
            if (GL.IsDirectStateAccessCreateAvailable)
            {
                framebuffer = GL.CreateFramebuffer();
            }
            else
            {
                framebuffer = GL.GenFramebuffer();
                GL.ThrowIfError();

                if (!GL.IsDirectStateAccessAvailable)
                {
                    GL.BindFramebuffer(GL.GL_FRAMEBUFFER, framebuffer);
                    GL.ThrowIfError();

                    newGL_FRAMEBUFFER_BINDING = framebuffer;
                    oldGL_FRAMEBUFFER_BINDING = OpenGLState.GL_FRAMEBUFFER_BINDING.Update(framebuffer);
                }
            }
        }

        private void Apply_CreateRenderbuffer(out UInt32 renderbuffer)
        {
            if (GL.IsDirectStateAccessCreateAvailable)
            {
                renderbuffer = GL.CreateRenderbuffer();
            }
            else
            {
                renderbuffer = GL.GenRenderbuffer();
                GL.ThrowIfError();

                if (!GL.IsDirectStateAccessAvailable)
                {
                    GL.BindRenderbuffer(GL.GL_RENDERBUFFER, renderbuffer);
                    GL.ThrowIfError();

                    newGL_RENDERBUFFER_BINDING = renderbuffer;
                    oldGL_RENDERBUFFER_BINDING = GL_RENDERBUFFER_BINDING.Update(renderbuffer);
                }
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

                case OpenGLStateType.ActiveTexture:
                    Dispose_ActiveTexture();
                    break;

                case OpenGLStateType.BindTexture2D:
                    Dispose_BindTexture2D();
                    break;

                case OpenGLStateType.BindTexture3D:
                    Dispose_BindTexture3D();
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

                case OpenGLStateType.BindRenderbuffer:
                    Dispose_BindRenderbuffer();
                    break;

                case OpenGLStateType.UseProgram:
                    Dispose_UseProgram();
                    break;

                case OpenGLStateType.CreateTexture2D:
                    Dispose_CreateTexture2D();
                    break;

                case OpenGLStateType.CreateTexture3D:
                    Dispose_CreateTexture3D();
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

                case OpenGLStateType.CreateRenderbuffer:
                    Dispose_CreateRenderbuffer();
                    break;

                default:
                    throw new InvalidOperationException();
            }

            OpenGLState.VerifyCache();

            disposed = true;
            pool.Release(this);
        }

        private void Dispose_ActiveTexture()
        {
            GL.ActiveTexture(oldGL_ACTIVE_TEXTURE);
            GL.ThrowIfError();

            glTextureBinding2DByTextureUnit.TryGetValue(oldGL_ACTIVE_TEXTURE, out var tb2d);
            glTextureBinding3DByTextureUnit.TryGetValue(oldGL_ACTIVE_TEXTURE, out var tb3d);

            OpenGLState.GL_ACTIVE_TEXTURE.Update(oldGL_ACTIVE_TEXTURE);
            GL_TEXTURE_BINDING_2D.Update(tb2d);
            GL_TEXTURE_BINDING_3D.Update(tb3d);
        }

        private void Dispose_BindTexture2D()
        {
            if (forced || !GL.IsDirectStateAccessAvailable)
            {
                GL.BindTexture(GL.GL_TEXTURE_2D, oldGL_TEXTURE_BINDING_2D);
                GL.ThrowIfError();

                GL_TEXTURE_BINDING_2D.Update(oldGL_TEXTURE_BINDING_2D);
                glTextureBinding2DByTextureUnit[GL_ACTIVE_TEXTURE] = oldGL_TEXTURE_BINDING_2D;
            }
        }

        private void Dispose_BindTexture3D()
        {
            if (forced || !GL.IsDirectStateAccessAvailable)
            {
                GL.BindTexture(GL.GL_TEXTURE_3D, oldGL_TEXTURE_BINDING_3D);
                GL.ThrowIfError();

                GL_TEXTURE_BINDING_2D.Update(oldGL_TEXTURE_BINDING_3D);
                glTextureBinding3DByTextureUnit[GL_ACTIVE_TEXTURE] = oldGL_TEXTURE_BINDING_3D;
            }
        }

        private void Dispose_BindVertexArrayObject()
        {
            if (forced || !GL.IsDirectStateAccessAvailable)
            {
                GL.BindVertexArray(oldGL_VERTEX_ARRAY_BINDING);
                GL.ThrowIfError();

                OpenGLState.GL_VERTEX_ARRAY_BINDING.Update(oldGL_VERTEX_ARRAY_BINDING);
                OpenGLState.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(oldGL_ELEMENT_ARRAY_BUFFER_BINDING);
            }
        }

        private void Dispose_BindArrayBuffer()
        {
            if (forced || !GL.IsDirectStateAccessAvailable)
            {
                GL.BindBuffer(GL.GL_ARRAY_BUFFER, oldGL_ARRAY_BUFFER_BINDING);
                GL.ThrowIfError();

                OpenGLState.GL_ARRAY_BUFFER_BINDING.Update(oldGL_ARRAY_BUFFER_BINDING);
            }
        }

        private void Dispose_BindElementArrayBuffer()
        {
            if (forced || !GL.IsDirectStateAccessAvailable)
            {
                GL.BindBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, oldGL_ELEMENT_ARRAY_BUFFER_BINDING);
                GL.ThrowIfError();

                OpenGLState.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(oldGL_ELEMENT_ARRAY_BUFFER_BINDING);
            }
        }

        private void Dispose_BindFramebuffer()
        {
            if (forced || !GL.IsDirectStateAccessAvailable)
            {
                GL.BindFramebuffer(GL.GL_FRAMEBUFFER, oldGL_FRAMEBUFFER_BINDING);
                GL.ThrowIfError();

                OpenGLState.GL_FRAMEBUFFER_BINDING.Update(oldGL_FRAMEBUFFER_BINDING);
            }
        }

        private void Dispose_BindRenderbuffer()
        {
            if (forced || !GL.IsDirectStateAccessAvailable)
            {
                GL.BindRenderbuffer(GL.GL_RENDERBUFFER, oldGL_RENDERBUFFER_BINDING);
                GL.ThrowIfError();

                GL_RENDERBUFFER_BINDING.Update(oldGL_RENDERBUFFER_BINDING);
            }
        }

        private void Dispose_UseProgram()
        {
            GL.UseProgram(oldGL_CURRENT_PROGRAM);
            GL.ThrowIfError();

            currentProgram = oldCurrentProgram;

            OpenGLState.GL_CURRENT_PROGRAM.Update(oldGL_CURRENT_PROGRAM);
        }

        private void Dispose_CreateArrayBuffer()
        {
            if (!GL.IsDirectStateAccessAvailable)
            {
                GL.BindBuffer(GL.GL_ARRAY_BUFFER, oldGL_ARRAY_BUFFER_BINDING);
                GL.ThrowIfError();

                OpenGLState.GL_ARRAY_BUFFER_BINDING.Update(oldGL_ARRAY_BUFFER_BINDING);
            }
        }

        private void Dispose_CreateElementArrayBuffer()
        {
            if (!GL.IsDirectStateAccessAvailable)
            {
                GL.BindBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, oldGL_ELEMENT_ARRAY_BUFFER_BINDING);
                GL.ThrowIfError();

                OpenGLState.GL_ELEMENT_ARRAY_BUFFER_BINDING.Update(oldGL_ELEMENT_ARRAY_BUFFER_BINDING);
            }
        }

        private void Dispose_CreateTexture2D()
        {
            if (!GL.IsDirectStateAccessAvailable)
            {
                GL.BindTexture(GL.GL_TEXTURE_2D, oldGL_TEXTURE_BINDING_2D);
                GL.ThrowIfError();

                GL_TEXTURE_BINDING_2D.Update(oldGL_TEXTURE_BINDING_2D);
                glTextureBinding2DByTextureUnit[GL_ACTIVE_TEXTURE] = oldGL_TEXTURE_BINDING_2D;
            }
        }

        private void Dispose_CreateTexture3D()
        {
            if (!GL.IsDirectStateAccessAvailable)
            {
                GL.BindTexture(GL.GL_TEXTURE_3D, oldGL_TEXTURE_BINDING_3D);
                GL.ThrowIfError();

                GL_TEXTURE_BINDING_3D.Update(oldGL_TEXTURE_BINDING_3D);
                glTextureBinding3DByTextureUnit[GL_ACTIVE_TEXTURE] = oldGL_TEXTURE_BINDING_3D;
            }
        }

        private void Dispose_CreateFramebuffer()
        {
            if (!GL.IsDirectStateAccessAvailable)
            {
                GL.BindFramebuffer(GL.GL_FRAMEBUFFER, oldGL_FRAMEBUFFER_BINDING);
                GL.ThrowIfError();

                OpenGLState.GL_FRAMEBUFFER_BINDING.Update(oldGL_FRAMEBUFFER_BINDING);
            }
        }

        private void Dispose_CreateRenderbuffer()
        {
            if (!GL.IsDirectStateAccessAvailable)
            {
                GL.BindRenderbuffer(GL.GL_RENDERBUFFER, oldGL_RENDERBUFFER_BINDING);
                GL.ThrowIfError();

                GL_RENDERBUFFER_BINDING.Update(oldGL_RENDERBUFFER_BINDING);
            }
        }

        /// <summary>
        /// Gets the current shader program, if any is currently active.
        /// </summary>
        public static OpenGLShaderProgram CurrentProgram
        {
            get { return currentProgram; }
        }

        /// <summary>
        /// Gets or sets the value to which the color buffer is cleared.
        /// </summary>
        public static Color ClearColor
        {
            get { return (Color)cachedClearColor; }
            set { CachedClearColor.TryUpdate(ref cachedClearColor, value); }
        }

        /// <summary>
        /// Gets or sets the value to which the depth buffer is cleared.
        /// </summary>
        public static Double ClearDepth
        {
            get { return (Double)cachedClearDepth; }
            set { CachedClearDepth.TryUpdate(ref cachedClearDepth, value); }
        }

        /// <summary>
        /// Gets or sets the value to which the stencil buffer is cleared.
        /// </summary>
        public static Int32 ClearStencil
        {
            get { return (Int32)cachedClearStencil; }
            set { CachedClearStencil.TryUpdate(ref cachedClearStencil, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating which color channels are enabled for writing.
        /// </summary>
        public static ColorWriteChannels ColorMask
        {
            get { return (ColorWriteChannels)cachedColorMask; }
            set { CachedColorMask.TryUpdate(ref cachedColorMask, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether GL_DEPTH_TEST is enabled.
        /// </summary>
        public static Boolean DepthTestEnabled
        {
            get { return (Boolean)cachedDepthTestEnabled; }
            set { CachedCapability.TryUpdate(GL.GL_DEPTH_TEST, ref cachedDepthTestEnabled, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether writing into the depth buffer is enabled.
        /// </summary>
        public static Boolean DepthMask
        {
            get { return (Boolean)cachedDepthMask; }
            set { CachedDepthMask.TryUpdate(ref cachedDepthMask, value); }
        }

        /// <summary>
        /// Gets or sets the depth comparison function.
        /// </summary>
        public static UInt32 DepthFunc
        {
            get { return (UInt32)cachedDepthFunc; }
            set { CachedDepthFunc.TryUpdate(ref cachedDepthFunc, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether GL_STENCIL_TEST is enabled.
        /// </summary>
        public static Boolean StencilTestEnabled
        {
            get { return (Boolean)cachedStencilTestEnabled; }
            set { CachedCapability.TryUpdate(GL.GL_STENCIL_TEST, ref cachedStencilTestEnabled, value); }
        }

        /// <summary>
        /// Gets or sets the combined stencil function.
        /// </summary>
        public static CachedStencilFunc StencilFuncCombined
        {
            get { return cachedStencilFuncFront; }
            set { CachedStencilFunc.TryUpdateCombined(ref cachedStencilFuncFront, ref cachedStencilFuncBack, value); }
        }

        /// <summary>
        /// Gets or sets the front stencil function.
        /// </summary>
        public static CachedStencilFunc StencilFuncFront
        {
            get { return cachedStencilFuncFront; }
            set { CachedStencilFunc.TryUpdate(GL.GL_FRONT, ref cachedStencilFuncFront, value); }
        }

        /// <summary>
        /// Gets or sets the back stencil function.
        /// </summary>
        public static CachedStencilFunc StencilFuncBack
        {
            get { return cachedStencilFuncFront; }
            set { CachedStencilFunc.TryUpdate(GL.GL_BACK, ref cachedStencilFuncBack, value); }
        }

        /// <summary>
        /// Gets or sets the combined stencil operation.
        /// </summary>
        public static CachedStencilOp StencilOpCombined
        {
            get { return cachedStencilOpFront; }
            set { CachedStencilOp.TryUpdateCombined(ref cachedStencilOpFront, ref cachedStencilOpBack, value); }
        }

        /// <summary>
        /// Gets or sets the front stencil operation.
        /// </summary>
        public static CachedStencilOp StencilOpFront
        {
            get { return cachedStencilOpFront; }
            set { CachedStencilOp.TryUpdate(GL.GL_FRONT, ref cachedStencilOpFront, value); }
        }

        /// <summary>
        /// Gets or sets the back stencil operation.
        /// </summary>
        public static CachedStencilOp StencilOpBack
        {
            get { return cachedStencilOpBack; }
            set { CachedStencilOp.TryUpdate(GL.GL_BACK, ref cachedStencilOpBack, value); }
        }

        /// <summary>
        /// Gets or sets the stencil mask.
        /// </summary>
        public static UInt32 StencilMask
        {
            get { return (UInt32)cachedStencilMask; }
            set { CachedStencilMask.TryUpdate(ref cachedStencilMask, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether GL_BLEND is enabled.
        /// </summary>
        public static Boolean BlendEnabled
        {
            get { return (Boolean)cachedBlendEnabled; }
            set { CachedCapability.TryUpdate(GL.GL_BLEND, ref cachedBlendEnabled, value); }
        }

        /// <summary>
        /// Gets or sets the blending color.
        /// </summary>
        public static Color BlendColor
        {
            get { return (Color)cachedBlendColor; }
            set { CachedBlendColor.TryUpdate(ref cachedBlendColor, value); }
        }

        /// <summary>
        /// Gets or sets the blending equation.
        /// </summary>
        public static CachedBlendEquation BlendEquation
        {
            get { return cachedBlendEquation; }
            set { CachedBlendEquation.TryUpdate(ref cachedBlendEquation, value); }
        }

        /// <summary>
        /// Gets or sets the blending function.
        /// </summary>
        public static CachedBlendFunction BlendFunction
        {
            get { return cachedBlendFunction; }
            set { CachedBlendFunction.TryUpdate(ref cachedBlendFunction, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether face culling is enabled.
        /// </summary>
        public static Boolean CullingEnabled
        {
            get { return (Boolean)cachedCullingEnabled;  }
            set { CachedCapability.TryUpdate(GL.GL_CULL_FACE, ref cachedCullingEnabled, value); }
        }

        /// <summary>
        /// Gets or sets the polygon face which is being culled.
        /// </summary>
        public static UInt32 CulledFace
        {
            get { return (UInt32)cachedCulledFace; }
            set { CachedCulledFace.TryUpdate(ref cachedCulledFace, value); }
        }

        /// <summary>
        /// Gets or sets the front face of polygons.
        /// </summary>
        public static UInt32 FrontFace
        {
            get { return (UInt32)cachedFrontFace; }
            set { CachedFrontFace.TryUpdate(ref cachedFrontFace, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether polygon offsets are enabled.
        /// </summary>
        public static Boolean PolygonOffsetEnabled
        {
            get { return (Boolean)cachedPolygonOffsetFillEnabled; }
            set { CachedCapability.TryUpdate(GL.GL_POLYGON_OFFSET_FILL, ref cachedPolygonOffsetFillEnabled, value); }
        }

        /// <summary>
        /// Gets or sets the polygon offset values.
        /// </summary>
        public static CachedPolygonOffset PolygonOffset
        {
            get { return cachedPolygonOffset; }
            set { CachedPolygonOffset.TryUpdate(ref cachedPolygonOffset, value); }
        }

        /// <summary>
        /// Gets or sets the polygon rasterization mode.
        /// </summary>
        public static UInt32 PolygonMode
        {
            get { return (UInt32)cachedPolygonMode; }
            set { CachedPolygonMode.TryUpdate(ref cachedPolygonMode, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the scissor test is enabled.
        /// </summary>
        public static Boolean ScissorTestEnabled
        {
            get { return (Boolean)cachedScissorTestEnabled; }
            set { CachedCapability.TryUpdate(GL.GL_SCISSOR_TEST, ref cachedScissorTestEnabled, value); }
        }

        /// <summary>
        /// Gets the cached value of GL_ACTIVE_TEXTURE.
        /// </summary>
        public static OpenGLStateInteger GL_ACTIVE_TEXTURE { get { return glActiveTexture; } }

        /// <summary>
        /// Gets the cached value of GL_TEXTURE_BINDING_2D.
        /// </summary>
        public static OpenGLStateInteger GL_TEXTURE_BINDING_2D { get { return glTextureBinding2D; } }

        /// <summary>
        /// Gets the cached value of GL_TEXTURE_BINDING_3D.
        /// </summary>
        public static OpenGLStateInteger GL_TEXTURE_BINDING_3D { get { return glTextureBinding3D; } }

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
        /// Gets the cached value of GL_RENDERBUFFER_BINDING.
        /// </summary>
        public static OpenGLStateInteger GL_RENDERBUFFER_BINDING { get { return glRenderbufferBinding; } }

        /// <summary>
        /// Gets the cached value of GL_CURRENT_PROGRAM.
        /// </summary>
        public static OpenGLStateInteger GL_CURRENT_PROGRAM { get { return glCurrentProgram; } }

        // State values.
        private OpenGLStateType stateType;
        private Boolean disposed;
        private Boolean forced;

        private UInt32 newGL_ACTIVE_TEXTURE = GL.GL_TEXTURE0;
        private UInt32 newGL_TEXTURE_BINDING_2D;
        private UInt32 newGL_TEXTURE_BINDING_3D;
        private UInt32 newGL_VERTEX_ARRAY_BINDING;
        private UInt32 newGL_ARRAY_BUFFER_BINDING;
        private UInt32 newGL_ELEMENT_ARRAY_BUFFER_BINDING;
        private UInt32 newGL_FRAMEBUFFER_BINDING;
        private UInt32 newGL_RENDERBUFFER_BINDING;
        private UInt32 newGL_CURRENT_PROGRAM;
        private OpenGLShaderProgram newCurrentProgram;

        private UInt32 oldGL_ACTIVE_TEXTURE = GL.GL_TEXTURE0;
        private UInt32 oldGL_TEXTURE_BINDING_2D;
        private UInt32 oldGL_TEXTURE_BINDING_3D;
        private UInt32 oldGL_VERTEX_ARRAY_BINDING;
        private UInt32 oldGL_ARRAY_BUFFER_BINDING;
        private UInt32 oldGL_ELEMENT_ARRAY_BUFFER_BINDING;
        private UInt32 oldGL_FRAMEBUFFER_BINDING;
        private UInt32 oldGL_RENDERBUFFER_BINDING;
        private UInt32 oldGL_CURRENT_PROGRAM;
        private OpenGLShaderProgram oldCurrentProgram;

        // Cached OpenGL state values.
        private static readonly OpenGLStateInteger[] glCachedIntegers;
        private static readonly OpenGLStateInteger glActiveTexture = new OpenGLStateInteger("GL_ACTIVE_TEXTURE", GL.GL_ACTIVE_TEXTURE, (int)GL.GL_TEXTURE0);
        private static readonly OpenGLStateInteger glTextureBinding2D = new OpenGLStateInteger("GL_TEXTURE_BINDING_2D", GL.GL_TEXTURE_BINDING_2D);
        private static readonly OpenGLStateInteger glTextureBinding3D = new OpenGLStateInteger("GL_TEXTURE_BINDING_3D", GL.GL_TEXTURE_BINDING_3D);
        private static readonly OpenGLStateInteger glVertexArrayBinding = new OpenGLStateInteger("GL_VERTEX_ARRAY_BINDING", GL.GL_VERTEX_ARRAY_BINDING);
        private static readonly OpenGLStateInteger glArrayBufferBinding = new OpenGLStateInteger("GL_ARRAY_BUFFER_BINDING", GL.GL_ARRAY_BUFFER_BINDING);
        private static readonly OpenGLStateInteger glElementArrayBufferBinding = new OpenGLStateInteger("GL_ELEMENT_ARRAY_BUFFER_BINDING", GL.GL_ELEMENT_ARRAY_BUFFER_BINDING);
        private static readonly OpenGLStateInteger glFramebufferBinding = new OpenGLStateInteger("GL_FRAMEBUFFER_BINDING", GL.GL_FRAMEBUFFER_BINDING);
        private static readonly OpenGLStateInteger glRenderbufferBinding = new OpenGLStateInteger("GL_RENDERBUFFER_BINDING", GL.GL_RENDERBUFFER_BINDING);
        private static readonly OpenGLStateInteger glCurrentProgram = new OpenGLStateInteger("GL_CURRENT_PROGRAM", GL.GL_CURRENT_PROGRAM);

        private static OpenGLShaderProgram currentProgram;

        private static CachedClearColor cachedClearColor;
        private static CachedClearDepth cachedClearDepth;
        private static CachedClearStencil cachedClearStencil;
        private static CachedColorMask cachedColorMask;
        private static CachedCapability cachedDepthTestEnabled;
        private static CachedDepthMask cachedDepthMask;
        private static CachedDepthFunc cachedDepthFunc;
        private static CachedCapability cachedStencilTestEnabled;
        private static CachedStencilFunc cachedStencilFuncFront;
        private static CachedStencilFunc cachedStencilFuncBack;
        private static CachedStencilOp cachedStencilOpFront;
        private static CachedStencilOp cachedStencilOpBack;
        private static CachedStencilMask cachedStencilMask;
        private static CachedCapability cachedBlendEnabled;
        private static CachedBlendColor cachedBlendColor;
        private static CachedBlendEquation cachedBlendEquation;
        private static CachedBlendFunction cachedBlendFunction;
        private static CachedCapability cachedCullingEnabled;
        private static CachedCulledFace cachedCulledFace;
        private static CachedFrontFace cachedFrontFace;
        private static CachedCapability cachedPolygonOffsetFillEnabled;
        private static CachedPolygonOffset cachedPolygonOffset;
        private static CachedPolygonMode cachedPolygonMode;
        private static CachedCapability cachedScissorTestEnabled;

        private static readonly Dictionary<UInt32, UInt32> glTextureBinding2DByTextureUnit =
            new Dictionary<UInt32, UInt32>();
        private static readonly Dictionary<UInt32, UInt32> glTextureBinding3DByTextureUnit =
            new Dictionary<UInt32, UInt32>();

        // The pool of state objects.
        private static readonly IPool<OpenGLState> pool = new ExpandingPool<OpenGLState>(1, () => new OpenGLState());
    }
}