using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ZTPwords.Models.QuestionViewModels;

namespace ZTPwords.Logic.Connector
{
    interface IAnswerQuestionConnector
    {
        QuestionModel GetQuestion();

    }
}
