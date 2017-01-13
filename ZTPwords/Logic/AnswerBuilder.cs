using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZTPwords.Logic
{
    public interface AnswerBuilder
    {
        void buildPart();

        string getResult();
    }
}