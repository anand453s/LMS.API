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
    public class AdminService : IAdminService
    {
        int i = 0;
        private readonly ICourseRepository _courseRepository;
        private readonly IInstructorServices _instructorServices;
        private readonly IStudentRepository _studentRepository;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly IInstructorRepository _instructorRepository;

        public AdminService(ICourseRepository courseRepository, IInstructorServices instructorServices, IStudentRepository studentRepository, IUserLoginRepository userLoginRepository, IInstructorRepository instructorRepository)
        {
            _courseRepository = courseRepository;
            _instructorServices = instructorServices;
            _studentRepository = studentRepository;
            _userLoginRepository = userLoginRepository;
            _instructorRepository = instructorRepository;
        }
        public async Task<ResponseModel<List<CourseResponse>>> GetAllCourse(RequestParameter reqParameter)
        {
            var response = new ResponseModel<List<CourseResponse>>();
            var allCourseList = await _courseRepository.GetAllCourses();
            if (!string.IsNullOrWhiteSpace(reqParameter.Search))
            {
                allCourseList = allCourseList.Where(x => x.CourseName.ToLower().Contains(reqParameter.Search.Trim().ToLower())).ToList();
            }
            if (allCourseList != null)
            {
                var courseList = allCourseList.OrderBy(on => on.CourseName)
                    .Skip((reqParameter.PageNumber - 1) * reqParameter.PageSize).Take(reqParameter.PageSize).ToList();
                response.IsSuccess = true;
                response.Message = $"Page {reqParameter.PageNumber} Course {allCourseList.Count} of All Course Created by Instructors.";
                response.Data = courseList.Select(x => new CourseResponse
                {
                    CourseId = x.Id,
                    CourseName = x.CourseName,
                    CourseDesc = x.CourseDesc,
                    CourseCapacity = x.CourseCapacity,
                    CreatedByID = x.CreatedBy,
                    CreatedByName = x.Instructor.UserLogin.FullName,
                    IsPublish = x.IsPublish,
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted,
                    CreatedOn = x.CreatedOn,
                    ModifyOn = x.ModifyOn
                }).ToList();
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
                if(course.IsDeleted == false)
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
                    response.Message = "Cannot publish this course, It is already deleated.";
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Course Id not match to any Course.";
            }
            return response;
        }

        public async Task<ResponseModel<string>> BlockCourse(Guid courseId)
        {
            var response = new ResponseModel<string>();
            var course = await _courseRepository.GetCourseById(courseId);
            if (course != null)
            {
                if (course.IsDeleted == false)
                {
                    course.IsActive = false;
                    course.IsPublish = false;
                    i = await _courseRepository.UpdateCourse(course);
                    if (i > 0)
                    {
                        response.IsSuccess = true;
                        response.Message = "Course Blocked Successfully.";
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Failed to block course.";
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Cannot block this course. It is already deleated.";
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Course Id not match to any Course.";
            }
            return response;
        }

        public async Task<ResponseModel<List<StudentDetailsResponse>>> GetAllStudents(RequestParameter reqParameter)
        {
            var response = new ResponseModel<List<StudentDetailsResponse>>();
            var allStudents = await FullDetailOfStudents();
            if (!string.IsNullOrWhiteSpace(reqParameter.Search))
            {
                allStudents = allStudents.Where(x => x.FullName.ToLower().Contains(reqParameter.Search.Trim().ToLower())).ToList();
            }
            var studentList = allStudents.OrderBy(on => on.FullName).Skip((reqParameter.PageNumber - 1) * reqParameter.PageSize)
                .Take(reqParameter.PageSize).ToList();
            if (studentList.Any())
            {
                response.IsSuccess = true;
                response.Message = "Success";
                response.Data = studentList;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "No record Found.";
            }
            return response;
        }

        private async Task<List<StudentDetailsResponse>> FullDetailOfStudents()
        {
            List<StudentDetailsResponse> studentDetails = new List<StudentDetailsResponse>();
            var allStudents = await _studentRepository.GetAllStudents();
            if (allStudents.Count > 0)
            {
                var allUsers = await _userLoginRepository.GetAllUsers();
                foreach (var student in allStudents)
                {
                    var user = allUsers.Find(x => x.Id == student.LoginId);
                    studentDetails.Add(
                        new StudentDetailsResponse()
                        {
                            UserId = student.LoginId,
                            StudentId = student.Id,
                            FullName = user.FullName,
                            Email= user.Email,
                            Mobile = student.Mobile,
                            Gender = student.Gender,
                            ProfilePicPath = student.ProfilePicPath,
                            College = student.College,
                            Address = student.Address
                        });
                }
            }
            return studentDetails;
        }

        public async Task<ResponseModel<List<InstructorDetailsResponse>>> GetAllInstructors(RequestParameter reqParameter)
        {
            var response = new ResponseModel<List<InstructorDetailsResponse>>();
            var allInstructors = await FullDetailOfInstructors();
            if (!string.IsNullOrWhiteSpace(reqParameter.Search))
            {
                allInstructors = allInstructors.Where(x => x.FullName.ToLower().Contains(reqParameter.Search.Trim().ToLower())).ToList();
            }
            var instructorList = allInstructors.OrderBy(on => on.FullName).Skip((reqParameter.PageNumber - 1) * reqParameter.PageSize)
                .Take(reqParameter.PageSize).ToList();
            if (instructorList.Any())
            {
                response.IsSuccess = true;
                response.Message = "Success";
                response.Data = instructorList;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "No record Found.";
            }
            return response;
        }

        private async Task<List<InstructorDetailsResponse>> FullDetailOfInstructors()
        {
            List<InstructorDetailsResponse> instructorDetails = new List<InstructorDetailsResponse>();
            var allInstructors = await _instructorRepository.GetAllInstructors();
            if (allInstructors.Count > 0)
            {
                var allUsers = await _userLoginRepository.GetAllUsers();
                foreach (var instructor in allInstructors)
                {
                    var user = allUsers.Find(x => x.Id == instructor.LoginId);
                    instructorDetails.Add(
                        new InstructorDetailsResponse()
                        {
                            UserId = instructor.LoginId,
                            InstructorId = instructor.Id,
                            FullName = user.FullName,
                            Email = user.Email,
                            Mobile = instructor.Mobile,
                            Gender = instructor.Gender,
                            ProfilePicPath = instructor.ProfilePicPath,
                            Experience = instructor.Experience,
                            Specialization = instructor.Specialization
                        });
                }
            }
            return instructorDetails;
        }
    }
}
