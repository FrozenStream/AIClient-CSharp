using System.Text.Json;

namespace AIExtension.Tools;

class getCurrentWeatherTool : IFunction
{
    public Dictionary<string, Parameter> parameters { get; set; }
    public string description { get; }
    public bool? strict { get; }
    public string functionJson { get; }
    private Parameter location = new Parameter(typeof(string))
        .SetDescription("Location")
        .SetIsRequired(true)
        .SetEnum(new List<object> { "New York", "London", "Tokyo" });

    public getCurrentWeatherTool()
    {
        parameters = new Dictionary<string, Parameter>
        {
            { nameof(location), location }
        };
        description = "Get the current weather in a given location.";
        strict = true;
        functionJson = IFunction.BuildFunctionJson(this);
    }

    public string Call(string args)
    {
        var argsJson = JsonSerializer.Deserialize<JsonElement>(args);
        var p_location = argsJson.GetProperty(nameof(location)).GetString();
        return $"The weather in {p_location} is sunny.";
    }
}