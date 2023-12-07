using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Interfaces
{
    public interface IChallengeProgressService
    {
        public struct ChallengeReturn
        {
            String ChallengeName { get; set; }
            DateTime StartDate { get; set; }
            DateTime ExpireDate { get; set; }
            bool UserSelected { get; set; }
            double CompletionPercentage { get; set; }
            int Completed { get; set; }
            int Goal { get; set; }
        }

        Task<IEnumerable<ChallengeReturn>> GetChallengeProgress(Guid userId); 
    }
}