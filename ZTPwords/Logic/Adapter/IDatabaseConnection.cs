using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZTPwords.Models;

namespace ZTPwords.Logic.Adapter
{
    public interface IDatabaseConnection
    {
        Word getRandWord();
        Word getSameLetterWord(Word correctAnswer);
        Word getSameLengthWord(Word correctAnswer);
        List<Word> getWords();
        ApplicationUser getUser(string username);
        Word FindWord(int id);
        void SaveChanges();
        void wordAdd(Word word);
        void wordState(Word word);
        void removeWord(Word word);
    }
}