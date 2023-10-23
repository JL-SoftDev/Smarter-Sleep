using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DeviceScheduler.ModelObjects;

namespace DeviceScheduler
{
    public class DeviceSchedulerAPIClient
    {
        static HttpClient client = new HttpClient();

        public static async Task RunAsync()
        {
            client.BaseAddress = new Uri("https://localhost:443");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<AppUser> GetAppUserAsync(Guid userId)
        {
            AppUser user = null;
            HttpResponseMessage response = await client.GetAsync("/api/AppUsers/" + userId);
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<AppUser>();
            }
            return user;
        }

        public static async Task<List<Device>> GetDevicesAsync(Guid userId)
        {
            List<Device> devices = null;
            HttpResponseMessage response = await client.GetAsync("/api/Devices");
            if (response.IsSuccessStatusCode)
            {
                devices = await response.Content.ReadAsAsync<List<Device>>();
            }
            if (devices != null)
            {
                for (int i = 0; i < devices.Count; i++)
                {
                    if (devices[i].user_id != userId)
                    {
                        devices.RemoveAt(i);
                        i--;
                    }
                }
            }
            return devices;
        }

        public static async Task<SleepReview> GetLastWeekTomorrowSleepReviewAsync(Guid userId)
        {
            SleepReview review = null;
            List<SleepReview> reviews = null;
            HttpResponseMessage response = await client.GetAsync("/api/SleepReviews");
            if (response.IsSuccessStatusCode)
            {
                reviews = await response.Content.ReadAsAsync<List<SleepReview>>();
            }
            if (reviews != null)
            {
                for (int i = 0; i < reviews.Count; i++)
                {
                    if (reviews[i].user_id != userId)
                    {
                        reviews.RemoveAt(i);
                        i--;
                    }
                    else if (reviews[i].created_at.Date != DateTime.Today.AddDays(-6))
                    {
                        reviews.RemoveAt(i);
                        i--;
                    }
                }
                if (reviews.Count == 1)
                {
                    review = reviews[0];
                }
            }

            return review;
        }

        public static async Task<WearableData> GetLastWeekTomorrowWearableDataAsync(int id)
        {
            WearableData data = null;
            HttpResponseMessage response = await client.GetAsync("/api/WearableDatas/" + id);
            if (response.IsSuccessStatusCode)
            {
                data = await response.Content.ReadAsAsync<WearableData>();
            }
            return data;
        }

        public static async Task<Survey> GetLastWeekTomorrowSurveyAsync(int id)
        {
            Survey survey = null;
            HttpResponseMessage response = await client.GetAsync("/api/Surveys/" + id);
            if (response.IsSuccessStatusCode)
            {
                survey = await response.Content.ReadAsAsync<Survey>();
            }
            return survey;
        }

        public static async Task<CustomSchedule> GetTomorrowScheduleAsync(Guid userId)
        {
            DateTime tomorrow = DateTime.Today.AddDays(1);
            int? day = null;
            switch (tomorrow.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    day = 0;
                    break;
                case DayOfWeek.Monday:
                    day = 1;
                    break;
                case DayOfWeek.Tuesday:
                    day = 2;
                    break;
                case DayOfWeek.Wednesday:
                    day = 3;
                    break;
                case DayOfWeek.Thursday:
                    day = 4;
                    break;
                case DayOfWeek.Friday:
                    day = 5;
                    break;
                case DayOfWeek.Saturday:
                    day = 6;
                    break;
            }
            CustomSchedule schedule = null;
            List<CustomSchedule> schedules = null;
            HttpResponseMessage response = await client.GetAsync("/api/CustomSchedules");
            if (response.IsSuccessStatusCode)
            {
                schedules = await response.Content.ReadAsAsync<List<CustomSchedule>>();
            }
            if (schedules != null)
            {
                for (int i = 0; i < schedules.Count; i++)
                {
                    if (schedules[i].user_id != userId)
                    {
                        schedules.RemoveAt(i);
                        i--;
                    }
                    else if (schedules[i].day_of_week != day)
                    {
                        schedules.RemoveAt(i);
                        i--;
                    }
                }
                if (schedules.Count == 1)
                {
                    schedule = schedules[0];
                }
            }
            return schedule;
        }

        public static async Task<SleepSettings> GetLastWeekTomorrowSettingsAsync(Guid userId)
        {
            SleepSettings settings = null;
            List<SleepSettings> listSettings = null;
            HttpResponseMessage response = await client.GetAsync("/api/SleepSettings");
            if (response.IsSuccessStatusCode)
            {
                listSettings = await response.Content.ReadAsAsync<List<SleepSettings>>();
            }
            if (listSettings != null)
            {
                for (int i = 0; i < listSettings.Count; i++)
                {
                    if (listSettings[i].user_id != userId)
                    {
                        listSettings.RemoveAt(i);
                        i--;
                    }
                    else if (listSettings[i].scheduled_wake.DayOfWeek != DateTime.Today.AddDays(1).DayOfWeek)
                    {
                        listSettings.RemoveAt(i);
                        i--;
                    }
                }
                if (listSettings.Count == 1)
                {
                    settings = listSettings[0];
                }
            }
            return settings;
        }

        public static async Task<SleepSettings> GetSleepSettingsAsync(int id)
        {
            SleepSettings settings = null;
            HttpResponseMessage response = await client.GetAsync("/api/SleepSettings/" + id);
            if (response.IsSuccessStatusCode)
            {
                settings = await response.Content.ReadAsAsync<SleepSettings>();
            }
            return settings;
        }

        public static async Task<SleepSettings> CreateSleepSettingsAsync(SleepSettingsDefaultID settings)
        {
            SleepSettings responseSettings = null;
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/SleepSettings", settings);
            response.EnsureSuccessStatusCode();
            responseSettings = await response.Content.ReadAsAsync<SleepSettings>();
            return responseSettings;
        }

        public static async Task<DeviceSettings> CreateDeviceSettingsAsync(DeviceSettingsDefaultID settigns)
        {
            DeviceSettings responseSettings = null;
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/DeviceSettings", settigns);
            response.EnsureSuccessStatusCode();
            responseSettings = await response.Content.ReadAsAsync<DeviceSettings>();
            return responseSettings;
        }
    }
}