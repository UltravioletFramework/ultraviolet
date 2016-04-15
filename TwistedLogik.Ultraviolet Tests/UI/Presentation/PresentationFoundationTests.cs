using System;
using System.Drawing;
using NUnit.Framework;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Tests.UI.Presentation.Screens;
using TwistedLogik.Ultraviolet.UI;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation
{
    [TestFixture]
    public class PresentationFoundationTests : PresentationFoundationTestFramework
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            StandardInitialization();
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that a reasonably complex screen featuring multiple control types is correctly laid out by the Presentation Foundation.")]
        public void UPF_ArrangesComplexScreenCorrectly()
        {
            var result = RunPresentationTestFor(content => new UPF_ArrangesComplexScreenCorrectly(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_ArrangesComplexScreenCorrectly.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that a UIElement with a non-identity RenderTransform is drawn correctly.")]
        public void UPF_UIElement_DrawsCorrectly_WithRenderTransform()
        {
            var result = RunPresentationTestFor(content => new UPF_UIElement_DrawsCorrectly_WithRenderTransform(content));

            TheResultingImage(result).WithinThreshold(0.01f)
                .ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_UIElement_DrawsCorrectly_WithRenderTransform.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that a UIElement with an Effect is drawn correctly.")]
        public void UPF_UIElement_DrawsCorrectly_WithEffect()
        {
            var result = RunPresentationTestFor(content => new UPF_UIElement_DrawsCorrectly_WithEffect(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_UIElement_DrawsCorrectly_WithEffect.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that a UIElement with a non-identity RenderTransform and an Effect is drawn correctly.")]
        public void UPF_UIElement_DrawsCorrectly_WithEffectAndTransform()
        {
            var result = RunPresentationTestFor(content => new UPF_UIElement_DrawsCorrectly_WithEffectAndTransform(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_UIElement_DrawsCorrectly_WithEffectAndTransform.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that a UIElement with a non-identity LayoutTransform is correctly arranged relative to other elements.")]
        public void UPF_UIElement_ArrangesCorrectly_WithLayoutTransform()
        {
            var result = RunPresentationTestFor(content => new UPF_UIElement_ArrangesCorrectly_WithLayoutTransform(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_UIElement_ArrangesCorrectly_WithLayoutTransform.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that a UIElement with a non-identity LayoutTransform and a non-identity RenderTransform is correctly arranged relative to other elements.")]
        public void UPF_UIElement_ArrangesCorrectly_WithLayoutAndRenderTransform()
        {
            var result = RunPresentationTestFor(content => new UPF_UIElement_ArrangesCorrectly_WithLayoutAndRenderTransform(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_UIElement_ArrangesCorrectly_WithLayoutAndRenderTransform.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that the Canvas container correctly arranges its child elements.")]
        public void UPF_Canvas_ArrangesChildrenCorrectly()
        {
            var result = RunPresentationTestFor(content => new UPF_Canvas_ArrangesChildrenCorrectly(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_Canvas_ArrangesChildrenCorrectly.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that the Grid container correctly arranges its child elements.")]
        public void UPF_Grid_ArrangesChildrenCorrectly()
        {
            var result = RunPresentationTestFor(content => new UPF_Grid_ArrangesChildrenCorrectly(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_Grid_ArrangesChildrenCorrectly.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that the StackPanel container correctly arranges its child elements when its Orientation property is set to Vertical.")]
        public void UPF_StackPanel_ArrangesChildrenCorrectly_WithVerticalOrientation()
        {
            var result = RunPresentationTestFor(content => new UPF_StackPanel_ArrangesChildrenCorrectly_WithVerticalOrientation(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_StackPanel_ArrangesChildrenCorrectly_WithVerticalOrientation.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that the StackPanel container correctly arranges its child elements when its Orientation property is set to Horizontal.")]
        public void UPF_StackPanel_ArrangesChildrenCorrectly_WithHorizontalOrientation()
        {
            var result = RunPresentationTestFor(content => new UPF_StackPanel_ArrangesChildrenCorrectly_WithHorizontalOrientation(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_StackPanel_ArrangesChildrenCorrectly_WithHorizontalOrientation.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that the WrapPanel container correctly arranges its child elements when its Orientation property is set to Vertical.")]
        public void UPF_WrapPanel_ArrangesChildrenCorrectly_WithVerticalOrientation()
        {
            var result = RunPresentationTestFor(content => new UPF_WrapPanel_ArrangesChildrenCorrectly_WithVerticalOrientation(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_WrapPanel_ArrangesChildrenCorrectly_WithVerticalOrientation.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that the StackPanel container correctly arranges its child elements when its Orientation property is set to Horizontal.")]
        public void UPF_WrapPanel_ArrangesChildrenCorrectly_WithHorizontalOrientation()
        {
            var result = RunPresentationTestFor(content => new UPF_WrapPanel_ArrangesChildrenCorrectly_WithHorizontalOrientation(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_WrapPanel_ArrangesChildrenCorrectly_WithHorizontalOrientation.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that the DockPanel container correctly arranges its child elements.")]
        public void UPF_DockPanel_ArrangesChildrenCorrectly()
        {
            var result = RunPresentationTestFor(content => new UPF_DockPanel_ArrangesChildrenCorrectly(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_DockPanel_ArrangesChildrenCorrectly.png");
        }
        
        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that the TabPanel container correctly arranges its child elements.")]
        public void UPF_TabControl_ArrangesChildrenCorrectly()
        {
            var result = RunPresentationTestFor(content => new UPF_TabControl_ArrangesChildrenCorrectly(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_TabControl_ArrangesChildrenCorrectly.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that the ListBox container correctly arranges its items.")]
        public void UPF_ListBox_ArrangesItemsCorrectly()
        {
            var result = RunPresentationTestFor(content => new UPF_ListBox_ArrangesItemsCorrectly(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_ListBox_ArrangesItemsCorrectly.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that the ComboBox container correctly arranges its items.")]
        public void UPF_ComboBox_ArrangesItemsCorrectly()
        {
            var result = RunPresentationTestFor(content => new UPF_ComboBox_ArrangesItemsCorrectly(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_ComboBox_ArrangesItemsCorrectly.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that the Adorner control is drawn correctly when applied to an element.")]
        public void UPF_Adorner_DrawsCorrectly()
        {
            var result = RunPresentationTestFor(content => new UPF_Adorner_DrawsCorrectly(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_Adorner_DrawsCorrectly.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that the Adorner control is drawn correctly when applied to an element inside of a Popup.")]
        public void UPF_Adorner_DrawsCorrectly_WhenInsideOfAPopup()
        {
            var result = RunPresentationTestFor(content => new UPF_Adorner_DrawsCorrectly_WhenInsideOfAPopup(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_Adorner_DrawsCorrectly_WhenInsideOfAPopup.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that the Popup control is laid out correctly when it has a placement target.")]
        public void UPF_Popup_LaidOutCorrectly_WithPlacementTarget()
        {
            var result = RunPresentationTestFor(content => new UPF_Popup_LaidOutCorrectly_WithPlacementTarget(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_Popup_LaidOutCorrectly_WithPlacementTarget.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that the Popup control is laid out correctly when it has a placement target and the Popup is transformed.")]
        public void UPF_Popup_LaidOutCorrectly_WithPlacementTargetAndTransform()
        {
            var result = RunPresentationTestFor(content => new UPF_Popup_LaidOutCorrectly_WithPlacementTargetAndTransform(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_Popup_LaidOutCorrectly_WithPlacementTargetAndTransform.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that the Popup control is laid out correctly when it has a placement target which is inside of another Popup.")]
        public void UPF_Popup_LaidOutCorrectly_WithPlacementTargetInsidePopup()
        {
            var result = RunPresentationTestFor(content => new UPF_Popup_LaidOutCorrectly_WithPlacementTargetInsidePopup(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_Popup_LaidOutCorrectly_WithPlacementTargetInsidePopup.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that the Popup control is laid out correctly when it has a placement target which is transformed.")]
        public void UPF_Popup_LaidOutCorrectly_WithTransformedPlacementTarget()
        {
            var result = RunPresentationTestFor(content => new UPF_Popup_LaidOutCorrectly_WithTransformedPlacementTarget(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_Popup_LaidOutCorrectly_WithTransformedPlacementTarget.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that the Popup control is laid out correctly when it has a placement target which is transformed and it is inested inside of another Popup.")]
        public void UPF_Popup_LaidOutCorrectly_WithTransformedPlacementTarget_WhenNestedInsidePopup()
        {
            var result = RunPresentationTestFor(content => new UPF_Popup_LaidOutCorrectly_WithTransformedPlacementTarget_WhenNestedInsidePopup(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_Popup_LaidOutCorrectly_WithTransformedPlacementTarget_WhenNestedInsidePopup.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that the Popup control is laid out correctly when it has a placement rectangle.")]
        public void UPF_Popup_LaidOutCorrectly_WithPlacementRectangle()
        {
            var result = RunPresentationTestFor(content => new UPF_Popup_LaidOutCorrectly_WithPlacementRectangle(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_Popup_LaidOutCorrectly_WithPlacementRectangle.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that the Popup control is laid out correctly when it is nested inside of another Popup.")]
        public void UPF_Popup_LaidOutCorrectly_WhenNestedInsidePopup()
        {
            var result = RunPresentationTestFor(content => new UPF_Popup_LaidOutCorrectly_WhenNestedInsidePopup(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_Popup_LaidOutCorrectly_WhenNestedInsidePopup.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that the Popup control is drawn correctly when it has a placement target which is inside of another popup and it also has an Effect.")]
        public void UPF_Popup_DrawsCorrectly_WithPlacementTargetInsidePopupAndEffect()
        {
            var result = RunPresentationTestFor(content => new UPF_Popup_DrawsCorrectly_WithPlacementTargetInsidePopupAndEffect(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_Popup_DrawsCorrectly_WithPlacementTargetInsidePopupAndEffect.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that a VisualClone element is drawn correctly when cloning elements with and without shader effects.")]
        public void UPF_VisualClone_DrawsCorrectly()
        {
            var result = RunPresentationTestFor(content => new UPF_VisualClone_DrawsCorrectly(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_VisualClone_DrawsCorrectly.png");
        }

        [Test]
        [Category("UPF"), Category("Rendering")]
        [Description("Ensures that a VisualClone element with a transformed parent is drawn correctly when cloning elements with and without shader effects.")]
        public void UPF_VisualClone_DrawsCorrectly_WhenParentIsTransformed()
        {
            var result = RunPresentationTestFor(content => new UPF_VisualClone_DrawsCorrectly_WithTransformedParent(content));

            TheResultingImage(result).ShouldMatch(@"Resources/Expected/UI/Presentation/UPF_VisualClone_DrawsCorrectly_WhenParentIsTransformed.png");
        }

        /// <summary>
        /// Runs a standard test by spinning up an Ultraviolet application and displaying the specified UPF screen.
        /// </summary>
        private Bitmap RunPresentationTestFor<T>(Func<ContentManager, T> ctor) where T : UIScreen
        {
            return GivenAPresentationFoundationTestFor<T>(ctor)
                .Render(uv =>
                {
                    using (var spriteBatch = SpriteBatch.Create())
                    {
                        uv.GetUI().GetScreens().Draw(new UltravioletTime(), spriteBatch);
                    }
                });
        }
    }
}
