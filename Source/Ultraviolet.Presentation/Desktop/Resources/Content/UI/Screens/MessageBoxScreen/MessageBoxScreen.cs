using System;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Presentation
{
#pragma warning disable 1591
#pragma warning disable 0184
    [System.CLSCompliant(false)]
    [System.CodeDom.Compiler.GeneratedCode("UPF Binding Expression Compiler", "1.3.11.0")]
    public sealed partial class MessageBoxViewModel_Impl : Ultraviolet.Presentation.CompiledDataSourceWrapper
    {
        #region Constructors
        public MessageBoxViewModel_Impl(Ultraviolet.Presentation.MessageBoxViewModel dataSource, Ultraviolet.Presentation.Namescope namescope) : base(namescope)
        {
            this.dataSource = dataSource;
        }
        #endregion
        
        #region IDataSourceWrapper
        public override Object WrappedDataSource
        {
            get { return dataSource; }
        }
        private readonly Ultraviolet.Presentation.MessageBoxViewModel dataSource;
        #endregion
        
        #region Methods
        public void HandleClickYes(Ultraviolet.Presentation.DependencyObject dobj, Ultraviolet.Presentation.RoutedEventData data)
        {
            dataSource.HandleClickYes(dobj, data);
        }
        public void HandleClickNo(Ultraviolet.Presentation.DependencyObject dobj, Ultraviolet.Presentation.RoutedEventData data)
        {
            dataSource.HandleClickNo(dobj, data);
        }
        public void HandleClickOK(Ultraviolet.Presentation.DependencyObject dobj, Ultraviolet.Presentation.RoutedEventData data)
        {
            dataSource.HandleClickOK(dobj, data);
        }
        public void HandleClickCancel(Ultraviolet.Presentation.DependencyObject dobj, Ultraviolet.Presentation.RoutedEventData data)
        {
            dataSource.HandleClickCancel(dobj, data);
        }
        #endregion
        
        #region Properties
        public System.String Caption
        {
            get { return dataSource.Caption; }
        }
        public System.String Text
        {
            get { return dataSource.Text; }
        }
        public Ultraviolet.Presentation.MessageBoxButton Button
        {
            get { return dataSource.Button; }
        }
        public Ultraviolet.Presentation.MessageBoxImage Image
        {
            get { return dataSource.Image; }
        }
        public Ultraviolet.Presentation.MessageBoxResult DefaultResult
        {
            get { return dataSource.DefaultResult; }
        }
        public Ultraviolet.Presentation.MessageBoxResult MessageBoxResult
        {
            get { return dataSource.MessageBoxResult; }
            set { dataSource.MessageBoxResult = value; }
        }
        public System.Boolean HasText
        {
            get { return dataSource.HasText; }
        }
        public System.Boolean HasCaption
        {
            get { return dataSource.HasCaption; }
        }
        public System.Boolean HasImage
        {
            get { return dataSource.HasImage; }
        }
        #endregion
        
        #region Fields
        #endregion
        
        #region Expressions
        [Ultraviolet.Presentation.CompiledBindingExpressionAttribute(@"{{Caption}}", SimpleDependencyPropertyOwner = null, SimpleDependencyPropertyName = null)]
        public System.String __UPF_Expression0
        {
            get
            {
                var value = Caption;
                return (System.String)__UPF_ConvertToString(value, null);
            }
        }
        [Ultraviolet.Presentation.CompiledBindingExpressionAttribute(@"{{HasCaption ? Visibility.Visible : Visibility.Collapsed}}", SimpleDependencyPropertyOwner = null, SimpleDependencyPropertyName = null)]
        public Ultraviolet.Presentation.Visibility __UPF_Expression1
        {
            get { return (Ultraviolet.Presentation.Visibility)(HasCaption ? Visibility.Visible : Visibility.Collapsed); }
        }
        [Ultraviolet.Presentation.CompiledBindingExpressionAttribute(@"{{HasImage ? Visibility.Visible : Visibility.Collapsed}}", SimpleDependencyPropertyOwner = null, SimpleDependencyPropertyName = null)]
        public Ultraviolet.Presentation.Visibility __UPF_Expression2
        {
            get { return (Ultraviolet.Presentation.Visibility)(HasImage ? Visibility.Visible : Visibility.Collapsed); }
        }
        [Ultraviolet.Presentation.CompiledBindingExpressionAttribute(@"{{Text}}", SimpleDependencyPropertyOwner = null, SimpleDependencyPropertyName = null)]
        public System.String __UPF_Expression3
        {
            get
            {
                var value = Text;
                return (System.String)__UPF_ConvertToString(value, null);
            }
        }
        [Ultraviolet.Presentation.CompiledBindingExpressionAttribute(@"{{HasText ? Visibility.Visible : Visibility.Collapsed}}", SimpleDependencyPropertyOwner = null, SimpleDependencyPropertyName = null)]
        public Ultraviolet.Presentation.Visibility __UPF_Expression4
        {
            get { return (Ultraviolet.Presentation.Visibility)(HasText ? Visibility.Visible : Visibility.Collapsed); }
        }
        [Ultraviolet.Presentation.CompiledBindingExpressionAttribute(@"{{Localization.Strings.Get(""MSGBOX_YES"")}}", SimpleDependencyPropertyOwner = null, SimpleDependencyPropertyName = null)]
        public System.Object __UPF_Expression5
        {
            get { return (System.Object)(Localization.Strings.Get("MSGBOX_YES")); }
        }
        [Ultraviolet.Presentation.CompiledBindingExpressionAttribute(@"{{Localization.Strings.Get(""MSGBOX_NO"")}}", SimpleDependencyPropertyOwner = null, SimpleDependencyPropertyName = null)]
        public System.Object __UPF_Expression6
        {
            get { return (System.Object)(Localization.Strings.Get("MSGBOX_NO")); }
        }
        [Ultraviolet.Presentation.CompiledBindingExpressionAttribute(@"{{Localization.Strings.Get(""MSGBOX_OK"")}}", SimpleDependencyPropertyOwner = null, SimpleDependencyPropertyName = null)]
        public System.Object __UPF_Expression7
        {
            get { return (System.Object)(Localization.Strings.Get("MSGBOX_OK")); }
        }
        [Ultraviolet.Presentation.CompiledBindingExpressionAttribute(@"{{Localization.Strings.Get(""MSGBOX_CANCEL"")}}", SimpleDependencyPropertyOwner = null, SimpleDependencyPropertyName = null)]
        public System.Object __UPF_Expression8
        {
            get { return (System.Object)(Localization.Strings.Get("MSGBOX_CANCEL")); }
        }
        #endregion
    }
#pragma warning restore 0184
#pragma warning restore 1591
}
