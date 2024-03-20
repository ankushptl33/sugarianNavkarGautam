using System.Collections.Generic;
using System.Web;


/// <summary>
/// Summary description for IpAddressHelper
/// </summary>
public class IpAddressHelper
{

    public static List<string> AllowedIpAddress = new List<string>
    {
       "::1",
       "127.0.0.1",
       "192.168.1.1"
    };
    public static string GetClientIpAddress(HttpRequest request)
    {
        // Check the X-Forwarded-For HTTP header
        string forwardedFor = request.Headers["X-Forwarded-For"];
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            string[] addresses = forwardedFor.Split(',');
            if (addresses.Length != 0)
            {
                // Return the first IP address in the list, which should be the client's original IP
                return addresses[0];
            }
        }

        // If there is no X-Forwarded-For header, or it's not valid, fall back to the remote address
        return request.UserHostAddress;
    }
}