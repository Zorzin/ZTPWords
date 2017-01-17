using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZTPwords.Controllers;
using ZTPwords.Logic.Adapter;
using ZTPwords.Models;

namespace ZTPwords.Logic.State
{
    public class TestState : StateMode
    {
        private IDatabaseConnection db = new EntityFrameworkDatabaseConnection();
        private double Points { get; set; }
        public QuestionHandling AnswerQuestion(Word question, Word answer)
        {

            var lang = (string)HttpContext.Current.Session["lang"];
            switch (lang)
            {
                case "en":
                    if (question.WordEn == answer.WordEn)
                        return QuestionHandling.CorrectAnswer;
                    return QuestionHandling.WrongAnswer;
                case "pl":
                    if (question.WordPl == answer.WordPl)
                        return QuestionHandling.CorrectAnswer;
                    return QuestionHandling.WrongAnswer;
                default:
                    return QuestionHandling.WrongAnswer;
            }
        }

        public void Po(double point)
        {
            
            
        }

        public void SetPoints(double point)
        {

            var mode = (string)System.Web.HttpContext.Current.Session ["lang"];
            var username = HttpContext.Current.User.Identity.Name;
            var user = db.getUser(username);
            var userlang = user.Language;
            if (userlang == mode)
            {
                //more points
                point = point*1.5;
            }
            Points += point;
        }
        

        public double GetPoints()
        {
            return Points;
        }

        
    }
}