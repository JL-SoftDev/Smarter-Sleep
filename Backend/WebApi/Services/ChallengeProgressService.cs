using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Services
{
    public class ChallengeProgressService : IChallengeProgressService
    {
        private readonly postgresContext _databaseContext;
        private readonly ISleepDataService _sleepDataService;
        private readonly IUserDataService _userDataService;

        public ChallengeProgressService(postgresContext databaseContext, ISleepDataService sleepDataService, IUserDataService userDataService)
        {
            _databaseContext = databaseContext;
            _sleepDataService = sleepDataService;
            _userDataService = userDataService;
        }

        public async Task<IEnumerable<IChallengeProgressService.ChallengeReturn>> GetChallengeProgress(Guid userId, DateTime? dateTime)
        {
            if (dateTime == null)
            {
                dateTime = DateTime.Now;
            }
            List<IChallengeProgressService.ChallengeReturn> challengeReturns = new List<IChallengeProgressService.ChallengeReturn>();
            List<Challenge> typeList = new List<Challenge>();
            if (_databaseContext.Challenges != null)
            {
                typeList = await _databaseContext.Challenges.ToListAsync();
            }
            List<Challenge> orderedTypeList = typeList.OrderBy(challenge => challenge.Id).ToList();
            List<UserChallenge> challengesList = new List<UserChallenge>();
            if (_databaseContext.UserChallenges != null)
            {
                challengesList = await _databaseContext.UserChallenges.ToListAsync();
            }
            for (int i = 0; i < challengesList.Count(); i++)
            {
                if (challengesList[i].UserId != userId)
                {
                    challengesList.RemoveAt(i);
                    i --;
                }
                else if (challengesList[i].Completed != null)
                {
                    if ((bool)challengesList[i].Completed!)
                    {
                        challengesList.RemoveAt(i);
                        i --;
                    }
                }
            }
            DateTime earliestDate = (DateTime)dateTime;
            DateTime latestDate = (DateTime)dateTime;
            for (int i = 0; i < challengesList.Count(); i++)
            {
                if (challengesList[i].StartDate != null)
                {
                    if (challengesList[i].StartDate < earliestDate)
                    {
                        earliestDate = (DateTime)challengesList[i].StartDate!;
                    }
                }
                if (challengesList[i].ExpireDate != null)
                {
                    if (challengesList[i].ExpireDate > latestDate)
                    {
                        latestDate = (DateTime)challengesList[i].ExpireDate!;
                    }
                }
            }
            var getSleepReviews = await _sleepDataService.GetSleepReviews();
            List<SleepReview> sleepReviews = getSleepReviews.ToList();
            for (int i = 0; i < sleepReviews.Count(); i++)
            {
                if (sleepReviews[i].UserId != userId)
                {
                    sleepReviews.RemoveAt(i);
                    i--;
                }
                else if (sleepReviews[i].CreatedAt < earliestDate || sleepReviews[i].CreatedAt > latestDate)
                {
                    sleepReviews.RemoveAt(i);
                    i--;
                }
            }
            List<SleepReview> orderedSleepReviews = sleepReviews.OrderBy(review => review.CreatedAt).ToList();
            for (int i = 0; i < challengesList.Count(); i++)
            {
                IChallengeProgressService.ChallengeReturn newReturn = new IChallengeProgressService.ChallengeReturn();
                switch (challengesList[i].ChallengeId)
                {
                    case 1:
                        newReturn = type1Progress(challengesList[i], orderedSleepReviews, (DateTime)dateTime, orderedTypeList);
                        break;
                    case 2:
                        newReturn = type2Progress(challengesList[i], orderedSleepReviews, (DateTime)dateTime, orderedTypeList);
                        break;
                    case 3:
                        newReturn = type3Progress(challengesList[i], orderedSleepReviews, (DateTime)dateTime, orderedTypeList);
                        break;
                    case 4:
                        newReturn = type4Progress(challengesList[i], orderedSleepReviews, (DateTime)dateTime, orderedTypeList);
                        break;
                    case 5:
                        newReturn = type5Progress(challengesList[i], orderedSleepReviews, (DateTime)dateTime, orderedTypeList);
                        break;
                    case 6:
                        newReturn = await type6Progress(challengesList[i], orderedSleepReviews, (DateTime)dateTime, userId, orderedTypeList);
                        break;
                    default:
                        break;
                }
                challengeReturns.Add(newReturn);
            }
            return challengeReturns;
        }

        private IChallengeProgressService.ChallengeReturn type1Progress(UserChallenge challenge, List<SleepReview> sleepReviews, DateTime dateTime, List<Challenge> orderedTypes)
        {
            IChallengeProgressService.ChallengeReturn newReturn = new IChallengeProgressService.ChallengeReturn();
            List<SleepReview> filteredReviewList = filterReviews(challenge, sleepReviews, dateTime);
            newReturn.ChallengeLogId = challenge.Id;
            newReturn.ChallengeId = 1;
            if (orderedTypes.Count() > 0 && orderedTypes[0].Id == 1)
            {
                newReturn.ChallengeName = orderedTypes[0].Name;
                newReturn.ChallengeDescription = orderedTypes[0].Description;
            }
            else
            {
                newReturn.ChallengeName = "Zzz Zoom";
                newReturn.ChallengeDescription = "Zoom into bed earlier for a night of uninterrupted Zzz''s, setting the stage for a morning full of possibilities.";
            }
            newReturn.StartDate = challenge.StartDate;
            newReturn.ExpireDate = challenge.ExpireDate;
            newReturn.UserSelected = challenge.UserSelected;
            newReturn.Goal = 3;
            for (int i = 0; i < filteredReviewList.Count(); i ++)
            {
                if (filteredReviewList[i].Survey == null)
                {
                    filteredReviewList.RemoveAt(i);
                    i --;
                }
                else if (filteredReviewList[i].Survey!.SleepEarlier == null)
                {
                    filteredReviewList.RemoveAt(i);
                    i --;
                }
                else if (!(bool)filteredReviewList[i].Survey!.SleepEarlier!)
                {
                    filteredReviewList.RemoveAt(i);
                    i --;
                }
            }
            newReturn.Completed = filteredReviewList.Count();
            newReturn.CompletionPercentage = Math.Min(1.0, ((double)newReturn.Completed/(double)newReturn.Goal));
            return newReturn;
        }

        private IChallengeProgressService.ChallengeReturn type2Progress(UserChallenge challenge, List<SleepReview> sleepReviews, DateTime dateTime, List<Challenge> orderedTypes)
        {
            IChallengeProgressService.ChallengeReturn newReturn = new IChallengeProgressService.ChallengeReturn();
            List<SleepReview> filteredReviewList = filterReviews(challenge, sleepReviews, dateTime);
            newReturn.ChallengeLogId = challenge.Id;
            newReturn.ChallengeId = 2;
            if (orderedTypes.Count() > 1 && orderedTypes[1].Id == 2)
            {
                newReturn.ChallengeName = orderedTypes[1].Name;
                newReturn.ChallengeDescription = orderedTypes[1].Description;
            }
            else
            {
                newReturn.ChallengeName = "Dreamy Eight";
                newReturn.ChallengeDescription = "Dive into a dreamy world with the challenge of securing a solid eight hours of sleep for ultimate rejuvenation.";
            }
            newReturn.StartDate = challenge.StartDate;
            newReturn.ExpireDate = challenge.ExpireDate;
            newReturn.UserSelected = challenge.UserSelected;
            newReturn.Goal = 5;
            for (int i = 0; i < filteredReviewList.Count(); i ++)
            {
                if (filteredReviewList[i].Survey == null)
                {
                    filteredReviewList.RemoveAt(i);
                    i --;
                }
                else if (filteredReviewList[i].Survey!.SleepDuration < 480)
                {
                    filteredReviewList.RemoveAt(i);
                    i --;
                }
            }
            newReturn.Completed = filteredReviewList.Count();
            newReturn.CompletionPercentage = Math.Min(1.0, ((double)newReturn.Completed/(double)newReturn.Goal));
            return newReturn;
        }

        private IChallengeProgressService.ChallengeReturn type3Progress(UserChallenge challenge, List<SleepReview> sleepReviews, DateTime dateTime, List<Challenge> orderedTypes)
        {
            IChallengeProgressService.ChallengeReturn newReturn = new IChallengeProgressService.ChallengeReturn();
            List<SleepReview> filteredReviewList = filterReviews(challenge, sleepReviews, dateTime);
            newReturn.ChallengeLogId = challenge.Id;
            newReturn.ChallengeId = 3;
            if (orderedTypes.Count() > 2 && orderedTypes[2].Id == 3)
            {
                newReturn.ChallengeName = orderedTypes[2].Name;
                newReturn.ChallengeDescription = orderedTypes[2].Description;
            }
            else
            {
                newReturn.ChallengeName = "Midnight Munch Ban";
                newReturn.ChallengeDescription = "Avoid the temptation of a snack before bed, go 90 minutes without eating prior to sleep.";
            }
            newReturn.StartDate = challenge.StartDate;
            newReturn.ExpireDate = challenge.ExpireDate;
            newReturn.UserSelected = challenge.UserSelected;
            newReturn.Goal = 3;
            for (int i = 0; i < filteredReviewList.Count(); i ++)
            {
                if (filteredReviewList[i].Survey == null)
                {
                    filteredReviewList.RemoveAt(i);
                    i --;
                }
                else if (filteredReviewList[i].Survey!.AteLate == null)
                {
                    filteredReviewList.RemoveAt(i);
                    i --;
                }
                else if ((bool)filteredReviewList[i].Survey!.AteLate!)
                {
                    filteredReviewList.RemoveAt(i);
                    i --;
                }
            }
            newReturn.Completed = filteredReviewList.Count();
            newReturn.CompletionPercentage = Math.Min(1.0, ((double)newReturn.Completed/(double)newReturn.Goal));
            return newReturn;
        }

        private IChallengeProgressService.ChallengeReturn type4Progress(UserChallenge challenge, List<SleepReview> sleepReviews, DateTime dateTime, List<Challenge> orderedTypes)
        {
            IChallengeProgressService.ChallengeReturn newReturn = new IChallengeProgressService.ChallengeReturn();
            List<SleepReview> filteredReviewList = filterReviews(challenge, sleepReviews, dateTime);
            newReturn.ChallengeLogId = challenge.Id;
            int startToNow = (dateTime - (DateTime)challenge.StartDate!).Days;
            newReturn.ChallengeId = 4;
            if (orderedTypes.Count() > 3 && orderedTypes[3].Id == 4)
            {
                newReturn.ChallengeName = orderedTypes[3].Name;
                newReturn.ChallengeDescription = orderedTypes[3].Description;
            }
            else
            {
                newReturn.ChallengeName = "Dynamic Dream Duo";
                newReturn.ChallengeDescription = "Interact with the Smarter Sleep app everyday for two weeks straight.";
            }
            newReturn.StartDate = challenge.StartDate;
            newReturn.ExpireDate = challenge.ExpireDate;
            newReturn.UserSelected = challenge.UserSelected;
            newReturn.Goal = 14;
            newReturn.Completed = filteredReviewList.Count();
            newReturn.CompletionPercentage = Math.Min(1.0, ((double)newReturn.Completed/(double)newReturn.Goal));
            return newReturn;
        }

        private IChallengeProgressService.ChallengeReturn type5Progress(UserChallenge challenge, List<SleepReview> sleepReviews, DateTime dateTime, List<Challenge> orderedTypes)
        {
            IChallengeProgressService.ChallengeReturn newReturn = new IChallengeProgressService.ChallengeReturn();
            List<SleepReview> filteredReviewList = filterReviews(challenge, sleepReviews, dateTime);
            newReturn.ChallengeLogId = challenge.Id;
            newReturn.ChallengeId = 5;
            if (orderedTypes.Count() > 4 && orderedTypes[4].Id == 5)
            {
                newReturn.ChallengeName = orderedTypes[4].Name;
                newReturn.ChallengeDescription = orderedTypes[4].Description;
            }
            else
            {
                newReturn.ChallengeName = "Seamless Sleep Automation";
                newReturn.ChallengeDescription = "Allow Smarter Sleep to schedule smart home devices without overriding any options.";
            }
            newReturn.StartDate = challenge.StartDate;
            newReturn.ExpireDate = challenge.ExpireDate;
            newReturn.UserSelected = challenge.UserSelected;
            newReturn.Goal = 5;
            newReturn.Completed = 0;
            for (int i = 0; i < filteredReviewList.Count(); i ++)
            {
                if (filteredReviewList[i].SleepSetting == null)
                {
                    filteredReviewList.RemoveAt(i);
                    i --;
                }
                else if (filteredReviewList[i].SleepSetting!.DeviceSettings == null)
                {
                    filteredReviewList.RemoveAt(i);
                    i --;
                }
                else
                {
                    bool success = true;
                    List<DeviceSetting> checkList = filteredReviewList[i].SleepSetting!.DeviceSettings!.ToList();
                    for (int j = 0; j < checkList.Count(); j ++)
                    {
                        if (checkList[j].UserModified != null)
                        {
                            if ((bool)checkList[j].UserModified!)
                            {
                                success = false;
                            }
                        }
                    }
                    if (success)
                    {
                        newReturn.Completed ++;
                    }
                }
            }
            newReturn.CompletionPercentage = Math.Min(1.0, ((double)newReturn.Completed/(double)newReturn.Goal));
            return newReturn;
        }

        private async Task<IChallengeProgressService.ChallengeReturn> type6Progress(UserChallenge challenge, List<SleepReview> sleepReviews, DateTime dateTime, Guid userId, List<Challenge> orderedTypes)
        {
            IChallengeProgressService.ChallengeReturn newReturn = new IChallengeProgressService.ChallengeReturn();
            newReturn.ChallengeLogId = challenge.Id;
            newReturn.ChallengeId = 6;
            if (orderedTypes.Count() > 5 && orderedTypes[5].Id == 6)
            {
                newReturn.ChallengeName = orderedTypes[5].Name;
                newReturn.ChallengeDescription = orderedTypes[5].Description;
            }
            else
            {
                newReturn.ChallengeName = "Sunrise Scheduler";
                newReturn.ChallengeDescription = "Specify your non-negotiable wake-up times, allowing us to tailor your sleep routine to meet your daily commitments.";
            }
            newReturn.StartDate = challenge.StartDate;
            newReturn.UserSelected = challenge.UserSelected;
            newReturn.Goal = 7;
            var getCustomSchedules = await _userDataService.GetAllCustomSchedules();
            List<CustomSchedule> customSchedules = getCustomSchedules.ToList();
            for (int i = 0; i < customSchedules.Count(); i ++)
            {
                if (customSchedules[i].UserId != userId)
                {
                    customSchedules.RemoveAt(i);
                    i --;
                }
            }
            newReturn.Completed = customSchedules.Count();
            newReturn.CompletionPercentage = Math.Min(1.0, ((double)newReturn.Completed/(double)newReturn.Goal));
            return newReturn;
        }

        private List<SleepReview> filterReviews(UserChallenge challenge, List<SleepReview> sleepReviews, DateTime dateTime)
        {
            List<SleepReview> filteredReviewList = new List<SleepReview>(sleepReviews);
            for (int i = 0; i < filteredReviewList.Count(); i++)
            {
                if (filteredReviewList[i].CreatedAt < challenge.StartDate || filteredReviewList[i].CreatedAt > challenge.ExpireDate || filteredReviewList[i].CreatedAt > dateTime)
                {
                    filteredReviewList.RemoveAt(i);
                    i --;
                }
            }
            return filteredReviewList;
        }
    }
}