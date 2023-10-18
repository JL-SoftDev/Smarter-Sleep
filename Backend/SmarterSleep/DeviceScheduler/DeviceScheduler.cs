using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DeviceScheduler
{
    public class DeviceScheduler
    {
        static HttpClient client = new HttpClient();

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("https://localhost:443/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        
    }
}