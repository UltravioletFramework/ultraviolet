using System;
using Ultraviolet.Input;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Contains a standard set of media-related commands.
    /// </summary>
    public static class MediaCommands
    {
        /// <summary>
        /// Gets the value that represents the Play command.
        /// </summary>
        public static RoutedUICommand Play => play.Value;

        /// <summary>
        /// Gets the value that represents the Pause command.
        /// </summary>
        public static RoutedUICommand Pause => pause.Value;

        /// <summary>
        /// Gets the value that represents the Stop command.
        /// </summary>
        public static RoutedUICommand Stop => stop.Value;

        /// <summary>
        /// Gets the value that represents the Record command.
        /// </summary>
        public static RoutedUICommand Record => record.Value;

        /// <summary>
        /// Gets the value that represents the Next Track command.
        /// </summary>
        public static RoutedUICommand NextTrack => nextTrack.Value;

        /// <summary>
        /// Gets the value that represents the Previous Track command.
        /// </summary>
        public static RoutedUICommand PreviousTrack => previousTrack.Value;

        /// <summary>
        /// Gets the value that represents the Fast Forward command.
        /// </summary>
        public static RoutedUICommand FastForward => fastForward.Value;

        /// <summary>
        /// Gets the value that represents the Rewind command.
        /// </summary>
        public static RoutedUICommand Rewind => rewind.Value;

        /// <summary>
        /// Gets the value that represents the Channel Up command.
        /// </summary>
        public static RoutedUICommand ChannelUp => channelUp.Value;

        /// <summary>
        /// Gets the value that represents the Channel Down command.
        /// </summary>
        public static RoutedUICommand ChannelDown => channelDown.Value;

        /// <summary>
        /// Gets the value that represents the Toggle Play/Pause command.
        /// </summary>
        public static RoutedUICommand TogglePlayPause => togglePlayPause.Value;

        /// <summary>
        /// Gets the value that represents the Increase Volume command.
        /// </summary>
        public static RoutedUICommand IncreaseVolume => increaseVolume.Value;

        /// <summary>
        /// Gets the value that represents the Decrease Volume command.
        /// </summary>
        public static RoutedUICommand DecreaseVolume => decreaseVolume.Value;

        /// <summary>
        /// Gets the value that represents the Mute Volume command.
        /// </summary>
        public static RoutedUICommand MuteVolume => muteVolume.Value;

        /// <summary>
        /// Gets the value that represents the Increase Treble command.
        /// </summary>
        public static RoutedUICommand IncreaseTreble => increaseTreble.Value;

        /// <summary>
        /// Gets the value that represents the Decrease Treble command.
        /// </summary>
        public static RoutedUICommand DecreaseTreble => decreaseTreble.Value;

        /// <summary>
        /// Gets the value that represents the Increase Bass command.
        /// </summary>
        public static RoutedUICommand IncreaseBass => increaseBass.Value;

        /// <summary>
        /// Gets the value that represents the Decrease Bass command.
        /// </summary>
        public static RoutedUICommand DecreaseBass => decreaseBass.Value;

        /// <summary>
        /// Gets the value that represents the Boost Bass command.
        /// </summary>
        public static RoutedUICommand BoostBass => boostBass.Value;

        /// <summary>
        /// Gets the value that represents the Increase Microphone Volume command.
        /// </summary>
        public static RoutedUICommand IncreaseMicrophoneVolume => increaseMicrophoneVolume.Value;

        /// <summary>
        /// Gets the value that represents the Decrease Microphone Volume command.
        /// </summary>
        public static RoutedUICommand DecreaseMicrophoneVolume => decreaseMicrophoneVolume.Value;

        /// <summary>
        /// Gets the value that represents the Mute Microphone Volume command.
        /// </summary>
        public static RoutedUICommand MuteMicrophoneVolume => muteMicrophoneVolume.Value;

        /// <summary>
        /// Gets the value that represents the Toggle Microphone On/Off command.
        /// </summary>
        public static RoutedUICommand ToggleMicrophoneOnOff => toggleMicrophoneOnOff.Value;

        /// <summary>
        /// Gets the value that represents the Select command.
        /// </summary>
        public static RoutedUICommand Select => select.Value;

        /// <summary>
        /// Gets the collection of default gestures for the specified command.
        /// </summary>
        private static InputGestureCollection GetInputGestures(String name)
        {
            var gestures = new InputGestureCollection();

            switch (name)
            {
                case nameof(TogglePlayPause):
                    gestures.Add(new KeyGesture(Key.AudioPlay, ModifierKeys.None, "Play"));
                    break;

                case nameof(Stop):
                    gestures.Add(new KeyGesture(Key.AudioStop, ModifierKeys.None, "Stop"));
                    break;

                case nameof(NextTrack):
                    gestures.Add(new KeyGesture(Key.AudioNext, ModifierKeys.None, "Next"));
                    break;

                case nameof(PreviousTrack):
                    gestures.Add(new KeyGesture(Key.AudioPrev, ModifierKeys.None, "Prev"));
                    break;

                case nameof(MuteVolume):
                    gestures.Add(new KeyGesture(Key.AudioMute, ModifierKeys.None, "Mute"));
                    break;

                case nameof(IncreaseVolume):
                    gestures.Add(new KeyGesture(Key.VolumeUp, ModifierKeys.None, "VolumeUp"));
                    break;

                case nameof(DecreaseVolume):
                    gestures.Add(new KeyGesture(Key.VolumeDown, ModifierKeys.None, "VolumeDown"));
                    break;
            }

            return gestures;
        }

        // Property values.
        private static Lazy<RoutedUICommand> play = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_PLAY", nameof(Play), typeof(MediaCommands), GetInputGestures(nameof(Play))));
        private static Lazy<RoutedUICommand> pause = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_PAUSE", nameof(Pause), typeof(MediaCommands), GetInputGestures(nameof(Pause))));
        private static Lazy<RoutedUICommand> stop = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_STOP", nameof(Stop), typeof(MediaCommands), GetInputGestures(nameof(Stop))));
        private static Lazy<RoutedUICommand> record = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_RECORD", nameof(Record), typeof(MediaCommands), GetInputGestures(nameof(Record))));
        private static Lazy<RoutedUICommand> nextTrack = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_NEXT_TRACK", nameof(NextTrack), typeof(MediaCommands), GetInputGestures(nameof(NextTrack))));
        private static Lazy<RoutedUICommand> previousTrack = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_PREVIOUS_TRACK", nameof(PreviousTrack), typeof(MediaCommands), GetInputGestures(nameof(PreviousTrack))));
        private static Lazy<RoutedUICommand> fastForward = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_FAST_FORWARD", nameof(FastForward), typeof(MediaCommands), GetInputGestures(nameof(FastForward))));
        private static Lazy<RoutedUICommand> rewind = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_REWIND", nameof(Rewind), typeof(MediaCommands), GetInputGestures(nameof(Rewind))));
        private static Lazy<RoutedUICommand> channelUp = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_CHANNEL_UP", nameof(ChannelUp), typeof(MediaCommands), GetInputGestures(nameof(ChannelUp))));
        private static Lazy<RoutedUICommand> channelDown = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_CHANNEL_DOWN", nameof(ChannelDown), typeof(MediaCommands), GetInputGestures(nameof(ChannelDown))));
        private static Lazy<RoutedUICommand> togglePlayPause = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_TOGGLE_PLAY_PAUSE", nameof(TogglePlayPause), typeof(MediaCommands), GetInputGestures(nameof(TogglePlayPause))));
        private static Lazy<RoutedUICommand> increaseVolume = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_INCREASE_VOLUME", nameof(IncreaseVolume), typeof(MediaCommands), GetInputGestures(nameof(IncreaseVolume))));
        private static Lazy<RoutedUICommand> decreaseVolume = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_DECREASE_VOLUME", nameof(DecreaseVolume), typeof(MediaCommands), GetInputGestures(nameof(DecreaseVolume))));
        private static Lazy<RoutedUICommand> muteVolume = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_MUTE_VOLUME", nameof(MuteVolume), typeof(MediaCommands), GetInputGestures(nameof(MuteVolume))));
        private static Lazy<RoutedUICommand> increaseTreble = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_INCREASE_TREBLE", nameof(IncreaseTreble), typeof(MediaCommands), GetInputGestures(nameof(IncreaseTreble))));
        private static Lazy<RoutedUICommand> decreaseTreble = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_DECREASE_TREBLE", nameof(DecreaseTreble), typeof(MediaCommands), GetInputGestures(nameof(DecreaseTreble))));
        private static Lazy<RoutedUICommand> increaseBass = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_INCREASE_BASS", nameof(IncreaseBass), typeof(MediaCommands), GetInputGestures(nameof(IncreaseBass))));
        private static Lazy<RoutedUICommand> decreaseBass = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_DECREASE_BASS", nameof(DecreaseBass), typeof(MediaCommands), GetInputGestures(nameof(DecreaseBass))));
        private static Lazy<RoutedUICommand> boostBass = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_BOOST_BASS", nameof(BoostBass), typeof(MediaCommands), GetInputGestures(nameof(BoostBass))));
        private static Lazy<RoutedUICommand> increaseMicrophoneVolume = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_INCREASE_MICROPHONE_VOLUME", nameof(IncreaseMicrophoneVolume), typeof(MediaCommands), GetInputGestures(nameof(IncreaseMicrophoneVolume))));
        private static Lazy<RoutedUICommand> decreaseMicrophoneVolume = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_DECREASE_MICROPHONE_VOLUME", nameof(DecreaseMicrophoneVolume), typeof(MediaCommands), GetInputGestures(nameof(DecreaseMicrophoneVolume))));
        private static Lazy<RoutedUICommand> muteMicrophoneVolume = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_MUTE_MICROPHONE_VOLUME", nameof(MuteMicrophoneVolume), typeof(MediaCommands), GetInputGestures(nameof(MuteMicrophoneVolume))));
        private static Lazy<RoutedUICommand> toggleMicrophoneOnOff = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_TOGGLE_MICROPHONE_ON_OFF", nameof(ToggleMicrophoneOnOff), typeof(MediaCommands), GetInputGestures(nameof(ToggleMicrophoneOnOff))));
        private static Lazy<RoutedUICommand> select = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("MEDIA_COMMAND_SELECT", nameof(Select), typeof(MediaCommands), GetInputGestures(nameof(Select))));
    }
}
