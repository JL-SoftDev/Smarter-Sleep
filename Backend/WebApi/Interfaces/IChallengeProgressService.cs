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
            public String ChallengeName { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? ExpireDate { get; set; }
            public bool UserSelected { get; set; }
            public double CompletionPercentage { get; set; }
            public int Completed { get; set; }
            public int Goal { get; set; }
        }

        Task<IEnumerable<ChallengeReturn>> GetChallengeProgress(Guid userId, DateTime? dateTime); 
    }
}