using System;
using Android.App;
using Ultraviolet.Core.Messages;

namespace Ultraviolet.Messages
{
    /// <summary>
    /// Represents the message data for an Android lifecycle message.
    /// </summary>
    [CLSCompliant(false)]
    public sealed class AndroidLifecycleMessageData : MessageData
    {
        /// <summary>
        /// Gets or sets the application's main activity.
        /// </summary>
        public Activity Activity
        {
            get;
            set;
        }
    }
}