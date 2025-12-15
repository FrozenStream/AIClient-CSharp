using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AIExtension
{
    public class AIClient(string model, string apiKey, string apiUrl, string? tools = null)
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly string _model = model;
        private readonly string _apiKey = apiKey;
        private readonly string _apiUrl = apiUrl;
        private readonly string? _tools = tools;

        private string _buildContent(string history)
        {
            if (_tools == null)
                return $@"{{""model"":""{_model}"",""messages"":{history}}}";
            else
                return $@"{{""model"":""{_model}"",""messages"":{history},""tools"":{_tools}}}";
        }

        public async Task<ChatResult> CompleteChat(string history)
        {
            try
            {
                var content = new StringContent(_buildContent(history), Encoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await httpClient.PostAsync(_apiUrl, content);
                string responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Request failed: {response.StatusCode}, Error content: {responseContent}");

                ChatResult? result = JsonSerializer.Deserialize<ChatResult>(responseContent);
                if (result == null)
                    throw new InvalidOperationException($"API response format error: {responseContent}");

                return result.Value;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"API response JSON format error: {ex.Message}", ex);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Request exception: {ex.Message}", ex);
            }
        }
    }
}