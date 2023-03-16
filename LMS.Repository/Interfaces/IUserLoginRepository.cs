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
        Task<bool> CheckUserExist(string email);
        Task<bool> VarifyEmailPassword(string email, string password);
        Task<UserLogin> GetUserLoginDetails(string email);
        Task<UserLogin> GetUserLoginDetails(Guid loginId);
        Task<int> AddUser(UserLogin newLogin);
        Task<List<RoleType>> GetAllUserRoles();
        Task<string> GetRoleTypeByRoleId(int roleId);
    }
}
