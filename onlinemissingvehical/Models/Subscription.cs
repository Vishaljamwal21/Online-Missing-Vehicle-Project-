using System.ComponentModel.DataAnnotations;

namespace onlinemissingvehical.Models
{
    public class Subscription
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Range(200, 200, ErrorMessage = "Subscription price must be 200")]
        public int SubscriptionPrice { get; set; } = 200; // Default value set to 200
    }
}
