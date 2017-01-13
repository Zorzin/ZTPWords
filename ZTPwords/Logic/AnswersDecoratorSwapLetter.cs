using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ZTPwords.Logic
{
    public class AnswersDecoratorSwapLetter : AnswersDecorator
    {
        public AnswersDecoratorSwapLetter(Answers _answers) : base(_answers)
        {
        }

        public override  List<string> getAnswerList()
        {
            List<string> temp = base.answers.getAnswerList();
            var size = temp.Count;
            var rand = new Random().Next() % size;
            if (true)
            {
                string s = temp[rand];
                temp.Remove(s);


                Random swap = new Random();
                int position1 = swap.Next(1, s.Length - 1);
                int position2 = swap.Next(1, s.Length - 1);
                string tempString1 = s[position1].ToString();
                string tempString2 = s[position2].ToString();
                s = s.Remove(position1, 1);
                s = s.Insert(position1, tempString2);
                s = s.Remove(position2, 1);
                s = s.Insert(position2, tempString1);

                temp.Add(s);
            }
            return temp;
        }
    }
}