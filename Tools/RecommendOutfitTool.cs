using System.Text.Json;

namespace AIExtension.Tools;

class RecommendOutfit : IFunction
{
    public Dictionary<string, Parameter> parameters { get; set; }
    public string description { get; }
    public bool? strict { get; }
    public string functionJson { get; }

    private Parameter weather = new Parameter(typeof(string))
        .SetDescription("Weather condition, e.g.: sunny, rainy, cloudy, etc.")
        .SetIsRequired(true);

    private Parameter temperature = new Parameter(typeof(int))
        .SetDescription("Temperature value")
        .SetIsRequired(true);

    private Parameter occasion = new Parameter(typeof(string))
        .SetDescription("Occasion, e.g.: business, casual, sport, formal, etc.")
        .SetIsRequired(true)
        .SetEnum(new List<object> { "business", "casual", "sport", "formal" });

    private Parameter userPreferences = new Parameter(typeof(List<string>))
        .SetDescription("User preferences, e.g.: color preference, style preference, etc.")
        .SetIsRequired(false);

    public RecommendOutfit()
    {
        parameters = new Dictionary<string, Parameter>
        {
            { nameof(weather), weather },
            { nameof(temperature), temperature },
            { nameof(occasion), occasion },
            { nameof(userPreferences), userPreferences }
        };

        description = "Recommend outfit based on weather, occasion and user preferences";
        strict = true;
        functionJson = IFunction.BuildFunctionJson(this);
    }

    public string Call(in string args)
    {
        var argsJson = JsonSerializer.Deserialize<JsonElement>(args);
        var p_weather = argsJson.GetProperty(nameof(weather)).GetString();
        var p_temperature = argsJson.GetProperty(nameof(temperature)).GetInt32();
        var p_occasion = argsJson.GetProperty(nameof(occasion)).GetString();
        var p_userPreferences = argsJson.GetProperty(nameof(userPreferences)).EnumerateArray().Select(x => x.GetString()).ToArray();
        return $@"The recommended outfit for {p_weather} weather,
                {p_temperature} temperature, 
                {p_occasion} occasion,
                and user preferences [{string.Join(", ", p_userPreferences)}] is a {p_occasion} outfit.";
    }
}