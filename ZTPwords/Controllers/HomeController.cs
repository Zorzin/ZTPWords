using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZTPwords.Logic;
using ZTPwords.Models;

namespace ZTPwords.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            
            ViewBag.Message = "Your application description page.";
            Word w = new Word();
            w.WordEn = "dog";
            w.WordPl = "pies";
            IAnswers al =new AnswersDecoratorSwapLetter(new AnswersDecoratorListMix( new Answers(w,"SameLetter")));
            ViewBag.list = al.getAnswerList();
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}