using LMS.Repository.EF.Context;
using LMS.Repository.Entities;
using LMS.Service.Interfaces;
using LMS.Shared.RequestModel;
using LMS.Shared.ResponseModel;
using LMS.Repository.Interfaces;

namespace LMS.Service.Services
{
    public class StudentService : IStudentService
    {
        int i = 0;
        private readonly IStudentRepository _studentRepository;
        private readonly IUserLoginRepository _userLoginRepository;
        public StudentService(IStudentRepository studentRepository, IUserLoginRepository userLoginRepository)
        {
            _studentRepository = studentRepository;
            _userLoginRepository = userLoginRepository;
        }

        //public async Task<ResponseModel<StudentDetailsResponse>> AddStudent(StudentDetailsRequest stdReq)
        //{
        //    var response = new ResponseModel<StudentDetailsResponse>();
        //    var studentDetails = await _studentRepository.GetStudentByLoginId(stdReq.LoginId);
        //    if(studentDetails != null)
        //    {
        //        response.IsSuccess = false;
        //        response.Message = "Student Id is already generated, Please go through Update process.";
        //        return response;
        //    }
        //    string imageSavePath = "";
        //    if(stdReq.ProfilePic != null)
        //    {
        //        imageSavePath = SaveImage(stdReq.ProfilePic);
        //    }
        //    StudentDetails newStd = new StudentDetails
        //    {
        //        Id = Guid.NewGuid(),
        //        LoginId = stdReq.LoginId,
        //        Mobile = stdReq.Mobile,
        //        ProfilePicPath = imageSavePath,
        //        Gender = stdReq.Gender,
        //        College = stdReq.College,
        //        Address = stdReq.Address,
        //        IsActive = true,
        //        IsDeleted = false,
        //        CreatedOn = DateTime.Now,
        //        ModifyOn = DateTime.Now,
        //    };
        //    i = await _studentRepository.AddNewStudent(newStd);
        //    if (i > 0)
        //    {
        //        response.IsSuccess = true;
        //        response.Message = "Student Details Saved Successfully!";
        //        response.Data = new StudentDetailsResponse
        //        {
        //            StudentId = newStd.Id,
        //            LoginId = newStd.LoginId,
        //            ProfilePicPath = newStd.ProfilePicPath,
        //            Mobile = newStd.Mobile,
        //            Gender = newStd.Gender,
        //            College = newStd.College,
        //            Address = newStd.Address
        //        };
        //    }
        //    else
        //    {
        //        response.IsSuccess = false;
        //        response.Message = "Failed to Save Student Details. Please try again.";
        //    }
        //    return response;
        //}

        public async Task<int> AddStudent(Guid loginId)
        {
            StudentDetails newStd = new StudentDetails
            {
                Id = Guid.NewGuid(),
                LoginId = loginId,
                Mobile = 0,
                IsActive = true,
                IsDeleted = false,
                CreatedOn = DateTime.Now,
                ModifyOn = DateTime.Now,
            };
            i = await _studentRepository.AddNewStudent(newStd);
            return i;
        }

        public async Task<ResponseModel<StudentDetailsResponse>> UpdateStudent(StudentDetailsRequest updateReq)
        {
            var response = new ResponseModel<StudentDetailsResponse>();
            var studentDetails = await _studentRepository.GetStudentByLoginId(updateReq.UserId);
            if(studentDetails != null)
            {
                if (updateReq.Mobile != 0)
                    studentDetails.Mobile = updateReq.Mobile;
                if (updateReq.Gender != null)
                    studentDetails.Gender = updateReq.Gender;
                if (updateReq.College != null)
                    studentDetails.College = updateReq.College;
                if (updateReq.Address != null)
                    studentDetails.Address = updateReq.Address;
                if (updateReq.ProfilePic != null && updateReq.ProfilePic != "")
                {
                    var imageSavePath = SaveImage(updateReq.ProfilePic);
                    studentDetails.ProfilePicPath = imageSavePath;
                }
                studentDetails.ModifyOn = DateTime.Now;
                i = await _studentRepository.UpdateStudent(studentDetails);
            }
            if (i > 0)
            {
                response.IsSuccess = true;
                response.Message = "Student Details Updated Successfully.";
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Unabled to update Student Details.";
            }
            return response;
        }

        public async Task<ResponseModel<StudentDetailsResponse>> GetStudentByLoginId(Guid loginId)
        {
            var response = new ResponseModel<StudentDetailsResponse>();
            var stdDetail = await _studentRepository.GetStudentByLoginId(loginId);
            if (stdDetail != null)
            {
                var loginDetail = await _userLoginRepository.GetUserLoginDetails(loginId);
                response.IsSuccess = true;
                response.Message = "Success";
                response.Data = new StudentDetailsResponse
                {
                    UserId = stdDetail.LoginId,
                    StudentId = stdDetail.Id,
                    FullName = loginDetail.FullName,
                    Email = loginDetail.Email,
                    ProfilePicPath = stdDetail.ProfilePicPath,
                    Mobile = stdDetail.Mobile,
                    Gender = stdDetail.Gender,
                    College = stdDetail.College,
                    Address = stdDetail.Address
                };
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "No record Found.";
            }
            return response;
        }

        //public ResponseModel<IEnumerable<Course>> MyCourseList(Guid loginId)
        //{
        //    return new ResponseModel<IEnumerable<Course>> { } ;
        //}

        private static string GetFileExtension(string base64String)
        {
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                case "AAAAF":
                    return "mp4";
                case "JVBER":
                    return "pdf";
                case "AAABA":
                    return "ico";
                case "UMFYI":
                    return "rar";
                case "E1XYD":
                    return "rtf";
                case "U1PKC":
                    return "txt";
                case "MQOWM":
                case "77U/M":
                    return "srt";
                default:
                    return string.Empty;
            }
        }

        private string SaveImage(string file)
        {
            byte[] imageBytes = Convert.FromBase64String(file);
            string extension = GetFileExtension(file);
            if (extension == string.Empty)
            {
                extension = file.Split(';')[0].Split('/')[1];
            }
            var fileName = DateTime.Now.Ticks.ToString() + "." + extension;

            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\ProfilePic");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string exactPath = Path.Combine(filePath, fileName);
                File.WriteAllBytes(exactPath, imageBytes);
                return exactPath;
            }
            catch
            {
                return string.Empty;
            }
        }

    }
}
