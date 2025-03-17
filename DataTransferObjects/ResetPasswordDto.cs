using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PizzaStore.DataTransferObjects
{
    public class ResetPasswordDto
    {
        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
        
        [Required]
        [MinLength(8)]
        [JsonPropertyName("newPassword")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
