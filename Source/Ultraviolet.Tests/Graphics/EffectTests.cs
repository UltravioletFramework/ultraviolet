using System;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests.Graphics
{
    [TestFixture]
    public partial class EffectTests : UltravioletApplicationTestFramework
    {
        [Test]
        [Category("Rendering")]
        [Description("Ensures that directives in GLSL shader source are correctly processed.")]
        public void Effect_GLSLDirectivesAreProcessed()
        {
            GivenAnUltravioletApplicationWithNoWindow()
                .WithInitialization(uv =>
                {
                    uv.GetContent().Processors.RegisterProcessor<ShaderSourceProcessor>();
                })
                .WithContent(content =>
                {
                    var effect = content.Load<String>("Effects/IncludeDirective");

                    TheResultingString(effect)
                        .ShouldBe(
                            @"// This is IncludedDirective1.vert" + Environment.NewLine +
                            @"" + Environment.NewLine +
                            @"// This is IncludedDirective2.vert" + Environment.NewLine + 
                            @"" + Environment.NewLine);
                })
                .RunForOneFrame();
        }

        [Test]
        [Category("Rendering")]
        [Description("Ensures that directives in GLSL shader source are ignored if they fall within comment blocks.")]
        public void Effect_GLSLDirectivesAreIgnoredInsideComments()
        {
            GivenAnUltravioletApplicationWithNoWindow()
                .WithInitialization(uv =>
                {
                    uv.GetContent().Processors.RegisterProcessor<ShaderSourceProcessor>();
                })
                .WithContent(content =>
                {
                    var effect = content.Load<String>("Effects/CommentedOutDirective");

                    TheResultingString(effect)
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
