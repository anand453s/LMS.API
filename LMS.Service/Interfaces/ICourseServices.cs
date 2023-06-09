﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Shared.RequestModel;
using LMS.Shared.ResponseModel;

namespace LMS.Service.Interfaces
{
    public interface ICourseServices
    {
        Task<ResponseModel<string>> AddCourse(CourseRequest courseReq);
        Task<ResponseModel<string>> UpdateCourse(CourseRequest courseReq);
        Task<ResponseModel<string>> DeleteCourse(Guid courseId);
        Task<ResponseModel<List<CourseResponse>>> AllPublishedCourseList(RequestParameter reqParameter);
        Task<ResponseModel<List<CourseResponse>>> AllCourseOfInst(Guid instId, RequestParameter reqParameter);
        Task<ResponseModel<CourseResponse>> GetCourseById(Guid courseId);
    }
}
