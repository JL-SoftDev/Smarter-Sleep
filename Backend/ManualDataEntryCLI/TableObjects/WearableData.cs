using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace TableObjects
{
    public class WearableData
    {
        private int _id;
        public int id
        {
            get => _id;
            set => _id = value;
        }
        private DateTime? _sleepStart;
        public DateTime? sleepStart
        {
            get => _sleepStart;
            set => _sleepStart = value;
        }
        private DateTime? _sleepEnd;
        public DateTime? sleepEnd
        {
            get => _sleepEnd;
            set => _sleepEnd = value;
        }
        private string? _hypnogram;
        public string? hypnogram
        {
            get => _hypnogram;
            set => _hypnogram = value;
        }
        private int? _sleepScore;
        public int? sleepScore
        {
            get => _sleepScore;
            set => _sleepScore = value;
        }
        private DateTime _sleepDate;
        public DateTime sleepDate
        {
            get => _sleepDate;
            set => _sleepDate = value;
        }
        public WearableData(int inId, DateTime inSleepStart, DateTime inSleepEnd, string inHypnogram, int inSleepScore, DateTime inSleepDate)
        {
            _id = inId;
            _sleepStart = inSleepStart;
            _sleepEnd = inSleepEnd;
            _hypnogram = inHypnogram;
            _sleepScore = inSleepScore;
            _sleepDate = inSleepDate;
        }

        public override string ToString()
        {
            string output = "id " + id + Environment.NewLine + "sleep_start " + sleepStart + Environment.NewLine + "sleep_end " + sleepEnd + Environment.NewLine + "hypnogram " + hypnogram + Environment.NewLine + "sleep_score " + sleepScore + Environment.NewLine + "sleep_date " + sleepDate.ToString("M/d/yyyy");
            return output;
        }

        public int InsertWearableData(NpgsqlConnection activeConnection)
        {
            var inputWearableData = "INSERT INTO wearable_data(sleep_start, sleep_end, hypnogram, sleep_score, sleep_date) VALUES(@start, @end, @hypno, @score, @date) RETURNING id";

            using var cmd = new NpgsqlCommand(inputWearableData, activeConnection);

            cmd.Parameters.AddWithValue("start", sleepStart);
            cmd.Parameters.AddWithValue("end", sleepEnd);
            cmd.Parameters.AddWithValue("hypno", hypnogram);
            cmd.Parameters.AddWithValue("score", sleepScore);
            cmd.Parameters.AddWithValue("date", sleepDate);
            cmd.Prepare();

            return (int)cmd.ExecuteScalar();
        }

        public int InsertWithIDWearableData(NpgsqlConnection activeConnection)
        {
            var inputWearableData = "INSERT INTO wearable_data(id, sleep_start, sleep_end, hypnogram, sleep_score, sleep_date) VALUES(@id, @start, @end, @hypno, @score, @date)";

            using var cmd = new NpgsqlCommand(inputWearableData, activeConnection);

            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("start", sleepStart);
            cmd.Parameters.AddWithValue("end", sleepEnd);
            cmd.Parameters.AddWithValue("hypno", hypnogram);
            cmd.Parameters.AddWithValue("score", sleepScore);
            cmd.Parameters.AddWithValue("date", sleepDate);
            cmd.Prepare();

            return cmd.ExecuteNonQuery();
        }

        public WearableData(NpgsqlConnection activeConnection, int getId)
        {
            var getSleepStart = "SELECT sleep_start FROM wearable_data WHERE id=" + getId;
            var getSleepEnd = "SELECT sleep_end FROM wearable_data WHERE id=" + getId;
            var getHypno = "SELECT hypnogram FROM wearable_data WHERE id=" + getId;
            var getSleepScore = "SELECT sleep_score FROM wearable_data WHERE id=" + getId;
            var getSleepDate = "SELECT sleep_date FROM wearable_data WHERE id=" + getId;

            using var sleepStartCmd = new NpgsqlCommand(getSleepStart, activeConnection);
            using var sleepEndCmd = new NpgsqlCommand(getSleepEnd, activeConnection);
            using var hypnoCmd = new NpgsqlCommand(getHypno, activeConnection);
            using var sleepScoreCmd = new NpgsqlCommand(getSleepScore, activeConnection);
            using var sleepDateCmd = new NpgsqlCommand(getSleepDate, activeConnection);

            _id = getId;
            _sleepStart = (DateTime?)sleepStartCmd.ExecuteScalar();
            _sleepEnd = (DateTime?)sleepEndCmd.ExecuteScalar();
            _hypnogram = (string?)hypnoCmd.ExecuteScalar();
            _sleepScore = (int?)sleepScoreCmd.ExecuteScalar();
            _sleepDate = (DateTime)sleepDateCmd.ExecuteScalar();
        }

        public int RemoveWearableData(NpgsqlConnection activeConnection)
        {
            var removeWearableData = "DELETE FROM wearable_data WHERE id=" + id;

            using var cmd = new NpgsqlCommand(removeWearableData, activeConnection);

            return cmd.ExecuteNonQuery();
        }

        public static int RemoveWearableData(NpgsqlConnection activeConnection, int remId)
        {
            var removeWearableData = "DELETE FROM wearable_data WHERE id=" + remId;

            using var cmd = new NpgsqlCommand(removeWearableData, activeConnection);

            return cmd.ExecuteNonQuery();
        }
    }
}