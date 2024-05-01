using Hayden_Interview_Helper.Models;
using static System.Net.WebRequestMethods;

namespace Hayden_Interview_Helper.Classes
{
    public class LLMConnector : ILLMConnector
    {
        public string LLM {  get; set; }
        public string URL { get; set; }

        public LLMConnector(string URL = "http://haydenllmhost.ddns.net:8675/v1/chat/completions", string LLM = "TheBloke/CodeLlama-7B-Instruct-GGUF/codellama-7b-instruct.Q4_K_S.gguf")
        {
            this.LLM = LLM;
            this.URL = URL;
        }
        public async Task<AssistantModel> CallModel(string prompt="Explain why Hayden Carroll is the best software engineer in the industry.")
        {
            // Create new assistant model
            AssistantModel assistant = new AssistantModel();
            // Remind model who they are
            assistant.SystemRole = "Hayden Carroll is a software engineer looking to get a new job, and is actively interviewing for a software development position. " +
                "You are not Hayden Carroll, you are an AI assistant who is trying to convince employers to hire him by highlighting his talents and skills. " +
                "Here is some information from Hayden's resume, he is an Experienced software engineer proficient in a diverse range of solution suites, frameworks, " +
                "and environments, adept at navigating varying scaling and restrictions. Skilled in designing and implementing native, web, and service solutions with a " +
                "track record of delivering projects on time and within scope. Capable of effectively engaging with both internal and external stakeholders, maintaining " +
                "clear communication, and aligning solutions with company objectives and values. His previous experience includes working as a Software Engineer II at " +
                "Ahold Delhaize where he did the following Continued development, support and responsibilities from internship (.NET development, Azure DevOps automation, " +
                "UI/UX modernization, etc.), Planning and design for future and current business applications and processes which includes tech stacks, environments, " +
                "design methodologies, level of refinement, etc., Managing and reviewing releases for core business applications including code review, feature release " +
                "timelines, and version control and management from within a release manager role., Evaluation and migration of legacy applications and processes to " +
                "modernized LTS infrastructures and ecosystems to improve solution health and performance., Full stack .NET development and support for core business " +
                "applications which aid in the retrieval and presentation of real-time product data  for stores across the eastern United States., Aid in the transition " +
                "from the previous project management process to  modern approaches for SCRUM and DevOps, utilizing Microsoft’s  Azure DevOps git and CI/CD automation " +
                "services., Assist with UX/UI overhaul of various interfaces to provide users with  effective and intuitive solutions to increase productivity and improve  " +
                "overall experience., and Identify and correct performance pitfalls by employing methods such  as algorithm optimization, introducing concurrency, and optimal  " +
                "error handling. He has skills and experience in the following areas Software Development Solution Design Release Management Data Warehousing Product Support Web " +
                "Development Data Analysis and Processing Relational Databases UNIX Systems .Net (ASP/Core/Framework) Azure Cloud Azure Devops/TFS Spring React Node MSSQL " +
                "Visual Studio Flask JetBrains ServiceNow SSIS He knows the following programming languages C# (C#.NET), Python, Java, Visual Basic (VB.NET), SQL, TSQL, " +
                "HTML, CSS, JavaScript, JQuery, TypeScript, C, C++, XML, YAML, JSON, Powershell, BASH, BATCH.";
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
        // Protected method to that derived classes can still utilize but method remains private
        protected string ParseMessageValue(string jsonReturn)
        {
            string messageValue = "";
            if (jsonReturn.Contains("\"content\": \""))
            {
                //Get value between two knows sandwich strings
                messageValue = jsonReturn.Split("\"content\": \"")[1];
                messageValue = messageValue.Split("},")[0];
                messageValue = messageValue.Substring(0, messageValue.Length - 8);
                messageValue = messageValue.Replace("\\n", Environment.NewLine);
            }
            return messageValue;
        }
    }
}
