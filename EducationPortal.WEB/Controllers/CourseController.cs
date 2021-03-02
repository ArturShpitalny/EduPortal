using AutoMapper;
using EducationPortal.BLL.Services;
using EducationPortal.Core.Models.Entities;
using EducationPortal.Core.Models.States;
using EducationPortal.DAL.Repository;
using EducationPortal.WEB.Models.ViewModel;
using System;
using System.IO;
using System.Web.Configuration;
using System.Web.Mvc;

namespace EducationPortal.WEB.Controllers
{
    public class CourseController : Controller
    {
        public readonly CourseService courseService;
        private readonly int pageSize;
        private readonly string saveFolderPath;

        public CourseController(CourseService courseService)
        {
            this.courseService = courseService;

            int.TryParse(WebConfigurationManager.AppSettings["ElementOnPage"], out pageSize); //Number of items per page
            this.saveFolderPath = WebConfigurationManager.AppSettings["SaveFolder"];
        }

        //The main page of the site
        public ActionResult MainPageCourses(int page = 1, string search = "")
        {
            ViewBag.SearchText = search;
            return PartialView(this.courseService.GetCourses(pageSize, page, search));
        }

        //Course page from the main page
        public ActionResult MainPageAboutCourses(int courseId)
        {
            return PartialView(this.courseService.GetCourseInformation(courseId));
        }

        //Admin Panel main page
        [Authorize(Roles = "Admin")]
        public ActionResult MainAdminPanelCourses(int page = 1, string search = "")
        {
            return PartialView(this.courseService.GetCourses(pageSize, page, search));
        }

        //The first page of the training course
        [Authorize(Roles = "Admin, User")]
        public ActionResult CourseFirstPage(int courseId)
        {
            return PartialView(this.courseService.GetCourseInformation(courseId));
        }

        //Editing and creating a course
        [Authorize(Roles = "Admin")]
        public ActionResult EditCourse(int courseId)
        {
            Course course = this.courseService.GetCourse(courseId);

            if (course == null)
            {
                return View(new CourseModel() { Id = -1 });
            }

            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Course, CourseModel>()));
            CourseModel courseModel = mapper.Map<Course, CourseModel>(course); //From-to

            return View(courseModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditCourse(CourseModel model)
        {
            if (model != null)
            {
                if (ModelState.IsValid)
                {
                    Course course = new Course();
                    course.Id = model.Id;
                    course.CourseName = model.CourseName;
                    course.CourseDescription = model.CourseDescription;

                    if (model.ImageFile != null)
                    {
                        string directory = Server.MapPath(this.saveFolderPath);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        string path = $"{directory}{Path.GetFileName(model.ImageFile.FileName)}";

                        model.ImageFile.SaveAs(path);

                        course.CourseImagePath = $"{this.saveFolderPath}{Path.GetFileName(model.ImageFile.FileName)}";
                    }

                    if (model.Id > 0)
                    {
                        this.courseService.UpdateCourse(course);
                    }
                    else
                    {
                        this.courseService.AddCourse(course);
                    }

                    return RedirectToAction("MainAdminPanelCourses");  
                }
                return View(model);
            }
            return View();
        }

        //Deleting a course (Ajax request)
        [HttpPost]
        public ActionResult DeleteCourse(int id)
        {
            if (User.Identity.IsAuthenticated && User.Identity.GetUserRole() == "Admin")
            {
                ResponseState state = this.courseService.DeleteCourse(id);

                if (state.State == true && state.Massage == "OK")
                {
                    return Json(new ResultModel { Index = id, Message = state.Massage });
                }
                else if (state.State == false && state.Massage == "CourseIsAbsent")
                {
                    return Json(new ResultModel { Index = id, Message = state.Massage });
                }

                return Json(new ResultModel { Index = id, Message = "ERR" }); 
            }
            return Json(new ResultModel { Index = id, Message = "UserNotAuthorize" });
        }
    }
}