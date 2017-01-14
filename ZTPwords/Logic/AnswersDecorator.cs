using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZTPwords.Models;

namespace ZTPwords.Logic
{
    public abstract class AnswersDecorator : IAnswers
    {
       protected IAnswers answers;

        public AnswersDecorator(IAnswers _answers)
        {
            answers = _answers;
        }

        public override List<Word> getAnswerList()
        {
            return answers.getAnswerList();
        }
    }
}