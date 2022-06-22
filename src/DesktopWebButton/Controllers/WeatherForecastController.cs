using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DesktopWebButton.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = "a"//Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}

public abstract class ButtonDataBase
{
    public abstract string Name { get; }

    public abstract void Action();

}

public class NormalButtonData : ButtonDataBase
{
    public override string Name { get; } = "Normal";
    public override void Action()
    {
        Console.WriteLine(Name);
        Process.Start("C:/Program Files (x86)/Dell/Dell Display Manager/ddm.exe", arguments:"/SetPxPMode Off");
    }
}


public class WorkButtonData : ButtonDataBase
{
    public override string Name { get; } = "Work";
    public override void Action()
    {
        Console.WriteLine(Name);
        Process.Start("C:/Program Files (x86)/Dell/Dell Display Manager/ddm.exe", arguments:"/SetPxPMode PBP");
    }
}

[ApiController]
[Route("api/[controller]")]
public class ButtonController : ControllerBase
{
    private readonly ILogger<ButtonController> _logger;
    private readonly Dictionary<string, ButtonDataBase> _buttonDic = new Dictionary<string, ButtonDataBase>();

    public ButtonController(ILogger<ButtonController> logger)
    {
        _logger = logger;
        
        var normal = new NormalButtonData();
        _buttonDic[normal.Name.ToLowerInvariant()] = normal;

        var work = new WorkButtonData();
        _buttonDic[work.Name.ToLowerInvariant()] = work;

    }
    
    [HttpGet]
    public IEnumerable<ButtonDataBase> Get()
    {
        return _buttonDic.Values;
    }
    
    [HttpPost]
    public void Action(string name)
    {
        if (!_buttonDic.TryGetValue(name.ToLowerInvariant(), out var button)) return;
        
        button.Action();
    }
}
