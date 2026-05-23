using Microsoft.AspNetCore.Identity;

namespace YandexGoClone.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = "";
        public string Role { get; set; } = "Client"; // Client или Courier
        public bool IsAvailable { get; set; } = true; // для курьера
    }
}