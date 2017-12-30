using ImdbGrapher.Business;
using ImdbGrapher.Models.Home;
using ImdbGrapher.Models.Logic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ImdbGrapher.Controllers
{
    public class HomeController : Controller
    {
        private ApiLogic apiLogic;

        public HomeController()
        {
            var apiKey = ConfigurationManager.AppSettings["ApiKey"];
            apiLogic = new ApiLogic(apiKey);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult GraphShow(string id)
        {
            return View(new GraphShowModel()
            {
                ShowId = id
            });
        }

        public async Task<ActionResult> SearchShows(string showTitle)
        {
            var results = await apiLogic.SearchForShowsAsync(showTitle);

            return View(new SearchShowModel()
            {
                Results = results
            });
        }

        public async Task<string> GetShowId(string showTitle)
        {
            return await apiLogic.GetShowIdFromTitle(showTitle);
        }

        public async Task<JsonResult> GetShowData(string showId)
        {
            ShowRating rating = await apiLogic.GetShowEpisodeRatingsAsync(showId);
            return Json(rating, JsonRequestBehavior.AllowGet);
        }
    }
}