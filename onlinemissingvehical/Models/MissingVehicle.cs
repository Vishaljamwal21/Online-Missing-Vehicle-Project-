using System.ComponentModel.DataAnnotations;

namespace onlinemissingvehical.Models
{
    public class MissingVehicle
    {
        public int Id { get; set; }

        [Required]
        public string VehicleType { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        [Display(Name = "License Plate Number")]
        public string LicensePlateNumber { get; set; }

        [Required]
        [Display(Name = "Last Known Location")]
        public string LastKnownLocation { get; set; }

        public string? ImageUrl { get; set; }

        public string? Comment { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
