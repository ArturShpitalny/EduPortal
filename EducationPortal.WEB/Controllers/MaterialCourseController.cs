using EducationPortal.BLL.Services;
using EducationPortal.Core.Models.Entities;
using EducationPortal.Core.Models.States;
using EducationPortal.DAL.Repository;
using EducationPortal.WEB.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EducationPortal.WEB.Controllers
{
    public class MaterialCourseController : Controller
    {
        public readonly MaterialCourseService materialCourseService;

        public MaterialCourseController(MaterialCourseService materialCourseService)
        {
            this.materialCourseService = materialCourseService;
        }

        [HttpPost]
        public ActionResult AddMaterialToCourse(int courseid, int materialid)
        {
            if (User.Identity.IsAuthenticated && User.Identity.GetUserRole() == "Admin")
            {
                ResponseState state = this.materialCourseService.AddMaterialCourseRelation(materialid, courseid);

                if (state.State == true && state.Massage == "OK")
                {
                    return Json(new ResultModel { Index = materialid, Message = state.Massage });
                }
                else if (state.State == false && state.Massage == "MaterialCourseAlreadyAdded")
                {
                    return Json(new ResultModel { Index = materialid, Message = state.Massage });
                }

                return Json(new ResultModel { Index = materialid, Message = "ERR" });
            }
            return Json(new ResultModel { Index = materialid, Message = "UserNotAuthorize" });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminPanelMaterialCourse(int courseId)
        {
            return PartialView(new EntityIndexModel<IEnumerable<Material>> { Index = courseId, Entity = this.materialCourseService.GetMaterialCourse(courseId) });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult MaterialPositionInCourseUp(int courseId, int materialId)
        {
            this.materialCourseService.MaterialPositionInCourseUp(materialId, courseId);
            return RedirectToAction("AdminPanelMaterialCourse", "MaterialCourse", new { courseId = courseId });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult MaterialPositionInCourseDown(int courseId, int materialId)
        {
            this.materialCourseService.MaterialPositionInCourseDown(materialId, courseId);
            return RedirectToAction("AdminPanelMaterialCourse", "MaterialCourse", new { courseId = courseId });
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult MaterialCoursePage(int courseId, int materialId, int page = 1)
        {
            if (page > 1)
            {
                this.materialCourseService.CompleteMaterial(User.Identity.GetUserId<int>(), materialId, courseId);
            }
            return PartialView(this.materialCourseService.GetMaterialCoursePage(User.Identity.GetUserId<int>(), courseId, page));
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult MaterialCoursePageEnd(int courseId, int materialId)
        {
            this.materialCourseService.CompleteMaterial(User.Identity.GetUserId<int>(), materialId, courseId);

            return RedirectToAction("UserCourseEnd", "UserCourse", new { courseId = courseId });
        }

        [HttpPost]
        public ActionResult RemoveMaterial(int courseid, int materialid)
        {
            if (User.Identity.IsAuthenticated && User.Identity.GetUserRole() == "Admin")
            {
                ResponseState state = this.materialCourseService.DeleteMaterialFromCourse(courseid, materialid);

                if (state.State == true && state.Massage == "OK")
                {
                    return Json(new ResultModel { Index = materialid, Message = state.Massage });
                }
                else if (state.State == false && state.Massage == "MaterialCourseIsAbsent")
                {
                    return Json(new ResultModel { Index = materialid, Message = state.Massage });
                }

                return Json(new ResultModel { Index = materialid, Message = "ERR" }); 
            }
            return Json(new ResultModel { Index = materialid, Message = "UserNotAuthorize" });
        }
    }
}