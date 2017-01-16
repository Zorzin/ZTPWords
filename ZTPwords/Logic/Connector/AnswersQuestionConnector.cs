using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using ZTPwords.Models;
using static ZTPwords.Models.QuestionViewModels;

namespace ZTPwords.Logic.Connector
{
    public class AnswersQuestionConnector
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public List<QuestionModel> Questions;
        private Iterator.Iterator _iterator;
        private List<Word> _answers;
        public QuestionModel GetQuestion()
        {
            if (_iterator==null)
            {
                _iterator = new Iterator.Iterator();
            }
            if (Questions==null)
            {
                Questions = new List<QuestionModel>();
            }
            _iterator.Questions = Questions;
            if (!_iterator.IsDone())
            {
                var currentWord = _iterator.Next();
                getAnswers(currentWord);
                var answers = _answers;
                var newquestion = new QuestionModel()
                {
                    Answers = answers,
                    Word = currentWord,
                    QuestionNumber = Questions.Count
                    
                };

                HttpContext.Current.Session["answers"] = answers;
                Questions.Add(newquestion);
                return newquestion;
            }
            return null;
        }

        private int getLevel()
        {
            string level =(string) System.Web.HttpContext.Current.Session["difficulty"];
            if (level != null)
            {
                if (level == "continious")
                {
                    level = CheckLevel();
                }
            }
            int lvl = 0;
            switch (level)
            {
                case "easy":
                    lvl = 1;
                    break;
                case "medium":
                    lvl = 2;
                    break;
                case "hard":
                    lvl = 3;
                    break;
            }
            return lvl;
        }

        private int getQuantity()
        {
            int level = getLevel();
            int quantity;
            switch (level)
            {
                case 1:
                    quantity = 2;
                    break;
                case 2:
                    quantity = 3;
                    break;
                case 3:
                    quantity = 5;
                    break;
                default:
                    quantity = 2;
                    break;
                    
            }
            return quantity;
        }

        private void getAnswers(Word currentWord)
        {
            int level = getLevel();
            if (level==1)
            {
                _answers = new AnswersDecoratorListMix(new Answers(currentWord, "", getQuantity(), level)).getAnswerList();
            }
            else if (level == 2)
            {

                _answers = new AnswersDecoratorListMix(new Answers(currentWord, "SameLetter", getQuantity(), level)).getAnswerList();
            }
            else if(level==3)
            {

                _answers = new AnswersDecoratorSwapLetter(new AnswersDecoratorListMix(new Answers(currentWord, "SameLetter", getQuantity(), level))).getAnswerList();
            }

            
        }

        private string CheckLevel()
        {
            var username = HttpContext.Current.User.Identity.Name;
            var user = db.Users.FirstOrDefault(u => u.UserName == username);
            var level = user.Level;
            if (level < 5)
            {
                return "easy";
            }
            if (level < 10)
            {
                return "medium";
            }
            return "hard";

        }
    }
}