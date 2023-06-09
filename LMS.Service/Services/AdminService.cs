﻿using LMS.Repository.Interfaces;
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
        private readonly IStudentRepository _studentRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly IUserLoginRepository _userLoginRepository;

        public AdminService(ICourseRepository courseRepository, IStudentRepository studentRepository,IInstructorRepository instructorRepository, IUserLoginRepository userLoginRepository)
        {
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _instructorRepository = instructorRepository;
            _userLoginRepository = userLoginRepository;
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


        public async Task<ResponseModel<List<StudentDetailsResponse>>> GetAllStudents(RequestParameter reqParameter)
        {
            var response = new ResponseModel<List<StudentDetailsResponse>>();
            var allStudentList = await _studentRepository.GetAllStudents();
            if(allStudentList.Count > 0)
            {
                foreach (var std in allStudentList)
                {
                    String pic = "";
                    if (std.ProfilePic != null && std.ProfilePic != "")
                    {
                        Byte[] bytes = File.ReadAllBytes(std.ProfilePic);
                        pic = Convert.ToBase64String(bytes);
                        std.ProfilePic = pic;
                    }
                }
                var allStudents = allStudentList.Select(x => new StudentDetailsResponse
                {
                    UserId = x.LoginId,
                    StudentId = x.Id,
                    FullName = x.UserLogin.FullName,
                    Email = x.UserLogin.Email,
                    Mobile = x.Mobile,
                    Gender = x.Gender,
                    ProfilePic = x.ProfilePic,
                    College = x.College,
                    Address = x.Address,
                    IsActive = x.UserLogin.IsActive,
                    IsDeleted = x.IsDeleted
                }).ToList();

                if (!string.IsNullOrWhiteSpace(reqParameter.Search))
                {
                    allStudents = allStudents.Where(x => x.FullName.ToLower().Contains(reqParameter.Search.Trim().ToLower())).ToList();
                }

                allStudents = allStudents.OrderBy(on => on.FullName)
                    .Skip((reqParameter.PageNumber - 1) * reqParameter.PageSize)
                    .Take(reqParameter.PageSize).ToList();
            
                response.IsSuccess = true;
                response.Message = "Success";
                response.Data = allStudents;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "No record Found.";
            }
            return response;
        }

        public async Task<ResponseModel<List<InstructorDetailsResponse>>> GetAllInstructors(RequestParameter reqParameter)
        {
            var response = new ResponseModel<List<InstructorDetailsResponse>>();
            var allInstructorList = await _instructorRepository.GetAllInstructors();
            if (allInstructorList.Count > 0)
            {
                foreach (var inst in allInstructorList)
                {
                    String pic = "";
                    if (inst.ProfilePic != null && inst.ProfilePic != "")
                    {
                        Byte[] bytes = File.ReadAllBytes(inst.ProfilePic);
                        pic = Convert.ToBase64String(bytes);
                        inst.ProfilePic = pic;
                    }
                }
                var allInstructors = allInstructorList.Select(x => new InstructorDetailsResponse
                {
                    UserId = x.LoginId,
                    InstructorId = x.Id,
                    FullName = x.UserLogin.FullName,
                    Email = x.UserLogin.Email,
                    Mobile = x.Mobile,
                    Gender = x.Gender,
                    ProfilePic = x.ProfilePic,
                    Experience = x.Experience,
                    Specialization = x.Specialization,
                    IsActive = x.UserLogin.IsActive,
                    IsDeleted = x.IsDeleted

                }).ToList();

                if (!string.IsNullOrWhiteSpace(reqParameter.Search))
                {
                    allInstructors = allInstructors.Where(x => x.FullName.ToLower().Contains(reqParameter.Search.Trim().ToLower())).ToList();
                }

                allInstructors = allInstructors.OrderBy(on => on.FullName)
                    .Skip((reqParameter.PageNumber - 1) * reqParameter.PageSize)
                    .Take(reqParameter.PageSize).ToList();

                response.IsSuccess = true;
                response.Message = "Success";
                response.Data = allInstructors;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "No record Found.";
            }
            return response;
        }

        public async Task<ResponseModel<string>> ToggleBlockUser(Guid userId)
        {
            var response = new ResponseModel<string>();
            var user = await _userLoginRepository.GetUserById(userId);
            if (user != null)
            {
                if(user.IsDeleted == false)
                {
                    var isBlocked = user.IsActive;
                    user.IsActive = !user.IsActive;
                    user.ModifyOn = DateTime.Now;
                    i = await _userLoginRepository.UpdateUserLogin(user);
                    if (i > 0)
                    {
                        response.IsSuccess = true;
                        if (isBlocked)
                        {
                            response.Message = "User Blocked SuccessFully.";
                        }
                        else
                        {
                            response.Message = "User Unbocked SuccessFully.";
                        }
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Unable to block user.";
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Cannot block this User. It is already deleted.";
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "User Not Found.";
            }
            return response;
        }
    }
}
