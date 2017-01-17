using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZTPwords.Logic.Adapter;
using ZTPwords.Models;

namespace ZTPwords.Logic
{
    public class AnswerBuilderSameLetter : AnswerBuilder
    {
        private List<Word> list;
        private Word correctAnswer;
        private IDatabaseConnection db = new EntityFrameworkDatabaseConnection();
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
            do
            {
                w = db.getRandWord();

            } while (correctAnswer == w);
            list.Add(w);

        }
        public override void buildSpecialWord()
        {
            Word w = null;
            do
            {
                w = db.getSameLetterWord(correctAnswer);

            } while (correctAnswer == w);
            list.Add(w);
        }

        public override List<Word> getResult()
        {
            return list;
        }


    }
}