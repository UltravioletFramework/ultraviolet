﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Nucleus.Testing;
using static TwistedLogik.Ultraviolet.UI.Presentation.Uvss.SyntaxFactory;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Tests
{
    [TestClass]
    public class SyntaxNormalizerTests : NucleusTestFramework
    {
        [TestMethod]
        public void SyntaxNormalizer_Block_IsCorrectlyNested()
        {
            var node = Block(
                Block(
                    Block(
                        Block().WithLeadingTrivia(Comment("// Hello, world!"))
                    )
                )
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "{\r\n" +
                    "\t{\r\n" +
                        "\t\t{\r\n" +
                            "\t\t\t// Hello, world!\r\n" +
                            "\t\t\t{\r\n" +
                            "\t\t\t}\r\n" +
                        "\t\t}\r\n" +
                    "\t}\r\n" +
                "}");
        }

        [TestMethod]
        public void SyntaxNormalizer_PropertyName_IsCorrectlyNormalized()
        {
            var node = PropertyName("foo", "bar");

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "foo.bar");
        }

        [TestMethod]
        public void SyntaxNormalizer_PropertyValueWithBraces_IsCorrectlyNormalized()
        {
            var node = PropertyValueWithBraces("hello world");

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "{ hello world }");
        }

        [TestMethod]
        public void SyntaxNormalizer_EventName_IsCorrectlyNormalized()
        {
            var node = EventName("foo", "bar");

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "foo.bar");
        }

        [TestMethod]
        public void SyntaxNormalizer_Selector_IsCorrectlyNormalized()
        {
            var node = Selector(
                SelectorPartByName("foo", "pseudoclass"),
                VisualDescendantCombinator(),
                SelectorPart(
                    SelectorSubPartByClass("bar"),
                    SelectorSubPartByClass("baz")
                ),
                VisualChildCombinator(),
                SelectorPartByType("qux")
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "#foo:pseudoclass .bar.baz > qux");
        }

        [TestMethod]
        public void SyntaxNormalizer_Selector_IsCorrectlyNormalized_WithNavigationExpression()
        {
            var node = Selector(
                List(new SyntaxNode[] {
                    SelectorPartByName("foo", "pseudoclass"),
                    VisualDescendantCombinator(),
                    SelectorPart(
                        SelectorSubPartByClass("bar"),
                        SelectorSubPartByClass("baz")
                    ),
                    VisualChildCombinator(),
                    SelectorPartByType("qux")
                }),
                NavigationExpression(PropertyName("some", "prop"), "SomeType")
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "#foo:pseudoclass .bar.baz > qux | some.prop as SomeType");
        }

        [TestMethod]
        public void SyntaxNormalizer_SelectorWithParentheses_IsCorrectlyNormalized()
        {
            var node = SelectorWithParentheses(
                Selector(
                    SelectorPartByName("foo", "pseudoclass"),
                    VisualDescendantCombinator(),
                    SelectorPart(
                        SelectorSubPartByClass("bar"),
                        SelectorSubPartByClass("baz")
                    ),
                    VisualChildCombinator(),
                    SelectorPartByType("qux")
                )
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "(#foo:pseudoclass .bar.baz > qux)");
        }

        [TestMethod]
        public void SyntaxNormalizer_RuleSet_IsCorrectlyNormalized()
        {
            var node = RuleSet(
                SelectorByName("test"),
                Block()
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "#test\r\n" +
                "{\r\n" +
                "}");
        }

        [TestMethod]
        public void SyntaxNormalizer_RuleSet_IsCorrectlyNormalized_WhenMultipleSelectorsInList()
        {
            var node = RuleSet(
                SeparatedList(
                    SelectorByName("foo"),
                    SelectorByName("bar")
                ),
                Block()
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "#foo, #bar\r\n" +
                "{\r\n" +
                "}");
        }

        [TestMethod]
        public void SyntaxNormalizer_RuleSet_HasTrailingLineBreaks()
        {
            var node = Block(
                RuleSet(SelectorByName("test"), Block()),
                RuleSet(SelectorByName("test"), Block())
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "{\r\n" +
                    "\t#test\r\n" +
                    "\t{\r\n" +
                    "\t}\r\n" +
                    "\r\n" +
                    "\t#test\r\n" +
                    "\t{\r\n" +
                    "\t}\r\n" +
                "}");
        }

        [TestMethod]
        public void SyntaxNormalizer_Rule_IsCorrectlyNormalized()
        {
            var node = Rule(
                PropertyName("foo", "bar"),
                PropertyValue("baz"),
                important: false
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "foo.bar: baz;");
        }

        [TestMethod]
        public void SyntaxNormalizer_Rule_IsCorrectlyNormalized_WhenImportant()
        {
            var node = Rule(
                PropertyName("foo", "bar"),
                PropertyValue("baz"),
                important: true);

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "foo.bar: baz !important;");
        }

        [TestMethod]
        public void SyntaxNormalizer_Rule_HasTrailingLineBreak()
        {
            var node = Block(
                Rule(
                   PropertyName("foo", "bar"),
                   PropertyValue("baz")
                ),
                Rule(
                    PropertyName("foo", "bar"),
                    PropertyValue("baz")
                )
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "{\r\n" +
                    "\tfoo.bar: baz;\r\n" +
                    "\tfoo.bar: baz;\r\n" +
                "}");
        }

        [TestMethod]
        public void SyntaxNormalizer_Transition_IsCorrectlyNormalized()
        {
            var node = Transition("common", "normal", "test-storyboard", important: false);

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "transition (common, normal): test-storyboard;");
        }

        [TestMethod]
        public void SyntaxNormalizer_Transition_IsCorrectlyNormalized_WhenImportant()
        {
            var node = Transition("common", "normal", "test-storyboard", important: true);

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "transition (common, normal): test-storyboard !important;");
        }

        [TestMethod]
        public void SyntaxNormalizer_Transition_HasTrailingLineBreak()
        {
            var node = Block(
                Transition("common", "normal", "test-storyboard", important: false),
                Transition("common", "normal", "test-storyboard", important: false)
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "{\r\n" +
                    "\ttransition (common, normal): test-storyboard;\r\n" + 
                    "\ttransition (common, normal): test-storyboard;\r\n" +
                "}");
        }

        [TestMethod]
        public void SyntaxNormalizer_Transition_WithFullArgList_IsCorrectlyNormalized()
        {
            var node = Transition("common", "normal", "pressed", "test-storyboard", important: false);

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "transition (common, normal, pressed): test-storyboard;");
        }

        [TestMethod]
        public void SyntaxNormalizer_Transition_WithFullArgList_IsCorrectlyNormalized_WhenImportant()
        {
            var node = Transition("common", "normal", "pressed", "test-storyboard", important: true);

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "transition (common, normal, pressed): test-storyboard !important;");
        }

        [TestMethod]
        public void SyntaxNormalizer_PropertyTriggerCondition_IsCorrectlyNormalized_WhenEquals()
        {
            var node = PropertyTriggerCondition(
                PropertyName("some", "property"),
                EqualsComparison(),
                PropertyValueWithBraces("somevalue")
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "some.property = { somevalue }");
        }

        [TestMethod]
        public void SyntaxNormalizer_PropertyTriggerCondition_IsCorrectlyNormalized_WhenNotEquals()
        {
            var node = PropertyTriggerCondition(
                PropertyName("some", "property"),
                NotEqualsComparison(),
                PropertyValueWithBraces("somevalue")
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "some.property <> { somevalue }");
        }

        [TestMethod]
        public void SyntaxNormalizer_PropertyTriggerCondition_IsCorrectlyNormalized_WhenLessThan()
        {
            var node = PropertyTriggerCondition(
                PropertyName("some", "property"),
                LessThanComparison(),
                PropertyValueWithBraces("somevalue")
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "some.property < { somevalue }");
        }

        [TestMethod]
        public void SyntaxNormalizer_PropertyTriggerCondition_IsCorrectlyNormalized_WhenGreaterThan()
        {
            var node = PropertyTriggerCondition(
                PropertyName("some", "property"),
                GreaterThanComparison(),
                PropertyValueWithBraces("somevalue")
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "some.property > { somevalue }");
        }

        [TestMethod]
        public void SyntaxNormalizer_PropertyTriggerCondition_IsCorrectlyNormalized_WhenLessThanEquals()
        {
            var node = PropertyTriggerCondition(
                PropertyName("some", "property"),
                LessThanEqualsComparison(),
                PropertyValueWithBraces("somevalue")
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "some.property <= { somevalue }");
        }

        [TestMethod]
        public void SyntaxNormalizer_PropertyTriggerCondition_IsCorrectlyNormalized_WhenGreaterThanEquals()
        {
            var node = PropertyTriggerCondition(
                PropertyName("some", "property"),
                GreaterThanEqualsComparison(),
                PropertyValueWithBraces("somevalue")
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "some.property >= { somevalue }");
        }

        [TestMethod]
        public void SyntaxNormalizer_PropertyTrigger_IsCorrectlyNormalized()
        {
            var node = PropertyTrigger(
                SeparatedList(
                    PropertyTriggerCondition("foo", EqualsComparison(), "bar"),
                    PropertyTriggerCondition("baz", EqualsComparison(), "qux")
                ),
                Block(),
                important: false
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "trigger property foo = { bar }, baz = { qux }\r\n" +
                "{\r\n" +
                "}");
        }

        [TestMethod]
        public void SyntaxNormalizer_PropertyTrigger_HasTrailingLineBreak()
        {
            var node = Block(
                PropertyTrigger(
                    SeparatedList(
                        PropertyTriggerCondition("foo", EqualsComparison(), "bar"),
                        PropertyTriggerCondition("baz", EqualsComparison(), "qux")
                    ),
                    Block(),
                    important: false
                ),
                Rule("prop", "value")
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "{\r\n" +
                    "\ttrigger property foo = { bar }, baz = { qux }\r\n" +
                    "\t{\r\n" +
                    "\t}\r\n" +
                    "\tprop: value;\r\n" +
                "}");
        }

        [TestMethod]
        public void SyntaxNormalizer_EventTriggerArgumentList_IsCorrectlyNormalized_WhenHandled()
        {
            var node = EventTriggerArgumentList(handled: true, sethandled: false);

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "(handled)");
        }

        [TestMethod]
        public void SyntaxNormalizer_EventTriggerArgumentList_IsCorrectlyNormalized_WhenSetHandled()
        {
            var node = EventTriggerArgumentList(handled: false, sethandled: true);

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "(set-handled)");
        }

        [TestMethod]
        public void SyntaxNormalizer_EventTriggerArgumentList_IsCorrectlyNormalized_WhenHandledAndSetHandled()
        {
            var node = EventTriggerArgumentList(handled: true, sethandled: true);

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "(handled, set-handled)");
        }

        [TestMethod]
        public void SyntaxNormalizer_EventTrigger_IsCorrectlyNormalized()
        {
            var node = EventTrigger(
                EventName("some", "event"),
                EventTriggerArgumentList(true, true),
                Block()
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "trigger event some.event (handled, set-handled)\r\n" +
                "{\r\n" +
                "}");
        }

        [TestMethod]
        public void SyntaxNormalizer_EventTrigger_HasTrailingLineBreak()
        {
            var node = Block(
                EventTrigger(
                    EventName("some", "event"),
                    EventTriggerArgumentList(true, true),
                    Block()
                ),
                Rule("prop", "value")
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "{\r\n" +
                    "\ttrigger event some.event (handled, set-handled)\r\n" +
                    "\t{\r\n" +
                    "\t}\r\n" +
                    "\tprop: value;\r\n" +
                "}");
        }

        [TestMethod]
        public void SyntaxNormalizer_PlayStoryboardTriggerAction_IsCorrectlyNormalized()
        {
            var node = PlayStoryboardTriggerAction(
                "some-storyboard"
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "play-storyboard { some-storyboard }");
        }

        [TestMethod]
        public void SyntaxNormalizer_PlayStoryboardTriggerAction_IsCorrectlyNormalized_WithSelector()
        {
            var node = PlayStoryboardTriggerAction(
                SelectorWithParenthesesByName("foo"),
                PropertyValueWithBraces("some-storyboard")
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "play-storyboard (#foo) { some-storyboard }");
        }

        [TestMethod]
        public void SyntaxNormalizer_PlaySfxTriggerActionIsCorrectlyNormalized()
        {
            var node = PlaySfxTriggerAction(
                "some-sfx"
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "play-sfx { some-sfx }");
        }

        [TestMethod]
        public void SyntaxNormalizer_SetTriggerAction_IsCorrectlyNormalized()
        {
            var node = SetTriggerAction(
                PropertyName("some", "property"),
                PropertyValueWithBraces("hello world")
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "set some.property { hello world }");
        }

        [TestMethod]
        public void SyntaxNormalizer_SetTriggerAction_IsCorrectlyNormalized_WithSelector()
        {
            var node = SetTriggerAction(
                PropertyName("some", "property"),
                SelectorWithParenthesesByName("foo"),
                PropertyValueWithBraces("hello world")
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "set some.property (#foo) { hello world }");
        }

        [TestMethod]
        public void SyntaxNormalizer_Storyboard_IsCorrectlyNormalized()
        {
            var node = Storyboard("some-storyboard", 
                Block()
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "@some-storyboard\r\n" +
                "{\r\n" +
                "}");
        }

        [TestMethod]
        public void SyntaxNormalizer_Storyboard_HasTrailingLineBreaks()
        {
            var node = Block(
                Storyboard("some-storyboard",
                    Block()
                ),
                Storyboard("some-storyboard",
                    Block()
                )
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "{\r\n" +
                    "\t@some-storyboard\r\n" +
                    "\t{\r\n" +
                    "\t}\r\n" +
                    "\r\n" +
                    "\t@some-storyboard\r\n" +
                    "\t{\r\n" +
                    "\t}\r\n" +
                "}");
        }

        [TestMethod]
        public void SyntaxNormalizer_StoryboardTarget_IsCorrectlyNormalized()
        {
            var node = StoryboardTarget(
                "SomeType",
                Block()
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "target SomeType\r\n" +
                "{\r\n" +
                "}");
        }

        [TestMethod]
        public void SyntaxNormalizer_StoryboardTarget_IsCorrectlyNormalized_WithSelector()
        {
            var node = StoryboardTarget(
                "SomeType",
                SelectorWithParenthesesByName("foo"),
                Block()
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "target SomeType (#foo)\r\n" +
                "{\r\n" +
                "}");
        }

        [TestMethod]
        public void SyntaxNormalizer_StoryboardTarget_HasTrailingLineBreak()
        {
            var node = Block(
                StoryboardTarget(
                    "SomeType",
                    Block()
                ),
                StoryboardTarget(
                    "SomeType",
                    Block()
                )
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "{\r\n" +
                    "\ttarget SomeType\r\n" +
                    "\t{\r\n" +
                    "\t}\r\n" +
                    "\ttarget SomeType\r\n" +
                    "\t{\r\n" +
                    "\t}\r\n" +
                "}");
        }

        [TestMethod]
        public void SyntaxNormalizer_StoryboardTarget_HasTrailingLineBreak_WithSelector()
        {
            var node = Block(
                StoryboardTarget(
                    "SomeType",
                    SelectorWithParenthesesByName("foo"),
                    Block()
                ),
                StoryboardTarget(
                    "SomeType",
                    SelectorWithParenthesesByName("foo"),
                    Block()
                )
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "{\r\n" +
                    "\ttarget SomeType (#foo)\r\n" +
                    "\t{\r\n" +
                    "\t}\r\n" +
                    "\ttarget SomeType (#foo)\r\n" +
                    "\t{\r\n" +
                    "\t}\r\n" +
                "}");
        }

        [TestMethod]
        public void SyntaxNormalizer_Animation_IsCorrectlyNormalized()
        {
            var node = Animation(PropertyName("some", "prop"), Block());

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "animation some.prop\r\n" +
                "{\r\n" +
                "}");
        }
        
        [TestMethod]
        public void SyntaxNormalizer_Animation_IsCorrectlyNormalized_WithNavigationProperty()
        {
            var node = Animation(
                PropertyName("some", "prop"),
                NavigationExpression(
                    PropertyName("another", "prop"),
                    "SomeType"
                ),
                Block()
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "animation some.prop | another.prop as SomeType\r\n" +
                "{\r\n" +
                "}");
        }

        [TestMethod]
        public void SyntaxNormalizer_Animation_HasTrailingLineBreak()
        {
            var node = Block(
                Animation(PropertyName("some", "prop"), Block()),
                Animation(PropertyName("some", "prop"), Block())
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "{\r\n" +
                    "\tanimation some.prop\r\n" +
                    "\t{\r\n" +
                    "\t}\r\n" +
                    "\tanimation some.prop\r\n" +
                    "\t{\r\n" +
                    "\t}\r\n" +
                "}");
        }

        [TestMethod]
        public void SyntaxNormalizer_AnimationKeyframe_IsCorrectlyNormalized()
        {
            var node = AnimationKeyframe(0,
                PropertyValueWithBraces("somevalue")
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "keyframe 0 { somevalue }");
        }

        [TestMethod]
        public void SyntaxNormalizer_AnimationKeyframe_IsCorrectlyNormalized_WithEasing()
        {
            var node = AnimationKeyframe(0, "ease-out-linear",
                PropertyValueWithBraces("somevalue")
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "keyframe 0 ease-out-linear { somevalue }");
        }

        [TestMethod]
        public void SyntaxNormalizer_AnimationKeyframe_HasTrailingLineBreak()
        {
            var node = Block(
                AnimationKeyframe(0, PropertyValueWithBraces("somevalue")),
                AnimationKeyframe(100, PropertyValueWithBraces("somevalue"))
            );

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(
                "{\r\n" +
                    "\tkeyframe 0 { somevalue }\r\n" + 
                    "\tkeyframe 100 { somevalue }\r\n" +
                "}");
        }
    }
}
