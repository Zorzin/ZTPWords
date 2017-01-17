using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZTPwords.Models;

namespace ZTPwords.Logic
{
    public abstract class IAnswers
    {
        public abstract List<Word> getAnswerList();
    }
}