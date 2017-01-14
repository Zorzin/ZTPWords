using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZTPwords.Controllers;
using ZTPwords.Models;

namespace ZTPwords.Logic.State
{
    public class LearningState : StateMode
    {
        public QuestionHandling AnswerQuestion(QuestionViewModels.AnsweredQuestionModel model)
        {
            return QuestionHandling.WrongAnswer;
            
        }
    }
}