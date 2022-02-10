namespace Wbv.WhatsappDigester.Messaging.Model;

public class Message
{
    public Message()
    {
        Id = Guid.NewGuid();
    }

    public DateTime Timestamp { get; set; }
    public string Content { get; set; }

    public string From { get; set; }

    public MessageType Type { get; set; }

    public Guid Id { get; set; }


}