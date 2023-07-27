using System;

namespace Ultraviolet.OpenGL.Bindings
{
    static partial class GL
    {
        /// <summary>
        /// Gets a value indicating whether instanced rendering is supported by the current OpenGL context..
        /// </summary>
        public static Boolean IsInstancedRenderingAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether a non-zero base instance for instanced rendering is supported 
        /// by the current OpenGL context.
        /// </summary>
        public static Boolean IsNonZeroBaseInstanceAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether Sampler Objects are supported by the current OpenGL context.
        /// </summary>
        public static Boolean IsSamplerObjectAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether anisotropic filtering is supported by the current OpenGL context.
        /// </summary>
        public static Boolean IsAnisotropicFilteringAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether glVertexAttribBinding() is supported by the current OpenGL context.
        /// </summary>
        public static Boolean IsVertexAttribBindingAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether texture storage is supported by the current OpenGL context.
        /// </summary>
        public static Boolean IsTextureStorageAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether GL_TEXTURE_MAX_LEVEL is supported by the current OpenGL context.
        /// </summary>
        public static Boolean IsTextureMaxLevelSupported { get; private set; }

        /// <summary>
        /// Gets a value indicating whether GL_TEXTURE_LOD_BIAS is supported by the current OpenGL context.
        /// </summary>
        public static Boolean IsMapMapLevelOfDetailBiasAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether glMapBufferRange() is supported by the current OpenGL context.
        /// </summary>
        public static Boolean IsMapBufferRangeAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether sRGB encoded buffers are supported by the current OpenGL context.
        /// </summary>
        public static Boolean IsHardwareSrgbSupportAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether GL_FRAMEBUFFER_SRGB is supported by the current OpenGL context.
        /// </summary>
        public static Boolean IsFramebufferSrgbAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether glFramebufferTexture() is supported by the current OpenGL context.
        /// </summary>
        public static Boolean IsFramebufferTextureAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether glDrawBuffer() is supported by the current OpenGL context.
        /// </summary>
        public static Boolean IsDrawBufferAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether glDrawBuffers() is supported by the current OpenGL context.
        /// </summary>
        public static Boolean IsDrawBuffersAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether combined buffers for depth and stencil are supported by the
        /// current OpenGL context.
        /// </summary>
        public static Boolean IsCombinedDepthStencilAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether transposing matrix uniforms is supported by the current OpenGL context.
        /// </summary>
        public static Boolean IsMatrixTranspositionAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether glReadBuffer() is supported by the current OpenGL context.
        /// </summary>
        public static Boolean IsReadBufferAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether Vertex Array Objects (VAOs) are supported by the current OpenGL context.
        /// </summary>
        public static Boolean IsVertexArrayObjectAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether Direct State Access glCreateX() functions are supported by the 
        /// current OpenGL context.
        /// </summary>
        public static Boolean IsDirectStateAccessCreateAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether 3D textures are supported by the current OpenGL context.
        /// </summary>
        public static Boolean IsTexture3DAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether glVertexAttribI() is supported by the current OpenGL context.
        /// </summary>
        public static Boolean IsIntegerVertexAttribAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether double-precision parameters to glVertexAttribL() are supported 
        /// by the current OpenGL context.  
        /// </summary>
        public static Boolean IsDoublePrecisionVertexAttribAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether sized values (i.e. GL_RGBA8) for the internalformat parameter 
        /// of glTexImage() are supported by the current OpenGL context.
        /// </summary>
        public static Boolean IsSizedTextureInternalFormatAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether double-precision values for GL_DEPTH_CLEAR_VALUE are supported
        /// by the current OpenGL context.
        /// </summary>
        public static Boolean IsDoublePrecisionClearDepthAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether GL_POLYGON_MODE is supported by the current OpenGL context.
        /// </summary>
        public static Boolean IsPolygonModeAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether support for framebuffer invalidation is available.
        /// </summary>
        public static Boolean IsFramebufferInvalidationAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the ARB_invalidate_subdata extension is available. This property
        /// will also return true if running OpenGL 4.3 or higher.
        /// </summary>
        public static Boolean IsInvalidateSubdataAvailable { get; private set; }

        /// <summary>
        /// Initializes the context's feature flags.
        /// </summary>
        private static void InitializeFeatureFlags()
        {
            IsInstancedRenderingAvailable = !IsGLES2;

            IsNonZeroBaseInstanceAvailable = IsInstancedRenderingAvailable && !IsGLES && 
                (IsVersionAtLeast(4, 2) || IsExtensionSupported("GL_ARB_base_instance"));

            IsSamplerObjectAvailable = (IsGLES ? IsVersionAtLeast(3, 0) : IsVersionAtLeast(3, 3)) || 
                IsExtensionSupported("GL_ARB_sampler_objects");

            IsAnisotropicFilteringAvailable =
                IsExtensionSupported("GL_EXT_texture_filter_anisotropic");

            IsVertexAttribBindingAvailable = 
                IsExtensionSupported("GL_ARB_vertex_attrib_binding");

            IsTextureStorageAvailable = IsVersionAtLeast(4, 2) ||
                IsExtensionSupported("GL_ARB_texture_storage");

            IsTextureMaxLevelSupported = !IsGLES2;

            IsMapMapLevelOfDetailBiasAvailable = !IsGLES;

            IsMapBufferRangeAvailable = !IsGLES2 ||
                IsExtensionSupported("GL_ARB_map_buffer_range") ||
                IsExtensionSupported("GL_EXT_map_buffer_range");

            IsHardwareSrgbSupportAvailable = !IsGLES2 ||
                IsExtensionSupported("GL_EXT_sRGB");

            IsFramebufferSrgbAvailable = IsHardwareSrgbSupportAvailable && !IsGLES;

            IsFramebufferTextureAvailable = !IsGLES || IsVersionAtLeast(3, 2);

            IsDrawBuffersAvailable = IsGLES2 || 
                IsExtensionSupported("GL_ARB_draw_buffers");

            IsDrawBufferAvailable = !IsGLES;

            IsCombinedDepthStencilAvailable = !IsGLES2 || 
                IsExtensionSupported("GL_OES_packed_depth_stencil");

            IsMatrixTranspositionAvailable = !IsGLES2;

            IsReadBufferAvailable = !IsGLES2;

            IsVertexArrayObjectAvailable = !IsGLES2;

            IsDirectStateAccessCreateAvailable = IsVersionAtLeast(4, 5) || IsARBDirectStateAccessAvailable;

            IsTexture3DAvailable = !IsGLES2 || 
                IsExtensionSupported("GL_OES_texture_3D");

            IsIntegerVertexAttribAvailable = !IsGLES2 || 
                IsExtensionSupported("GL_EXT_gpu_shader4");

            IsDoublePrecisionVertexAttribAvailable = !IsGLES && IsVersionAtLeast(4, 1);

            IsSizedTextureInternalFormatAvailable = !IsGLES2;

            IsDoublePrecisionClearDepthAvailable = !IsGLES;

            IsPolygonModeAvailable = !IsGLES;

            IsFramebufferInvalidationAvailable = (IsGLES && IsVersionAtLeast(3, 0)) || (!IsGLES && IsVersionAtLeast(4, 3)) ||
                IsExtensionSupported("GL_ARB_invalidate_subdata") ||
                IsExtensionSupported("GL_EXT_discard_framebuffer");

            IsInvalidateSubdataAvailable = (!IsGLES && IsVersionAtLeast(4, 3)) ||
                IsExtensionSupported("GL_ARB_invalidate_subdata");
        }
    }
}
