using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApi.Models;

public class SeedData
{
    public static void Initialize(postgresContext context)
    {
        context.Database.EnsureCreated();

        // Declare guid1, set to either existing test user or new test user.
        Guid guid1;
        var existingTestUser = context.AppUsers.FirstOrDefault(u => u.Username == "initialUser1");

        //Add a test user
        if (existingTestUser == null)
        {
            guid1 = Guid.NewGuid();
            context.AppUsers.Add(new AppUser
            {
                UserId = guid1,
                Username = "initialUser1",
                CreatedAt = DateTime.Now,
                Points = 100
            });
            context.SaveChanges();
        } 
        else 
        {
            guid1 = existingTestUser.UserId;
        }
        
        if (!context.CustomSchedules.Any())
        {
            context.CustomSchedules.Add(new CustomSchedule
            {
                UserId = guid1,
                DayOfWeek = 1,
                WakeTime = new TimeOnly(8, 00, 00)
            });
            context.SaveChanges();
        }

        //Generate a test Sleep Review along with a Survey and Wearable Log.
        if (!context.SleepReviews.Any())
        {
            context.SleepReviews.Add(new SleepReview
            {
                UserId = guid1,
                CreatedAt = DateTime.Now,
                SmarterSleepScore = 85,
                Survey = new Survey {
                    CreatedAt = DateTime.Now,
                    SleepQuality = 4, 
                    WakePreference = 1,
                    TemperaturePreference = 0,
                    LightsDisturbance = false,
                    SleepEarlier = true,
                    SleepDuration = 420, 
                    SurveyDate = DateOnly.FromDateTime(DateTime.Now)
                },
                WearableLog = new WearableData
                {
                    SleepStart = DateTime.Now,
                    SleepEnd = DateTime.Now.AddHours(8),
                    Hypnogram = "443432222211222333321112222222222111133333322221112233333333332232222334",
                    SleepScore = 90,
                    SleepDate = DateOnly.FromDateTime(DateTime.Now)
                }

            });
            context.SaveChanges();
        }

        if (!context.Devices.Any())
        {
            context.Devices.Add(new Device
            {
                UserId = guid1,
                Name = "Example Light",
                Type = "Light",
                Ip = "192.168.1.100",
                Port = 8080,
                Status = "On"
            });
            context.SaveChanges();
        }

        if (!context.SleepSettings.Any())
        {
            context.SleepSettings.Add(new SleepSetting
            {
                UserId = guid1,
                ScheduledSleep = DateTime.Parse("22:00:00"),
                ScheduledWake = DateTime.Parse("06:00:00"),
                ScheduledHypnogram = "443432222211222333321112222222222111133333322221112233333333332232222334", // Using example data from Oura ring.
                DeviceSettings = {
                    new DeviceSetting
                    {
                        DeviceId = 1,
                        ScheduledTime = DateTime.Parse("21:30:00"),
                        Settings = JsonConvert.SerializeObject(new {Brightness = 0})
                    },
                    new DeviceSetting
                    {
                        DeviceId = 1,
                        ScheduledTime = DateTime.Parse("6:00:00"),
                        Settings = JsonConvert.SerializeObject(new {Brightness = 100})
                    }
                }
            });
            context.SaveChanges();
        }

        if (!context.DailyStreaks.Any())
        {
            context.DailyStreaks.Add(new DailyStreak
            {
                UserId = guid1,
                StartDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-7),
                LastDate = DateOnly.FromDateTime(DateTime.Now)
            });
            context.SaveChanges();
        }

        if (!context.Items.Any())
        {
            context.Items.Add(new Item
            {
                Name = "Sample Hat",
                Description = "Test hat seed data",
                Cost = 50
            });
            context.SaveChanges();
        }

        if (!context.Challenges.Any())
        {
            context.Challenges.Add(new Challenge
            {
                Name = "Sample Challenge",
                Description = "Test challenge seed data",
                Reward = 100
            });
            context.SaveChanges();
        }

        if (!context.UserChallenges.Any())
        {
            //"initialUser1" will be assigned "Sample Challenge"
            context.UserChallenges.Add(new UserChallenge
            {
                UserId = guid1, // "initialUser1"
                ChallengeId = 1, // Sample Challenge
                UserSelected = false,
                StartDate = DateTime.Now,
                ExpireDate = DateTime.Now.AddDays(7) // Expires in 7 days
            });
            context.SaveChanges();
        }

        if (!context.PurchaseLogs.Any())
        {
            context.PurchaseLogs.Add(new PurchaseLog
            {
                ItemId = 1, // Sample Hat
                Transaction = new Transaction {
                    UserId = guid1, // "initialUser1"
                    CreatedAt = DateTime.Now,
                    PointAmount = 50, // Spent 50 on the hat
                    Description = "Example Purchase"
                }
            });
            context.SaveChanges();
        }

        if (!context.ChallengeLogs.Any())
        {
            context.ChallengeLogs.Add(new ChallengeLog
            {
                ChallengeId = 1, // "Sample Challenge"
                Transaction = new Transaction {
                    UserId = guid1, // "initialUser1"
                    CreatedAt = DateTime.Now,
                    PointAmount = 100, // User earned 100
                    Description = "Example challenge completion"
                }
            });
            context.SaveChanges();
        }
    }
}
