using System.Threading.Tasks;

namespace CSD.Common.VoiceRecognition;

public interface IVoiceRecognitionService
{
    Task<string> GetTextFromAudioAsync(string pathToAudio);
}
