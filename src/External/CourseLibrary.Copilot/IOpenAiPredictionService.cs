using OpenAI.Chat;
using System.ClientModel;

namespace CourseLibrary.Copilot
{
    public interface IOpenAiPredictionService
    {
        Task<ClientResult<ChatCompletion>> PredictWeatherAsync(string json);
    }
}