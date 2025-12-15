using System.Text.Json;

namespace AIExtension.Tools;

class getTimeTool : IFunction
{
    public Dictionary<string, Parameter> parameters { get; set; }
    public string description { get; }
    public bool? strict { get; }
    public string functionJson { get; }

    public getTimeTool()
    {
        parameters = new Dictionary<string, Parameter>{};
        description = "Get the current time.";
        strict = true;
        functionJson = IFunction.BuildFunctionJson(this);
    }

    public string Call(in string args)
    {
        var argsJson = JsonSerializer.Deserialize<JsonElement>(args);
        return $"The current time! {argsJson}";
    }
}