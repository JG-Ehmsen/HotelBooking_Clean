using System.Collections.Generic;
using HotelBooking.Core;
using Microsoft.AspNetCore.Mvc;


namespace HotelBooking.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class RoomsController : Controller
    {
        private readonly IRepository<Room> repository;

        public RoomsController(IRepository<Room> repos)
        {
            repository = repos;
        }

        // GET: api/rooms
        [HttpGet]
        public IEnumerable<Room> Get()
        {
            return repository.GetAll();
        }

        // GET api/rooms/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST api/rooms
        [HttpPost]
        public IActionResult Post([FromBody]Room room)
        {
            if (room == null)
            {
                return BadRequest();
            }

            var created = repository.Add(room);

            if (created != null)
            {
                return Ok(created);
            }
            else
            {
                return Conflict("The customer could not be created.");
            }

        }

        // PUT api/rooms/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Room room)
        {
            if (room == null || room.Id != id)
            {
                return BadRequest();
            }

            var origRoom = repository.Get(id);

            if (origRoom == null)
            {
                return NotFound();
            }

            var editedRoom = repository.Edit(room);
            return Ok(editedRoom);
        }

        // DELETE api/rooms/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (repository.Get(id) == null)
            {
                return NotFound();
            }

            var removedRoom = repository.Remove(id);
            return Ok(removedRoom);
        }

    }
}
