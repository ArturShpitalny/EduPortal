using AutoMapper;
using EducationPortal.BLL.Services;
using EducationPortal.Core.Models.Entities;
using EducationPortal.Core.Models.States;
using EducationPortal.Core.Models.DTO;
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
    public class SkillController : Controller
    {
        public readonly SkillService skillService;
        private readonly int pageSize;

        public SkillController(SkillService skillService)
        {
            this.skillService = skillService;

            int.TryParse(WebConfigurationManager.AppSettings["ElementOnPage"], out pageSize);
        }

        //Admin Panel Skills Page
        [Authorize(Roles = "Admin")]
        public ActionResult AdminPanelSkill(int page = 1, string search = "")
        {
            return PartialView(this.skillService.GetSkills(pageSize, page, search));
        }

        //Skill combo box for material
        [Authorize(Roles = "Admin")]
        public ActionResult MaterialSkill(int selectedId)
        {
            return PartialView(new EntityIndexModel<IEnumerable<Skill>> { Entity = this.skillService.GetSkills(), Index = selectedId } );
        }

        //Skill editing
        [Authorize(Roles = "Admin")]
        public ActionResult EditSkill(int id)
        {
            Skill skill = this.skillService.GetSkill(id);

            if (skill == null)
            {
                return View(new SkillModel() { Id = -1 });
            }
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Skill, SkillModel>()));
            SkillModel skillModel = mapper.Map<Skill, SkillModel>(skill); //From-to

            return View(skillModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditSkill(SkillModel model)
        {
            if (model != null)
            {
                if (ModelState.IsValid)
                {
                    var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<SkillModel, Skill>()));
                    Skill skill = mapper.Map<SkillModel, Skill>(model); //From-to

                    if (model.Id > 0)
                    {
                        this.skillService.UpdateSkill(skill);
                    }
                    else
                    {
                        this.skillService.AddSkill(skill);
                    }

                    return RedirectToAction("AdminPanelSkill");
                }
                return View(model);
            }
            return View();
        }

        //Removing skill by ID
        [HttpPost]
        public ActionResult DeleteSkill(int id)
        {
            if (User.Identity.IsAuthenticated && User.Identity.GetUserRole() == "Admin")
            {
                ResponseState state = this.skillService.DeleteSkill(id);

                if (state.State == true && state.Massage == "OK")
                {
                    return Json(new ResultModel { Index = id, Message = state.Massage });
                }
                else if (state.State == false && state.Massage == "SkillIsStillInUse")
                {
                    return Json(new ResultModel { Index = id, Message = state.Massage });
                }
                else if (state.State == false && state.Massage == "SkillIsAbsent")
                {
                    return Json(new ResultModel { Index = id, Message = state.Massage });
                }

                return Json(new ResultModel { Index = id, Message = "ERR" }); 
            }
            return Json(new ResultModel { Index = id, Message = "UserNotAuthorize" });
            }
    }
}