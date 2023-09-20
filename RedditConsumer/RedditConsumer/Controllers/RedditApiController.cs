using System;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace RedditConsumer.Controllers
{
	public abstract class RedditApiController
	{
        

        protected static readonly string clientId = ConfigurationManager.AppSettings.Get("clientId");
        protected static readonly string clientSecret = ConfigurationManager.AppSettings.Get("clientSecret");
        protected static readonly string userAgent = ConfigurationManager.AppSettings.Get("userAgent");
        protected static readonly string baseAddress = ConfigurationManager.AppSettings.Get("redditBaseAddress");
        protected static readonly string baseOAuthAddress = ConfigurationManager.AppSettings.Get("redditOAuthBaseAddress");

        /// <summary>
        /// Keeps the remaining number of ratelimit capacity based on X-RateLimit-Remaining http header
        /// </summary>
        int remainingRateLimit = int.MaxValue;

        /// <summary>
        /// Points to the end of current RateLimit period based on X-RateLimit-Reset http header
        /// </summary>
        DateTime endOfCurrentRateLimit = DateTime.Now;

        static string token;
        static int expiresIn = 0;


        /// <summary>
        /// Updates Ratelimit info based on headers returned by Reddit API
        /// </summary>
        /// <param name="headers"></param>
        protected void UpdateRateLimit(HttpResponseHeaders headers)
        {
            try
            {
                remainingRateLimit = (int)double.Parse(headers.GetValues("X-Ratelimit-Remaining").First());
                int remainingSecondsToEnd = int.Parse(headers.GetValues("X-Ratelimit-Reset").First());
                endOfCurrentRateLimit = DateTime.Now.AddSeconds(remainingSecondsToEnd);
            }
            catch(Exception e)
            {
                endOfCurrentRateLimit = DateTime.Now;
                remainingRateLimit = int.MaxValue;
                Console.WriteLine($"Unable to update RateLimit info: {e.Message}");
            }
        }

        /// <summary>
        /// Waits until the RateLimit period is reset when the API reached the limit
        /// </summary>
        protected void WaitOnRateLimit()
        {
            if(remainingRateLimit == 0)
            {
                int waitSeconds = (int) (DateTime.Now - endOfCurrentRateLimit).TotalSeconds;
                Console.WriteLine($"Waiting for {waitSeconds} due to RateLimit");
                Task.Delay(TimeSpan.FromSeconds(waitSeconds));
            } 
        }
        

        /// <summary>
        /// If the access token is not close to expireation, it returns the token,
        /// Otherwise it requests a new token and cache it.
        /// </summary>
        /// <returns></returns>
        protected async Task<string> GetAccesstoken()
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

