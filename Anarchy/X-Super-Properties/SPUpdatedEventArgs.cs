using System;

namespace Discord
{
    public class SPUpdatedEventArgs : EventArgs
    {
        public SPInformation Properties { get; private set; }

        public SPUpdatedEventArgs(SPInformation properties)
        {
            Properties = properties;
        }
    }
}
