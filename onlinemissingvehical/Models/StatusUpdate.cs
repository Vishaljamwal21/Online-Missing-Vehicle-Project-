using System.ComponentModel.DataAnnotations;

namespace onlinemissingvehical.Models
{
    public class StatusUpdate
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Status { get; set; }  
        public int MissingVehicleId { get; set; } 
        public MissingVehicle MissingVehicle { get; set; }
    }
}
