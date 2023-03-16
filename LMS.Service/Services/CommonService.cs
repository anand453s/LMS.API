//using LMS.Repository.EF.Context;
//using LMS.Repository.Entities;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;

//namespace LMS.Service.Services
//{
//    public class CommonService
//    {
//        int i = 0;
//        private readonly AppDbContext _dbContext;
//        public CommonService(AppDbContext dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        public RoleType GetRoleTypeById(int roleId)
//        {
//            return _dbContext.roleTypes.Find(roleId);
//        }

//        public bool UserExists(string email)
//        {
//            return _dbContext.userLogins.Where(x => (x.Email.ToLower() == email.ToLower() && x.IsDeleted == false)).Any();
//        }

//        public async Task<string> SaveImage(IFormFile image)
//        {
//            string exactPath = string.Empty;
//            try
//            {
//                var extension = Path.GetExtension(image.FileName);
//                var fileName = DateTime.Now.Ticks.ToString() + extension;
//                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\ProfilePic");
//                if (!Directory.Exists(filePath))
//                {
//                    Directory.CreateDirectory(filePath);
//                }
//                exactPath = Path.Combine(filePath, fileName);
//                using (var stream = new FileStream(exactPath, FileMode.Create))
//                {
//                    await image.CopyToAsync(stream);
//                }
//            }
//            catch
//            {
//                throw new Exception("File Not Saved.");
//            }

//            return exactPath;
//        }

//        //Get Instructor Full Details By Login Id / Instructor Id
//        public InstructorDetails GetInstructorById(Guid Id)
//        {
//            var res = _dbContext.instructors.Where(x => x.LoginId == Id).FirstOrDefault();
//            if(res == null)
//            {
//                res = _dbContext.instructors.Where(x => x.Id == Id).FirstOrDefault();
//            }
//            res.UserLogin = _dbContext.userLogins.Where(x => x.Id == res.LoginId).FirstOrDefault();
//            return res;
//        }

//        //Get Student Full Details By Login Id / Student Id
//        public StudentDetails GetStudentById(Guid Id)
//        {
//            var res = _dbContext.students.Where(x => x.LoginId == Id).FirstOrDefault();
//            if (res == null)
//            {
//                res = _dbContext.students.Where(x => x.Id == Id).FirstOrDefault();
//            }
//            res.UserLogin = _dbContext.userLogins.Where(x => x.Id == res.LoginId).FirstOrDefault();
//            return res;
//        }

//        //Get User Id from  their Role Id and Login Id
//        public Guid GetUserId(Guid loginId, int roleId)
//        {
//            if(roleId == 2)
//                return GetInstructorById(loginId).Id;
//            else if(roleId == 3)
//                return GetStudentById(loginId).Id;
//            else
//                return Guid.Empty;
//        }
//    }
//}
