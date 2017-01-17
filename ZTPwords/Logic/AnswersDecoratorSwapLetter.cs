using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Troschuetz.Random;
using ZTPwords.Models;

namespace ZTPwords.Logic
{
    public class AnswersDecoratorSwapLetter : AnswersDecorator
    {
        protected Word correctAnswer { get; set; }
        public AnswersDecoratorSwapLetter(IAnswers _answers, Word _correctAnswer) : base(_answers)
        {
            correctAnswer = _correctAnswer;
        }

        public override List<Word> getAnswerList()
        {
            List<Word> temp = base.answers.getAnswerList();
            var size = temp.Count;
            int rand;
            do
            {
                rand = new System.Random().Next() % size;
            } while (correctAnswer == temp[rand]);


            var v = correctAnswer;
            var vv =temp[rand];
                string s = temp[rand].WordEn;

                var swap = new TRandom();
                int position1 = swap.Next(1, s.Length - 1);
                int position2 = swap.Next(1, s.Length - 1);
                string tempString1 = s[position1].ToString();
                string tempString2 = s[position2].ToString();
                s = s.Remove(position1, 1);
                s = s.Insert(position1, tempString2);
                s = s.Remove(position2, 1);
                s = s.Insert(position2, tempString1);

                temp[rand].WordEn = s;
                s = temp[rand].WordPl;

                position1 = swap.Next(1, s.Length - 1);
                position2 = swap.Next(1, s.Length - 1);
                tempString1 = s[position1].ToString();
                tempString2 = s[position2].ToString();
                s = s.Remove(position1, 1);
                s = s.Insert(position1, tempString2);
                s = s.Remove(position2, 1);
                s = s.Insert(position2, tempString1);
                temp[rand].WordPl = s;
            return temp;
        }
    }
}