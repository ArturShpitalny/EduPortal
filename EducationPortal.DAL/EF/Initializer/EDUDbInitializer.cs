using EducationPortal.Core.Models.Auth;
using EducationPortal.Core.Models.Entities;
using EducationPortal.DAL.EF.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.DAL.EF.Initializer
{
    public class EDUDbInitializer : DropCreateDatabaseIfModelChanges<EDUDbContext>
    {
        protected override void Seed(EDUDbContext db)
        {
            db.Role.Add(new Role { Id = 1, RoleName = "Admin" });            
            db.Role.Add(new Role { Id = 2, RoleName = "User" });

            db.User.Add(new User { UserEmail = "senya@i.ua", UserPassword = "?>?>??`[a?x?-??w?\u0016?<", UserBirthdate = DateTime.Now, RoleId = 1 });

            db.UserSkill.Add(new UserSkill { Id = 1, Rating = 0 });

            db.SaveChanges();
        }
    }
}
