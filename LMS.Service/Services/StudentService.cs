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
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

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
            var student = await _studentRepository.GetStudentByLoginId(updateReq.UserId);
            if (student != null)
            {
                if (updateReq.Mobile != 0)
                {
                    student.Mobile = updateReq.Mobile;
                }
                if (updateReq.Gender != null && updateReq.Gender != "")
                {
                    student.Gender = updateReq.Gender;
                }
                if (updateReq.College != null && updateReq.College != "")
                {
                    student.College = updateReq.College;
                }
                if (updateReq.Address != null && updateReq.Address != "")
                {
                    student.Address = updateReq.Address;
                }
                if (updateReq.ProfilePic != null && updateReq.ProfilePic != "")
                {
                    var imageSavePath = SaveImage(updateReq.ProfilePic);
                    student.ProfilePicPath = imageSavePath;
                }
                student.ModifyOn = DateTime.Now;
                i = await _studentRepository.UpdateStudent(student);
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
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Student not found.";
            }
            return response;
        }

        public async Task<ResponseModel<StudentDetailsResponse>> GetStudentByLoginId(Guid userId)
        {
            var response = new ResponseModel<StudentDetailsResponse>();
            var student = await _studentRepository.GetStudentByLoginId(userId);
            if (student != null)
            {
                response.IsSuccess = true;
                response.Message = "Success";
                response.Data = new StudentDetailsResponse
                {
                    UserId = student.LoginId,
                    StudentId = student.Id,
                    FullName = student.UserLogin.FullName,
                    Email = student.UserLogin.Email,
                    ProfilePicPath = student.ProfilePicPath,
                    Mobile = student.Mobile,
                    Gender = student.Gender,
                    College = student.College,
                    Address = student.Address
                };
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "No record Found.";
            }
            return response;
        }

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
            byte[] imageBytes;
            string extension = GetFileExtension(file);
            if(extension != string.Empty)
            {
                imageBytes = Convert.FromBase64String(file);
            }
            else
            {
                string img = file.Split(',')[1];
                extension = GetFileExtension(img);
                imageBytes = Convert.FromBase64String(img);
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
