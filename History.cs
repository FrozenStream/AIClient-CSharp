using System.Text.Json.Serialization;
using System.Text.Json;

struct Message
{
    [JsonPropertyName("role")]
    public string role { get; }

    [JsonPropertyName("content")]
    public string content { get; }

    [JsonPropertyName("tool_call_id")]
    public string? tool_call_id { get; }

    [JsonPropertyName("name")]
    public string? tool_call_name { get; }

    public Message(string role, string content)
    {
        this.role = role;
        this.content = content;
        this.tool_call_id = null;
        this.tool_call_name = null;
    }

    public Message(string tool_call_id, string tool_call_name, string content)
    {
        this.role = "tool";
        this.tool_call_id = tool_call_id;
        this.tool_call_name = tool_call_name;
        this.content = content;
    }
}

public class HistoryList
{
    private List<Message> _messages;

    public HistoryList()
    {
        _messages = new List<Message>();
    }

    public void AddSystem(string content)
    {
        _messages.Add(new Message("system", content));
    }

    public void AddUser(string content)
    {
        _messages.Add(new Message("user", content));
    }

    public void AddAssistant(string content)
    {
        _messages.Add(new Message("assistant", content));
    }

    public void AddToolResult(string tool_call_id, string tool_call_name, string content)
    {
        _messages.Add(new Message(tool_call_id, tool_call_name, content));
    }

    public string ToJsonString()
    {
        return JsonSerializer.Serialize(_messages,
            new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            }
        );
    }
}