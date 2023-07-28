using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the component of a <see cref="PasswordEditor"/> which is responsible for performing text editing.
    /// </summary>
    [UvmlKnownType]
    public sealed class PasswordEditor : TextEditor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextEditor"/> control.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public PasswordEditor(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets the password which has been entered into the editor.
        /// </summary>
        /// <returns>A string containing the password which has been entered into the editor.</returns>
        public String GetPassword()
        {
            var result = default(String);

            var ptr = Marshal.SecureStringToBSTR(securePassword);
            try
            {
                unsafe
                {
                    result = new String((char*)ptr);
                }
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }

            return result;
        }

        /// <summary>
        /// Gets the password which has been entered into the editor.
        /// </summary>
        /// <param name="stringBuilder">A <see cref="StringBuilder"/> to populate with the password which has been entered into the editor.</param>
        public void GetPassword(StringBuilder stringBuilder)
        {
            Contract.Require(stringBuilder, nameof(stringBuilder));

            var password = GetPassword();

            stringBuilder.Length = 0;
            stringBuilder.Append(password);
        }

        /// <summary>
        /// Sets the password which has been entered into the editor.
        /// </summary>
        /// <param name="value">A <see cref="String"/> containing the password to set.</param>
        public void SetPassword(String value)
        {
            SetPassword(new StringSegment(value));
        }

        /// <summary>
        /// Sets the password which has been entered into the editor.
        /// </summary>
        /// <param name="value">A <see cref="StringBuilder"/> containing password to set.</param>
        public void SetPassword(StringBuilder value)
        {
            SetPassword(new StringSegment(value));
        }

        /// <summary>
        /// Sets the password which has been entered into the editor.
        /// </summary>
        /// <param name="value">A <see cref="StringSegment"/> containing password to set.</param>
        public void SetPassword(StringSegment value)
        {
            settingPassword = true;
            try
            {
                Clear();

                if (value.Length > 0)
                    AppendText(new String(MaskCharacter ?? '*', value.Length));

                securePassword.Clear();
                for (int i = 0; i < value.Length; i++)
                    securePassword.AppendChar(value[i]);

                var owner = TemplatedParent as PasswordBox;
                if (owner != null)
                    RaisePasswordChanged(owner);
            }
            finally
            {
                settingPassword = false;
            }
        }

        /// <summary>
        /// Gets an instance of <see cref="SecureString"/> that represents the editor's current content.
        /// </summary>
        public SecureString SecurePassword
        {
            get { return (securePassword == null) ? null : securePassword.Copy(); }
        }
        
        /// <inheritdoc/>
        protected override void PrepareOverride()
        {
            if (securePassword == null)
                securePassword = new SecureString();

            base.PrepareOverride();
        }

        /// <inheritdoc/>
        protected override void CleanupOverride()
        {
            SafeDispose.DisposeRef(ref securePassword);

            base.CleanupOverride();
        }

        /// <inheritdoc/>
        protected override void OnCharacterInserted(Int32 offset, Char charOriginal, Char charInserted, Boolean raiseChangeEvents)
        {
            if (!settingPassword)
            {
                securePassword.InsertAt(offset, charOriginal);

                var owner = TemplatedParent as PasswordBox;
                if (owner != null && raiseChangeEvents)
                    RaisePasswordChanged(owner);
            }
            base.OnCharacterInserted(offset, charOriginal, charInserted, raiseChangeEvents);
        }

        /// <inheritdoc/>
        protected override void OnCharacterDeleted(Int32 offset, Int32 length, Boolean raiseChangeEvents)
        {
            if (!settingPassword)
            {
                for (int i = 0; i < length; i++)
                    securePassword.RemoveAt(offset);

                var owner = TemplatedParent as PasswordBox;
                if (owner != null && raiseChangeEvents)
                    RaisePasswordChanged(owner);
            }
            base.OnCharacterDeleted(offset, length, raiseChangeEvents);
        }

        /// <inheritdoc/>
        protected override Char? MaskCharacter
        {
            get
            {
                var owner = TemplatedParent as PasswordBox;
                if (owner == null)
                    return base.MaskCharacter;
                
                return owner.PasswordChar;
            }
        }

        /// <summary>
        /// Raises the <see cref="PasswordBox.PasswordChangedEvent"/> routed event against the editor's templated parent.
        /// </summary>
        private void RaisePasswordChanged(PasswordBox target)
        {
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(PasswordBox.PasswordChangedEvent);
            var evtData = RoutedEventData.Retrieve(target);
            evtDelegate(target, evtData);
        }

        // The secure string buffer that contains the user's password.
        private SecureString securePassword;
        private Boolean settingPassword;
    }
}
