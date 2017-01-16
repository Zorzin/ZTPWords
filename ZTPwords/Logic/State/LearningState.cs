using System.Web;
using ZTPwords.Controllers;
using ZTPwords.Models;


namespace ZTPwords.Logic.State
{
    public class LearningState : StateMode
    {
        public QuestionHandling AnswerQuestion(Word question, Word answer)
        {
            var lang = (string) HttpContext.Current.Session["lang"];
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

        public void SetPoints(double point)
        {
            //do nothing
        }

        public double GetPoints()
        {
            return 0;
        }
    }
}