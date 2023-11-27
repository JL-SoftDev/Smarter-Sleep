using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Data
{
    public class WearableDataList
    {
        public readonly List<string> hypnograms = new List<string>();
        public readonly List<int> scores = new List<int>();

        public WearableDataList()
        {
            hypnograms.Add("444444444222222211111111332222222221111113333222222222211111333333222222222222111133333333331144");
            scores.Add(55);

            hypnograms.Add("444444222211111111332222111111133322222111111333322222211111333332222222111133333322222222111244");
            scores.Add(67);

            hypnograms.Add("444422221111111133222211111113332222211111133332222221111133333222222211113333332222222211112444");
            scores.Add(82);

            hypnograms.Add("442222111111113322221111111333222221111113333222222111113333322222221111333322222222111133322224");
            scores.Add(88);

            hypnograms.Add("422221111111133222211111113332222211111133332222221111133333222222211113333222222221111333222224");
            scores.Add(94);
        }
    }
}