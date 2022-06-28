using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace DesktopWebButton.Controllers;

public class ButtonData
{
    public string Kind { get; set; } = "";
    public string Name { get; set; } = "";
    public string? Display { get; set; }
    public string? Description { get; set; }
    public string? Program { get; set; }
    public string? Args { get; set; }

    public void Action(ILogger? logger = null)
    {
        logger?.LogInformation("Button: {Name}", Display);

        switch (Kind.ToLowerInvariant())
        {
            case "program":
                {
                    if (Program is null)
                    {
                        throw new InvalidOperationException("'Program' is null");
                    }
                    Process.Start(Program, arguments: Args ?? "");
                }
                break;
            default:
                logger?.LogInformation("'{Kind}' is not supported", Kind);
                break;
        }

        
    }
}

public class ButtonDataSetClient
{
    public string DataSetFile { get; }

    public ButtonDataSetClient(string dataSetFile)
    {
        DataSetFile = dataSetFile;
    }
    
    public ButtonDataList Load()
    {
        var file = DataSetFile;
        if (!File.Exists(file)) return new ButtonDataList();
        
        using var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None);
        var option = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReadCommentHandling = JsonCommentHandling.Skip
        };
        var dataSet = JsonSerializer.Deserialize<ButtonDataList>(fs, option);

        return dataSet ?? new ButtonDataList();
    }

    public void Save()
    {
        var file = DataSetFile;
        
        using var fs = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.None);
        var option = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        JsonSerializer.Serialize(fs, this, option);
    }
}

public class ButtonDataList: List<ButtonData>
{
}


[ApiController]
[Route("api/[controller]")]
public class ButtonController : ControllerBase
{
    private readonly ILogger<ButtonController> _logger;
    private readonly ButtonDataList _buttonDataList;

    public ButtonController(ILogger<ButtonController> logger, ButtonDataList buttonDataList)
    {
        _logger = logger;
        _buttonDataList = buttonDataList;
    }
    
    [HttpGet]
    public IEnumerable<ButtonData> Get()
    {
        return _buttonDataList;
    }
    
    [HttpPost]
    public void Action(string name)
    {
        var button = _buttonDataList.FirstOrDefault(x => string.Equals(name, x.Name, StringComparison.OrdinalIgnoreCase));

        if (button is null)
        {
            _logger.LogInformation("'{Name}' is not found",name);
            return;
        }

        try
        {
            button.Action();
            _logger.LogInformation("Success: {Name}", name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed: {Name} : {Message}",name, ex.Message);
        }
        
    }
}