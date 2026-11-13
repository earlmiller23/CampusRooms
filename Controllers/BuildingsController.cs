using CampusRooms.Data;
using CampusRooms.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusRooms.Controllers
{
    public class BuildingController : Controller
    {
        private readonly AppDbContext _db;
        public BuildingController(AppDbContext db) { _db = db; }

        [HttpGet("buildings/{id:int}")]
        public IActionResult Details(int id)
        {
            Building? building = _db.Buildings
                .Include(each_building => each_building.Rooms)
                .FirstOrDefault(each_building => each_building.Id == id);
            if (building == null)
            {
                return NotFound();
            }
            ViewData["Title"] = building.Name;
            return View(building);
        }
    }
}