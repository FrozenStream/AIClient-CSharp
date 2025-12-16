using System.Text.Json;
using System.Diagnostics;

namespace AIExtension.Tools;

/// <summary>
/// 参数数据类型，用于表示各种类型的参数值
/// </summary>
public class Parameter
{
    public Dictionary<string, object> parameter { get; }
    Type Type;

    public Parameter(Type type)
    {
        parameter = new Dictionary<string, object>();
        parameter["type"] = TypeToSchemaMapper.GetJsonSchemaType(type);
        Type = type;
    }

    public Parameter SetDescription(string description)
    {
        parameter["description"] = description;
        return this;
    }

    public Parameter SetIsRequired(bool isRequired)
    {
        parameter["isRequired"] = isRequired;
        return this;
    }

    public Parameter SetDefaultValue(object defaultValue)
    {
        Debug.Assert(Type == defaultValue.GetType(), $"value type: {defaultValue.GetType()} not match parameter type: {Type}");
        parameter["default"] = defaultValue;
        return this;
    }

    public Parameter SetEnum(List<object> enumValues)
    {
        Debug.Assert(enumValues.All(e => Type == e.GetType()), $"enum value type not match parameter type: {Type}");
        parameter["enum"] = enumValues;
        return this;
    }
}


public interface IFunction
{
    public Dictionary<string, Parameter> parameters { get; protected set; }
    string description { get; }
    bool? strict { get; }
    string functionJson { get; }

    static protected string BuildFunctionJson(IFunction function)
    {
        var dic = new Dictionary<string, object>()
        {
            ["type"] = "function",
            ["function"] = new Dictionary<string, object>()
            {
                ["name"] = function.GetType().Name,
                ["description"] = function.description ?? "",
                ["parameters"] = new Dictionary<string, object>()
                {
                    ["type"] = "object",
                    ["properties"] = function.parameters.ToDictionary(p => p.Key, p => p.Value.parameter),
                    ["required"] = function.parameters
                                            .Where(p => p.Value.parameter["isRequired"].Equals(true))
                                            .Select(p => p.Key).ToList()
                }
            }
        };

        return JsonSerializer.Serialize(dic, new JsonSerializerOptions { WriteIndented = true });
    }

    public string Call(in string argsJson);

};



public class ToolsList
{
    public Dictionary<string, IFunction> tools { get; protected set; }
    public ToolsList()
    {
        tools = new Dictionary<string, IFunction>();
    }


    public void Add(IFunction tool)
    {
        tools.Add(tool.GetType().Name, tool);
    }

    public string serialize()
    {
        var toolList = tools.Values.Select(tool => tool.functionJson).ToList();
        return "[" + string.Join(",", toolList) + "]";
    }

    public Dictionary<string, string> CallTools(in Message message)
    {
        var results = new Dictionary<string, string>();
        if (message.tool_calls == null) throw new ArgumentException("message not contain tool_calls");

        foreach (var toolCall in message.tool_calls)
        {
            Console.WriteLine($"工具调用ID: {toolCall.Id}");
            Console.WriteLine($"工具调用名称: {toolCall.function.name}");
            Console.WriteLine($"工具调用内容: {toolCall.function.arguments}");
            if (tools.TryGetValue(toolCall.function.name, out IFunction? tool))
            {
                results.Add(toolCall.Id, tool.Call(toolCall.function.arguments));
            }
        }
        return results;
    }
}
