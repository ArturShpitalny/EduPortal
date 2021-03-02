using AutoMapper;
using EducationPortal.Core.Models.Auth;
using EducationPortal.Core.Models.Entities;
using EducationPortal.Core.Models.States;
using EducationPortal.Core.Models.DTO;
using EducationPortal.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.BLL.Services
{
    public class AccountService
    {
        private readonly IRepository repository;

        public AccountService(IRepository repository)
        {
            this.repository = repository;
        }

        //Conclusion of a role object by role name
        public Role GetRole(string roleName)
        {
            return this.repository.FirstOrDefault<Role>(x => x.RoleName == roleName);
        }

        //Output a role object by role ID
        public Role GetRole(int roleId)
        {
            return this.repository.FirstOrDefault<Role>(x => x.Id == roleId);
        }

        //Output all roles without pagination
        public IEnumerable<Role> GetRoles()
        {
            return this.repository.Where<Role>(x => true).ToList();
        }

        //Add new user
        public ResponseState AddUser(User entity)
        {
            bool contains = this.repository.Any<User>(x => x.UserEmail == entity.UserEmail);

            if (contains == false)
            {
                var user = this.repository.Create(entity);

                if (user != null)
                {
                    var uskill = this.repository.Create<UserSkill>(new UserSkill { Id = user.Id, Rating = 0 });
                    this.repository.SaveChanges();

                    return new ResponseState { State = true, Massage = $"OK" };
                }
                return new ResponseState { State = false, Massage = "ERR" };
            }
            return new ResponseState { State = false, Massage = "UserAlreadyRegistered" };
        }

        //Output all users with pagination
        public EntityItemModel<User> GetUsers(int elementOnPageCount, int page, string search)
        {
            int countTotalItems = this.repository.Count<User>(x => x.UserEmail.ToUpper().Contains(search.ToUpper()));

            IEnumerable<User> paginationCourses = this.repository.GetDataBlock<User, int>((page - 1) * elementOnPageCount, elementOnPageCount, o => o.Id, x => x.UserEmail.Contains(search)).ToList();

            Pagination pages = new Pagination
            {
                PageNumber = page,
                PageSize = elementOnPageCount,
                TotalItems = countTotalItems
            };

            return new EntityItemModel<User> { Entities = paginationCourses.Select(s => new User { Id = s.Id, UserEmail = s.UserEmail, UserBirthdate = s.UserBirthdate, RoleId = s.RoleId }), Pagination = pages };
        }

        //Output user object by ID
        public UserModel GetUser(int id)
        {
            return this.GetUserModel(x => x.Id == id);
        }

        //User object output by login and password
        public UserModel GetUser(string eMail, string password)
        {
            return this.GetUserModel(x => x.UserEmail == eMail && x.UserPassword == password);
        }

        private UserModel GetUserModel(Func<User, bool> predicate)
        {
            var user = this.repository.FirstOrDefault<User>(predicate);

            if (user != null)
            {
                var model = new UserModel();
                model.Id = user.Id;
                model.Email = user.UserEmail;
                model.Birthdate = user.UserBirthdate;
                model.RoleId = user.RoleId;

                return model; 
            }
            return null;
        }

        //User Editing
        public bool UpdateUser(User user)
        {
            User entity = this.repository.FirstOrDefault<User>(x => x.Id == user.Id);

            if (entity != null)
            {
                entity.UserEmail = user.UserEmail;

                if (user.UserPassword != null)
                {
                    entity.UserPassword = user.UserPassword; 
                }

                entity.UserBirthdate = user.UserBirthdate;
                entity.RoleId = user.RoleId;

                this.repository.Update<User>(entity);
                this.repository.SaveChanges();

                return true;
            }
            return false;
        }

        //Delete user by ID
        public ResponseState DeleteUser(int userId)
        {
            User user = this.repository.FirstOrDefault<User>(x => x.Id == userId);

            if (user != null)
            {
                this.repository.Delete<User>(user);
                this.repository.SaveChanges();

                return new ResponseState { State = true, Massage = "OK" };
            }

            return new ResponseState { State = false, Massage = "UserIsAbsent" };
        }

        public AboutUserModel UserInfo(int userId)
        {
            User user = this.repository.FirstOrDefault<User>(x => x.Id == userId);

            if (user != null)
            {
                UserModel userModel = new UserModel();
                userModel.Id = user.Id;
                userModel.Email = user.UserEmail;
                userModel.Birthdate = user.UserBirthdate;
                userModel.RoleId = user.RoleId;

                var userMaterial = this.repository.Where<CompletedUserMaterial>(x => x.Id == userId);

                var materials = this.repository.Join<CompletedUserMaterial, Material, int, Material>(userMaterial, x => true, cum => cum.MaterialId, m => m.Id, (cum, m) => m);

                var skills = this.repository.Join<Material, Skill, int, Skill>(materials, x => true, m => m.SkillId, s => s.Id, (m, s) => s).GroupBy(x => x.SkillName).Select(x => x.FirstOrDefault()).ToList();

                UserSkill userSkill = this.repository.FirstOrDefault<UserSkill>(x => x.Id == userId);

                return new AboutUserModel { UserModel = userModel, Skills = skills, TotalUserSkill = userSkill }; 
            }
            return null;
        }
    }
}
