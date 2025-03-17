using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PizzaStore.DataTransferObjects
{
    public class RegisterUserDto
    {
        [Required]
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }
}
