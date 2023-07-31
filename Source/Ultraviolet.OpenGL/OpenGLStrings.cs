using System.Reflection;
using Ultraviolet.Core.Text;

namespace Ultraviolet.OpenGL
{
    /// <summary>
    /// Contains the implementation's string resources.
    /// </summary>
    public static class OpenGLStrings
    {
        /// <summary>
        /// Initializes the OpenGLStrings type.
        /// </summary>
        static OpenGLStrings()
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var stream = asm.GetManifestResourceStream("Ultraviolet.OpenGL.Resources.Strings.xml"))
            {
                StringDatabase.LoadFromStream(stream);
            }
        }

        private static readonly LocalizationDatabase StringDatabase = new LocalizationDatabase();

#pragma warning disable 1591
        public static readonly StringResource DoesNotMeetMinimumVersionRequirement  = new StringResource(StringDatabase, "DOES_NOT_MEET_MINIMUM_VERSION_REQUIREMENT");
        public static readonly StringResource UnsupportedGraphicsDevice             = new StringResource(StringDatabase, "UNSUPPORTED_GRAPHICS_DEVICE");
        public static readonly StringResource UnsupportedShaderType                 = new StringResource(StringDatabase, "UNSUPPORTED_SHADER_TYPE");
        public static readonly StringResource UnsupportedDataType                   = new StringResource(StringDatabase, "UNSUPPORTED_DATA_TYPE");
        public static readonly StringResource UnsupportedElementType                = new StringResource(StringDatabase, "UNSUPPORTED_ELEMENT_TYPE");
        public static readonly StringResource UnsupportedImageType                  = new StringResource(StringDatabase, "UNSUPPORTED_IMAGE_TYPE");
        public static readonly StringResource UnsupportedVertexFormat               = new StringResource(StringDatabase, "UNSUPPORTED_VERTEX_FORMAT");
        public static readonly StringResource UnsupportedIndexFormat                = new StringResource(StringDatabase, "UNSUPPORTED_INDEX_FORMAT");
        public static readonly StringResource UnsupportedPrimitiveType              = new StringResource(StringDatabase, "UNSUPPORTED_PRIMITIVE_TYPE");
        public static readonly StringResource DataTooLargeForBuffer                 = new StringResource(StringDatabase, "DATA_TOO_LARGE_FOR_BUFFER");
        public static readonly StringResource NoGeometryStream                      = new StringResource(StringDatabase, "NO_GEOMETRY_STREAM");
        public static readonly StringResource InvalidGeometryStream                 = new StringResource(StringDatabase, "INVALID_GEOMETRY_STREAM");
        public static readonly StringResource NoEffect                              = new StringResource(StringDatabase, "NO_EFFECT");
        public static readonly StringResource RenderTargetNeedsBuffers              = new StringResource(StringDatabase, "RENDER_TARGET_NEEDS_BUFFERS");
        public static readonly StringResource RenderTargetFramebufferIsNotComplete  = new StringResource(StringDatabase, "RENDER_TARGET_FRAMEBUFFER_IS_NOT_COMPLETE");
        public static readonly StringResource RenderBufferCannotBeUsedAsTexture     = new StringResource(StringDatabase, "RENDER_BUFFER_CANNOT_BE_USED_AS_TEXTURE");
        public static readonly StringResource RenderBufferWillNotBeSampled          = new StringResource(StringDatabase, "RENDER_BUFFER_WILL_NOT_BE_SAMPLED");
        public static readonly StringResource RenderBufferIsWrongSize               = new StringResource(StringDatabase, "RENDER_BUFFER_IS_WRONG_SIZE");
        public static readonly StringResource RenderBufferExceedsTargetCapacity     = new StringResource(StringDatabase, "RENDER_BUFFER_EXCEEDS_TARGET_CAPACITY");
        public static readonly StringResource ResourceCannotBeReadWhileWriting      = new StringResource(StringDatabase, "RESOURCE_CANNOT_BE_READ_WHILE_WRITING");
        public static readonly StringResource ResourceCannotBeWrittenWhileReading   = new StringResource(StringDatabase, "RESOURCE_CANNOT_BE_WRITTEN_WHILE_READING");
        public static readonly StringResource ResourceNotBound                      = new StringResource(StringDatabase, "RESOURCE_NOT_BOUND");
        public static readonly StringResource StaleOpenGLCache                      = new StringResource(StringDatabase, "STALE_OPENGL_CACHE");
        public static readonly StringResource EffectUniformTypeMismatch             = new StringResource(StringDatabase, "EFFECT_UNIFORM_TYPE_MISMATCH");
        public static readonly StringResource EffectParameterCannotFindUniform      = new StringResource(StringDatabase, "EFFECT_PARAMETER_CANNOT_FIND_UNIFORM");
        public static readonly StringResource ImmutableValueAlreadySet              = new StringResource(StringDatabase, "IMMUTABLE_VALUE_ALREADY_SET");
        public static readonly StringResource InvalidEffectPass                     = new StringResource(StringDatabase, "INVALID_EFFECT_PASS");
        public static readonly StringResource DocumentDoesNotContainEffect          = new StringResource(StringDatabase, "DOCUMENT_DOES_NOT_CONTAIN_EFFECT");
        public static readonly StringResource EffectMustHaveVertexAndFragmentShader = new StringResource(StringDatabase, "EFFECT_MUST_HAVE_VERTEX_AND_FRAGMENT_SHADER");
        public static readonly StringResource EffectMustHaveTechniques              = new StringResource(StringDatabase, "EFFECT_MUST_HAVE_TECHNIQUES");
        public static readonly StringResource EffectTechniqueMustHavePasses         = new StringResource(StringDatabase, "EFFECT_TECHNIQUE_MUST_HAVE_PASSES");
        public static readonly StringResource TechniqueBelongsToDifferentEffect     = new StringResource(StringDatabase, "TECHNIQUE_BELONGS_TO_DIFFERENT_EFFECT");
        public static readonly StringResource DebugOutputNotSupported               = new StringResource(StringDatabase, "DEBUG_OUTPUT_NOT_SUPPORTED");
        public static readonly StringResource ShaderUniformHasNoSource              = new StringResource(StringDatabase, "SHADER_UNIFORM_HAS_NO_SOURCE");
        public static readonly StringResource UnsupportedFillModeGLES               = new StringResource(StringDatabase, "UNSUPPORTED_FILLMODE_GLES");
        public static readonly StringResource UnsupportedLODBiasGLES                = new StringResource(StringDatabase, "UNSUPPORTED_LOD_BIAS_GLES");
        public static readonly StringResource InvalidOperationWhileBound            = new StringResource(StringDatabase, "INVALID_OPERATION_WHILE_BOUND");
        public static readonly StringResource CannotResizeAttachedRenderBuffer      = new StringResource(StringDatabase, "CANNOT_RESIZE_ATTACHED_RENDER_BUFFER");
        public static readonly StringResource TextureIsImmutable                    = new StringResource(StringDatabase, "TEXTURE_IS_IMMUTABLE");
        public static readonly StringResource RenderBufferIsImmutable               = new StringResource(StringDatabase, "RENDER_BUFFER_IS_IMMUTABLE");
        public static readonly StringResource RenderBufferAlreadyAttached           = new StringResource(StringDatabase, "RENDER_BUFFER_ALREADY_ATTACHED");
        public static readonly StringResource InvalidIncludedResource               = new StringResource(StringDatabase, "INVALID_INCLUDED_RESOURCE");
        public static readonly StringResource CannotIncludeShaderHeadersInStream    = new StringResource(StringDatabase, "CANNOT_INCLUDE_SHADER_HEADERS_IN_STREAM");
        public static readonly StringResource InstancedRenderingNotSupported        = new StringResource(StringDatabase, "INSTANCED_RENDERING_NOT_SUPPORTED");
        public static readonly StringResource DoublePrecisionVAttribsNotSupported   = new StringResource(StringDatabase, "DOUBLE_PRECISION_VATTRIBS_NOT_SUPPORTED");
        public static readonly StringResource IntegerVAttribsNotSupported           = new StringResource(StringDatabase, "INTEGER_VATTRIBS_NOT_SUPPORTED");
        public static readonly StringResource NonZeroBaseInstanceNotSupported       = new StringResource(StringDatabase, "NON_ZERO_BASE_INSTANCE_NOT_SUPPORTED");
        public static readonly StringResource SamplerDirectiveInvalidUniform        = new StringResource(StringDatabase, "SAMPLER_DIRECTIVE_INVALID_UNIFORM");
        public static readonly StringResource SamplerDirectiveAlreadyInUse          = new StringResource(StringDatabase, "SAMPLER_DIRECTIVE_ALREADY_IN_USE");
        public static readonly StringResource ImplicitEffectsMustLoadFromShaders    = new StringResource(StringDatabase, "IMPLICIT_EFFECTS_MUST_LOAD_FROM_SHADERS");
        public static readonly StringResource ShaderExternHasInvalidName            = new StringResource(StringDatabase, "SHADER_EXTERN_HAS_INVALID_NAME");
        public static readonly StringResource InvalidGraphicsConfiguration          = new StringResource(StringDatabase, "INVALID_GRAPHICS_CONFIGURATION");
        public static readonly StringResource EffectTechniqueAlreadyHasImpl         = new StringResource(StringDatabase, "EFFECT_TECHNIQUE_ALREADY_HAS_IMPL"); 
        public static readonly StringResource EffectPassAlreadyHasImpl              = new StringResource(StringDatabase, "EFFECT_PASS_ALREADY_HAS_IMPL"); 
        public static readonly StringResource EffectPassNotAssociatedWithEffect     = new StringResource(StringDatabase, "EFFECT_PASS_NOT_ASSOCIATED_WITH_EFFECT");
#pragma warning restore 1591
    }
}
