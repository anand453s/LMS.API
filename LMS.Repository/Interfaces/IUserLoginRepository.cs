using LMS.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Interfaces
{
    public interface IUserLoginRepository
    {
        Task<List<RoleType>> GetAllRoles();
        Task<string> GetRoleTypeByRoleId(int roleId);

        Task<int> AddUser(UserLogin newLogin);
        Task<List<UserLogin>> GetAllUsers();
        Task<UserLogin> GetUserLoginDetails(Guid loginId);
    }
}
