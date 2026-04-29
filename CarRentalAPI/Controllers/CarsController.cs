using CarRentalAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private static readonly List<Car> _cars = new();
    private static readonly object _lock = new();

    // GET api/cars
    [HttpGet]
    public ActionResult<IEnumerable<Car>> GetAll()
    {
        lock (_lock)
        {
            return Ok(_cars.ToList());
        }
    }

    // GET api/cars/{id}
    [HttpGet("{id}")]
    public ActionResult<Car> GetById(string id)
    {
        lock (_lock)
        {
            var car = _cars.FirstOrDefault(c => c.Id == id);
            if (car is null)
                return NotFound(new { message = $"Car with id '{id}' was not found." });

            return Ok(car);
        }
    }

    // POST api/cars
    [HttpPost]
    public ActionResult<Car> Create([FromBody] Car car)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        car.Id = Guid.NewGuid().ToString();

        lock (_lock)
        {
            _cars.Add(car);
        }

        return CreatedAtAction(nameof(GetById), new { id = car.Id }, car);
    }

    // PUT api/cars/{id}
    [HttpPut("{id}")]
    public ActionResult<Car> Update(string id, [FromBody] Car updated)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        lock (_lock)
        {
            var existing = _cars.FirstOrDefault(c => c.Id == id);
            if (existing is null)
                return NotFound(new { message = $"Car with id '{id}' was not found." });

            existing.Model = updated.Model;
            existing.Year = updated.Year;

            return Ok(existing);
        }
    }

    // DELETE api/cars/{id}
    [HttpDelete("{id}")]
    public ActionResult Delete(string id)
    {
        lock (_lock)
        {
            var car = _cars.FirstOrDefault(c => c.Id == id);
            if (car is null)
                return NotFound(new { message = $"Car with id '{id}' was not found." });

            _cars.Remove(car);
        }

        return NoContent();
    }
}
