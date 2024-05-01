using Hayden_Interview_Helper.Models;

namespace Hayden_Interview_Helper.Classes
{
    public class AdvancedLLMConnector:LLMConnector
    {
        public string Role { get; set; }
        public AdvancedLLMConnector(LLMConnector connector, string Role)
            : base(connector.URL, connector.LLM)
        {
            this.Role = Role;
        }
        public async Task<AssistantModel> CallModel(string prompt, string Role)
        {
            // Create new assistant model
            AssistantModel assistant = new AssistantModel();
            // Remind model who they are
            assistant.SystemRole = Role;
                // Set prompt value
            assistant.Prompt = prompt;
            // Generate json request body with role and prompt
            string requestBody = $"{{ \"model\": \"{LLM}\", \"messages\": [{{ \"role\": \"system\", \"content\": \"{assistant.SystemRole}\" }},{{ \"role\": \"user\", \"content\": \"{assistant.Prompt}\" }} ], \"temperature\": 0.7, \"max_tokens\": -1,\"stream\": false}}";
            // Make post request
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the content type
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    // Create the request content
                    HttpContent content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");

                    // Send the POST request
                    HttpResponseMessage response = await client.PostAsync(URL, content);

                    // Check if the response is successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and display the response
                        string responseBody = await response.Content.ReadAsStringAsync();
                        assistant.Response = ParseMessageValue(responseBody);
                        Console.WriteLine("Response: " + responseBody);
                    }
                    else
                    {
                        assistant.Response = response.StatusCode.ToString();
                        Console.WriteLine("Failed to make a POST request. Status code: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    assistant.Response = ex.Message;
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
            return assistant;
        }
    }
}
