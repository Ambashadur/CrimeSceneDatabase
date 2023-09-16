using CSD.Domain.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace CSD.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SceneController : ControllerBase
{
    private const string PHOTO_FOLDER = "photos";
    private const string SCENE_FOLDER = "scene";
    private const string AUDIO_FOLDER = "audio";
    private const string TEXT_FOLDER = "text";

    public SceneController() {
        if (!Directory.Exists(PHOTO_FOLDER)) Directory.CreateDirectory(PHOTO_FOLDER);
        if (!Directory.Exists(SCENE_FOLDER)) Directory.CreateDirectory(SCENE_FOLDER);
        if (!Directory.Exists(AUDIO_FOLDER)) Directory.CreateDirectory(AUDIO_FOLDER);
        if (!Directory.Exists(TEXT_FOLDER)) Directory.CreateDirectory(TEXT_FOLDER);
    }

    [HttpPost("{id:long}")]
    public async Task SetScene([FromRoute] long id, IFormFile formFile) {
        if (formFile.Length > 0) {
            var folderPath = Path.Combine(SCENE_FOLDER, id.ToString());

            if (Directory.Exists(folderPath)) {
                foreach (var file in Directory.GetFiles(folderPath)) {
                    System.IO.File.Delete(file);
                }
            } else {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, formFile.FileName);

            using var stream = System.IO.File.Create(filePath);
            await formFile.CopyToAsync(stream);
        }
    }

    [HttpGet("{id:long}")]
    public IActionResult GetScene([FromRoute] long id) {
        var files = Directory.GetFiles(Path.Combine(SCENE_FOLDER, id.ToString()));

        if (files.Length == 0) return NotFound();

        var stream = new FileStream(files[0], FileMode.Open);
        var fileInfo = new FileInfo(files[0]);

        return File(stream, "image/" + fileInfo.Extension[1..], fileInfo.Name);
    }

    [HttpPost("{id:long}/photos")]
    public async Task AddPhoto([FromRoute] long id, IFormFile formFile) {
        if (formFile.Length > 0) {
            var folderPath = Path.Combine(PHOTO_FOLDER, id.ToString());

            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, formFile.FileName);

            using var stream = System.IO.File.Create(filePath);
            await formFile.CopyToAsync(stream);
        }
    }

    [HttpGet("{id:long}/photos")]
    public IActionResult GetPhotos([FromRoute] long id) {
        var files = Directory.GetFiles(Path.Combine(PHOTO_FOLDER, id.ToString()));

        if (files.Length == 0) return NotFound();

        var zipName = $"Фотографии_пользователя_{id}_{DateTime.Now.ToString("yyyy_MM_dd-HH-mm-ss")}.zip";
        using var memoryStream = new MemoryStream();
        using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create)) {
            foreach (var file in files) {
                var entry = zip.CreateEntry(file);
                using (var fileStream = new FileStream(file, FileMode.Open)) {
                    using (var entryStream = entry.Open()) {
                        fileStream.CopyTo(entryStream);
                    }
                }
            }
        }

        return File(memoryStream.ToArray(), "application/zip", zipName);
    }

    [HttpPost("{id:long}/text")]
    public void AddText([FromRoute] long id, CommentDto comment) {
        var folderPath = Path.Combine(TEXT_FOLDER, id.ToString());

        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, string.Format("comment_{0}.txt", DateTime.Now.ToString("yyyy_MM_dd-HH-mm-ss")));
        System.IO.File.WriteAllText(filePath, comment.Comment);
    }
}
