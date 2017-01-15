using System.Web;
using ZTPwords.Controllers;
using ZTPwords.Models;


namespace ZTPwords.Logic.State
{
    public class LearningState : StateMode
    {
        public QuestionHandling AnswerQuestion(QuestionViewModels.AnsweredQuestionModel model)
        {
            var id = model.AnswerId;
            var list = model.Answers.getAnswerList();
            var word = model.Word;

            var lang = (string) HttpContext.Current.Session["lang"];
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