using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using ShoppingCart.Models;
using Xamarin.Forms.Internals;

namespace ShoppingCart.DataService
{
    /// <summary>
    /// Data service to load the data from database using Web API.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class UserDataService : IUserDataService
    {
        private readonly HttpClient httpClient;

        /// <summary>
        /// Creates an instance for the <see cref="UserDataService" /> class.
        /// </summary>
        public UserDataService()
        {
            httpClient = new HttpClient();
        }

        /// <summary>
        /// Login credential.
        /// </summary>
        public async Task<Status> Login(string email, string password)
        {
            var status = new Status();
            try
            {
                var uri = new UriBuilder($"{App.BaseUri}users/authentication?key=zmacs3jmjprNC9gba7xf5nZZDg87NM6dLYdaTwzT&email={email}&password={password}");
                var response = await httpClient.GetAsync(uri.ToString());
                if (response != null && response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (result != null) status = JsonConvert.DeserializeObject<Status>(result);
                }
            }
            catch (HttpRequestException ex)
            {
                Crashes.TrackError(ex);
                status.IsSuccess = false;
                status.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                status.IsSuccess = false;
                status.Message = ex.Message;
            }

            return status;
        }

        /// <summary>
        /// This method is to create a new account.
        /// </summary>
        public async Task<Status> SignUp(User user)
        {
            var status = new Status();
            try
            {
                var serializedUser = JsonConvert.SerializeObject(user);
                var httpContent = new StringContent(serializedUser, Encoding.UTF8, "application/json");
                var uri = new UriBuilder($"{App.BaseUri}users/signup?key=zmacs3jmjprNC9gba7xf5nZZDg87NM6dLYdaTwzT");
                var response = await httpClient.PostAsync(uri.ToString(), httpContent);
                if (response != null && response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (result != null) status = JsonConvert.DeserializeObject<Status>(result);
                }
            }
            catch (HttpRequestException ex)
            {
                Crashes.TrackError(ex);
                status.IsSuccess = false;
                status.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                status.IsSuccess = false;
                status.Message = ex.Message;
            }

            return status;
        }

        /// <summary>
        /// This method is to get forgettable password.
        /// </summary>
        public async Task<Status> ForgotPassword(string emailId)
        {
            var status = new Status();
            try
            {
                var uri = new UriBuilder($"{App.BaseUri}users/forgot?key=zmacs3jmjprNC9gba7xf5nZZDg87NM6dLYdaTwzT&emailId={emailId}");
                var response = await httpClient.PostAsync(uri.ToString(), null);
                if (response != null && response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (result != null) status = JsonConvert.DeserializeObject<Status>(result);
                }
            }
            catch (HttpRequestException ex)
            {
                Crashes.TrackError(ex);
                status.IsSuccess = false;
                status.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                status.IsSuccess = false;
                status.Message = ex.Message;
            }

            return status;
        }

       
    }
}