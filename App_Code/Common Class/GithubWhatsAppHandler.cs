using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;

public class GithubWhatsAppHandler
{
    public async Task<string> SendPdfToWhatsAppAsync(string WhatsappUrl,string authKey, string repo, string pdfFileNameWithExtension, string authToken, string instanceid, string accesstoken, string pdfmessage, string mobileNumber, string base64String, string bill_no, string txtmessage)
    {
        try
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            DateTime currentDateAndTime = DateTime.Now;
            var requestData = new
            {
                message = pdfmessage + "-" + bill_no + "-" + currentDateAndTime,
                content = base64String,
                branch = "main"
            };
            pdfFileNameWithExtension = "pdfFile" + mobileNumber + "-" + currentDateAndTime.ToString("yyyyMMddHHmmss") + ".pdf";
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);

            string apiUrl = "https://api.github.com/repos/" + authKey + "/" + repo + "/contents/" + pdfFileNameWithExtension;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "YourAppName");

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(apiUrl),
                    Headers =
                    {
                        { "Authorization", "Bearer " + authToken },
                        { "Accept", "application/vnd.github.VERSION.raw" }
                    },
                    Content = new StringContent(jsonData, Encoding.UTF8, "application/json")
                };

                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    // Constructing the URL for sending a WhatsApp text message
                    string whatsappTextUrl = "" + WhatsappUrl + "number=91" + mobileNumber + "&type=text&message=" + txtmessage + "&instance_id=" + instanceid + "&access_token=" + accesstoken;

                    // Sending the WhatsApp text message using a GET request
                    HttpWebRequest whatsappTextRequest = (HttpWebRequest)WebRequest.Create(whatsappTextUrl);
                    HttpWebResponse whatsappTextResponse = (HttpWebResponse)whatsappTextRequest.GetResponse();

                    // Check the WhatsApp text response for success
                    if (whatsappTextResponse.StatusCode == HttpStatusCode.OK)
                    {
                        // Constructing the URL for sending a WhatsApp media message
                        string whatsappMediaUrl = "" + WhatsappUrl + "number=91" + mobileNumber + "&type=media&message=" + pdfmessage + "&media_url=https://raw.githubusercontent.com/" + authKey + "/" + repo + "/main/" + pdfFileNameWithExtension + "&filename=" + pdfFileNameWithExtension + "&instance_id=" + instanceid + "&access_token=" + accesstoken;

                        // Sending the WhatsApp media message using a GET request
                        HttpWebRequest whatsappMediaRequest = (HttpWebRequest)WebRequest.Create(whatsappMediaUrl);
                        HttpWebResponse whatsappMediaResponse = (HttpWebResponse)whatsappMediaRequest.GetResponse();

                        // Check the WhatsApp media response for success
                        if (whatsappMediaResponse.StatusCode == HttpStatusCode.OK)
                        {
                            return "Successfully sent messages to WhatsApp!";
                        }
                        else
                        {
                            return "Error sending media message to WhatsApp. WhatsApp API Status Code: " + whatsappMediaResponse.StatusCode;
                        }
                    }
                    else
                    {
                        return "Error sending text message to WhatsApp. WhatsApp API Status Code: " + whatsappTextResponse.StatusCode;
                    }
                }
                else
                {
                    return "Error uploading file to GitHub: " + response.StatusCode;
                }
            }
        }
        catch (Exception ex)
        {
            return "Error: " + ex.Message;
        }
    }
}
