using LMS.Repository.Entities;
using LMS.Repository.Interfaces;
using LMS.Service.Interfaces;
using LMS.Shared.RequestModel;
using LMS.Shared.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.Services
{
    public class CourseServices : ICourseServices
    {
        int i = 0;
        private readonly ICourseRepository _courseRepository;
        private readonly IInstructorServices _instructorServices;

        public CourseServices(ICourseRepository courseRepository, IInstructorServices instructorServices)
        {
            _courseRepository = courseRepository;
            _instructorServices = instructorServices;
        }

        public async Task<ResponseModel<CourseResponse>> AddCourse(CourseRequest courseReq)
        {
            Course newCourse = new Course
            {

                Id = Guid.NewGuid(),
                CourseName = courseReq.CourseName,
                CourseDesc = courseReq.CourseDesc,
                CreatedBy = courseReq.CreatedBy_InstId,
                CourseCapacity = courseReq.CourseCapacity,
                IsPublish = false,
                IsActive = true,
                IsDeleted = false,
                CreatedOn = DateTime.Now,
                ModifyOn = DateTime.Now,
            };
            i = await _courseRepository.AddNewCourse(newCourse);
            if (i > 0)
                return new ResponseModel<CourseResponse>
                {
                    IsSuccess = true,
                    Message = "Course Added Successfully!",
                    Data = new CourseResponse
                    {
                        CourseName = newCourse.CourseName,
                        CourseDesc = newCourse.CourseDesc,
                        CourseCapacity = newCourse.CourseCapacity,
                        IsActive = newCourse.IsActive
                    }
                };
            else
                return new ResponseModel<CourseResponse>
                {
                    IsSuccess = false,
                    Message = "Failed to Add Course."
                };
        }

        public async Task<ResponseModel<List<CourseResponse>>> AllPublishedCourseList()
        {
            var courseList = await _courseRepository.GetAllCourses();
            List<CourseResponse> courseResponseList = new List<CourseResponse>();
            foreach (var course in courseList)
            {
                if(course.IsPublish == true && course.IsActive == true && course.IsDeleted == false)
                {
                    var createrDetails = await _instructorServices.GetInstructorByInstId(course.CreatedBy);
                    courseResponseList.Add(
                    new CourseResponse()
                    {
                        CourseId = course.Id,
                        CourseName = course.CourseName,
                        CourseDesc= course.CourseDesc,
                        CourseCapacity = course.CourseCapacity,
                        CreatedByID = createrDetails.InstructorId,
                        CreatedByName = createrDetails.FullName,
                        CreatedOn = course.CreatedOn,
                        ModifyOn = course.ModifyOn
                    });
                }
            }
            return new ResponseModel<List<CourseResponse>>
            {
                IsSuccess = true,
                Message = "List of all Course from all Instructors.",
                Data = courseResponseList
            };
        }

        public async Task<ResponseModel<List<CourseResponse>>> AllCourseOfInst(Guid instId)
        {
            var courseList = await _courseRepository.GetAllCourses();
            List<CourseResponse> courseResponseList = new List<CourseResponse>();
            foreach (var course in courseList)
            {
                if(course.CreatedBy == instId)
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
            }
            return new ResponseModel<List<CourseResponse>>
            {
                IsSuccess = true,
                Message = $"List of All Course created by {courseResponseList[0].CreatedByName}",
                Data = courseResponseList
            };
        }
    }
}
