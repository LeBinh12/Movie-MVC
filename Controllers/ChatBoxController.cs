using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WbeMovieUser.Controllers
{
    public class ChatBoxController : Controller
    {
        private const string openAiApiKey = "AIzaSyBXzG_fW6shj6AGS1bSN6PbNxZH1IM7adU";
        [HttpPost]
        public async Task<JsonResult> GetMovieSuggestionFromGemini(string userMessage)
        {
            string gptResponse = await SendToGeminiAsync(userMessage);
            return Json(gptResponse);
        }

        public async Task<string> SendToGeminiAsync(string userPrompt)
        {
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={openAiApiKey}";

            using (var httpClient = new HttpClient())
            {
                var prompt = $"Bạn là một trợ lý chuyên tư vấn phim. Hãy gợi ý phim cho người dùng bằng tiếng Việt, dựa trên yêu cầu sau: \"{userPrompt}\" và trả lời ngắn gọn giúp tôi";
                var requestBody = new
                {
                    contents = new[]
                    {
                new
                {
                    parts = new[]
                    {
                        new {
                            text = prompt
                        }
                    }
                }
            }
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await httpClient.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return $"Lỗi gọi API: {response.StatusCode} - {result}";
                }


                dynamic json = JsonConvert.DeserializeObject(result);
                return json?.candidates?[0]?.content?.parts?[0]?.text ?? "Không có phản hồi.";
            }
        }
    }
}