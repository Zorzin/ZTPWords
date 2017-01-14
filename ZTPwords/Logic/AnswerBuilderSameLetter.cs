using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZTPwords.Models;

namespace ZTPwords.Logic
{
    public class AnswerBuilderSameLetter : AnswerBuilder
    {
        private List<Word> list;
        private Word correctAnswer;
        private ApplicationDbContext db = new ApplicationDbContext();
        public AnswerBuilderSameLetter(Word _correctAnswer)
        {
            list = new List<Word>();
            correctAnswer = _correctAnswer;
        }
        public override void buildCorrectAnswer()
        {
            list.Add(correctAnswer);
        }
        public override void buildRandWord()
        {
            Word w = null;
            while (correctAnswer != w)
            {
                w = db.Words.OrderBy(t => Guid.NewGuid())
                             .FirstOrDefault();

            }
            list.Add(w);

        }
        public override void buildSpecialWord()
        {
            Word w = null;
            while (correctAnswer != w)
            {
                w = db.Words.Where(ww=> ww.WordEn==correctAnswer.WordEn).OrderBy(ww => Guid.NewGuid())
                             .FirstOrDefault();

            }
            list.Add(w);
        }

        public override List<Word> getResult()
        {
            return list;
        }


    }
}