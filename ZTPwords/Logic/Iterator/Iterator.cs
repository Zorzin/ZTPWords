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
using ZTPwords.Logic.Adapter;

namespace ZTPwords.Logic.Iterator
{
    public class Iterator : IIterator
    {
        private EntityFrameworkDatabaseConnection db = new EntityFrameworkDatabaseConnection();
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
                word = db.FindWord(id);
                mode = (string)System.Web.HttpContext.Current.Session["lang"];
                string level =(string) System.Web.HttpContext.Current.Session["difficulty"];
                if (level!=null)
                {
                    if (level=="continious")
                    {
                        level = CheckLevel();
                    }
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

        private string CheckLevel()
        {
            var username = HttpContext.Current.User.Identity.Name;
            var user = db.getUser(username);
            var level = user.Level;
            if (level<5)
            {
                return "easy";
            }
            if (level <10)
            {
                return "medium";
            }
            return "hard";
            
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
            if (state is LearningState) //mam nadzieję ze to tak się sprawdza
            {
                return false;
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