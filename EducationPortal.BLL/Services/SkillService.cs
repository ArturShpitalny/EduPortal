using EducationPortal.Core.Models.Entities;
using EducationPortal.Core.Models.States;
using EducationPortal.Core.Models.DTO;
using EducationPortal.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.BLL.Services
{
    public class SkillService
    {
        private readonly IRepository repository;

        public SkillService(IRepository repository)
        {
            this.repository = repository;
        }

        //Adding a new skill
        public ResponseState AddSkill(Skill skill)
        {
            this.repository.Create(skill);
            this.repository.SaveChanges();
            
            return new ResponseState { State = true, Massage = $"OK" };
        }

        //Display all pagination skills
        public EntityItemModel<Skill> GetSkills(int elementOnPageCount, int page, string search)
        {
            int countTotalItems = this.repository.Count<Skill>(x => x.SkillName.ToUpper().Contains(search.ToUpper()));
            IEnumerable<Skill> paginationSkills = this.repository.GetDataBlock<Skill, int>((page - 1) * elementOnPageCount, elementOnPageCount, o => o.Id, x => x.SkillName.ToUpper().Contains(search.ToUpper())).ToList();

            Pagination pages = new Pagination
            {
                PageNumber = page,
                PageSize = elementOnPageCount,
                TotalItems = countTotalItems
            };

            return new EntityItemModel<Skill> { Entities = paginationSkills, Pagination = pages };
        }

        //The conclusion of all skills without pagination
        public IEnumerable<Skill> GetSkills()
        {
            return this.repository.Where<Skill>(x => true).ToList();
        }

        //Return skill object by ID
        public Skill GetSkill(int id)
        {
            return this.repository.FirstOrDefault<Skill>(x => x.Id == id);
        }

        //Skill editing
        public bool UpdateSkill(Skill skill)
        {
            Skill entity = this.repository.FirstOrDefault<Skill>(x => x.Id == skill.Id);

            if (entity != null)
            {
                entity.SkillName = skill.SkillName;
                entity.SkillScore = skill.SkillScore;

                this.repository.Update<Skill>(entity);
                this.repository.SaveChanges();

                return true;
            }
            return false;
        }

        //Delete skill by ID
        public ResponseState DeleteSkill(int skillId)
        {
            Skill skill = this.repository.FirstOrDefault<Skill>(x => x.Id == skillId);

            if (skill != null)
            {
                bool contains = this.repository.Any<Material>(x => x.SkillId == skillId);

                if (contains == false)
                {
                    this.repository.Delete<Skill>(skill);
                    this.repository.SaveChanges();

                    return new ResponseState { State = true, Massage = "OK" }; 
                }
                return new ResponseState { State = false, Massage = "SkillIsStillInUse" };
            }
            return new ResponseState { State = false, Massage = "SkillIsAbsent" };
        }
    }
}
