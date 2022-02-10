using System.Text.RegularExpressions;
using Wbv.WhatsappDigester.Messaging.Model;

namespace Wbv.WhatsappDigester.Messaging.Parser;

internal class MessageParser
{
    private MessageInfo? _incompleteMessage;

    public MessageInfo Parse(string line)
    {
        var messageType = GetMessageTipe(line);
        var timestamp = ExtractTimestamp(line, messageType);
        if (timestamp.HasValue)
        {
            var completeMessage = new MessageInfo();

            if (_incompleteMessage != null)
            {
                completeMessage.Content = _incompleteMessage.Content;
                completeMessage.IsComplete = true;
            }
            
            _incompleteMessage = new MessageInfo()
            {
                IsComplete = false,
                Content = new Message()
                {
                    Content = GetMessage(line, true, messageType),
                    Timestamp = timestamp.Value,
                    From = GetSender(line),
                    Type = messageType
                }
            };

            return completeMessage;
        }

        _incompleteMessage.Content.Content += $"\n{GetMessage(line, false, MessageType.Text)}";
        return _incompleteMessage;
    }

    private DateTime? ExtractTimestamp(string line, MessageType messageType)
    {

        var result = Regex.Match(line, "[0-9]{2}/[0-9]{2}/[0-9]{4} [0-9]{2}:[0-9]{2}:[0-9]{2}", RegexOptions.IgnoreCase);
        if (result.Success)
        {
            var date = DateTime.ParseExact(result.Value, "dd/MM/yyyy HH:mm:ss", null);
            return date;
        }

        return null;
    }   

    private string GetSender(string line)
    {
        return line.Substring(22,line.IndexOf(": ", StringComparison.Ordinal) - 22);
    }

    private string GetMessage(string line, bool newMessage, MessageType type)
    {
        if (type == MessageType.Audio)
        {
            var result = Regex.Match(line, "[0-9]{8}-AUDIO-[0-9]{4}-[0-9]{2}-[0-9]{2}-[0-9]{2}-[0-9]{2}-[0-9]{2}.opus", RegexOptions.IgnoreCase);
            if (result.Success)
            {
                return result.Value;
            }
        }

        return !newMessage ? line : line.Substring(line.IndexOf(": ", StringComparison.InvariantCulture) + 2);
    }

    private MessageType GetMessageTipe(string line)
    {
        if (line.Contains("anexado:") && line.Contains(".opus"))
            return MessageType.Audio;

        return MessageType.Text;
    }
    
}