using LMS.Repository.Entities;
using LMS.Repository.Interfaces;
using LMS.Service.Interfaces;
using LMS.Shared.RequestModel;
using LMS.Shared.ResponseModel;

namespace LMS.Service.Services
{
    public class StudentCourseServices : IStudentCourseServices
    {
        int i = 0;
        private readonly IStudentCourseRepository _studentCourseRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IInstructorServices _instructorServices;

        public StudentCourseServices(IStudentCourseRepository studentCourseRepository, ICourseRepository courseRepository, IInstructorServices instructorServices)
        {
            _studentCourseRepository = studentCourseRepository;
            _courseRepository = courseRepository;
            _instructorServices = instructorServices;
        }

        public async Task<ResponseModel<List<CourseResponse>>> GetAllEnrollCourse(Guid stdId, RequestParameter reqParameter)
        {
            var response = new ResponseModel<List<CourseResponse>>();
            var stdCourseList = await _studentCourseRepository.GetEnrolledCourseOfStudent(stdId);
            stdCourseList = stdCourseList.Skip((reqParameter.PageNumber - 1) * reqParameter.PageSize)
                .Take(reqParameter.PageSize).ToList();

            List<CourseResponse> coursesDetailsList = stdCourseList.Select(x => new CourseResponse
            {
                CourseId = x.CourseId,
                CourseName = x.Course.CourseName,
                CourseDesc = x.Course.CourseDesc,
                CourseCapacity = x.Course.CourseCapacity,
                CreatedByID = x.Course.CreatedBy,
                CreatedByName = x.Course.Instructor.UserLogin.FullName,
                IsActive = x.IsActive,
                IsPublish = x.Course.IsPublish,
                IsDeleted = x.Course.IsDeleted,
                CreatedOn = x.CreatedOn,
                ModifyOn = x.ModifyOn
            }).ToList();
            if(coursesDetailsList.Count > 0)
            {
                response.IsSuccess = true;
                response.Message = "List of enrolled course details.";
                response.Data = coursesDetailsList;
            }
            else if(coursesDetailsList.Count == 0)
            {
                response.IsSuccess = true;
                response.Message = "Not enrolled in any course.";
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Something went wrong.";
            }
            return response;
        }

        public async Task<ResponseModel<string>> EnrollInCourse(StudentCourseRequest stdCourseRequest)
        {
            var response = new ResponseModel<string>();
            var isExists = await _studentCourseRepository.IsStdEnrolledInCourse(stdCourseRequest.StudentId, stdCourseRequest.CourseId);
            var course = await _courseRepository.GetCourseById(stdCourseRequest.CourseId);
            var totalEntolled = await _studentCourseRepository.TotalEnrolled(stdCourseRequest.CourseId);
            if (isExists)
            {
                response.IsSuccess = true;
                response.Message = "Already Enrolled in this course.";
            }else if (totalEntolled >= course.CourseCapacity)
            {
                response.IsSuccess = true;
                response.Message = "Course Capacity is full.";
            }
            else
            {
                StudentCourse newStdCourse = new StudentCourse
                {
                    Id = Guid.NewGuid(),
                    StudentId = stdCourseRequest.StudentId,
                    CourseId = stdCourseRequest.CourseId,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyOn = DateTime.Now,
                };
                i = await _studentCourseRepository.AddStudentCourse(newStdCourse);
                if (i > 0)
                {
                    response.IsSuccess = true;
                    response.Message = "Successfully enrolled in course.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Failed to  enrolled in course. Try again.";
                }
            }
            return response;
        }
    }
}
