using System;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
#pragma warning disable 1591
    [System.CLSCompliant(false)]
    [System.CodeDom.Compiler.GeneratedCode("UPF Binding Expression Compiler", "1.2.0.0")]
    public sealed class MessageBoxViewModel_Impl : TwistedLogik.Ultraviolet.UI.Presentation.IDataSourceWrapper
    {
        #region Constructors
        public MessageBoxViewModel_Impl(TwistedLogik.Ultraviolet.UI.Presentation.MessageBoxViewModel dataSource)
        {
            this.dataSource = dataSource;
        }
        #endregion
        
        #region IDataSourceWrapper
        Object IDataSourceWrapper.WrappedDataSource
        {
            get { return dataSource; }
        }
        private readonly TwistedLogik.Ultraviolet.UI.Presentation.MessageBoxViewModel dataSource;
        #endregion
        
        #region Methods
        public void HandleClickYes(TwistedLogik.Ultraviolet.UI.Presentation.DependencyObject dobj, ref TwistedLogik.Ultraviolet.UI.Presentation.RoutedEventData data)
        {
            dataSource.HandleClickYes(dobj, ref data);
        }
        public void HandleClickNo(TwistedLogik.Ultraviolet.UI.Presentation.DependencyObject dobj, ref TwistedLogik.Ultraviolet.UI.Presentation.RoutedEventData data)
        {
            dataSource.HandleClickNo(dobj, ref data);
        }
        public void HandleClickOK(TwistedLogik.Ultraviolet.UI.Presentation.DependencyObject dobj, ref TwistedLogik.Ultraviolet.UI.Presentation.RoutedEventData data)
        {
            dataSource.HandleClickOK(dobj, ref data);
        }
        public void HandleClickCancel(TwistedLogik.Ultraviolet.UI.Presentation.DependencyObject dobj, ref TwistedLogik.Ultraviolet.UI.Presentation.RoutedEventData data)
        {
            dataSource.HandleClickCancel(dobj, ref data);
        }
        #endregion
        
        #region Properties
        public System.String Text
        {
            get { return dataSource.Text; }
        }
        public System.String Caption
        {
            get { return dataSource.Caption; }
        }
        public TwistedLogik.Ultraviolet.UI.Presentation.MessageBoxButton Button
        {
            get { return dataSource.Button; }
        }
        public TwistedLogik.Ultraviolet.UI.Presentation.MessageBoxImage Image
        {
            get { return dataSource.Image; }
        }
        public TwistedLogik.Ultraviolet.UI.Presentation.MessageBoxResult DefaultResult
        {
            get { return dataSource.DefaultResult; }
        }
        public TwistedLogik.Ultraviolet.UI.Presentation.MessageBoxResult MessageBoxResult
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
        [TwistedLogik.Ultraviolet.UI.Presentation.CompiledBindingExpressionAttribute(@"{{Caption}}", SimpleDependencyPropertyOwner = null, SimpleDependencyPropertyName = null)]
        public System.String __UPF_Expression0
        {
            get
            {
                var value = Caption;
                if (value == null || value.GetType() == typeof(String))
                {
                    return (String)(Object)value;
                }
                return String.Format("{0}", Caption);
            }
        }
        [TwistedLogik.Ultraviolet.UI.Presentation.CompiledBindingExpressionAttribute(@"{{HasCaption ? Visibility.Visible : Visibility.Collapsed}}", SimpleDependencyPropertyOwner = null, SimpleDependencyPropertyName = null)]
        public TwistedLogik.Ultraviolet.UI.Presentation.Visibility __UPF_Expression1
        {
            get { return (TwistedLogik.Ultraviolet.UI.Presentation.Visibility)(HasCaption ? Visibility.Visible : Visibility.Collapsed); }
        }
        [TwistedLogik.Ultraviolet.UI.Presentation.CompiledBindingExpressionAttribute(@"{{HasImage ? Visibility.Visible : Visibility.Collapsed}}", SimpleDependencyPropertyOwner = null, SimpleDependencyPropertyName = null)]
        public TwistedLogik.Ultraviolet.UI.Presentation.Visibility __UPF_Expression2
        {
            get { return (TwistedLogik.Ultraviolet.UI.Presentation.Visibility)(HasImage ? Visibility.Visible : Visibility.Collapsed); }
        }
        [TwistedLogik.Ultraviolet.UI.Presentation.CompiledBindingExpressionAttribute(@"{{Text}}", SimpleDependencyPropertyOwner = null, SimpleDependencyPropertyName = null)]
        public System.String __UPF_Expression3
        {
            get
            {
                var value = Text;
                if (value == null || value.GetType() == typeof(String))
                {
                    return (String)(Object)value;
                }
                return String.Format("{0}", Text);
            }
        }
        [TwistedLogik.Ultraviolet.UI.Presentation.CompiledBindingExpressionAttribute(@"{{HasText ? Visibility.Visible : Visibility.Collapsed}}", SimpleDependencyPropertyOwner = null, SimpleDependencyPropertyName = null)]
        public TwistedLogik.Ultraviolet.UI.Presentation.Visibility __UPF_Expression4
        {
            get { return (TwistedLogik.Ultraviolet.UI.Presentation.Visibility)(HasText ? Visibility.Visible : Visibility.Collapsed); }
        }
        [TwistedLogik.Ultraviolet.UI.Presentation.CompiledBindingExpressionAttribute(@"{{Localization.Strings.Get(""MSGBOX_YES"")}}", SimpleDependencyPropertyOwner = null, SimpleDependencyPropertyName = null)]
        public System.Object __UPF_Expression5
        {
            get { return (System.Object)(Localization.Strings.Get("MSGBOX_YES")); }
        }
        [TwistedLogik.Ultraviolet.UI.Presentation.CompiledBindingExpressionAttribute(@"{{Localization.Strings.Get(""MSGBOX_NO"")}}", SimpleDependencyPropertyOwner = null, SimpleDependencyPropertyName = null)]
        public System.Object __UPF_Expression6
        {
            get { return (System.Object)(Localization.Strings.Get("MSGBOX_NO")); }
        }
        [TwistedLogik.Ultraviolet.UI.Presentation.CompiledBindingExpressionAttribute(@"{{Localization.Strings.Get(""MSGBOX_OK"")}}", SimpleDependencyPropertyOwner = null, SimpleDependencyPropertyName = null)]
        public System.Object __UPF_Expression7
        {
            get { return (System.Object)(Localization.Strings.Get("MSGBOX_OK")); }
        }
        [TwistedLogik.Ultraviolet.UI.Presentation.CompiledBindingExpressionAttribute(@"{{Localization.Strings.Get(""MSGBOX_CANCEL"")}}", SimpleDependencyPropertyOwner = null, SimpleDependencyPropertyName = null)]
        public System.Object __UPF_Expression8
        {
            get { return (System.Object)(Localization.Strings.Get("MSGBOX_CANCEL")); }
        }
        #endregion
    }
#pragma warning restore 1591
}
