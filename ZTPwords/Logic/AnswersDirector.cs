using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZTPwords.Logic
{
    public class AnswersDirector
    {
        public void Construct(AnswerBuilder answerBuilder, int quantity, int level)
        {
            answerBuilder.buildCorrectAnswer();
            for (int i = 0; i < quantity - 1; i++)
            {
                if (level == 1)
                {
                    answerBuilder.buildSpecialWord();
                }
                else if (level == 2)
                {
                    if (i < 1)
                    {
                        answerBuilder.buildSpecialWord();
                    }
                    else
                    {
                        answerBuilder.buildRandWord();
                    }
                }
                else if (level == 3)
                {
                    if (i < 2)
                    {
                        answerBuilder.buildSpecialWord();
                    }
                    else
                    {
                        answerBuilder.buildRandWord();
                    }
                }
                else
                {
                    answerBuilder.buildRandWord();
                }
            }





        }
    }
}