using LMS.Repository.Entities;
using LMS.Service.Interfaces;
using LMS.Shared.RequestModel;
using LMS.Shared.ResponseModel;
using LMS.Repository.Interfaces;

namespace LMS.Service.Services
{
    public class InstructorServices : IInstructorServices
    {
        int i = 0;
        private readonly IInstructorRepository _instructorRepository;

        public InstructorServices(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }


        public async Task<int> AddInstructor(Guid loginId)
        {
            InstructorDetails newInst = new InstructorDetails
            {
                Id = Guid.NewGuid(),
                LoginId = loginId,
                Mobile = 0,
                Experience = 0,
                IsActive = true,
                IsDeleted = false,
                CreatedOn = DateTime.Now,
                ModifyOn = DateTime.Now,
            };
            i = await _instructorRepository.AddNewInstructor(newInst);
            return i;
        }

        public async Task<ResponseModel<InstructorDetailsResponse>> UpdateInstructor(InstructorDetailsRequest updateReq)
        {
            var response = new ResponseModel<InstructorDetailsResponse>();
            var instructor = await _instructorRepository.GetInstructorByLoginId(updateReq.UserId);
            if(instructor != null)
            {
                if (updateReq.Mobile != 0)
                {
                    instructor.Mobile = updateReq.Mobile;
                }
                if (updateReq.Gender != null && updateReq.Gender != "")
                {
                    instructor.Gender = updateReq.Gender;
                }
                if (updateReq.Specialization != null && updateReq.Specialization != "")
                {
                    instructor.Specialization = updateReq.Specialization;
                }
                if (updateReq.Experience >= 0)
                {
                    instructor.Experience = updateReq.Experience;
                }
                if (updateReq.ProfilePic != null && updateReq.ProfilePic != "")
                {
                    var imageSavePath = SaveImage(updateReq.ProfilePic);
                    instructor.ProfilePic = imageSavePath;
                }
                instructor.ModifyOn = DateTime.Now;
                i = await _instructorRepository.UpdateInstructor(instructor);
                if (i > 0)
                {
                    response.IsSuccess = true;
                    response.Message = "Instructor Details Updated Successfully";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Unabled to update Instructor Details.";
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Instructor not found.";
            }
            return response;
        }

        public async Task<InstructorDetailsResponse> GetInstructorByInstId(Guid instId)
        {
            var allInstructors = await _instructorRepository.GetAllInstructors();
            var isExists = allInstructors.Any(x => x.Id == instId);
            if (isExists)
            {
                var instructorLoginId = allInstructors.Where(x => x.Id == instId).First().LoginId;
                var result = await GetInstructorByLoginId(instructorLoginId);
                return result.Data;
            }
            else
            {
                return new InstructorDetailsResponse();
            }
        }

        public async Task<ResponseModel<InstructorDetailsResponse>> GetInstructorByLoginId(Guid userId)
        {
            var response = new ResponseModel<InstructorDetailsResponse>();
            var instructors = await _instructorRepository.GetInstructorByLoginId(userId);
            if(instructors != null)
            {
                String file = "";
                if (instructors.ProfilePic != null)
                {
                    Byte[] bytes = File.ReadAllBytes(instructors.ProfilePic);
                    file = Convert.ToBase64String(bytes);
                }
                response.IsSuccess = true;
                response.Message = "Success";
                response.Data = new InstructorDetailsResponse
                {
                    UserId = instructors.LoginId,
                    InstructorId = instructors.Id,
                    FullName = instructors.UserLogin.FullName,
                    Email = instructors.UserLogin.Email,
                    ProfilePic = file,
                    Mobile = instructors.Mobile,
                    Gender = instructors.Gender,
                    Specialization = instructors.Specialization,
                    Experience = instructors.Experience,
                    IsActive = instructors.IsActive,
                    IsDeleted = instructors.IsDeleted
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
            if (extension != string.Empty)
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
