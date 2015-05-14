using TwistedLogik.Ultraviolet.Audio;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents a trigger action which plays a specified sound effect.
    /// </summary>
    public sealed class PlaySoundEffectTriggerAction : TriggerAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaySoundEffectTriggerAction"/> class.
        /// </summary>
        /// <param name="sfxAssetID">The asset identifier of the sound effect to play.</param>
        public PlaySoundEffectTriggerAction(SourcedAssetID sfxAssetID)
        {
            this.sfxAssetID = sfxAssetID;
        }

        /// <inheritdoc/>
        public override void Activate(DependencyObject dobj)
        {
            var element = dobj as UIElement;
            if (element == null || element.View == null)
                return;

            var contentManager = (sfxAssetID.AssetSource == AssetSource.Global) ? 
                element.View.GlobalContent : element.View.LocalContent;

            if (contentManager == null)
                return;

            var sfx = contentManager.Load<SoundEffect>(sfxAssetID.AssetID);
            sfx.Play();

            base.Activate(dobj);
        }

        // State values.
        private readonly SourcedAssetID sfxAssetID;
    }
}
