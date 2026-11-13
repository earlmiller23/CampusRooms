using CampusRooms.Data;
using CampusRooms.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CampusRooms.Controllers
{
    public class RoomsController : Controller
    {
        private readonly AppDbContext _db;

        public RoomsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("rooms")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Rooms";
            List<Room> rooms = _db.Rooms
                .Include(room => room.Building)
                .OrderBy(room => room.Code)
                .ToList();
            ViewBag.Count = rooms.Count;
            return View(rooms);
        }

        [HttpGet("rooms/{id:int}")]
        public IActionResult Details(int id)
        {
            Room? room = _db.Rooms
                .Include(r => r.Building)
                .FirstOrDefault(r => r.Id == id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["Title"] = room.Code;
            return View(room);
        }

        private void LoadBuildings()
        {
            List<Building> buildings = _db.Buildings
                .OrderBy(each_building => each_building.Name)
                .ToList();
            ViewBag.Buildings = new SelectList(buildings, "Id", "Name");
        }

        [HttpGet("rooms/seats/{min:int}")]
        public IActionResult Seats(int min)
        {
            List<Room> rooms = _db.Rooms
                .FromSql($"SELECT * FROM Rooms WHERE Seats >= {min}")
                .Include(room => room.Building)
                .OrderByDescending(room => room.Seats)
                .ToList();

            ViewData["Title"] = $"Rooms with at least {min} seats";
            ViewBag.Count = rooms.Count;
            return View("Index", rooms);
        }

        [HttpGet("rooms/add")]
        public IActionResult Add()
        {
            ViewData["Title"] = "Add a room";
            LoadBuildings();
            return View(new Room());
        }

        [HttpPost("rooms/add")]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Room new_room)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Add a room";
                LoadBuildings();
                return View(new_room);
            }

            _db.Rooms.Add(new_room);
            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("Code", "Another room already has that code");
                ViewData["Title"] = "Add a room";
                LoadBuildings();
                return View(new_room);
            }

            TempData["Message"] = $"{new_room.Code} {new_room.Name} was added";
            return RedirectToAction("Index");
        }

        [HttpGet("rooms/edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            Room? room = _db.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Edit a room";
            LoadBuildings();
            return View(room);
        }

        [HttpPost("rooms/edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Room changed_room)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Edit a room";
                LoadBuildings();
                return View(changed_room);
            }

            Room? room = _db.Rooms.Find(id);
            if (room == null)
            {
                TempData["Message"] = "That room was deleted before you saved";
                return RedirectToAction("Index");
            }
            room.Code = changed_room.Code;
            room.Name = changed_room.Name;
            room.Seats = changed_room.Seats;
            room.HasProjector = changed_room.HasProjector;
            room.BuildingId = changed_room.BuildingId;

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("Code", "Another room already has that code");
                ViewData["Title"] = "Edit a room";
                LoadBuildings();
                return View(changed_room);
            }

            TempData["Message"] = $"Saved {room.Code}";
            return RedirectToAction("Index");
        }

        [HttpGet("rooms/delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            Room? room = _db.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Delete a room";
            return View(room);
        }

        [HttpPost("rooms/delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Room? room = _db.Rooms.Find(id);
            if (room == null)
            {
                TempData["Message"] = "That room was already deleted";
                return RedirectToAction("Index");
            }
            _db.Rooms.Remove(room);
            _db.SaveChanges();
            TempData["Message"] = $"{room.Code} {room.Name} was deleted";
            return RedirectToAction("Index");
        }
    }
}
