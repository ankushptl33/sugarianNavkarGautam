using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls; 
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

public partial class Sugar_Transaction_Async : System.Web.UI.Page
{
    protected  void Page_Load(object sender, EventArgs e)
    {
        //await LoadDataAsync();
    }


    protected async void btnasync_Click(object sender, EventArgs e)
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                var payload = new
                {
                    ClientCode = "NAVKAR007",
                    MessageId = "BHQMJU20231012077881",
                    TransactionDate = "2023-10-12"
                };

                // Serialize the payload to JSON.
                var jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Make an asynchronous POST request to the API.
                HttpResponseMessage response = await client.PostAsync("http://localhost:8087/api/values", content);


                if (response.IsSuccessStatusCode)
                {
                    // Handle the successful response here.
                    string contents = await response.Content.ReadAsStringAsync();
                    LblPartyname.Text = contents;
                    // Process the response content as needed.
                    // Example: Update UI controls with the API data.
                }
                else
                {
                    // Handle API error or non-successful response.
                    // Example: Display an error message to the user.
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions that may occur during the API call.
                // Example: Log the exception or display an error message.
            }

            // Update UI or process data
            //LblPartyname.Text = "Data loaded asynchronously";
        }
    }
}