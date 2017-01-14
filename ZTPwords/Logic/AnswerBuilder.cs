using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZTPwords.Models;

namespace ZTPwords.Logic
{
    public abstract class AnswerBuilder
    {
        public abstract void buildRandWord();
        public abstract void buildCorrectAnswer();
        public abstract void buildSpecialWord();
        public abstract List<Word> getResult();
    }
}