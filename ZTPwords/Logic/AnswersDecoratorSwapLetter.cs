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
        public AnswersDecoratorSwapLetter(IAnswers _answers) : base(_answers)
        {
        }

        public override List<Word> getAnswerList()
        {
            List<Word> temp = answers.getAnswerList();
            var size = temp.Count;
            int rand;
            Answers a = (Answers)answers;
            do
            {
                rand = new System.Random().Next() % size;
            } while (a.correctAnswer == temp[rand]);



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