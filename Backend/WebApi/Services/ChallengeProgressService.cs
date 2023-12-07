using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public class ChallengeProgressService : IChallengeProgressService
    {
        public Task<IEnumerable<IChallengeProgressService.ChallengeReturn>> GetChallengeProgress(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}