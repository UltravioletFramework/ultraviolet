using System;
using System.Collections.Generic;
using NUnit.Framework;
using Ultraviolet.OpenGL.Graphics;
using Ultraviolet.TestApplication;

namespace Ultraviolet.Tests.Graphics
{
    [TestFixture]
    public partial class EffectTests : UltravioletApplicationTestFramework
    {
        [Test]
        [Category("Content")]
        [Description("Ensures that #include directives in GLSL shader source are correctly processed.")]
        public void Effect_GLSLIncludeDirectivesAreProcessed()
        {
            GivenAnUltravioletApplicationWithNoWindow()
                .WithContent(content =>
                {
                    var shaderSource = content.Load<ShaderSource>("Effects/IncludeDirective");

                    TheResultingString(shaderSource.Source)
                        .ShouldBe(
                            @"// This is IncludedDirective1.vert" + Environment.NewLine +
                            @"" + Environment.NewLine +
                            @"// This is IncludedDirective2.vert" + Environment.NewLine + 
                            @"" + Environment.NewLine);
                })
                .RunForOneFrame();
        }

        [Test]
        [Category("Content")]
        [Description("Ensures that #extern directives in GLSL shader source are correctly processed.")]
        public void Effect_GLSLExternDirectivesAreProcessed()
        {
            GivenAnUltravioletApplicationWithNoWindow()
                .WithContent(content =>
                {
                    var externs = new Dictionary<String, String>
                    {
                        { "FOO", "123" },
                        { "BAZ", "456" },
                    };

                    ShaderSource shaderSource;
                    shaderSource = content.Load<ShaderSource>("Effects/ExternDirective");
                    shaderSource = ShaderSource.ProcessExterns(shaderSource, externs);

                    TheResultingString(shaderSource.Source)
                        .ShouldBe(
                            @"#define FOO 123" + Environment.NewLine +
                            @"/*" + Environment.NewLine +
                            @"#extern ""BAR""" + Environment.NewLine +
                            @"*/" + Environment.NewLine +
                            @"#define BAZ 456" + Environment.NewLine);
                })
                .RunForOneFrame();
        }

        [Test]
        [Category("Content")]
        [Description("Ensures that #extern directives in GLSL shader source cause an exception to be thrown if they have an invalid name.")]
        public void Effect_GLSLExternDirectivesThrowException_WhenExternHasInvalidName()
        {
            GivenAnUltravioletApplicationWithNoWindow()
                .WithContent(content =>
                {
                    var externs = new Dictionary<String, String>
                    {
                        { "FOO", "123" },
                        { "BAZ", "456" },
                    };

                    Assert.That(() =>
                    {
                        ShaderSource shaderSource;
                        shaderSource = content.Load<ShaderSource>("Effects/ExternDirectiveInvalid");
                        shaderSource = ShaderSource.ProcessExterns(shaderSource, externs);
                    },
                    Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("Shader extern has a name which is missing or invalid."));
                })
                .RunForOneFrame();
        }

        [Test]
        [Category("Content")]
        [Description("Ensures that #extern directives in GLSL shader source are excluded from the output if no value is provided.")]
        public void Effect_GLSLExternDirectivesAreRemovedFromSource_WhenValueIsNotProvided()
        {
            GivenAnUltravioletApplicationWithNoWindow()
                .WithContent(content =>
                {
                    var externs = new Dictionary<String, String>
                    {
                        { "BAZ", "456" },
                    };

                    ShaderSource shaderSource;
                    shaderSource = content.Load<ShaderSource>("Effects/ExternDirective");
                    shaderSource = ShaderSource.ProcessExterns(shaderSource, externs);

                    TheResultingString(shaderSource.Source)
                        .ShouldBe(
                            @"/*" + Environment.NewLine +
                            @"#extern ""BAR""" + Environment.NewLine +
                            @"*/" + Environment.NewLine +
                            @"#define BAZ 456" + Environment.NewLine);
                })
                .RunForOneFrame();
        }

        [Test]
        [Category("Content")]
        [Description("Ensures that #extern directives in GLSL shader source are excluded from the output if no value dictionary is provided.")]
        public void Effect_GLSLExternDirectivesAreRemovedFromSource_WhenDictionaryIsNotProvided()
        {
            GivenAnUltravioletApplicationWithNoWindow()
                .WithContent(content =>
                {
                    ShaderSource shaderSource;
                    shaderSource = content.Load<ShaderSource>("Effects/ExternDirective");
                    shaderSource = ShaderSource.ProcessExterns(shaderSource, null);

                    TheResultingString(shaderSource.Source)
                        .ShouldBe(
                            @"/*" + Environment.NewLine +
                            @"#extern ""BAR""" + Environment.NewLine +
                            @"*/" + Environment.NewLine);
                })
                .RunForOneFrame();
        }

        [Test]
        [Category("Content")]
        [Description("Ensures that directives in GLSL shader source are ignored if they fall within comment blocks.")]
        public void Effect_GLSLDirectivesAreIgnoredInsideComments()
        {
            GivenAnUltravioletApplicationWithNoWindow()
                .WithContent(content =>
                {
                    var effect = content.Load<ShaderSource>("Effects/CommentedOutDirective");

                    TheResultingString(effect.Source)
                        .ShouldBe(
                            @"// This is IncludedDirective1.vert" + Environment.NewLine +
                            @"" + Environment.NewLine +
                            @"/*" + Environment.NewLine +
                            @"#include ""IncludedDirective2.vert""" + Environment.NewLine +
                            @"*/" + Environment.NewLine);
                })
                .RunForOneFrame();
        }
    }
}
