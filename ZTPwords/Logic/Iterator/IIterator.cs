using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTPwords.Models;
using static ZTPwords.Models.QuestionViewModels;

namespace ZTPwords.Logic.Iterator
{
    interface IIterator
    {
        QuestionModel First();
        Word Next();
        bool IsDone();
        QuestionModel CurrentItem();

    }
}
