namespace Discord.Gateway
{
    public class VoiceStateEventArgs
    {
        public VoiceState State { get; private set; }


        internal VoiceStateEventArgs(VoiceState state)
        {
            State = state;
        }
    }
}
