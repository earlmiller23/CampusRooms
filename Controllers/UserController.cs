using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace CampusRooms.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _user_manager;
        public UsersController(UserManager<IdentityUser> user_manager)
        {
            _user_manager = user_manager;
        }

        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            List<IdentityUser> users = _user_manager.Users
                .OrderBy(user => user.Email).ToList();
            IList<IdentityUser> staff =
                await _user_manager.GetUsersInRoleAsync("Staff");
            ViewBag.StaffIds = staff.Select(user => user.Id).ToList();
            ViewData["Title"] = "Users";
            return View(users);
        }

        [HttpPost("users/make-staff/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeStaff(string id)
        {
            IdentityUser? user = await _user_manager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await _user_manager.AddToRoleAsync(user, "Staff");
            TempData["Message"] = $"{user.Email} is now Staff";
            return RedirectToAction("Index");
        }
    }
}