using EducationPortal.Core.Models.Auth;
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
    public class UserCourseService
    {
        private readonly IRepository repository;

        public UserCourseService(IRepository repository)
        {
            this.repository = repository;
        }

        public ResponseState AddCourse(int userId, int courseId)
        {
            bool userFind = this.repository.Any<User>(x => x.Id == userId);

            if (userFind == true)
            {
                bool contains = this.repository.Any<UserCourse>(x => x.Id == userId && x.CourseId == courseId);

                if (contains == false)
                {
                    this.repository.Create<UserCourse>(new UserCourse { Id = userId, CourseId = courseId });
                    this.repository.SaveChanges();

                    return new ResponseState { State = true, Massage = "OK" };
                }
                return new ResponseState { State = false, Massage = "CourseAlreadyAdded" }; 
            }
            return new ResponseState { State = false, Massage = "UserNotFound" };
        }

        public ResponseState RemoveCourse(int userId, int courseId)
        {
            bool userFind = this.repository.Any<User>(x => x.Id == userId);

            if (userFind == true)
            {
                UserCourse userCourse = this.repository.FirstOrDefault<UserCourse>(x => x.Id == userId && x.CourseId == courseId);

                if (userCourse != null)
                {
                    this.repository.Delete<UserCourse>(userCourse);
                    this.repository.SaveChanges();

                    return new ResponseState { State = true, Massage = "OK" };
                }
                return new ResponseState { State = false, Massage = "CourseIsAbsent" }; 
            }
            return new ResponseState { State = false, Massage = "UserNotFound" };
        }

        public EntityItemModel<CourseModel> GetUserCourse(int userId, int elementOnPageCount, int page)
        {
            bool userFind = this.repository.Any<User>(x => x.Id == userId);

            if (userFind == true)
            {
                int countTotalPages = this.repository.Count<UserCourse>(x => x.Id == userId);

                var userCourse = this.repository.GetDataBlock<UserCourse, int>((page - 1) * elementOnPageCount, elementOnPageCount, o => o.Id, x => x.Id == userId);

                var courses = this.repository.Join<UserCourse, Course, int, Course>(userCourse, x => true, uc => uc.CourseId, c => c.Id, (uc, c) => c).ToList();

                var courseViewModel = new List<CourseModel>();

                foreach (var i in userCourse.ToList())
                {
                    foreach (var y in courses)
                    {
                        if (i.CourseId == y.Id)
                        {
                            courseViewModel.Add(new CourseModel
                            {
                                Id = y.Id,
                                CourseName = y.CourseName,
                                CourseDescription = y.CourseDescription,
                                CourseImagePath = y.CourseImagePath,
                                IsComplete = i.IsComplete
                            });
                        }
                    }
                }

                Pagination pages = new Pagination
                {
                    PageNumber = page,
                    PageSize = elementOnPageCount,
                    TotalItems = countTotalPages
                };

                return new EntityItemModel<CourseModel> { Entities = courseViewModel, Pagination = pages }; 
            }
            return null;
        }

        public ResponseState UserCourseUpdate(UserCourse userCourse)
        {
            bool userFind = this.repository.Any<User>(x => x.Id == userCourse.Id);

            if (userFind == true)
            {
                var entity = this.repository.FirstOrDefault<UserCourse>(x => x.Id == userCourse.Id && x.CourseId == userCourse.CourseId);

                if (entity != null)
                {
                    entity.IsComplete = userCourse.IsComplete;
                    this.repository.Update<UserCourse>(entity);
                    this.repository.SaveChanges();

                    return new ResponseState { State = true, Massage = "OK" };
                }
                return new ResponseState { State = false, Massage = "EntityNotFound" }; 
            }
            return new ResponseState { State = false, Massage = "UserNotFound" };
        }
    }
}
