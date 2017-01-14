using System.Collections.Generic;
using System.IO;
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
    }
}
