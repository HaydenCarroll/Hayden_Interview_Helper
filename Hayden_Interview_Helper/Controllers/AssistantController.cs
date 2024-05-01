using Hayden_Interview_Helper.Classes;
using Hayden_Interview_Helper.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Net;
using System.Security.Policy;
using System.Text;
using Markdig;

namespace Hayden_Interview_Helper.Controllers
{
    public class AssistantController : Controller
    {
        private readonly string LLM = "lmstudio-ai/gemma-2b-it-GGUF/gemma-2b-it-q8_0.gguf";
        //private readonly string DEV_URL = "http://localhost:8676/v1/chat/completions";
        private readonly string DEV_URL = "http://haydenllmhost.ddns.net:8676/v1/chat/completions";
        public async Task<ActionResult> Index(AssistantModel assistant)
        {
            if (assistant.Prompt == null || assistant.Prompt.Length == 0)
            {
                assistant.Prompt = "Explain your goal and finish your answer by saying Please feel free to ask me any questions";
            }
            else
            {
                assistant.Prompt = assistant.Prompt;
            }
            // Example code demonstrating AdvancedLLMConnector
            //assistant = new AdvancedLLMConnector(new LLMConnector(DEV_URL, LLM), "You are in an interview with a company that is interviewing Hayden Carroll. You do not want him to get the job. You want to call out how bad he is as programming.")
            //    .CallModel(assistant.Prompt)
            //    .Result;
            assistant = await new LLMConnector(DEV_URL).CallModel(assistant.Prompt);
            if(assistant.Prompt == "Explain your goal and finish your answer by saying Please feel free to ask me any questions")
            {
                assistant.Prompt = "";
            }
            assistant.Response = Markdown.ToHtml(assistant.Response);
            this.ViewBag.Message = assistant;

            return View(assistant);
        }
        [HttpPost]
        public async Task<ActionResult> GetData(AssistantModel assistant)
        {
            // Call post method to get assistant return message
            AssistantModel updatedAssistant = await new LLMConnector(DEV_URL).CallModel(assistant.Prompt);
            // Example code demonstrating AdvancedLLMConnector
            //AssistantModel updatedAssistant = new AdvancedLLMConnector(new LLMConnector(DEV_URL, LLM), "You are in an interview with a company that is interviewing Hayden Carroll. You do not want him to get the job. You want to call out how bad he is as programming.")
            //    .CallModel(assistant.Prompt)
            //    .Result;
            updatedAssistant.Response = Markdown.ToHtml(updatedAssistant.Response);
            
            return View(updatedAssistant);


        }
        

        
    }

       
    
}
