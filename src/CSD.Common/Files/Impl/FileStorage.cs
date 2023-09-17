using System;
using System.IO;
using System.Threading.Tasks;

namespace CSD.Common.Files.Impl;

public class FileStorage : IFileStorage
{
    private const string PHOTO_FOLDER = "./photos";
    private const string AUDIO_FOLDER = "./audios";
    private const string SCENE_FOLDER = "./scenes";
    private const string TEXT_FOLDER = "./texts";

    public FileStorage() {
        if (!Directory.Exists(PHOTO_FOLDER)) Directory.CreateDirectory(PHOTO_FOLDER);
        if (!Directory.Exists(SCENE_FOLDER)) Directory.CreateDirectory(SCENE_FOLDER);
        if (!Directory.Exists(AUDIO_FOLDER)) Directory.CreateDirectory(AUDIO_FOLDER);
        if (!Directory.Exists(TEXT_FOLDER)) Directory.CreateDirectory(TEXT_FOLDER);
    }

    public async Task CreateAsync(Stream content, ContentType contentType, string name) {
        var pathToFile = GetPath(contentType, name);

        if (File.Exists(pathToFile)) {
            var fileExtenstion = Path.GetExtension(pathToFile);
            pathToFile = pathToFile.Insert(pathToFile.Length - fileExtenstion.Length - 1, DateTime.UtcNow.ToString("dd_MM_yyyy_hh_mm_ss"));
        }

        using (var fileStream = new FileStream(pathToFile, FileMode.Create)) {
            content.Seek(0, SeekOrigin.Begin);
            await content.CopyToAsync(fileStream);
        }
    }

    public Task<FileStream> GetContentAsync(ContentType contentType, string name) {
        var pathToFile = GetPath(contentType, name);

        var fileStream = new FileStream(pathToFile, FileMode.Open);
        return Task.FromResult(fileStream);
    }

    private static string GetPath(ContentType contentType, string name) => contentType switch {
        ContentType.Photo => Path.Combine(PHOTO_FOLDER, name),
        ContentType.Scene => Path.Combine(SCENE_FOLDER, name),
        ContentType.Audio => Path.Combine(AUDIO_FOLDER, name),
        ContentType.Text => Path.Combine(TEXT_FOLDER, name),
        _ => throw new ArgumentException("Incorrect content type!")
    };
}
