using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZTPwords.Logic.State;
using ZTPwords.Models;
using static ZTPwords.Models.QuestionViewModels;

namespace ZTPwords.Controllers
{
    public enum QuestionHandling
    {
        WrongAnswer,
        CorrectAnswer,
        NoMoreQuestions,
    }
    public class WordsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Context context = new Context();

        // GET: Words
        public ActionResult Index()
        {
            return View(db.Words.ToList());
        }


        public ActionResult Question() //QuestionModel qvm
        {
            //AnsweredQuestionModel aqm = new AnsweredQuestionModel()
            //{
            //    Answers = qvm.Answers,
            //    Word = qvm.Word,
            //    AnswerId = -1
            //};
            AnsweredQuestionModel aqm = new AnsweredQuestionModel();
            return View(aqm); //aqm
        }

        [HttpPost]
        public ActionResult Question(AnsweredQuestionModel aqm)
        {

            if (aqm.AnswerId != -1)
            {
                var mode = (StateMode) Session["mode"];
                var userAnswer = aqm.Answers.getAnswerList()[aqm.AnswerId];
                //SomeStrategryFunction(userAnswer);
                //logic here
                context.GetState().AnswerQuestion(aqm);
            }
            ViewBag.NoAnswer = "Pick answer";
            return View(aqm);
        }

        public ActionResult SelectLanguage()
        {
            return View();
        }
        
        public ActionResult ConfirmSelectLanguage(string language)
        {
            if (!string.IsNullOrEmpty(language))
            {
                if (language=="pl")
                {
                    Session["lang"] = "pl";
                    return RedirectToAction("Question");
                }
                else if (language == "eng")
                {
                    Session ["lang"] = "en";
                    return RedirectToAction("Question");
                }
            }
            return RedirectToAction("SelectLanguage");
        }


        public ActionResult SelectDifficulty()
        {
            switch (Request.QueryString["mode"])
            {
                case "learning":
                    context.ChangeState(State.Learning);
                    break;
                case "test":
                    context.ChangeState(State.Test);
                    break;
            }
            Session["mode"] = context.GetState();
            return View();
        }

        public ActionResult ConfirmDifficulty()
        {
            if (Request.QueryString["difficulty"] != null)
            {
                Session["difficulty"] = Request.QueryString["difficulty"];
                return RedirectToAction("SelectLanguage");
            }
            return RedirectToAction("SelectDifficulty");
        }

        // GET: Words/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Word word = db.Words.Find(id);
            if (word == null)
            {
                return HttpNotFound();
            }
            return View(word);
        }

        // GET: Words/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Words/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,WordEn,WordPl")] Word word)
        {
            if (ModelState.IsValid)
            {
                db.Words.Add(word);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(word);
        }

        // GET: Words/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Word word = db.Words.Find(id);
            if (word == null)
            {
                return HttpNotFound();
            }
            return View(word);
        }

        // POST: Words/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,WordEn,WordPl")] Word word)
        {
            if (ModelState.IsValid)
            {
                db.Entry(word).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(word);
        }

        // GET: Words/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Word word = db.Words.Find(id);
            if (word == null)
            {
                return HttpNotFound();
            }
            return View(word);
        }

        // POST: Words/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Word word = db.Words.Find(id);
            db.Words.Remove(word);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
