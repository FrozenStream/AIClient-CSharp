using System.Text.Json.Serialization;

namespace AIExtension;


public struct Function
{
    [JsonPropertyName("name")]
    public required string name { get; set; }

    [JsonPropertyName("arguments")]
    public required string arguments { get; set; }
}

public struct ToolCall
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public required string type { get; set; }

    [JsonPropertyName("function")]
    public required Function function { get; set; }

    [JsonPropertyName("index")]
    public required int index { get; set; }
}

public struct Message
{
    [JsonPropertyName("role")]
    public required string role { get; set; }

    [JsonPropertyName("content")]
    public required string content { get; set; }

    [JsonPropertyName("reasoning_content")]
    public string? reasoning_content { get; set; }

    [JsonPropertyName("tool_calls")]
    public ToolCall[]? tool_calls { get; set; }

    // [JsonPropertyName("refusal")]
    // public string? refusal { get; set; }

    // [JsonPropertyName("audio")]
    // public string? audio { get; set; }
}

public struct Choice
{
    [JsonPropertyName("message")]
    public required Message message { get; set; }

    [JsonPropertyName("finish_reason")]
    public required string finish_reason { get; set; }

    [JsonPropertyName("index")]
    public required int index { get; set; }

    // [JsonPropertyName("logprobs")]
    // public object? logprobs { get; set; }
}

public struct Usage
{
    [JsonPropertyName("completion_tokens")]
    public required int completion_tokens { get; set; }

    [JsonPropertyName("prompt_tokens")]
    public required int prompt_tokens { get; set; }

    [JsonPropertyName("total_tokens")]
    public required int total_tokens { get; set; }
}

public struct ChatResult
{
    [JsonPropertyName("id")]
    public required string id { get; set; }

    [JsonPropertyName("choices")]
    public required Choice[] choices { get; set; }

    [JsonPropertyName("created")]
    public required long created { get; set; }

    [JsonPropertyName("model")]
    public required string model { get; set; }

    [JsonPropertyName("usage")]
    public required Usage usage { get; set; }

        //固定值
    // [JsonPropertyName("object")]
    // public string obj { get; set; }

    // [JsonPropertyName("service_tier")]
    // public string service_tier { get; set; }

    // [JsonPropertyName("system_fingerprint")]
    // public string system_fingerprint { get; set; }
}

