using System.Text.Encodings.Web;
using System.Text.Json;
using static System.Console;
string jsonString = string.Empty;
TemperatureInfo? forecast = null;
var options = new JsonOptionsBuilder().Build();
try
{
    using FileStream fs = File.OpenRead("temperatura.json");
    options = CreateOptions(true); 
    // set to true to idented the json
    forecast = await JsonSerializer.DeserializeAsync<TemperatureInfo>(fs, options);
}
catch (Exception e)
{
    WriteLine($"The file could not be read: {e.Message[..20]}");
    // above file is example os string.substring method but new in C# 8.0
    // now called it range operator syntax
    // old use e.Message.Substring(0, 20) 
    WriteLine($"instead we use a default value.");
    jsonString = getTemperatureJson();
    options = CreateOptions(false);
    forecast = JsonSerializer.Deserialize<TemperatureInfo>(jsonString, options);
}

// Write JSON from a Method
WriteLine($"JSON with last opction used:");
//options is the last options used
string jsonOutput = GetJsonOutput(forecast, options);
WriteLine(jsonOutput);

//get a typed value from JsonNode
var temperature = forecast?.Temperature;
WriteLine($"Temperature:{temperature}");

//get JSON temperatures ranges Option 1
var temperatureRanges = forecast?.TemperatureRanges;
if (temperatureRanges != null)
{
    var temperatureRangesJson = JsonSerializer.Serialize(temperatureRanges, options);
    WriteLine($"Temperature Ranges Option 1, using diccionary: {temperatureRangesJson}");
}

if (temperatureRanges != null && temperatureRanges.TryGetValue("Cold", out var coldRange))
{
    WriteLine($"Cold High: {coldRange.High}");
}
WriteLine($"Temperature ranges using temperaturaRanges dictionary");
if (temperatureRanges != null)
{
    foreach (var grades in temperatureRanges)
    {
        WriteLine($"{grades.Key} High: {grades.Value.High}");
        WriteLine($"{grades.Key} Low: {grades.Value.Low}");
    }
}

//get temperatures ranges directly from forecast
WriteLine($"Temperatura ranges directly from forecast");
if (forecast?.TemperatureRanges != null)
{
    foreach (var grades in forecast.TemperatureRanges)
    {
        WriteLine($"{grades.Key} High: {grades.Value.High}");
        WriteLine($"{grades.Key} Low: {grades.Value.Low}");
    }
}

//get JSON temperatures ranges Option 2
//Calling DictionaryExtensions
WriteLine("Temperature Ranges Option 2, using DictionaryExtensions:" + 
$"{forecast!.TemperatureRanges!.ToJsonString(options)}");

//get JSON temperatures with new options
options = CreateOptions(true, JsonNamingPolicy.SnakeCaseLower, 
                        builder => builder.SetEncoder(JavaScriptEncoder.Default));

WriteLine($"JSON from a method with new options: \n{GetJsonOutput(forecast, options)}");

//get JSON temperatures with new options upper case
options = CreateOptions(true, new UpperCaseNamingPolicy(), 
                        builder => builder.SetEncoder(JavaScriptEncoder.Default));

WriteLine($"JSON from a method with new options upper case: \n{GetJsonOutput(forecast, options)}");

//Return JSON from a method outputting it at a higher level
static string GetJsonOutput(TemperatureInfo? forecast, JsonSerializerOptions options)
{
    return JsonSerializer.Serialize(forecast, options);
}

//function getTemperatureJson return json content
static string getTemperatureJson()
{
    var temperatureInfo = new TemperatureInfo
    {
        Date = new DateTime(2024, 4, 5),
        Temperature = 25,
        Summary = "Hot",
        DatesAvailable = new List<DateTime> { new DateTime(2024, 5, 1), new DateTime(2024, 5, 2) },
        TemperatureRanges = new Dictionary<string, TemperatureRange>
        {
            ["Cold"] = new TemperatureRange { High = 23, Low = -9 },
            ["Hot"] = new TemperatureRange { High = 51, Low = 32 }
        }
    };

    var opciones = new JsonOptionsBuilder().Build();
    opciones = CreateOptions(true);
    return JsonSerializer.Serialize(temperatureInfo, opciones);
}

static JsonSerializerOptions CreateOptions(bool? writeIndented = null, 
                   JsonNamingPolicy? namingPolicy = null, 
                   Action<JsonOptionsBuilder>? configure = null)
{
    var builder = new JsonOptionsBuilder();

    if (writeIndented.HasValue)
    {
        builder.SetWriteIndented(writeIndented.Value);
    }

    if (namingPolicy != null)
    {
        builder.SetPropertyNamingPolicy(namingPolicy);
    }

    configure?.Invoke(builder);

    return builder.Build();
}

public record TemperatureInfo
{
    public DateTime Date { get; set; }
    public int Temperature { get; set; }
    public string? Summary { get; set; }
    public List<DateTime>? DatesAvailable { get; set; }
    public Dictionary<string, TemperatureRange>? TemperatureRanges { get; set; }
}

public record TemperatureRange
{
    public int High { get; set; }
    public int Low { get; set; }
}

public class JsonOptionsBuilder
{
    private JsonSerializerOptions options;

    public JsonOptionsBuilder()
    {
        options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }

    public JsonOptionsBuilder SetOptions(JsonSerializerOptions value)
    {
        options = value;
        return this;
    }

    public JsonOptionsBuilder SetWriteIndented(bool? value = null)
    {
        options.WriteIndented = value ?? true;
        return this;
    }

    public JsonOptionsBuilder SetPropertyNamingPolicy(JsonNamingPolicy? policy = null)
    {
        options.PropertyNamingPolicy = policy ?? JsonNamingPolicy.CamelCase;
        return this;
    }

    public JsonOptionsBuilder SetEncoder(JavaScriptEncoder encoder)
    {
        options.Encoder = encoder;
        return this;
    }

    // Add more methods to set other options...

    public JsonSerializerOptions Build()
    {
        return options;
    }
    
}

public class UpperCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name.ToUpper();
    }
}

public static class DictionaryExtensions
{
    public static string ToJsonString<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, 
    JsonSerializerOptions options)
        where TKey : notnull
    {
        return JsonSerializer.Serialize(dictionary!, options);
    }
}

