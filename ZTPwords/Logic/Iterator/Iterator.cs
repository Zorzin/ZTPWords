using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using ZTPwords.Models;
using static ZTPwords.Models.QuestionViewModels;

namespace ZTPwords.Logic.Iterator
{
    public class Iterator : IIterator
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public List<QuestionModel> Questions;
        private Word word;
        private string mode;
        public QuestionModel First()
        {
            return Questions[0];
        }

        public QuestionModel Next()
        {
            while (true)
            {
                Random.Org.Random r = new Random.Org.Random();
                int id = r.Next(0, 66366);
                word = db.Words.Find(id);
                mode = (string)System.Web.HttpContext.Current.Session["lang"];
                string level =(string) System.Web.HttpContext.Current.Session["difficulty"];
                if (level!=null)
                {
                    switch (level)
                    {
                        case "Easy":
                            if (!CheckForEasy())
                            {
                                continue;
                            }
                            break;
                        case "Medium":
                            if (!CheckForMedium())
                            {
                                continue;
                            }
                            break;
                        case "Hard":
                            if (!CheckForHard())
                            {
                                continue;
                            }
                            break;
                        default:
                            break;
                    }
                }
                

                if (Questions.All(q => q.Word.Id != word.Id))
                {
                    return new QuestionModel() {Word = word};
                }
            }
        }

        private bool CheckForEasy()
        {
            if (mode=="pl")
            {

                if (word.WordEn.Length<6)
                {
                    return true;
                }
            }
            else
            {
                if (word.WordPl.Length < 6)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckForMedium()
        {

            if (mode == "Eng")
            {

                if (word.WordEn.Length >6 && word.WordEn.Length <12)
                {
                    return true;
                }
            }
            else
            {
                if (word.WordPl.Length > 6 && word.WordPl.Length < 12)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckForHard()
        {

            if (mode == "Eng")
            {

                if (word.WordEn.Length > 10 )
                {
                    return true;
                }
            }
            else
            {
                if (word.WordPl.Length > 10 )
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsDone()
        {
            if (Questions.Count == 10)
            {
                return true;
            }
            return false;
        }

        public QuestionModel CurrentItem()
        {
            return Questions[Questions.Count - 1];
        }
    }
}