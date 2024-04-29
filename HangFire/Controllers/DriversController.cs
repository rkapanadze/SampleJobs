using Hangfire;
using HangFire.Models;
using HangFire.Services;
using Microsoft.AspNetCore.Mvc;

namespace HangFire.Controllers;

[ApiController]
[Route("[controller]")]
public class DriversController : ControllerBase
{
    private static List<Driver> drivers = new List<Driver>();

    private readonly ILogger<DriversController> _logger;

    public DriversController(
        ILogger<DriversController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetDrivers()
    {
        var items = drivers.Where(x => x.Status == 1).ToList();
        return Ok(items);
    }

    [HttpPost]
    public IActionResult CreateDriver(Driver data)
    {
        if (ModelState.IsValid)
        {
            drivers.Add(data);

            // Fire and Forget Job
            var jobId = BackgroundJob.Enqueue<IServiceManagement>(x => x.SendEmail());
            Console.WriteLine($"Job id: {jobId}");

            return CreatedAtAction("GetDriver", new { data.Id }, data);
        }

        return new JsonResult("Something went wrong") { StatusCode = 500 };
    }

    [HttpGet("{id}")]
    public IActionResult GetDriver(Guid id)
    {
        var item = drivers.FirstOrDefault(x => x.Id == id);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateDriver(Guid id, Driver item)
    {
        if (id != item.Id)
            return BadRequest();

        var existItem = drivers.FirstOrDefault(x => x.Id == id);

        if (existItem == null)
            return NotFound();

        existItem.Name = item.Name;
        existItem.DriverNumber = item.DriverNumber;

        var jobId = BackgroundJob.Schedule<IServiceManagement>(x => x.UpdateDatabase(), TimeSpan.FromSeconds(20));
        Console.WriteLine($"Job id: {jobId}");

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteDriver(Guid id)
    {
        var existItem = drivers.FirstOrDefault(x => x.Id == id);

        if (existItem == null)
            return NotFound();

        existItem.Status = 0;

        RecurringJob.AddOrUpdate<IServiceManagement>(x => x.SyncRecords(), Cron.Hourly);

        return Ok(existItem);
    }
}