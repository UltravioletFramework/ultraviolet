using System;

namespace Ultraviolet.Presentation.Tests.ViewModels
{
    public class UPF_LoadsLinkHandlerMethods_AsViewResources_VM
    {
        public Color TestLinkColorizer(String target, Boolean visited, Boolean hovering, Boolean active, Color currentColor) =>
            visited ? Color.Magenta : Color.Yellow;

        public Boolean TestLinkStateEvaluator(String target) =>
            String.Equals(target, "visited", StringComparison.InvariantCulture);
    }
}
