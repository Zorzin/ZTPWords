using System.Collections.Generic;
using ZTPwords.Models;
using static ZTPwords.Models.QuestionViewModels;

namespace ZTPwords.Logic.Connector
{
    public class AnswersQuestionConnector
    {
        public List<QuestionModel> questions;
        public QuestionModel Connect()
        {
            var iterator = new Iterator.Iterator();

            if (questions==null)
            {
                questions = new List<QuestionModel>();
            }
            iterator.Questions = questions;
            Word currentWord;
            Answers answers;
            if (!iterator.IsDone())
            {
                currentWord = iterator.Next();
                answers = new Answers(currentWord,"");
                return new QuestionModel()
                {
                    Answers = answers,
                    Word = currentWord
                };
            }
            return null;
        }
    }
}