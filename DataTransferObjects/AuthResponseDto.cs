namespace PizzaStore.DataTransferObjects
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;

        public AuthResponseDto(string token)
        {
            Token = token;
        }
    }
}
