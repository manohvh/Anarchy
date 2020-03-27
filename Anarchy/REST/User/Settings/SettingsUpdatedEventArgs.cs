using System;

namespace Discord
{
    public class SettingsUpdatedEventArgs : EventArgs
    {
        public Settings Settings { get; private set; }

        internal SettingsUpdatedEventArgs(Settings settings)
        {
            Settings = settings;
        }
    }
}
