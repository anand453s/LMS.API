using LMS.Repository.Interfaces;
using LMS.Service.Interfaces;
using LMS.Shared.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.Services
{
    public class AdminService : IAdminService
    {
        int i = 0;
        private readonly ICourseRepository _courseRepository;
        private readonly IInstructorServices _instructorServices;

        public AdminService(ICourseRepository courseRepository, IInstructorServices instructorServices)
        {
            _courseRepository = courseRepository;
            _instructorServices = instructorServices;
        }
        public async Task<ResponseModel<List<CourseResponse>>> GetAllCourse()
        {
            var response = new ResponseModel<List<CourseResponse>>();
            var courseList = await _courseRepository.GetAllCourses();
            if(courseList != null)
            {
                List<CourseResponse> courseResponseList = new List<CourseResponse>();
                foreach (var course in courseList)
                {
                    var createrDetails = await _instructorServices.GetInstructorByInstId(course.CreatedBy);
                    courseResponseList.Add(
                    new CourseResponse()
                    {
                        CourseId = course.Id,
                        CourseName = course.CourseName,
                        CourseDesc = course.CourseDesc,
                        CourseCapacity = course.CourseCapacity,
                        CreatedByID = createrDetails.InstructorId,
                        CreatedByName = createrDetails.FullName,
                        IsPublish = course.IsPublish,
                        IsActive = course.IsActive,
                        IsDeleted = course.IsDeleted,
                        CreatedOn = course.CreatedOn,
                        ModifyOn = course.ModifyOn
                    });
                }
                response.IsSuccess = true;
                response.Message = "List of all Course from all Instructors.";
                response.Data = courseResponseList;
            }
            else
            {
                response.IsSuccess = true;
                response.Message = "No Course Avilable.";
            }
            return response;
        }

        public async Task<ResponseModel<string>> PublishCourse(Guid courseId)
        {
            var response = new ResponseModel<string>();
            var course = await _courseRepository.GetCourseById(courseId);
            if(course != null)
            {
                course.IsPublish = true;
                i = await _courseRepository.UpdateCourse(course);
                if (i > 0)
                {
                    response.IsSuccess = true;
                    response.Message = "Course is Published.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Course not published";
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Course Id not match to any Course.";
            }
            return response;
        }
    }
}
