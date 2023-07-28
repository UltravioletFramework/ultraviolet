using System;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Presentation
{
#pragma warning disable 1591
    internal sealed class MessageBoxViewModel_Impl : CompiledDataSourceWrapper
    {
        public MessageBoxViewModel_Impl(MessageBoxViewModel dataSource, Namescope namescope)
            : base(namescope)
        {
            this.dataSource = dataSource;
        }

        public void HandleClickYes(DependencyObject dobj, RoutedEventData data) =>
            dataSource.HandleClickYes(dobj, data);

        public void HandleClickNo(DependencyObject dobj, RoutedEventData data) =>
            dataSource.HandleClickNo(dobj, data);

        public void HandleClickOK(DependencyObject dobj, RoutedEventData data) =>
            dataSource.HandleClickOK(dobj, data);

        public void HandleClickCancel(DependencyObject dobj, RoutedEventData data) =>
            dataSource.HandleClickCancel(dobj, data);

        [CompiledBindingExpression(@"{{Caption}}")]
        public String __UPF_Expression_Caption => dataSource.Caption;

        [CompiledBindingExpression(@"{{Text}}")]
        public String __UPF_Expression_Text => dataSource.Text;

        [CompiledBindingExpression(@"{{CaptionVisibility}}")]
        public Visibility __UPF_Expression_CaptionVisibility => dataSource.HasCaption ? Visibility.Visible : Visibility.Collapsed;

        [CompiledBindingExpression(@"{{ImageVisibility}}")]
        public Visibility __UPF_Expression_ImageVisibility => dataSource.HasImage ? Visibility.Visible : Visibility.Collapsed;

        [CompiledBindingExpression(@"{{TextVisibility}}")]
        public Visibility __UPF_Expression_TextVisibility => dataSource.HasText ? Visibility.Visible : Visibility.Collapsed;

        [CompiledBindingExpression(@"{{StringYes}}")]
        public Object __UPF_Expression_StringYes => Localization.Strings.Get("MSGBOX_YES");

        [CompiledBindingExpression(@"{{StringNo}}")]
        public Object __UPF_Expression_StringNo => Localization.Strings.Get("MSGBOX_NO");

        [CompiledBindingExpression(@"{{StringOK}}")]
        public Object __UPF_Expression_StringOK => Localization.Strings.Get("MSGBOX_OK");

        [CompiledBindingExpression(@"{{StringCancel}}")]
        public Object __UPF_Expression_StringCancel => Localization.Strings.Get("MSGBOX_CANCEL");

        public override Object WrappedDataSource => dataSource;
        private readonly MessageBoxViewModel dataSource;
    }
#pragma warning restore 1591
}
