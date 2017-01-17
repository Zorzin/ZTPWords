using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZTPwords.Models;

namespace ZTPwords.Logic
{
    public class Answers : IAnswers
    {
        private List<Word> list;
        public Word CorrectAnswer { get; set; }
        public Answers(Word _correctAnswer,string buildingScheme, int quantity, int level)
        {
            CorrectAnswer=_correctAnswer;
            AnswerBuilder builder = null;
            if (buildingScheme=="SameLetter")
            {
               builder= new AnswerBuilderSameLetter(CorrectAnswer);
            }else
            {
                builder = new AnswerBuilderSameLength(CorrectAnswer);
            }
            AnswersDirector director = new AnswersDirector();
            director.Construct(builder,quantity,level);
            list = builder.getResult();
        }
        
        public override List<Word> getAnswerList()
        {
            return list;
        }
    }
}