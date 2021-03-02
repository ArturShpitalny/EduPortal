using EducationPortal.BLL.Services;
using EducationPortal.Core.Models.Entities;
using EducationPortal.Core.Models.States;
using EducationPortal.DAL.Repository;
using EducationPortal.WEB.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace EducationPortal.WEB.Controllers
{
    public class UserCourseController : Controller
    {
        public readonly UserCourseService userCourseService;
        private readonly int pageSize;

        public UserCourseController(UserCourseService userCourseService)
        {
            this.userCourseService = userCourseService;

            int.TryParse(WebConfigurationManager.AppSettings["ElementOnPage"], out pageSize);
        }

        //User Courses main page
        [Authorize(Roles = "Admin, User")]
        public ActionResult MainPageUserCourse(int page = 1)
        {
            return PartialView(this.userCourseService.GetUserCourse(User.Identity.GetUserId<int>(), pageSize, page));
        }

        //Adds a course to the user.
        [HttpPost]        
        public ActionResult AddCourse(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                ResponseState state = this.userCourseService.AddCourse(User.Identity.GetUserId<int>(), id);

                if (state.State == true && state.Massage == "OK")
                {
                    return Json(new ResultModel { Index = id, Message = state.Massage });
                }
                else if (state.State == false && state.Massage == "CourseAlreadyAdded")
                {
                    return Json(new ResultModel { Index = id, Message = state.Massage });
                }
                else if(state.State == false && state.Massage == "UserNotFound")
                {
                    return Json(new ResultModel { Index = id, Message = state.Massage });
                }

                return Json(new ResultModel { Index = id, Message = "ERR" }); 
            }
            return Json(new ResultModel { Index = id, Message = "UserNotAuthorize" });
        }

        //Deletes a user’s course
        [HttpPost]
        public ActionResult RemoveCourse(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                ResponseState state = this.userCourseService.RemoveCourse(User.Identity.GetUserId<int>(), id);

                if (state.State == true && state.Massage == "OK")
                {
                    return Json(new ResultModel { Index = id, Message = state.Massage });
                }
                else if (state.State == false && state.Massage == "CourseIsAbsent")
                {
                    return Json(new ResultModel { Index = id, Message = state.Massage });
                }
                else if (state.State == false && state.Massage == "UserNotFound")
                {
                    return Json(new ResultModel { Index = id, Message = state.Massage });
                }

                return Json(new ResultModel { Index = id, Message = "ERR" }); 
            }
            return Json(new ResultModel { Index = id, Message = "UserNotAuthorize" });
        }

        //Course Completion Event
        [Authorize(Roles = "Admin, User")]
        public ActionResult UserCourseEnd(int courseId)
        {
            this.userCourseService.UserCourseUpdate(new UserCourse { Id = User.Identity.GetUserId<int>(), CourseId = courseId, IsComplete = true });

            return RedirectToAction("MainPageUserCourse");
        }
    }
}