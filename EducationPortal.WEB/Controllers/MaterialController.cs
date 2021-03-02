using EducationPortal.BLL.Services;
using EducationPortal.Core.Models.Entities;
using EducationPortal.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;
using EducationPortal.WEB.Models.ViewModel;
using EducationPortal.Core.Models.DTO;
using EducationPortal.Core.Models.States;
using AutoMapper;

namespace EducationPortal.WEB.Controllers
{
    public class MaterialController : Controller
    {
        public readonly MaterialService materialService;
        private readonly int pageSize;

        public MaterialController(MaterialService materialService)
        {
            this.materialService = materialService;

            int.TryParse(WebConfigurationManager.AppSettings["ElementOnPage"], out pageSize); //Number of items per page
        }

        //List all materials in the admin panel
        [Authorize(Roles = "Admin")]
        public ActionResult AdminPanelMaterial(int page = 1, string search = "")
        {
            return PartialView(this.materialService.GetMaterials(pageSize, page, search));
        }

        //Definition of the type of editable material
        [Authorize(Roles = "Admin")]
        public ActionResult EditMaterial(int materialId)
        {
            Material editMaterial = this.materialService.GetMaterial<Material>(materialId);            

            if (editMaterial is Article)
            {
                return RedirectToAction("EditArticle", new { id = materialId });
            }
            else if (editMaterial is Book)
            {
                return RedirectToAction("EditBook", new { id = materialId });
            }
            else if (editMaterial is Video)
            {
                return RedirectToAction("EditVideo", new { id = materialId });
            }

            return RedirectToAction("AdminPanelMaterial");
        }

        //Material editing
        [Authorize(Roles = "Admin")]
        public ActionResult EditArticle(int id)
        {
            Article article = this.materialService.GetMaterial<Article>(id);

            if (article == null)
            {
                return View(new ArticleModel() { Id = -1 });
            }

            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Article, ArticleModel>()));
            ArticleModel courseModel = mapper.Map<Article, ArticleModel>(article); //From-to

            return View(courseModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditArticle(ArticleModel model)
        {
            if (model != null)
            {
                if (ModelState.IsValid)
                {
                    var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<ArticleModel, Article>()));
                    Article article = mapper.Map<ArticleModel, Article>(model); //From-to

                    if (model.Id > 0)
                    {
                        this.materialService.UpdateMaterial(article);
                    }
                    else
                    {
                        this.materialService.AddMaterial(article);
                    }

                    return RedirectToAction("AdminPanelMaterial");
                }
                return View(model);
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditBook(int id)
        {
            Book book = this.materialService.GetMaterial<Book>(id);

            if (book == null)
            {
                return View(new BookModel() { Id = -1 });
            }
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Book, BookModel>()));
            BookModel bookModel = mapper.Map<Book, BookModel>(book); //From-to

            return View(bookModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditBook(BookModel model)
        {
            if (model != null)
            {
                if (ModelState.IsValid)
                {
                    var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<BookModel, Book>()));
                    Book book = mapper.Map<BookModel, Book>(model); //From-to

                    if (model.Id > 0)
                    {
                        this.materialService.UpdateMaterial(book);
                    }
                    else
                    {
                        this.materialService.AddMaterial(book);
                    }

                    return RedirectToAction("AdminPanelMaterial");
                }
                return View(model);
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditVideo(int id)
        {
            Video video = this.materialService.GetMaterial<Video>(id);

            if (video == null)
            {
                return View(new VideoModel() { Id = -1 });
            }
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Video, VideoModel>()));
            VideoModel videoModel = mapper.Map<Video, VideoModel>(video); //From-to

            return View(videoModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditVideo(VideoModel model)
        {
            if (model != null)
            {
                if (ModelState.IsValid)
                {
                    var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<VideoModel, Video>()));
                    Video video = mapper.Map<VideoModel, Video>(model); //From-to

                    if (model.Id > 0)
                    {
                        this.materialService.UpdateMaterial(video);
                    }
                    else
                    {
                        this.materialService.AddMaterial(video);
                    }

                    return RedirectToAction("AdminPanelMaterial");
                }
                return View(model);
            }
            return View();
        }

        //List of materials when adding material to the course
        [Authorize(Roles = "Admin")]
        public ActionResult AddCourseMaterialsList(int courseId, int page = 1, string search = "")
        {
            return PartialView(new EntityIndexModel<EntityItemModel<Material>> { Index = courseId, Entity = this.materialService.GetMaterials(pageSize, page, search) });
        }

        //Delete material by ID
        [HttpPost]
        public ActionResult DeleteMaterial(int id)
        {
            if (User.Identity.IsAuthenticated && User.Identity.GetUserRole() == "Admin")
            {
                ResponseState state = this.materialService.DeleteMaterial(id);

                if (state.State == true && state.Massage == "OK")
                {
                    return Json(new ResultModel { Index = id, Message = state.Massage });
                }
                else if (state.State == false && state.Massage == "MaterialIsAbsent")
                {
                    return Json(new ResultModel { Index = id, Message = state.Massage });
                }

                return Json(new ResultModel { Index = id, Message = "ERR" }); 
            }
            return Json(new ResultModel { Index = id, Message = "UserNotAuthorize" });
            }
    }
}