// Simple console application to show how the whatsapp digester works

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wbv.WhatsappDigester;
using Wbv.WhatsappDigester.Digester;
using Wbv.WhatsappDigester.Digester.Options;
using Wbv.WhatsappDigester.Logging;

// Setting up dependency injection
var serviceCollection = new ServiceCollection();
serviceCollection.RegisterWhatsappDigesterServices();
serviceCollection.RegisterLoggingServices();
var serviceProvider = serviceCollection.BuildServiceProvider();

//Getting console app logger
var logger = serviceProvider.GetService<ILogger<Program>>();
logger.Info("Initializing application");

//Gettting whatsapp digester service
using var digester = serviceProvider.GetService<IWhatsappDigester>();
if (digester == null) return;

//Initializing whatsapp digester service
digester.InitializeDigester(new DigesterOptions()
{
    FileType = args[0].ToLower().Equals("zip") ? SourceData.Zip : SourceData.Folder,
    Data = args[1]
});

//Playing an audio
//var audioToPlay = digester.DecodeAudio("00000026-AUDIO-2021-05-31-21-00-53.opus");
//SoundPlayer p = new SoundPlayer(audioToPlay);
//p.Play();

//Getting messages
var messages = digester.GetNextMessages(2);
messages.ForEach(_ => Console.WriteLine($"[{_.Timestamp}] {_.From}: {_.Content}"));

digester.ResetReader();

messages = digester.GetNextMessages(2);
messages.ForEach(_ => Console.WriteLine($"[{_.Timestamp}] {_.From}: {_.Content}"));

Console.WriteLine("Press any key to stop");
Console.ReadLine();