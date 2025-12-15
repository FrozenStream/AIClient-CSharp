using System.Text.Json.Serialization;
using System.Text.Json;


namespace AIExtension;

// 添加 JsonInclude 特性以确保字段被包含在序列化中
class MessageIn(string role, string content)
{
    [JsonPropertyName("role")]
    [JsonInclude]
    public readonly string role = role;

    [JsonPropertyName("content")]
    [JsonInclude]
    public readonly string? content = content;
}

class SystemMessageIn: MessageIn
{
    public SystemMessageIn(string content) : base("system", content) { }
}

class UserMessageIn: MessageIn
{
    public UserMessageIn(string content) : base("user", content) { }
}

class AssistantMessageIn: MessageIn
{
    [JsonPropertyName("tool_calls")]
    [JsonInclude]
    public readonly ToolCall[]? tool_calls;
    
    public AssistantMessageIn(string content, ToolCall[]? tool_calls = null) : base("assistant", content)
    {
        this.tool_calls = tool_calls;
    }
}

class ToolMessageIn: MessageIn
{
    [JsonPropertyName("tool_call_id")]
    [JsonInclude]
    public readonly string tool_call_id;
    
    public ToolMessageIn(string content, string tool_call_id) : base("tool", content)
    {
        this.tool_call_id = tool_call_id;
    }
}



public class HistoryList
{
    private List<MessageIn> _messages;

    public HistoryList()
    {
        _messages = new List<MessageIn>();
    }

    public void AddSystem(in string content)
    {
        _messages.Add(new SystemMessageIn(content));
    }

    public void AddUser(in string content)
    {
        _messages.Add(new UserMessageIn(content));
    }

    public void AddAssistant(in string content, in ToolCall[]? tool_calls = null)
    {
        _messages.Add(new AssistantMessageIn(content, tool_calls));
    }

    public void AddToolResult(in string content, in string tool_call_id)
    {
        _messages.Add(new ToolMessageIn(content, tool_call_id));
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