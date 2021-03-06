﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ZTPwords.Logic;
using ZTPwords.Logic.Connector;
using ZTPwords.Logic.State;
using ZTPwords.Logic.UserLevel;
using ZTPwords.Models;
using static System.String;
using ZTPwords.Logic.Adapter;
using static ZTPwords.Models.QuestionViewModels;


namespace ZTPwords.Controllers
{
    public enum QuestionHandling
    {
        WrongAnswer,
        CorrectAnswer
    }
    [Authorize]
    public class WordsController : Controller
    {
        private IDatabaseConnection db = new EntityFrameworkDatabaseConnection();
        private Context context = new Context();

        public ActionResult Question()
        {
            var type = (Context)Session ["mode"];
            if (type == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.MaxPoints = GetMaxPoints();
            AnswersQuestionConnector connector = (AnswersQuestionConnector) Session["connector"];
            if (connector==null)
            {
                connector = new AnswersQuestionConnector();
                Session["connector"] = connector;
            }
            var question = connector.GetQuestion();
            
            string mode;
            StateMode state = type.GetState();;
            if (state is LearningState)
            {
                mode = "learn";
            }
            else
            {
                mode = "test";
            }
            ViewBag.Points = state.GetPoints();
            if (question==null)
            {
                return RedirectToAction("Summary");
            }
            AnsweredQuestionModel model = new AnsweredQuestionModel()
            {
                Answers = question.Answers,
                Word = question.Word,
                QuestionNumber = question.QuestionNumber+1,
                Mode = mode,
                Lang = (string) Session["lang"]
            };
            return View(model); //model
        }

        [HttpPost]
        public ActionResult Question(AnsweredQuestionModel model)
        {
            double points = 0;
            if (model.AnswerId != -1)
            {
                model.Answers =  (List<Word>) Session["answers"];
                var type = (Context)Session ["mode"];
                StateMode state = type.GetState();
                var result = type.GetState().AnswerQuestion(model.Word,model.Answers[model.AnswerId]);
                if (state is LearningState) //mam nadzieję ze to tak się sprawdza
                {
                    if (result == QuestionHandling.CorrectAnswer)
                    {
                        return RedirectToAction("Question");
                    }
                    ViewBag.NoAnswer = "Wrong answer";
                    return View(model);
                }
                if(state is TestState)
                {
                    if (result == QuestionHandling.CorrectAnswer)
                    {
                        state.SetPoints(1);
                        Debug.WriteLine(points);
                    }
                    
                    return RedirectToAction("Question");
                }
                
            }
            ViewBag.NoAnswer = "Pick answer";
            return View(model);
        }

        public ActionResult QuestionHard()
        {
            var type = (Context)Session ["mode"];
            if (type == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.MaxPoints = GetMaxPoints();
            AnswersQuestionConnector connector = (AnswersQuestionConnector) Session["connector"];
            if (connector == null)
            {
                connector = new AnswersQuestionConnector();
                Session ["connector"] = connector;
            }
            var question = connector.GetQuestion();
            string mode;
            StateMode state = type.GetState(); ;
            if (state is LearningState)
            {
                mode = "learn";
            }
            else
            {
                mode = "test";
            }
            ViewBag.Points = state.GetPoints();
            if (question == null)
            {
                return RedirectToAction("Summary");
            }
            QuestionHardModel model = new QuestionHardModel()
            {
                Lang = (string) Session["lang"],
                Mode = mode,
                QuestionNumber = question.QuestionNumber+1,
                Word = question.Word
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult QuestionHard(QuestionHardModel model)
        {
            if (!string.IsNullOrEmpty(model.Answer))
            {
                double points = 0;
                var type = (Context)Session ["mode"];
                StateMode state = type.GetState();
                var result = type.GetState().AnswerQuestion(model.Word,new Word() {WordEn = model.Answer, WordPl = model.Answer});
                if (state is LearningState)
                {
                    if (result == QuestionHandling.CorrectAnswer)
                    {
                        return RedirectToAction("QuestionHard");
                    }
                    ViewBag.NoAnswer = "Wrong answer";
                    return View(model);
                }
                if (state is TestState)
                {
                    if (result == QuestionHandling.CorrectAnswer)
                    {
                        state.SetPoints(1);
                        Debug.WriteLine(points);
                    }

                    return RedirectToAction("QuestionHard");
                }
            }
            ViewBag.NoAnswer = "Pick answer";
            return View(model);
        }

        public ActionResult Summary()
        {
            var type = (Context)Session ["mode"];
            if (type == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.MaxLvlPoints = GetMaxLvlPoints();
            StateMode state = type.GetState();
            var points = state.GetPoints();
            ViewBag.Points = points;
            var username = User.Identity.Name;
            var user = db.getUser(username);
            var levelChecker = new LevelChecker();
            levelChecker.CheckLevel(user, points);
            db.SaveChanges();
            ViewBag.Level = user.Level;
            ViewBag.userPoints = user.Points;
            ViewBag.MaxPoints = GetMaxPoints();
            Session ["connector"] = null;
            Session["mode"] = null;
            return View();
        }

        public double GetMaxLvlPoints()
        {
            var username = System.Web.HttpContext.Current.User.Identity.Name;
            var user = db.getUser(username);
            var userlvl = user.Level;
            if (userlvl==1)
            {
                return 80;
            }
            if(userlvl==2)
            {
                return 120;
            }
            return 200;
        }

        public double GetMaxPoints()
        {
            var mode = (string)System.Web.HttpContext.Current.Session ["lang"];
            var username = System.Web.HttpContext.Current.User.Identity.Name;
            var user = db.getUser(username);
            var userlang = user.Language;
            if (userlang == mode)
            {
               //more points
                return 15;
            }
            return 10;
        }

        public ActionResult SelectLanguage()
        {
            var type = (Context)Session ["mode"];
            if (type == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        
        public ActionResult ConfirmSelectLanguage(string language)
        {
            var type = (Context)Session ["mode"];
            if (type == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Session ["connector"] = null;
            if (!IsNullOrEmpty(language))
            {
                switch (language)
                {
                    case "pl":
                        Session["lang"] = "pl";
                        break;
                    case "eng":
                        Session ["lang"] = "en";
                        break;
                }
                if ((string)Session["difficulty"]=="hard")
                {
                    return RedirectToAction("QuestionHard");
                }
                return RedirectToAction("Question");
            }
            return RedirectToAction("SelectLanguage");
        }


        public ActionResult SelectDifficulty()
        {

            Session ["connector"] = null;
            switch (Request.QueryString["mode"])
            {
                case "learning":
                    context.ChangeState(State.Learning);
                    break;
                case "test":
                    context.ChangeState(State.Test);
                    break;
            }
            Session["mode"] = context;
            return View();
        }

        public ActionResult ConfirmDifficulty()
        {
            var type = (Context)Session ["mode"];
            if (type == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Session ["connector"] = null;
            if (Request.QueryString["difficulty"] != null)
            {
                Session["difficulty"] = Request.QueryString["difficulty"];
                return RedirectToAction("SelectLanguage");
            }
            return RedirectToAction("SelectDifficulty");
        }

        [Authorize]
        // GET: Words
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var words = db.getWords();

            if (!IsNullOrEmpty(searchString))
            {
                words = words.Where(x => x.WordEn.Contains(searchString) || x.WordPl.Contains(searchString)).ToList();
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(words.ToPagedList(pageNumber, pageSize));
        }


        [Authorize(Roles = "Admin")]
        // GET: Words/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Word word = db.FindWord(id.Value);
            if (word == null)
            {
                return HttpNotFound();
            }
            return View(word);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Words/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,WordEn,WordPl")] Word word)
        {
            if (ModelState.IsValid)
            {
                db.wordAdd(word);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(word);
        }

        // GET: Words/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Word word = db.FindWord(id.Value);
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,WordEn,WordPl")] Word word)
        {
            if (ModelState.IsValid)
            {
                db.wordState(word);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(word);
        }

        // GET: Words/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Word word = db.FindWord(id.Value);
            if (word == null)
            {
                return HttpNotFound();
            }
            return View(word);
        }

        // POST: Words/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Word word = db.FindWord(id);
            db.removeWord(word);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
    }
}
