using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZTPwords.Logic
{
    public abstract class AnswersDecorator : IAnswers
    {
       protected Answers answers;

        public AnswersDecorator(Answers _answers)
        {
            answers = _answers;
        }

        public override List<string> getAnswerList()
        {
            return answers.getAnswerList();
        }
    }
}