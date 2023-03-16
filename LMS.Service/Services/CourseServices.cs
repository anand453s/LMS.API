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
        private readonly IStudentCourseRepository _studentCourseRepository;
        private readonly IInstructorServices _instructorServices;

        public CourseServices(ICourseRepository courseRepository, IStudentCourseRepository studentCourseRepository, IInstructorServices instructorServices)
        {
            _courseRepository = courseRepository;
            _studentCourseRepository = studentCourseRepository;
            _instructorServices = instructorServices;
        }

        public async Task<ResponseModel<string>> AddCourse(CourseRequest courseReq)
        {
            Course newCourse = new Course();
            newCourse.Id = Guid.NewGuid();
            newCourse.CreatedBy = courseReq.CreatedBy_InstId;
            newCourse.CourseName = courseReq.CourseName;
            newCourse.CourseCapacity = courseReq.CourseCapacity;
            if (courseReq.CourseDesc != null)
            {
                newCourse.CourseDesc = courseReq.CourseDesc;
            }
            else
            {
                newCourse.CourseDesc = "";
            }
            newCourse.IsPublish = false;
            newCourse.IsActive = true;
            newCourse.IsDeleted = false;
            newCourse.CreatedOn = DateTime.Now;
            newCourse.ModifyOn = DateTime.Now;

            i = await _courseRepository.AddNewCourse(newCourse);
            if (i > 0)
                return new ResponseModel<string>
                {
                    IsSuccess = true,
                    Message = "Course Added Successfully!",
                };
            else
                return new ResponseModel<string>
                {
                    IsSuccess = false,
                    Message = "Failed to Add Course."
                };
        }

        public async Task<ResponseModel<string>> UpdateCourse(CourseRequest updateReq)
        {
            var response = new ResponseModel<string>();
            var c = await _studentCourseRepository.GetAllCourses();
            if(!c.Any(x => x.CourseId == updateReq.CourseId))
            {
                Course updatedCourse = new Course();
                if(updateReq.CourseName != null)
                    updatedCourse.CourseName = updateReq.CourseName;
                if (updatedCourse.CourseCapacity != updateReq.CourseCapacity)
                    updatedCourse.CourseCapacity = updateReq.CourseCapacity;
                if (updateReq.CourseDesc != null)
                    updatedCourse.CourseDesc = updateReq.CourseDesc;
                updatedCourse.ModifyOn = DateTime.Now;

                i = await _courseRepository.AddNewCourse(updatedCourse);
                if (i > 0)
                {
                    response.IsSuccess = true;
                    response.Message = "Course Added Successfully!";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Failed to Update Course.";
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Some student already enrolled in this course";
            }
            return response;
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
            var response = new ResponseModel<List<CourseResponse>>();
            var allCourseList = await _courseRepository.GetAllCourses();
            var courseList = allCourseList.Where(x => x.CreatedBy == instId).ToList();
            if(courseList.Count > 0)
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
                response.Message = $"List of All Course created by {courseResponseList[0].CreatedByName}";
                response.Data = courseResponseList;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "No course avilable.";
            }
            return response;
        }

    }
}
