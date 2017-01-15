using System.Collections.Generic;
using System.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using ZTPwords.Models;

namespace ZTPwords.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ZTPwords.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ZTPwords.Models.ApplicationDbContext";
        }

        public class Item
        {
            public string Pol { get; set; }
            public string Eng { get; set; }
        }

        protected override void Seed(ZTPwords.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            SeedRoles(context);
            SeedAdmin(context);

            if (context.Words.Count()<100)
            {
                Word word;
                string json;
                List<Item> objects = new List<Item>();
                
                string path = AppDomain.CurrentDomain.BaseDirectory; 
                for (int i = 0; i < 1075; i++)
                {
                    objects.Clear();
                    using (StreamReader reader = new StreamReader(path+"/Words/json"+i+".json"))
                    {
                        json = reader.ReadToEnd();
                        objects = JsonConvert.DeserializeObject<List<Item>>(json);
                        foreach (var item in objects)
                        {
                            word = new Word()
                            {
                                WordEn = item.Eng,
                                WordPl = item.Pol
                            };
                            context.Words.Add(word);
                            context.SaveChanges();
                        }
                    }
                }
            }
            
        }

        private void SeedRoles(ZTPwords.Models.ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var adminrole = roleManager.FindByName("Admin");
            if (adminrole == null)
            {
                adminrole = new IdentityRole("Admin");
                roleManager.Create(adminrole);
            }
            var userrole = roleManager.FindByName("User");
            if (userrole == null)
            {
                userrole = new IdentityRole("User");
                roleManager.Create(userrole);
            }
        }

        private void SeedAdmin(ZTPwords.Models.ApplicationDbContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var adminUser = userManager.FindByName("admin");
            if (adminUser == null)
            {
                var admin = new ApplicationUser()
                {
                    UserName = "admin@aa.aa",
                    Email = "admin@aa.aa",
                    PhoneNumber = "111222333",
                    LockoutEnabled = false,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    PasswordHash = new PasswordHasher().HashPassword("admin")
                };
                var adminResult = userManager.Create(admin);
                if (adminResult.Succeeded)
                {
                    userManager.AddToRole(admin.Id, "Admin");
                    userManager.AddToRole(admin.Id, "User");
                }
            }
        }
    }
}
