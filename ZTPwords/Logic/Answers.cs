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
        public Word correctAnswer { get; set; }
        public Answers(Word _correctAnswer,string buildingScheme)
        {
            correctAnswer=_correctAnswer;
            AnswerBuilder builder = null;
            if (buildingScheme=="SameLetter")
            {
               builder= new AnswerBuilderSameLetter(correctAnswer);
            }else
            {
                builder = new AnswerBuilderSameLength(correctAnswer);
            }
            AnswersDirector director = new AnswersDirector();
            director.Construct(builder,5,3);
            list = builder.getResult();
        }
        
        public override List<Word> getAnswerList()
        {
            return list;
        }
    }
}