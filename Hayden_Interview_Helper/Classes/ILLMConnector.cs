using Hayden_Interview_Helper.Models;

namespace Hayden_Interview_Helper.Classes
{
    public interface ILLMConnector
    {
        Task<AssistantModel> CallModel(string prompt);
        
    }
}