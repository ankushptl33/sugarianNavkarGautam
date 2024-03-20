using System;
using System.Net;
using System.Threading.Tasks;
using System.IO;

/// <summary>
/// Summary description for IPAPI
/// </summary>
public static class IPAPI
{
    public static string API(string apiUrl)
	{ 
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.Method = "GET";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string jsonResponse = reader.ReadToEnd();
                            return jsonResponse;
                        }
                    }
                }
                else
                {
                    // Handle the case when the API returns an error
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions if any
            return null;
        }
    }
}