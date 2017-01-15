using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZTPwords.Controllers;
using ZTPwords.Models;

namespace ZTPwords.Logic.State
{
    public interface StateMode
    {
        QuestionHandling AnswerQuestion(QuestionViewModels.AnsweredQuestionModel model);

        void SetPoints(double point);
        double GetPoints();
    }
}