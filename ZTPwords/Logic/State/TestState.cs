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
        private DatabaseConnection db = new DatabaseConnection();
        private double Points { get; set; }
        public QuestionHandling AnswerQuestion(QuestionViewModels.AnsweredQuestionModel model)
        {
            var id = model.AnswerId;
            var list = model.Answers;
            var word = model.Word;

            var lang = (string)HttpContext.Current.Session["lang"];
            switch (lang)
            {
                case "en":
                    if (word.WordEn == list[id].WordEn)
                        return QuestionHandling.CorrectAnswer;
                    return QuestionHandling.WrongAnswer;
                case "pl":
                    if (word.WordPl == list[id].WordPl)
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