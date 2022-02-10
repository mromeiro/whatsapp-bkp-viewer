using Microsoft.Extensions.Logging;
using Wbv.WhatsappDigester.Audio;
using Wbv.WhatsappDigester.Digester.Options;
using Wbv.WhatsappDigester.Logging;
using Wbv.WhatsappDigester.Messaging.Model;
using Wbv.WhatsappDigester.Messaging.Reader;

namespace Wbv.WhatsappDigester.Digester;

public interface IWhatsappDigester : IDisposable
{
    string DecodeAudio(string opusAudio);

    List<Message> GetNextMessages(int maxMessages);

    List<Message> GetMessages();

    void ResetReader();

    List<Message> FindMessages(string messageToFind);

    void InitializeDigester(DigesterOptions options);
}

public class WhatsappDigester : IWhatsappDigester
{
    private readonly ILogger _logger;

    public WhatsappDigester(ILogger<IWhatsappDigester> logger = null)
    {
        _logger = logger;
    }

    private DigesterOptions Options { get; set; }
    

    private MessageReader MessageReader { get; set; }

    private string _dataFolder;

    public void InitializeDigester(DigesterOptions options)
    {
        _logger?.Info(options.FileType == SourceData.Zip 
            ? $"Initializing digester from file {options.Data}" 
            : $"Initializing digester from folder {options.Data}");

        Options = options;
        _dataFolder = new OptionsValidator().Validate(Options);
        MessageReader = new MessageReader(_dataFolder, _logger);
    }

    public string DecodeAudio(string opusAudio)
    {
        _logger.Info($"Geting audio {opusAudio}");
        OpusDecoder decoder = new OpusDecoder();
        var convertedOpusAudioFile = $"{_dataFolder}\\{opusAudio}.wav";

        return File.Exists(convertedOpusAudioFile) ? convertedOpusAudioFile : decoder.Decode($"{ _dataFolder}\\{opusAudio}");
    }

    public List<Message> GetNextMessages(int maxMessages)
    {
        return MessageReader.ReadNextMessages(maxMessages);
    }

    public List<Message> GetMessages() 
    {
        return MessageReader.ReadNextMessages(-1);
    }

    public void ResetReader()
    {
        MessageReader.Dispose();
        MessageReader = new MessageReader(_dataFolder, _logger);
    }

    public List<Message> FindMessages(string messageToFind)
    {
        return MessageReader.ReadNextMessages(-1, messageToFind);
    }
    public void Dispose()
    {
        MessageReader.Dispose();
    }
}