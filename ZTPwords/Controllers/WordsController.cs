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

        public ActionResult Question()
        {
            AnswersQuestionConnector connector = (AnswersQuestionConnector) Session["connector"];
            if (connector==null)
            {
                connector = new AnswersQuestionConnector();
                Session["connector"] = connector;
            }
            var question = connector.GetQuestion();
            var type = (Context)Session ["mode"];
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
            QuestionViewModels.AnsweredQuestionModel model = new QuestionViewModels.AnsweredQuestionModel()
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
        public ActionResult Question(QuestionViewModels.AnsweredQuestionModel model)
        {
            double points = 0;
            if (model.AnswerId != -1)
            {
                model.Answers =  (List<Word>) Session["answers"];
                var type = (Context)Session ["mode"];
                StateMode state = type.GetState();
                var result = type.GetState().AnswerQuestion(model);
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

        public ActionResult Summary()
        {
            var type = (Context)Session ["mode"];
            StateMode state = type.GetState();
            var points = state.GetPoints();
            ViewBag.Points = points;
            var username = User.Identity.Name;
            var user = db.Users.FirstOrDefault(u => u.UserName == username);
            var levelChecker = new LevelChecker();
            levelChecker.CheckLevel(user, points);
            db.SaveChanges();
            ViewBag.Level = user.Level;
            ViewBag.userPoints = user.Points;
            Session ["connector"] = null;
            return View();
        }

        public ActionResult SelectLanguage()
        {
            return View();
        }
        
        public ActionResult ConfirmSelectLanguage(string language)
        {

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

            var words = db.Words.ToList();

            if (!IsNullOrEmpty(searchString))
            {
                words = words.Where(x => x.WordEn.Contains(searchString) || x.WordPl.Contains(searchString)).ToList();
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(words.ToPagedList(pageNumber, pageSize));
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
