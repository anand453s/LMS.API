using LMS.Repository.EF.Context;
using LMS.Repository.Entities;
using LMS.Service.Interfaces;
using LMS.Shared.RequestModel;
using LMS.Shared.ResponseModel;
using LMS.Repository.Interfaces;
using LMS.Repository.Repositories;

namespace LMS.Service.Services
{
    public class InstructorServices : IInstructorServices
    {
        int i = 0;
        private readonly IInstructorRepository _instructorRepository;
        private readonly IUserLoginRepository _userLoginRepository;

        public InstructorServices(IInstructorRepository instructorRepository, IUserLoginRepository userLoginRepository)
        {
            _instructorRepository = instructorRepository;
            _userLoginRepository = userLoginRepository;
        }

        public async Task<ResponseModel<InstructorDetailsResponse>> AddInstructor(InstructorDetailsRequest addReq)
        {
            var response = new ResponseModel<InstructorDetailsResponse>();
            var instructorDetails = await _instructorRepository.GetInstructorByLoginId(addReq.LoginId);
            if (instructorDetails != null)
            {
                response.IsSuccess = false;
                response.Message = "Instructor Id is already generated, Please go through Update process.";
                return response;
            }
            string imageSavePath = "";
            if (addReq.ProfilePic != null)
            {
                imageSavePath = SaveImage(addReq.ProfilePic);
            }
            InstructorDetails newInst = new InstructorDetails
            {
                Id = Guid.NewGuid(),
                LoginId = addReq.LoginId,
                Mobile = addReq.Mobile,
                ProfilePicPath = imageSavePath,
                Gender = addReq.Gender,
                Specialization = addReq.Specialization,
                Experience = addReq.Experience,
                IsActive = true,
                IsDeleted = false,
                CreatedOn = DateTime.Now,
                ModifyOn = DateTime.Now,
            };
            i = await _instructorRepository.AddNewInstructor(newInst);
            if (i > 0)
            {
                response.IsSuccess = true;
                response.Message = "Instructor Details Saved Successfully!";
                response.Data = new InstructorDetailsResponse
                {
                    InstructorId = newInst.Id,
                    LoginId = newInst.LoginId,
                    ProfilePicPath = newInst.ProfilePicPath,
                    Mobile = newInst.Mobile,
                    Gender = newInst.Gender,
                    Specialization = newInst.Specialization,
                    Experience = newInst.Experience
                };
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Failed to Save Instructor Details. Please try again.";
            }
            return response;
        }

        public async Task<ResponseModel<InstructorDetailsResponse>> UpdateInstructor(InstructorDetailsRequest updateReq)
        {
            var response = new ResponseModel<InstructorDetailsResponse>();
            var instructorDetails = await _instructorRepository.GetInstructorByLoginId(updateReq.LoginId);
            if(instructorDetails != null)
            {
                if(updateReq.Mobile != 0)
                    instructorDetails.Mobile = updateReq.Mobile;
                if(updateReq.Gender != null)
                    instructorDetails.Gender = updateReq.Gender;
                if(updateReq.Specialization != null)
                    instructorDetails.Specialization = updateReq.Specialization;
                if(updateReq.Experience >= 0)
                    instructorDetails.Experience = updateReq.Experience;
                if(updateReq.ProfilePic != null)
                {
                    var imageSavePath = SaveImage(updateReq.ProfilePic);
                    instructorDetails.ProfilePicPath = imageSavePath;
                }
                i = await _instructorRepository.UpdateInstructor(instructorDetails);
            }
            if(i > 0)
            {
                response.IsSuccess = true;
                response.Message = "Instructor Details Updated Successfully";
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Unabled to update Instructor Details.";
            }
            return response;
        }

        public async Task<InstructorDetailsResponse> GetInstructorByInstId(Guid instId)
        {
            var inst = await _instructorRepository.GetInstructorByInstId(instId);
            var x = await GetInstructorByLoginId(inst.LoginId);
            return x.Data;
        }

        public async Task<ResponseModel<InstructorDetailsResponse>> GetInstructorByLoginId(Guid loginId)
        {
            var response = new ResponseModel<InstructorDetailsResponse>();
            var instDetail = await _instructorRepository.GetInstructorByLoginId(loginId);
            if(instDetail != null)
            {
                var loginDetail = await _userLoginRepository.GetUserLoginDetails(loginId);
                response.IsSuccess = true;
                response.Message = "Success";
                response.Data = new InstructorDetailsResponse
                {
                    LoginId = instDetail.LoginId,
                    InstructorId = instDetail.Id,
                    FullName = loginDetail.FullName,
                    Email = loginDetail.Email,
                    ProfilePicPath = instDetail.ProfilePicPath,
                    Mobile = instDetail.Mobile,
                    Gender = instDetail.Gender,
                    Specialization = instDetail.Specialization,
                    Experience = instDetail.Experience
                };
            }
            else
            {
                response.IsSuccess = true;
                response.Message = "No details avilable, please update your profile.";
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
