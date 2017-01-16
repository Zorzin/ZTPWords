using System.Collections.Generic;
using System.Web;
using ZTPwords.Models;
using static ZTPwords.Models.QuestionViewModels;

namespace ZTPwords.Logic.Connector
{
    public class AnswersQuestionConnector
    {
        public List<QuestionModel> Questions;
        private Iterator.Iterator _iterator;
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
                var answers = new AnswersDecoratorListMix(new Answers(currentWord,"SameLetter")).getAnswerList();
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
    }
}