using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZTPwords.Logic
{
    public class Answers : IAnswers
    {
        private List<string> list;
        public Answers(string buildSchema)
        {
            AnswersDirector director = new AnswersDirector(buildSchema);
            //list = director.Construct();
            list = new List<string>();
            list.Add("abcdefg");

        }

        public override List<string> getAnswerList()
        {
            return list;
        }
    }
}