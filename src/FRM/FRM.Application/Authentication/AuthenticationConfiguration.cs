
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Forum.Application.Authentication;

public class AuthenticationConfiguration
{
    public required string Base64Key { get; set; } = "PdiAw9FvfRGHpWT3iaNjE4GTWY+z4+UDlzLUSh3Yceg=";
    
    public byte[] Key => Convert.FromBase64String(Base64Key);
}