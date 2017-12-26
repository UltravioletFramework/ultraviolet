using Ultraviolet.Audio;
using Ultraviolet.Platform;

namespace Ultraviolet.Presentation.Styles
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
        public override void Activate(UltravioletContext uv, DependencyObject dobj)
        {
            var element = dobj as UIElement;
            if (element == null || element.View == null)
                return;

            var contentManager = (sfxAssetID.AssetSource == AssetSource.Global) ? 
                element.View.GlobalContent : element.View.LocalContent;

            if (contentManager == null)
                return;

            var density = (dobj as UIElement)?.View?.Display?.DensityBucket ?? ScreenDensityBucket.Desktop;
            var sfx = contentManager.Load<SoundEffect>(sfxAssetID.AssetID, density);
            sfx.Play();

            base.Activate(uv, dobj);
        }

        // State values.
        private readonly SourcedAssetID sfxAssetID;
    }
}
