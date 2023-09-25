using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using TableObjects;

namespace ManualDataEntryCLI
{
    public class Core
    {
        private NpgsqlConnection con;
        public Core(NpgsqlConnection inCon)
        {
            con = inCon;
        }
        public (bool found, string? returnName, Guid? returnId) findUser()
        {
            bool userFound = false;
            Guid? userId = null;

            Console.WriteLine("Please enter the username of the account that you would like to access");

            string? userName = Console.ReadLine();

            if (String.IsNullOrEmpty(userName))
            {
                Console.WriteLine("Usernames can not be empty");
                userFound = false;
            }
            else
            {
                var selectUser = "SELECT user_id FROM app_user WHERE username='" + userName + "'";

                using var selectUserCmd = new NpgsqlCommand(selectUser, con);

                userId = (Guid?)selectUserCmd.ExecuteScalar();

                if (userId == null || userId == Guid.Empty)
                {
                    Console.WriteLine("User not found");
                    userFound = false;
                }
                else
                {
                    Console.WriteLine("Account " + userName + " found with uuid of " + userId.ToString());
                    userFound = true;
                }
            }
            return (userFound, userName, userId);
        }
        public void ViewWearableData(String userName, Guid userID, DateTime viewDate)
        {
            var getSleepReview = "SELECT wearable_log_id FROM sleep_review WHERE user_id='" + userID.ToString() + "' AND sleep_date='" + viewDate.ToString("yyyy-MM-dd") + "'";
            using var sleepReviewCmd = new NpgsqlCommand(getSleepReview, con);
            int? wearableId = (int?)sleepReviewCmd.ExecuteScalar();
            if (!wearableId.HasValue)
            {
                Console.WriteLine("No wearable data found for slected user for selected date");
                return;
            }
            WearableData retrievedWearableData = new WearableData(con, (int)wearableId);
            Console.WriteLine("The wearable data for user " + userName + " from the date " + viewDate.ToString("M/d/yyyy") + " is as follows");
            Console.WriteLine(retrievedWearableData.ToString());
        }
        public void InsertWearableData(String userName, Guid userID, DateTime accessDate)
        {
            var getSleepReview = "SELECT wearable_log_id FROM sleep_review WHERE user_id='" + userID.ToString() + "' AND sleep_date='" + accessDate.ToString("yyyy-MM-dd") + "'";
            using var sleepReviewCmd = new NpgsqlCommand(getSleepReview, con);
            int? wearableId = ConvertFromDBVal<int?>(sleepReviewCmd.ExecuteScalar());
            if (wearableId.HasValue)
            {
                Console.WriteLine("Wearable data was found for user " + userName + " from the date " + accessDate.ToString("M/d/yyyy"));
                bool overwriteData = false;
                do
                {
                    Console.WriteLine("Woud you like to overwrite the data for this date? yes/no");
                    string? yesNo = Console.ReadLine();
                    if (String.IsNullOrEmpty(yesNo))
                    {
                        overwriteData = false;
                    }
                    else
                    {
                        yesNo = yesNo.ToUpper();
                        if (String.Equals(yesNo, "YES"))
                        {
                            overwriteData = true;
                        }
                        else if (String.Equals(yesNo, "NO"))
                        {
                            overwriteData = true;
                            return;
                        }
                        else
                        {
                            overwriteData = false;
                        }
                    }
                } while (!overwriteData);
                WearableData.RemoveWearableData(con, (int)wearableId);
            }
            else
            {
                wearableId = 0;
            }
            bool startSelected = false;
            DateTime userStartTime;
            do
            {
                Console.WriteLine("Please enter the time for sleep_start using a 24-hour clock \"HH:mm\"");
                if (DateTime.TryParse(accessDate.ToString("M/d/yyyy") + " " + Console.ReadLine(), out userStartTime))
                {
                    bool startTimeConfirmed = false;
                    do
                    {
                        Console.WriteLine("You have entered the time " + userStartTime.ToString("HH:mm"));
                        Console.WriteLine("Is this the correct time for sleep_start? yes/no");
                        string? yesNo = Console.ReadLine();
                        if (String.IsNullOrEmpty(yesNo))
                        {
                            startTimeConfirmed = false;
                        }
                        else
                        {
                            yesNo = yesNo.ToUpper();
                            if (String.Equals(yesNo, "YES"))
                            {
                                startTimeConfirmed = true;
                                startSelected = true;
                            }
                            else if (String.Equals(yesNo, "NO"))
                            {
                                startTimeConfirmed = true;
                                startSelected = false;
                            }
                            else
                            {
                                startTimeConfirmed = false;
                            }
                        }
                    } while (!startTimeConfirmed);
                }
                else
                {
                    startSelected = false;
                }
            } while (!startSelected);
            bool endSelected = false;
            DateTime userEndTime;
            do
            {
                Console.WriteLine("Please enter the time for sleep_end using a 24-hour clock \"HH:mm\"");
                if (DateTime.TryParse(accessDate.ToString("M/d/yyyy") + " " + Console.ReadLine(), out userEndTime))
                {
                    bool endTimeConfirmed = false;
                    do
                    {
                        Console.WriteLine("You have entered the time " + userEndTime.ToString("HH:mm"));
                        Console.WriteLine("Is this the correct time for sleep_end? yes/no");
                        string? yesNo = Console.ReadLine();
                        if (String.IsNullOrEmpty(yesNo))
                        {
                            endTimeConfirmed = false;
                        }
                        else
                        {
                            yesNo = yesNo.ToUpper();
                            if (String.Equals(yesNo, "YES"))
                            {
                                endTimeConfirmed = true;
                                endSelected = true;
                            }
                            else if (String.Equals(yesNo, "NO"))
                            {
                                endTimeConfirmed = true;
                                endSelected = false;
                            }
                            else
                            {
                                endTimeConfirmed = false;
                            }
                        }
                    } while (!endTimeConfirmed);
                }
                else
                {
                    endSelected = false;
                }
            } while (!endSelected);
            bool hypnoSelected = false;
            string? userHypno = "";
            do
            {
                Console.WriteLine("Please enter the hypnogram data");
                userHypno = Console.ReadLine();
                if (!String.IsNullOrEmpty(userHypno))
                {
                    bool hypnoConfirmed = false;
                    do
                    {
                        Console.WriteLine("You have entered " + userHypno);
                        Console.WriteLine("Is this the correct? yes/no");
                        string? yesNo = Console.ReadLine();
                        if (String.IsNullOrEmpty(yesNo))
                        {
                            hypnoConfirmed = false;
                        }
                        else
                        {
                            yesNo = yesNo.ToUpper();
                            if (String.Equals(yesNo, "YES"))
                            {
                                hypnoConfirmed = true;
                                hypnoSelected = true;
                            }
                            else if (String.Equals(yesNo, "NO"))
                            {
                                hypnoConfirmed = true;
                                hypnoSelected = false;
                            }
                            else
                            {
                                hypnoConfirmed = false;
                            }
                        }
                    } while (!hypnoConfirmed);
                }
                else
                {
                    hypnoSelected = false;
                }
            } while (!hypnoSelected);
            bool scoreSelected = false;
            int? userScore = 0;
            do
            {
                Console.WriteLine("Please enter the sleep score");
                try
                {
                    userScore = Convert.ToInt32(Console.ReadLine());

                    if (!userScore.HasValue)
                    {
                        scoreSelected = false;
                    }
                    else
                    {
                        bool scoreConfirmed = false;
                        do
                        {
                            Console.WriteLine("You have entered " + userScore);
                            Console.WriteLine("Is this the correct? yes/no");
                            string? yesNo = Console.ReadLine();
                            if (String.IsNullOrEmpty(yesNo))
                            {
                                scoreConfirmed = false;
                            }
                            else
                            {
                                yesNo = yesNo.ToUpper();
                                if (String.Equals(yesNo, "YES"))
                                {
                                    scoreConfirmed = true;
                                    scoreSelected = true;
                                }
                                else if (String.Equals(yesNo, "NO"))
                                {
                                    scoreConfirmed = true;
                                    scoreSelected = false;
                                }
                                else
                                {
                                    scoreConfirmed = false;
                                }
                            }
                        } while (!scoreConfirmed);
                    }
                }
                catch (FormatException)
                {
                    scoreSelected = false;
                }
                catch (OverflowException)
                {
                    scoreSelected = false;
                }
            } while (!scoreSelected);
            int returnId = 0;
            int rowsAffectedWearable = 0;
            int rowsAffectedReview = 0;
            WearableData toInsert = new WearableData((int)wearableId, userStartTime, userEndTime, userHypno, (int)userScore, accessDate);
            if (toInsert.id == 0)
            {
                returnId = toInsert.InsertWearableData(con);
                if (returnId != 0)
                {
                    rowsAffectedWearable = 1;
                }
                var updateWearableId = "UPDATE sleep_review SET wearable_log_id=" + returnId + " WHERE user_id='" + userID.ToString() + "' AND sleep_date='" + accessDate.ToString("yyyy-MM-dd") + "'";
                using var updateWearableIdCmd = new NpgsqlCommand(updateWearableId, con);
                rowsAffectedReview = updateWearableIdCmd.ExecuteNonQuery();
                if (rowsAffectedReview == 0)
                {
                    var insertWearableId = "INSERT INTO sleep_review(user_id, wearable_log_id, sleep_date) VALUES(@id, @log, @date)";
                    using var insertWearableIdCmd = new NpgsqlCommand(insertWearableId, con);
                    insertWearableIdCmd.Parameters.AddWithValue("id", userID);
                    insertWearableIdCmd.Parameters.AddWithValue("log", returnId);
                    insertWearableIdCmd.Parameters.AddWithValue("date", accessDate);
                    insertWearableIdCmd.Prepare();
                    rowsAffectedReview = insertWearableIdCmd.ExecuteNonQuery();
                }
            }
            else
            {
                rowsAffectedWearable = toInsert.InsertWithIDWearableData(con);
            }
            if (rowsAffectedWearable == 0)
            {
                Console.WriteLine("No rows were affected in the wearable_data table");
            }
            else if (rowsAffectedWearable == -1)
            {
                Console.WriteLine("An unknown ammount of rows may have been affected in the wearable_data table");
                Console.WriteLine("Please check databse");
            }
            else
            {
                Console.WriteLine(rowsAffectedWearable + " rows were affected in the wearable_data table");
            }
            if (rowsAffectedReview == 0)
            {
                Console.WriteLine("No rows were affected in the sleep_review table");
            }
            else if (rowsAffectedReview == -1)
            {
                Console.WriteLine("An unknown ammount of rows may have been affected in the sleep_review table");
                Console.WriteLine("Please check databse");
            }
            else
            {
                Console.WriteLine(rowsAffectedReview + " rows were affected in the sleep_review table");
            }
        }
        public void RemoveWearableData(String userName, Guid userID, DateTime accessDate)
        {
            var getSleepReview = "SELECT wearable_log_id FROM sleep_review WHERE user_id='" + userID.ToString() + "' AND sleep_date='" + accessDate.ToString("yyyy-MM-dd") + "'";
            using var sleepReviewCmd = new NpgsqlCommand(getSleepReview, con);
            int? wearableId = (int?)sleepReviewCmd.ExecuteScalar();
            if (!wearableId.HasValue)
            {
                Console.WriteLine("No wearable data found for slected user for selected date");
                return;
            }
            bool removeConfirmed = false;
            bool removeSelected = false;
            do
            {
                Console.WriteLine("Are you sure you would like to remove the wearable data from the user " + userName + " for " + accessDate.ToString("M/d/yyyy") + "? yes/no");
                string? yesNo = Console.ReadLine();
                if (String.IsNullOrEmpty(yesNo))
                {
                    removeConfirmed = false;
                }
                else
                {
                    yesNo = yesNo.ToUpper();
                    if (String.Equals(yesNo, "YES"))
                    {
                        removeConfirmed = true;
                        removeSelected = true;
                    }
                    else if (String.Equals(yesNo, "NO"))
                    {
                        removeConfirmed = true;
                        removeSelected = false;
                    }
                    else
                    {
                        removeConfirmed = false;
                    }
                }
            } while (!removeConfirmed);
            int rowsAffectedWearable = 0;
            int rowsAffectedReview = 0;
            if (removeSelected)
            {
                var removeWearableId = "UPDATE sleep_review SET wearable_log_id=null WHERE user_id='" + userID.ToString() + "' AND sleep_date='" + accessDate.ToString("yyyy-MM-dd") + "'";
                using var removeWearableIdCmd = new NpgsqlCommand(removeWearableId, con);
                rowsAffectedReview = removeWearableIdCmd.ExecuteNonQuery();
                rowsAffectedWearable = WearableData.RemoveWearableData(con, (int)wearableId);
            }
            if (rowsAffectedWearable == 0)
            {
                Console.WriteLine("No rows were affected in the wearable_data table");
            }
            else if (rowsAffectedWearable == -1)
            {
                Console.WriteLine("An unknown ammount of rows may have been affected in the wearable_data table");
                Console.WriteLine("Please check databse");
            }
            else
            {
                Console.WriteLine(rowsAffectedWearable + " rows were affected in the wearable_data table");
            }
            if (rowsAffectedReview == 0)
            {
                Console.WriteLine("No rows were affected in the sleep_review table");
            }
            else if (rowsAffectedReview == -1)
            {
                Console.WriteLine("An unknown ammount of rows may have been affected in the sleep_review table");
                Console.WriteLine("Please check databse");
            }
            else
            {
                Console.WriteLine(rowsAffectedReview + " rows were affected in the sleep_review table");
            }
        }
        private T? ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default;
            }
            else
            {
                return (T)obj;
            }
        }
    }
}