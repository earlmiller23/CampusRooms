using System.ComponentModel.DataAnnotations;

namespace CampusRooms.Models
{
    public class RoomInput
    {
        [Required(ErrorMessage = "Give the room a code")]
        [StringLength(10, ErrorMessage = "Keep the code to 10 characters")]
        public string Code { get; set; } = "";

        [Required(ErrorMessage = "Give the room a name")]
        [StringLength(50, ErrorMessage = "Keep the name to 50 characters")]
        public string Name { get; set; } = "";

        [Range(1, 500, ErrorMessage = "Seats must be between 1 and 500")]
        public int Seats { get; set; }

        public bool HasProjector { get; set; }

        [Required(ErrorMessage = "Pick a building")]
        public int? BuildingId { get; set; }
    }
}