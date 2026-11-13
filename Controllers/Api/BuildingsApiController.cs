using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusRooms.Data;
using CampusRooms.Models;

namespace CampusRooms.Controllers.Api
{
    [ApiController]
    [Route("api/categories")]
    public class BuildingsApiController : ControllerBase
    {
        private readonly AppDbContext _db;

        public BuildingsApiController(AppDbContext db)
        {
            _db = db;
        }

        [AllowAnonymous]
        [HttpGet("{id:int}/dishes")]
        public IActionResult GetRooms(int id)
        {
            Building? building = _db.Buildings
                .Include(each_building => each_building.Rooms)
                .FirstOrDefault(each_building => each_building.Id == id);
            if (building == null)
            {
                return NotFound();
            }

            List<RoomDto> result = new List<RoomDto>();
            foreach (Room room in building.Rooms.OrderBy(room => room.Name))
            {
                RoomDto dto = new RoomDto();
                dto.Id = room.Id;
                dto.Name = room.Name;
                dto.Seats = room.Seats;
                dto.HasProjector = room.HasProjector;
                dto.BuildingId = room.BuildingId;
                dto.BuildingName = building.Name;
                result.Add(dto);
            }
            return Ok(result);
        }
    }
}