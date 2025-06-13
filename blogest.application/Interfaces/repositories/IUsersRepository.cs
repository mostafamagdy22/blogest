using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogest.application.Interfaces.repositories
{
    public interface IUsersRepository
    {
        public Task<bool> IsEmailExit(string email);
        public Task<bool> IsUserNameExit(string userName);
    }
}