using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PizzaStore.DataTransferObjects
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
    }
}
