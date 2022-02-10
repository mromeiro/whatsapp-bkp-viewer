using Microsoft.Extensions.Logging;
using Wbv.WhatsappDigester.Logging;
using Wbv.WhatsappDigester.Messaging.Model;
using Wbv.WhatsappDigester.Messaging.Parser;

namespace Wbv.WhatsappDigester.Messaging.Reader;


internal class MessageReader : IDisposable
{
    private StreamReader Reader { get; }

    private MessageParser Parser { get; }

    private ILogger _logger;

    public MessageReader(string outputFolder, ILogger logger)
    {
        Reader = new StreamReader($"{outputFolder}/_chat.txt");
        Parser = new MessageParser();
        _logger = logger;
    }

    public List<Message> ReadNextMessages(int maxMessages, string? messageToFind = null)
    {
        _logger.Info(maxMessages == -1 ? "Getting all messages" : $"Getting next {maxMessages} messages");
        int counter = 0;
        string? line;
        var messages = new List<Message>();

        while ((line = Reader.ReadLine()) != null)
        {
            var message = Parser.Parse(line);

            if (message.IsComplete && (messageToFind == null || (message.Content.Content.Contains(messageToFind))))
            {
                messages.Add(message.Content);
                counter++;

                if (counter >= maxMessages && maxMessages != -1) break;
            }
        }

        return messages;
    }

    public void Dispose()
    {
        Reader.Dispose();
    }
}