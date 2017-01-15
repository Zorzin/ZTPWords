using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZTPwords.Logic;

namespace ZTPwords.Models
{
    public class QuestionViewModels
    {

        public class QuestionModel
        { 
            public Word Word { get; set; }
            public Answers Answers { get; set; }
        }

        public class AnsweredQuestionModel
        {
            public Word Word { get; set; }
            public Answers Answers { get; set; }
            public int AnswerId { get; set; }
        }
    }
}