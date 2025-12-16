using System.Text.Json.Serialization;
using System.Text.Json;


namespace AIExtension;

class MessageIn(string role, object? content, ToolCall[]? tool_calls = null, string? tool_call_id = null)
{
    [JsonPropertyName("role")]
    [JsonInclude]
    public readonly string role = role;

    [JsonPropertyName("content")]
    [JsonInclude]
    public readonly object? content = content;

    [JsonPropertyName("tool_calls")]
    [JsonInclude]
    public readonly ToolCall[]? tool_calls = tool_calls;

    [JsonPropertyName("tool_call_id")]
    [JsonInclude]
    public readonly string? tool_call_id = tool_call_id;
}



public class HistoryList
{
    private List<MessageIn> _messages;
    private string? _savePath;

    public HistoryList(string? savePath = null)
    {
        if (savePath != null && File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            _messages = JsonSerializer.Deserialize<List<MessageIn>>(json) ?? new List<MessageIn>();
        }
        if (_messages == null) _messages = new List<MessageIn>();
        _savePath = savePath;
    }

    public void SaveHistory(string? savePath = null)
    {
        if (savePath == null) savePath = _savePath;
        if (savePath == null) return;
        string json = JsonSerializer.Serialize(_messages,
            new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true // 添加缩进便于调试
            }
        );
        Console.WriteLine($"Saving history to {Path.GetFullPath(savePath)}...");
        File.WriteAllText(savePath, json);
    }

    public void AddSystem(in string content)
    {
        _messages.Add(new MessageIn(
            role: "system",
            content: content));
    }

    public void AddUser(in string content)
    {
        _messages.Add(new MessageIn(
            role: "user",
            content: content));
    }

    public void AddAssistant(in string content, in ToolCall[]? tool_calls = null)
    {
        _messages.Add(new MessageIn(
            role: "assistant",
            content: content,
            tool_calls: tool_calls));
    }

    public void AddAssistant(in Message message)
    {
        AddAssistant(message.content, message.tool_calls);
    }

    public void AddToolResult(in string content, in string tool_call_id)
    {
        _messages.Add(new MessageIn(
            role: "tool",
            content: content,
            tool_call_id: tool_call_id));
    }

    public string ToJsonString()
    {
        // 添加 WriteIndented 以便调试，并确保正确序列化
        return JsonSerializer.Serialize(_messages,
            new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true // 添加缩进便于调试
            }
        );
    }

    // 添加一个用于调试的方法，检查列表内容
    public int Count()
    {
        return _messages.Count;
    }

    public bool IsEmpty()
    {
        return _messages.Count == 0;
    }
}