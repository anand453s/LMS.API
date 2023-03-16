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

        public async Task<int> AddUser(UserLogin newLogin)
        {
            _context.userLogins.Add(newLogin);  
           return await _context.SaveChangesAsync();  
        }

        public async Task<bool> CheckUserExist(string email)
        {
           return await _context.userLogins.AnyAsync(x => x.Email == email && x.IsDeleted == false);    
        }

        public async Task<List<RoleType>> GetAllUserRoles()
        {
            return await _context.roleTypes.Where(x => x.RoleName != "Admin").ToListAsync();
        }

        public async Task<string> GetRoleTypeByRoleId(int roleId)
        {
            var res = await _context.roleTypes.FindAsync(roleId);
            return res.RoleName;
        }

        public async Task<UserLogin> GetUserLoginDetails(string email)
        {
            return await _context.userLogins.Where(x => x.Email.ToLower() == email.ToLower()).SingleOrDefaultAsync();
        }
        public async Task<UserLogin> GetUserLoginDetails(Guid id)
        {
            return await _context.userLogins.Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        public async Task<bool> VarifyEmailPassword(string email, string password)
        {
            return await _context.userLogins.AnyAsync(x => x.Email.ToLower() == email.ToLower() && x.Password == password);

        }
    }
}
