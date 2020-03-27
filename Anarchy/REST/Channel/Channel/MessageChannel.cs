using System.Collections.Generic;

namespace Discord
{
    public interface MessageChannel
    {
        void TriggerTyping();
        Message SendMessage(string message, bool tts = false, Embed embed = null);
        IReadOnlyList<Message> GetMessages(MessageFilters filters = null);
        IReadOnlyList<Message> GetPinnedMessages();
        void PinMessage(ulong messageId);
        void PinMessage(Message message);
        void UnpinMessage(ulong messageId);
        void UnpinMessage(Message message);
    }
}
