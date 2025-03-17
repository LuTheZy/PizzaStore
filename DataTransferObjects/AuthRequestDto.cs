using System.Text.Json.Serialization;

namespace PizzaStore.DataTransferObjects
{
    public class AuthRequestDto
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }
}
