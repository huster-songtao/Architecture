using System.Security.Claims;

namespace Architecture.Security;

public interface IJwtService
{
    Dictionary<string, object> Decode(string token);

    string Encode(IList<Claim> claims);
}
