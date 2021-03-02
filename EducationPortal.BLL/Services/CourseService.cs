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
    public class CourseService
    {
        private readonly IRepository repository;

        public CourseService(IRepository repository)
        {
            this.repository = repository;
        }

        //Creating a new course
        public ResponseState AddCourse(Course course)
        {
            this.repository.Create(course);
            this.repository.SaveChanges();

            return new ResponseState { State = true, Massage = $"OK" };
        }

        //Information about the course and the materials contained therein
        public CourseInformationModel GetCourseInformation(int courseId)
        {
            var course = this.repository.FirstOrDefault<Course>(x => x.Id == courseId);

            var relation = this.repository.Where<MaterialCourse>(x => x.CourseId == courseId).OrderBy(x => x.Position);
            var courseMaterial = this.repository.Join<MaterialCourse, Material, int, Material>(relation, x => true, mc => mc.Id, m => m.Id, (mc, m) => m).ToList();

            if (course != null && courseMaterial != null)
            {
                return new CourseInformationModel { Course = course, Materials = courseMaterial };
            }

            return null;
        }

        //A collection of all courses with the usual page
        public EntityItemModel<Course> GetCourses(int elementOnPageCount, int page, string search)
        {
            int countTotalItems = this.repository.Count<Course>(x => x.CourseName.ToUpper().Contains(search.ToUpper()) || x.CourseDescription.ToUpper().Contains(search.ToUpper()));

            IEnumerable<Course> paginationCourses = this.repository.GetDataBlock<Course, int>((page - 1) * elementOnPageCount, elementOnPageCount, o => o.Id, x => x.CourseName.ToUpper().Contains(search.ToUpper()) || x.CourseDescription.ToUpper().Contains(search.ToUpper())).ToList();

            Pagination pages = new Pagination
            {
                PageNumber = page,
                PageSize = elementOnPageCount,
                TotalItems = countTotalItems
            };

            return new EntityItemModel<Course> { Entities = paginationCourses, Pagination = pages };
        }

        //Returns a course object by ID
        public Course GetCourse(int courseId)
        {
            return this.repository.FirstOrDefault<Course>(x => x.Id == courseId);
        }

        //Deleting a Course by ID
        public ResponseState DeleteCourse(int courseId)
        {
            Course course = this.repository.FirstOrDefault<Course>(x => x.Id == courseId);

            if (course != null)
            {
                this.repository.Delete<Course>(course);
                this.repository.SaveChanges();

                return new ResponseState { State = true, Massage = "OK" };
            }

            return new ResponseState { State = false, Massage = "CourseIsAbsent" };
        }

        //Edits course information.
        public bool UpdateCourse(Course course)
        {
            Course entity = this.repository.FirstOrDefault<Course>(x => x.Id == course.Id);

            if (entity != null)
            {
                entity.CourseName = course.CourseName;
                entity.CourseDescription = course.CourseDescription;

                if (course.CourseImagePath != null)
                {
                    entity.CourseImagePath = course.CourseImagePath; 
                }

                this.repository.Update<Course>(entity);
                this.repository.SaveChanges();

                return true;
            }
            return false;
        }
    }
}
