using System.IO;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Whisper.net;
using Whisper.net.Ggml;

namespace CSD.Common.VoiceRecognition.Impl;

public class VoiceRecognitionService : IVoiceRecognitionService
{
    private const string MODEL_NAME = "ggml-base.bin";
    private bool _isModelDownloadRequired = true;

    public async Task<string> GetTextFromAudioAsync(string pathToAudio) {
        if (_isModelDownloadRequired) await TryDownloadModelAsync();

        var sb = new StringBuilder();

        using var whisperFactory = WhisperFactory.FromPath(MODEL_NAME);
        using var processor = whisperFactory
            .CreateBuilder()
            .WithLanguage("ru")
            .Build();

        using WaveStream reader = Path.GetExtension(pathToAudio) switch {
            ".mp3" => new Mp3FileReader(pathToAudio),
            _ => new WaveFileReader(pathToAudio)
        };

        using var resampleStream = new MemoryStream();
        var resampler = new WdlResamplingSampleProvider(reader.ToSampleProvider(), 16000);
        WaveFileWriter.WriteWavFileToStream(resampleStream, resampler.ToWaveProvider16());

        resampleStream.Seek(0, SeekOrigin.Begin);

        await foreach (var result in processor.ProcessAsync(resampleStream)) {
            sb.Append(result.Start);
            sb.Append(" - ");
            sb.Append(result.End);
            sb.Append(": ");
            sb.Append(result.Text);
            sb.AppendLine();
        }

        return sb.ToString();
    }

    private async Task TryDownloadModelAsync() {
        if (File.Exists(MODEL_NAME)) {
            _isModelDownloadRequired = false;
            return;
        }

        using var modelStream = await WhisperGgmlDownloader.GetGgmlModelAsync(GgmlType.Base);
        using var fileWriter = File.OpenWrite(MODEL_NAME);
        await modelStream.CopyToAsync(fileWriter);
    }
}
