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
            var isExists = await _studentCourseRepository.AnyCourseInTable(updateReq.CourseId);
            if (isExists)
            {
                response.IsSuccess = false;
                response.Message = "Unable to update course details some student already enrolled in this course.";
                return response; 
            }
            var course = await _courseRepository.GetCourseById(updateReq.CourseId);
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
                        response.Data = "success";
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

        public async Task<ResponseModel<List<CourseResponse>>> AllPublishedCourseList(RequestParameter reqParameter)
        {
            var response = new ResponseModel<List<CourseResponse>>();
            var allPublishedCourse = await _courseRepository.GetAllPublishedCourse();
            List<CourseResponse> courseResponseList = allPublishedCourse.Select(x => new CourseResponse
            {
                CourseId = x.Id,
                CourseName = x.CourseName,
                CourseDesc = x.CourseDesc,
                CourseCapacity = x.CourseCapacity,
                CreatedByID = x.CreatedBy,
                CreatedByName = x.Instructor.UserLogin.FullName,
                IsActive = x.IsActive,
                IsPublish = x.IsPublish,
                IsDeleted = x.IsDeleted,
                CreatedOn = x.CreatedOn,
                ModifyOn = x.ModifyOn
                
            }).ToList();
            if (!string.IsNullOrWhiteSpace(reqParameter.Search))
            {
                courseResponseList = courseResponseList.Where(x => x.CourseName.ToLower().Contains(reqParameter.Search.Trim().ToLower())).ToList();
            }
            courseResponseList = courseResponseList.OrderBy(on => on.CourseName)
                .Skip((reqParameter.PageNumber - 1) * reqParameter.PageSize)
                .Take(reqParameter.PageSize).ToList();
            if(courseResponseList.Count > 0)
            {
                response.IsSuccess = true;
                response.Message = $"Page {reqParameter.PageNumber} Course {courseResponseList.Count} of all Instructors.";
                response.Data = courseResponseList;
            }
            else
            {
                response.IsSuccess = true;
                response.Message = "No course found.";
            }
            return response;
        }

        public async Task<ResponseModel<List<CourseResponse>>> AllCourseOfInst(Guid instId, RequestParameter reqParameter)
        {
            var response = new ResponseModel<List<CourseResponse>>();
            var allCourseOfInst = await _courseRepository.GetAllCoursesOfInst(instId);
            List<CourseResponse> courseResponseList = allCourseOfInst.Select(x => new CourseResponse
            {
                CourseId = x.Id,
                CourseName = x.CourseName,
                CourseDesc = x.CourseDesc,
                CourseCapacity = x.CourseCapacity,
                CreatedByID = x.CreatedBy,
                CreatedByName = x.Instructor.UserLogin.FullName,
                IsActive = x.IsActive,
                IsPublish = x.IsPublish,
                IsDeleted = x.IsDeleted,
                CreatedOn = x.CreatedOn,
                ModifyOn = x.ModifyOn

            }).ToList();
            if (!string.IsNullOrWhiteSpace(reqParameter.Search))
            {
                courseResponseList = courseResponseList.Where(x => x.CourseName.ToLower().Contains(reqParameter.Search.Trim().ToLower())).ToList();
            }
            courseResponseList = courseResponseList.OrderBy(on => on.CourseName)
                .Skip((reqParameter.PageNumber - 1) * reqParameter.PageSize)
                .Take(reqParameter.PageSize).ToList();
            if(courseResponseList.Count > 0)
            {
                response.IsSuccess = true;
                response.Message = $"Page {reqParameter.PageNumber} Course {courseResponseList.Count} of All Course created by {courseResponseList[0].CreatedByName}";
                response.Data = courseResponseList;
            }
            else
            {
                response.IsSuccess = true;
                response.Message = "No course avilable.";
            }
            return response;
        }

        public async Task<ResponseModel<string>> DeleteCourse(Guid courseId)
        {
            var response = new ResponseModel<string>();
            var isExists = await _studentCourseRepository.AnyCourseInTable(courseId);
            if (isExists)
            {
                response.IsSuccess = false;
                response.Message = "Unable to delete course some student already enrolled in this course.";
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
                    response.Message = "Course delete Successfully.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Unable to delete course.";
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
            var course = await _courseRepository.GetCourseById(courseId);
            if(course != null)
            {
                response.IsSuccess = true;
                response.Message = "Success.";
                response.Data = new CourseResponse
                {
                    CourseId = course.Id,
                    CourseName = course.CourseName,
                    CourseDesc = course.CourseDesc,
                    CourseCapacity = course.CourseCapacity,
                    CreatedByID = course.CreatedBy,
                    CreatedByName = course.Instructor.UserLogin.FullName,
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
