using System.Net;
using IdGen;

namespace NM.Studio.Domain.Utilities;

public static class CommonHelper
{
    private static readonly IdGenerator _idGenerator;

    static CommonHelper()
    {
        var epoch = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var structure = new IdStructure(45, 2, 16);
        var options = new IdGeneratorOptions(structure, new DefaultTimeSource(epoch));
        _idGenerator = new IdGenerator(1, options);
    }

    public static string GenerateId()
    {
        var id = _idGenerator.CreateId();
        return $"{id}";
    }

    public static string NormalizeIP(string ipAddress)
    {
        if (ipAddress.Contains(",")) ipAddress = ipAddress.Split(',')[0].Trim();

        if (IPAddress.TryParse(ipAddress, out var ip))
        {
            if (ip.IsIPv4MappedToIPv6) return ip.MapToIPv4().ToString();

            // Chuyển loopback IPv6 (::1) về loopback IPv4 (127.0.0.1)
            if (IPAddress.IPv6Loopback.Equals(ip)) return IPAddress.Loopback.ToString(); // Trả về 127.0.0.1
        }

        return ipAddress;
    }
}