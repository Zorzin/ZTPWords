using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Troschuetz.Random;
using ZTPwords.Controllers;
using ZTPwords.Logic.State;
using ZTPwords.Models;
using static ZTPwords.Models.QuestionViewModels;

namespace ZTPwords.Logic.Iterator
{
    public class Iterator : IIterator
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public List<QuestionModel> Questions;
        private TRandom r;
        private Word word;
        private string mode;
        public QuestionModel First()
        {
            return Questions[0];
        }

        public Word Next()
        {
            if (r==null)
            {
                r = new TRandom();
            }
            while (true)
            {
                int id = r.Next(0, 66366);
                word = db.Words.Find(id);
                mode = (string)System.Web.HttpContext.Current.Session["lang"];
                string level =(string) System.Web.HttpContext.Current.Session["difficulty"];
                if (level!=null)
                {
                    switch (level)
                    {
                        case "easy":
                            if (!CheckForEasy())
                            {
                                continue;
                            }
                            break;
                        case "medium":
                            if (!CheckForMedium())
                            {
                                continue;
                            }
                            break;
                        case "hard":
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
                    return word;
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

            var type = (Context)HttpContext.Current.Session ["mode"];
            StateMode state = type.GetState();
            if (state is TestState) //mam nadzieję ze to tak się sprawdza
            {
                Console.WriteLine("jest tego typu");
            }

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