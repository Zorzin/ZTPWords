using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ZTPwords.Models;

namespace ZTPwords.Logic.Adapter
{
    public class DatabaseConnection
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public Word getRandWord()
        {
            return db.Words.OrderBy(t => Guid.NewGuid())
                             .FirstOrDefault();
        }
        public Word getSameLetterWord(Word correctAnswer)
        {
            var mode = (string)System.Web.HttpContext.Current.Session["lang"];
            if (mode == "en")
            {
                return db.Words.Where(ww => ww.WordEn.Substring(0, 1).ToUpper() == correctAnswer.WordEn.Substring(0, 1).ToUpper()).OrderBy(ww => Guid.NewGuid())
                            .FirstOrDefault();
            }else
            {
                return db.Words.Where(ww => ww.WordPl.Substring(0, 1).ToUpper() == correctAnswer.WordPl.Substring(0, 1).ToUpper()).OrderBy(ww => Guid.NewGuid())
                           .FirstOrDefault();
            }
           
        }

        public Word getSameLengthWord(Word correctAnswer)
        {
            return db.Words.Where(ww => ww.WordEn.Length == correctAnswer.WordEn.Length).OrderBy(ww => Guid.NewGuid())
                             .FirstOrDefault();
        }

        public List<Word> getWords()
        {
            return db.Words.ToList();
        }

        public ApplicationUser getUser(string username)
        {
            return db.Users.FirstOrDefault(u => u.UserName == username);
        }
        public Word FindWord(int id)
        {
            return db.Words.Find(id);
        }
        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public void wordAdd(Word word)
        {
            db.Words.Add(word);
        }
        public void wordState(Word word)
        {
            db.Entry(word).State = EntityState.Modified;
        }

        public void removeWord(Word word)
        {
            db.Words.Remove(word);
        }
    }
}