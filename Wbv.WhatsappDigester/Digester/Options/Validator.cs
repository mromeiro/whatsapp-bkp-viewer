using System.IO.Compression;
using Wbv.WhatsappDigester.Digester.Options;

namespace Wbv.WhatsappDigester.Digester;

public interface IOptionsValidator
{
    string Validate(DigesterOptions options);
}

public class OptionsValidator : IOptionsValidator
{
    public string Validate(DigesterOptions options)
    {
        string dataFolder = options.Data;

        if (options.FileType == SourceData.Zip)
        {
            dataFolder = ZipValidation(options.Data);
        }

        if (options.FileType == SourceData.Folder)
        {
            FolderValidation(options.Data);
        }

        if (!File.Exists($"{dataFolder}/_chat.txt")) throw new IOException("chat file does not exist");

        return dataFolder;
    }

    private string ZipValidation(string zipFile)
    {
        var name = new FileInfo(zipFile).Name;
        var unpackedFolder = Path.GetTempPath() + $"Backup_{name}_{DateTime.UtcNow:yyymmddhhmiss}";

        if (string.IsNullOrEmpty(zipFile)) throw new IOException("Zip file not informed");

        if (!File.Exists(zipFile)) throw new IOException("Zip file does not exist");

        ZipFile.ExtractToDirectory(zipFile, unpackedFolder);
 
        return unpackedFolder;
    }

    private void FolderValidation(string folder)
    {
        if (string.IsNullOrEmpty(folder)) throw new IOException("Folder not informed");

        if (!Directory.Exists(folder)) throw new IOException("Folder does not exist");
    }
}