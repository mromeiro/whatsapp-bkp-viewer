using Concentus.Oggfile;
using NAudio.Wave;

namespace Wbv.WhatsappDigester.Audio;

public class OpusDecoder
{
 
    public string Decode(string file)
    {

        var fileInfo = new FileInfo(file);
        using (FileStream fileIn = new FileStream(file, FileMode.Open))
        {
            using (MemoryStream pcmStream = new MemoryStream())
            {
                Concentus.Structs.OpusDecoder decoder = Concentus.Structs.OpusDecoder.Create(48000, 1);
                OpusOggReadStream oggIn = new OpusOggReadStream(decoder, fileIn);
                while (oggIn.HasNextPacket)
                {
                    short[] packet = oggIn.DecodeNextPacket();
                    if (packet != null)
                    {
                        for (int i = 0; i < packet.Length; i++)
                        {
                            var bytes = BitConverter.GetBytes(packet[i]);
                            pcmStream.Write(bytes, 0, bytes.Length);
                        }
                    }
                }

                pcmStream.Position = 0;
                var wavStream = new RawSourceWaveStream(pcmStream, new WaveFormat(48000, 1));
                var sampleProvider = wavStream.ToSampleProvider();

                string outputFile = $"{fileInfo.Directory}\\{fileInfo.Name}.wav";
                WaveFileWriter.CreateWaveFile16(outputFile, sampleProvider);

                return outputFile;
            }
        }
    }
 
}