using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FoodReservation.User
{
    public partial class PaymentModel : System.Web.UI.Page
    {
        private const string ConsumerKey = "GswwLfq13u0yTxbeWtl8hFl9CPWJ6leSnCazjMQ5CbfroHH8";
        private const string ConsumerSecret = "mxSdtSKGnXC7G31nbfZ8FCuWg58dqOu6of2xWjZcG4HIATifT1sfy2L14UJrGUYG";
        private const string BusinessShortCode = "174379";
        private const string Passkey = "bfb279f9aa9bdbcf158e97dd71a467cd2e0c893059b10f78e6b72ada1ed2c919";
        private const string CallbackUrl = "https://mydomain.com/path";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["grandTotalPrice"] != null)
                {
                    decimal total = Convert.ToDecimal(Session["grandTotalPrice"]);
                    txtAmount.Text = Math.Round(total, 0).ToString();  //Removes the decimal places
                    txtAmount.ReadOnly = true;
                }
            }
        }

        protected void BtnPay_Click(object sender, EventArgs e)
        {
            string phone = txtPhone.Text.Trim();
            string amount = txtAmount.Text.Trim();

            if (phone.StartsWith("0"))
                phone = "254" + phone.Substring(1);

            var client = new HttpClient();

            var byteArray = Encoding.ASCII.GetBytes($"{ConsumerKey}:{ConsumerSecret}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var tokenResponse = client.GetAsync("https://sandbox.safaricom.co.ke/oauth/v1/generate?grant_type=client_credentials").GetAwaiter().GetResult();
            if (!tokenResponse.IsSuccessStatusCode)
            {
                var errorDetails = tokenResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                lblMessage.Text = $"Failed to get access token. Status: {tokenResponse.StatusCode}<br/>Details: {errorDetails}";
                return;
            }

            var tokenJson = tokenResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var tokenDoc = JsonDocument.Parse(tokenJson);
            var accessToken = tokenDoc.RootElement.GetProperty("access_token").GetString();

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string password = Convert.ToBase64String(Encoding.UTF8.GetBytes(BusinessShortCode + Passkey + timestamp));

            var paymentRequest = new
            {
                BusinessShortCode,
                Password = password,
                Timestamp = timestamp,
                TransactionType = "CustomerPayBillOnline",
                Amount = amount,
                PartyA = phone,
                PartyB = BusinessShortCode,
                PhoneNumber = phone,
                CallBackURL = CallbackUrl,
                AccountReference = "Robbs Foodies",
                TransactionDesc = "Payment of bill"
            };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = new StringContent(JsonSerializer.Serialize(paymentRequest), Encoding.UTF8, "application/json");

            var response = client.PostAsync("https://sandbox.safaricom.co.ke/mpesa/stkpush/v1/processrequest", content).GetAwaiter().GetResult();
            var respContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var jsonResponse = JsonDocument.Parse(respContent);

            if (jsonResponse.RootElement.TryGetProperty("ResponseCode", out var responseCode) && responseCode.GetString() == "0")
            {
                lblMessage.Text = "WAIT FOR STK POP UP";

                // Redirect to a confirmation or success page after successful payment request
                Response.Redirect("Payment.aspx");  // Change this to your success page URL
            }
            else if (jsonResponse.RootElement.TryGetProperty("errorMessage", out var errorMsg))
            {
                lblMessage.Text = "Transaction Failed: " + errorMsg.GetString();
            }
            else
            {
                lblMessage.Text = "Transaction Failed.";
            }
        }
    }
}
