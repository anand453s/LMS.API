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
            var allStudentCourse = await _studentCourseRepository.GetAllStudentCourses();
            var anyEnrolled = allStudentCourse.Any(x => x.CourseId == updateReq.CourseId);
            if (!anyEnrolled)
            {
                response.IsSuccess = false;
                response.Message = "Some student already enrolled in this course.";
                return response;
            }
            var allCourse = await _courseRepository.GetAllCourses();
            var course = allCourse.Find(x => x.Id == updateReq.CourseId);
            if(course != null)
            {
                if (course.IsActive == true)
                {
                    if(updateReq.CourseName != null)
                    {
                        course.CourseName = updateReq.CourseName;
                    }
                    if (course.CourseCapacity != updateReq.CourseCapacity)
                    {
                        course.CourseCapacity = updateReq.CourseCapacity;
                    }
                    if (updateReq.CourseDesc != null)
                    {
                        course.CourseDesc = updateReq.CourseDesc;
                    }
                    course.ModifyOn = DateTime.Now;
                    course.IsPublish = false;
                    i = await _courseRepository.UpdateCourse(course);
                    if (i > 0)
                    {
                        response.IsSuccess = true;
                        response.Message = "Course Updated Successfully!";
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Unable to update this course is blocked by Admin.";
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Failed to Update Course.";
            }
            return response;
        }

        public async Task<ResponseModel<List<CourseResponse>>> AllPublishedCourseList()
        {
            var courseList = await _courseRepository.GetAllCourses();
            List<CourseResponse> courseResponseList = new List<CourseResponse>();
            foreach (var course in courseList)
            {
                if(course.IsPublish == true && course.IsDeleted == false)
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
            var courseList = allCourseList.Where(x => x.CreatedBy == instId).Where(x => x.IsDeleted == false).ToList();
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

        public async Task<ResponseModel<string>> DeleteCourse(Guid courseId)
        {
            var response = new ResponseModel<string>();
            var allStudentCourse = await _studentCourseRepository.GetAllStudentCourses();
            var anyEnrolled = allStudentCourse.Any(x => x.CourseId == courseId);
            if (anyEnrolled)
            {
                response.IsSuccess = false;
                response.Message = "Some student already enrolled in this course.";
                return response;
            }
            var course = await _courseRepository.GetCourseById(courseId);
            if (course != null)
            {
                course.IsPublish = false;
                course.IsActive = false;
                course.IsDeleted = true;
                i = await _courseRepository.UpdateCourse(course);
                if(i > 0)
                {
                    response.IsSuccess = true;
                    response.Message = "Course Deleated Successfully.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Course Not Deleated.";
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Course not found.";
            }
            return response;
        }

        public async Task<ResponseModel<CourseResponse>> GetCourseById(Guid courseId)
        {
            var response = new ResponseModel<CourseResponse>();
            var allCourseList = await _courseRepository.GetAllCourses();
            var course = allCourseList.Where(x => x.Id == courseId).FirstOrDefault();
            if(course != null)
            {
                var createrDetails = await _instructorServices.GetInstructorByInstId(course.CreatedBy);
                response.IsSuccess = true;
                response.Message = "Success.";
                response.Data = new CourseResponse
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
                };
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Course not found.";
            }
            return response;
        }
    }
}
