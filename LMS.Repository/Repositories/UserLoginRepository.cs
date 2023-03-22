using LMS.Repository.EF.Context;
using LMS.Repository.Entities;
using LMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repository.Repositories
{
    public class UserLoginRepository : BaseRepository<UserLoginRepository>, IUserLoginRepository
    {
        readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        int i = 0;
        public UserLoginRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _context = (AppDbContext)_unitOfWork.Db;
        }

        public async Task<List<RoleType>> GetAllRoles()
        {
            return await _context.roleTypes.ToListAsync();
        }

        public async Task<string> GetRoleTypeByRoleId(int roleId)
        {
            var res = await _context.roleTypes.FindAsync(roleId);
            return res.RoleName;
        }


        public async Task<List<UserLogin>> GetAllUsers()
        {
            return await _context.userLogins.ToListAsync();
        }

        public async Task<UserLogin> GetUserById(Guid userId)
        {
            return await _context.userLogins.Where(x => x.Id == userId).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateUserLogin(UserLogin userLogin)
        {
            _context.Update(userLogin);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddUser(UserLogin newLogin)
        {
            _context.userLogins.Add(newLogin);
            return await _context.SaveChangesAsync();
        }
    }
}
