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
    public class MaterialCourseService
    {
        private readonly IRepository repository;

        public MaterialCourseService(IRepository repository)
        {
            this.repository = repository;
        }

        //Adds material to the course.
        public ResponseState AddMaterialCourseRelation(int materialId, int courseId)
        {
            bool contains = this.repository.Any<MaterialCourse>(x => x.Id == materialId && x.CourseId == courseId);

            if (contains == false)
            {
                var materialCoursePosition = this.repository.Where<MaterialCourse>(x => x.CourseId == courseId).OrderByDescending(x => x.Position).FirstOrDefault();

                if(materialCoursePosition != null)
                {
                    int position = materialCoursePosition.Position;
                    position++;

                    this.repository.Create(new MaterialCourse { Id = materialId, CourseId = courseId, Position = position});
                }
                else
                {
                    this.repository.Create(new MaterialCourse { Id = materialId, CourseId = courseId, Position = 0 });
                }

                this.repository.SaveChanges();

                return new ResponseState { State = true, Massage = $"OK" };
            }
            return new ResponseState { State = false, Massage = $"MaterialCourseAlreadyAdded" };
        }

        //Move material one position up in the course
        public ResponseState MaterialPositionInCourseUp(int materialId, int courseId)
        {
            var materialsCourse = this.repository.Where<MaterialCourse>(x => x.CourseId == courseId).OrderBy(x => x.Position).ToList();
            var value = materialsCourse.FirstOrDefault(x => x.Id == materialId);

            if(materialsCourse != null && value != null)
            {
                MaterialCourse prev = null;

                if (materialsCourse.Count > 1)
                {
                    for (int i = 0; i < materialsCourse.Count; i++)
                    {
                        if (materialsCourse[i].Id == materialId && materialsCourse[i].CourseId == courseId)
                        {
                            if ((i - 1) < 0)
                            {
                                prev = materialsCourse[materialsCourse.Count - 1];
                            }
                            else
                            {
                                prev = materialsCourse[i - 1];
                            }
                        }
                    } 
                }

                if (prev != null)
                {
                    int positionPrev = prev.Position;

                    prev.Position = value.Position;
                    value.Position = positionPrev;

                    this.repository.Update<MaterialCourse>(prev);
                    this.repository.Update<MaterialCourse>(value);

                    this.repository.SaveChanges();

                    return new ResponseState { State = true, Massage = $"OK" };
                }
                return new ResponseState { State = false, Massage = $"CourseHaveOneElement" };
            }
            return new ResponseState { State = false, Massage = $"CourseNotFound" };
        }

        //Move material one position down in the course
        public ResponseState MaterialPositionInCourseDown(int materialId, int courseId)
        {
            var materialsCourse = this.repository.Where<MaterialCourse>(x => x.CourseId == courseId).OrderBy(x => x.Position).ToList();
            var value = materialsCourse.FirstOrDefault(x => x.Id == materialId);

            if (materialsCourse != null && value != null)
            {
                MaterialCourse next = null;

                if (materialsCourse.Count > 1)
                {
                    for (int i = 0; i < materialsCourse.Count; i++)
                    {
                        if (materialsCourse[i].Id == materialId && materialsCourse[i].CourseId == courseId)
                        {
                            if ((i + 1) > materialsCourse.Count - 1)
                            {
                                next = materialsCourse[0];
                            }
                            else
                            {
                                next = materialsCourse[i + 1];
                            }
                        }
                    }
                }

                if (next != null)
                {
                    int positionPrev = next.Position;

                    next.Position = value.Position;
                    value.Position = positionPrev;

                    this.repository.Update<MaterialCourse>(next);
                    this.repository.Update<MaterialCourse>(value);

                    this.repository.SaveChanges();

                    return new ResponseState { State = true, Massage = $"OK" };
                }
                return new ResponseState { State = false, Massage = $"CourseHaveOneElement" };
            }
            return new ResponseState { State = false, Massage = $"CourseNotFound" };
        }

        //Displays all materials included in the course.
        public IEnumerable<Material> GetMaterialCourse(int courseId)
        {
            var materialsCourse = this.repository.Where<MaterialCourse>(x => x.CourseId == courseId).OrderBy(x => x.Position);

            return this.repository.Join<MaterialCourse, Material, int, Material>(materialsCourse, x => true, mc => mc.Id, m => m.Id, (mc, m) => m).ToList();
        }

        //Displays a page of materials in the course being studied.
        public MaterialCoursePageModel GetMaterialCoursePage(int userId, int courseId, int page = 1)
        {
            int countTotalPages = this.repository.Count<MaterialCourse>(mc => mc.CourseId == courseId);

            var materialCourse = this.repository.GetDataBlock<MaterialCourse, int>(page - 1, 1, o => o.Position, mc => mc.CourseId == courseId);

            var paginationMaterial = this.repository.Join<MaterialCourse, Material, int, Material>(materialCourse, x => true, mcx => mcx.Id, mx => mx.Id, (mcx, mx) => mx).ToList().FirstOrDefault();

            //Check if this material is in already studied materials
            bool isMaterialComplete = this.repository.Any<CompletedUserMaterial>(x => x.Id == userId && x.MaterialId == paginationMaterial.Id);

            return new MaterialCoursePageModel { Material = paginationMaterial, IsComplete = isMaterialComplete, CourseId = courseId, NextPage = page + 1, PageCount = countTotalPages };
        }

        //Removes material from a course.
        public ResponseState DeleteMaterialFromCourse(int courseId, int materialId)
        {
            MaterialCourse materialCourse = this.repository.FirstOrDefault<MaterialCourse>(x => x.CourseId == courseId && x.Id == materialId);

            if (materialCourse != null)
            {
                this.repository.Delete<MaterialCourse>(materialCourse);
                this.repository.SaveChanges();

                return new ResponseState { State = true, Massage = "OK" };
            }

            return new ResponseState { State = false, Massage = "MaterialCourseIsAbsent" };
        }

        //Adds material to the ones already studied by the user.
        public ResponseState CompleteMaterial(int userId, int materialId, int courseId)
        {
            var completedMaterial = this.repository.Where<CompletedUserMaterial>(x => x.Id == userId && x.MaterialId == materialId).ToList();

            //Есть ли у данного курса этот пройденный материал
            if (completedMaterial.Any(x => x.Id == userId && x.MaterialId == materialId && x.CourseId == courseId) == false) 
            {
                var userSkill = this.repository.FirstOrDefault<UserSkill>(x => x.Id == userId);

                //Записываем тогда материал из этого курса как пройденный
                this.repository.Create<CompletedUserMaterial>(new CompletedUserMaterial { Id = userId, MaterialId = materialId, CourseId = courseId });

                if (completedMaterial.Count < 1) //Есть ли вообще данный пройденный материал в любом курсе
                {
                    //Если материал вообще ещё не пройден
                    var skill = this.repository.Join<Material, Skill, int, Skill>(this.repository.Where<Material>(x => x.Id == materialId), s => true, mx => mx.SkillId, sx => sx.Id, (mx, sx) => sx).ToList().FirstOrDefault();

                    userSkill.Rating += skill.SkillScore; //Прибавляем оценку от умения
                    this.repository.Update<UserSkill>(userSkill);

                    this.repository.SaveChanges();

                    return new ResponseState { State = true, Massage = "OK" }; 
                }
                else
                {
                    //Если этот материал уже защитан в другом курсе, то прибавляем только 1 балл
                    userSkill.Rating += 1;
                    this.repository.Update<UserSkill>(userSkill);
                    this.repository.SaveChanges();

                    return new ResponseState { State = false, Massage = "MaterialAlreadyCompleted" };
                }
            }
            else
            {
                return new ResponseState { State = false, Massage = "MaterialInThisCourseAlreadyCompleted" };
            }            
        }
    }
}
