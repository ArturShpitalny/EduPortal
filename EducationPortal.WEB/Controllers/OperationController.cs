using EducationPortal.BLL.Services;
using EducationPortal.Core.Models.States;
using EducationPortal.WEB.Models.ViewModel;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EducationPortal.WEB.Controllers
{
    [RoutePrefix("api/Operation")]
    public class OperationController : ApiController
    {
        private readonly UserCourseService userCourseService;

        public OperationController(UserCourseService userCourseService)
        {
            this.userCourseService = userCourseService;
        }

        //Adds a course to the user.
        [HttpPost]
        [Route("AddCourse")]
        [SwaggerResponse(HttpStatusCode.OK, "AddCourseToUser", typeof(ResultModel))]
        public ResultModel AddCourse(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                ResponseState state = this.userCourseService.AddCourse(User.Identity.GetUserId<int>(), id);

                if (state.State == true && state.Massage == "OK")
                {
                    return new ResultModel { Index = id, Message = state.Massage };
                }
                else if (state.State == false && state.Massage == "CourseAlreadyAdded")
                {
                    return new ResultModel { Index = id, Message = state.Massage };
                }

                return new ResultModel { Index = id, Message = "ERR" };
            }
            return new ResultModel { Index = id, Message = "UserNotAuthorize" };
        }

        //Deletes a user’s course
        [HttpPost]
        [Route("RemoveCourse")]
        [SwaggerResponse(HttpStatusCode.OK, "RemoveCourseFromUser", typeof(ResultModel))]
        public ResultModel RemoveCourse(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                ResponseState state = this.userCourseService.RemoveCourse(User.Identity.GetUserId<int>(), id);

                if (state.State == true && state.Massage == "OK")
                {
                    return new ResultModel { Index = id, Message = state.Massage };
                }
                else if (state.State == false && state.Massage == "CourseIsAbsent")
                {
                    return new ResultModel { Index = id, Message = state.Massage };
                }

                return new ResultModel { Index = id, Message = "ERR" };
            }
            return new ResultModel { Index = id, Message = "UserNotAuthorize" };
        }
    }
}
