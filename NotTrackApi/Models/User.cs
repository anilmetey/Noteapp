using System.ComponentModel.DataAnnotations;

namespace NotTrackApi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

       
        [Required]
        public string Role { get; set; } = "User"; // Varsayılan User
        public string Name { get; set; }
    }
}
