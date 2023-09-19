using System;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace RedditConsumer.Controllers
{
	public abstract class RedditApiController
	{
        

        protected readonly string clientId = ConfigurationManager.AppSettings.Get("clientId");
        protected readonly string clientSecret = ConfigurationManager.AppSettings.Get("clientSecret");
        protected readonly string userAgent = ConfigurationManager.AppSettings.Get("userAgent");
        protected readonly string baseAddress = ConfigurationManager.AppSettings.Get("redditBaseAddress");
        protected readonly string baseOAuthAddress = ConfigurationManager.AppSettings.Get("redditOAuthBaseAddress");

        static string token;
        static int expiresIn = 0;
        

        /// <summary>
        /// If the access token is not close to expireation, it returns the token,
        /// Otherwise it requests a new token and cache it.
        /// </summary>
        /// <returns></returns>
        protected async Task<string> getAccesstoken()
        {
            // when the token expires in 2 minutes, then we need to get another token
            if(expiresIn < 120)
            {
                await requestAccessToken();
            }

            return token;
        }

        /// <summary>
        /// Requests a new access token
        /// </summary>
        /// <returns></returns>
        private async Task requestAccessToken()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Add("User-Agent", userAgent);

                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
            });

                var authValue = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"))
                );
                client.DefaultRequestHeaders.Authorization = authValue;

                var response = await client.PostAsync("/api/v1/access_token", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response JSON to get the access token
                    var responseData = JObject.Parse(responseContent);
                    token = responseData["access_token"].ToString();
                    expiresIn = (int) responseData["expires_in"];
                }
                else
                {
                    Console.WriteLine("Error getting access token:");
                    Console.WriteLine(responseContent);
                }
            }
        }


    }
}

