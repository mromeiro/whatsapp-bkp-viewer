using Wbv.WhatsappDigester.Messaging.Model;

namespace Wbv.WhatsappDigester.Messaging.Parser;

internal class MessageInfo
{
    public bool IsComplete { get; set; }
    public Message Content { get; set; }
}