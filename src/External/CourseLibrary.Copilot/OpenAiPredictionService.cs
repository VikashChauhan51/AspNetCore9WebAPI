using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace CourseLibrary.Copilot;

public class OpenAiPredictionService : IOpenAiPredictionService
{
    private readonly OpenAIClient _client;

    public OpenAiPredictionService(IConfiguration configuration)
    {
        var apiKey = configuration["OpenAI:ApiKey"];
        _client = new OpenAIClient(apiKey);
    }

    public async Task<ClientResult<ChatCompletion>> PredictWeatherAsync(string json)
    {
        string prompt = $"Analyze the following weather data and predict the temperature and humidity for the next 2 days:\n{json}";

        var parts = new List<ChatMessageContentPart>()
        {
            ChatMessageContentPart.CreateTextPart(prompt)
        };
        var chatRequest = ChatMessage.CreateUserMessage(parts);
        return await _client.GetChatClient("gpt-4o").CompleteChatAsync(chatRequest);
    }
}
