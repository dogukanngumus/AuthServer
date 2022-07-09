namespace AuthServer.Core.Dtos.Authentication;

public class ClientLoginDto
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}