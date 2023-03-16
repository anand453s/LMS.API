using LMS.Repository.Entities;
using LMS.Repository.Interfaces;
using LMS.Service.Interfaces;
using LMS.Shared.RequestModel;
using LMS.Shared.ResponseModel;

namespace LMS.Service.Services
{
    public class CourseMaterialService : ICourseMaterialService
    {
        int i = 0;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseMaterialRepository _courseMaterialRepository;

        public CourseMaterialService(ICourseRepository courseRepository, ICourseMaterialRepository courseMaterialRepository)
        {
            _courseRepository = courseRepository;
            _courseMaterialRepository = courseMaterialRepository;
        }

        public async Task<ResponseModel<string>> AddCourseMaterial(CourseMaterialRequest courseMaterialRequest)
        {
            var response = new ResponseModel<string>();
            var course = await _courseRepository.GetCourseById(courseMaterialRequest.CourseId);
            if(course.IsPublish == true)
            {
                CourseMaterial newCourseMaterial = new CourseMaterial();
                newCourseMaterial.Id = Guid.NewGuid();
                newCourseMaterial.CourseId = courseMaterialRequest.CourseId;
                if(courseMaterialRequest.MaterialName != null)
                    newCourseMaterial.MaterialName = courseMaterialRequest.MaterialName;
                if (courseMaterialRequest.MaterialDesc != null)
                    newCourseMaterial.MaterialDesc = courseMaterialRequest.MaterialDesc;
                if (courseMaterialRequest.File_Base64 != null && courseMaterialRequest.File_Base64 != "")
                {
                    var filePath = SaveFile(courseMaterialRequest.File_Base64);
                    if(filePath != String.Empty)
                    {
                        newCourseMaterial.FilePath = filePath;
                    }
                }
                newCourseMaterial.IsActive = true;
                newCourseMaterial.IsDeleted = false;
                newCourseMaterial.CreatedOn = DateTime.Now;
                newCourseMaterial.ModifyOn = DateTime.Now;
                i = await _courseMaterialRepository.AddNewCourseMaterial(newCourseMaterial);
                if (i > 0)
                {
                    response.IsSuccess = true;
                    response.Message = "Course Material added successfully.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Unable to added course material.";
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Course is not approved by Admin.";
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

        private string SaveFile(string file)
        {
            byte[] fileBytes = Convert.FromBase64String(file);
            string extension = GetFileExtension(file);
            if (extension == string.Empty)
            {
                extension = file.Split(';')[0].Split('/')[1];
            }
            var fileName = DateTime.Now.Ticks.ToString() + "." + extension;

            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\CourseMaterials");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string exactPath = Path.Combine(filePath, fileName);
                File.WriteAllBytes(exactPath, fileBytes);
                return exactPath;
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task<ResponseModel<List<CourseMaterialResponse>>> GetCourseMaterial(Guid courseId)
        {
            var response = new ResponseModel<List<CourseMaterialResponse>>();
            List<CourseMaterialResponse> courseMaterials = new List<CourseMaterialResponse>();
            var allCourseMaterials = await _courseMaterialRepository.GetCourseMaterialList();
            foreach (var item in allCourseMaterials.Where(x=>x.CourseId == courseId).ToList())
            {
                courseMaterials.Add( new CourseMaterialResponse()
                {
                    Id = item.Id,
                    CourseId = item.CourseId,
                    MaterialName = item.MaterialName,
                    MaterialDesc = item.MaterialDesc,
                    FilePath = item.FilePath
                });
            }
            if(courseMaterials.Count > 0)
            {
                response.IsSuccess = true;
                response.Message = "List of Materials.";
                response.Data = courseMaterials;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Material not uploaded by Instructor.";
            }
            return response;
        }
    }
}
