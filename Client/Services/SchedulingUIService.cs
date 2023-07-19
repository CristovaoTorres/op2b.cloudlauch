using Platform.Shared.Extensions;
using Platform.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Platform.Client.Services
{
    public class SchedulingUIService
    {
        private readonly HttpClient http;

        public SchedulingUIService(HttpClient Http)
        {
            http = Http;
        }


        public async Task<ResponseBase<IList<UserVMScheduling>>> GetAsync(string userId)
        {
            var response = await http.GetFromJsonAsync<ResponseBase<IList<UserVMScheduling>>>( $"UserScheduling?userId={userId}");
            return response;
        }

        public async Task<ResponseBase> AddAsync(string userId, IList<UserVMScheduling> items)
        {
            var response = await http.PostAsJsonAsync<IList<UserVMScheduling>, ResponseBase>($"UserScheduling?userId={userId}", items);
            return response;
        }
    }
}
