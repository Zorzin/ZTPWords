using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZTPwords.Logic
{
    public class AnswersDecoratorListMix : AnswersDecorator
    {
        public AnswersDecoratorListMix(Answers _answes) : base(_answes)
        {
        }

        public new List<string> getAnswerList()
        {
            throw new NotImplementedException();
        }
    }

}