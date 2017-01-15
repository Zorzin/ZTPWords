using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZTPwords.Models;

namespace ZTPwords.Logic
{
    public class AnswerBuilderSameLength : AnswerBuilder
    {
        private List<Word> list;
        private Word correctAnswer;
        private ApplicationDbContext db = new ApplicationDbContext();
        public AnswerBuilderSameLength(Word _correctAnswer)
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
            do
            {
                w = db.Words.OrderBy(t => Guid.NewGuid())
                             .FirstOrDefault();

            } while (correctAnswer == w);
            list.Add(w);

        }
        public override void buildSpecialWord()
        {
            Word w = null;
            do
            {
                w = db.Words.Where(ww => ww.WordEn.Length == correctAnswer.WordEn.Length).OrderBy(ww => Guid.NewGuid())
                             .FirstOrDefault();

            } while (correctAnswer == w) ;
            list.Add(w);
        }

        public override List<Word> getResult()
        {
            return list;
        }

    }
}