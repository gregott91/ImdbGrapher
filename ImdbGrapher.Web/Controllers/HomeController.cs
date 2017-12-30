using ImdbGrapher.Api;
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
            var queryBuilder = new ApiQueryBuilder();
            var api = new ImdbApi(queryBuilder);

            apiLogic = new ApiLogic(api);
        }

        /// <summary>
        /// The home page for the site
        /// </summary>
        /// <returns>The view</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// The about page for the site
        /// </summary>
        /// <returns>The view</returns>
        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// The page for graphing the series
        /// </summary>
        /// <param name="id">The show id</param>
        /// <returns>The view</returns>
        public ActionResult GraphShow(string id)
        {
            return View(new GraphShowModel()
            {
                ShowId = id
            });
        }
        
        /// <summary>
        /// The page for searching for shows
        /// </summary>
        /// <param name="showTitle">The show title</param>
        /// <returns>The view</returns>
        public async Task<ActionResult> SearchShows(string showTitle)
        {
            var results = await apiLogic.SearchForShowsAsync(showTitle);

            return View(new SearchShowModel()
            {
                Results = results
            });
        }

        /// <summary>
        /// Gets the show ID from the title
        /// </summary>
        /// <param name="showTitle">The title</param>
        /// <returns>The ID</returns>
        public async Task<string> GetShowId(string showTitle)
        {
            return await apiLogic.GetShowIdFromTitleAsync(showTitle);
        }

        /// <summary>
        /// Gets the show graph data
        /// </summary>
        /// <param name="showId">The show ID</param>
        /// <returns>The show data</returns>
        public async Task<JsonResult> GetShowData(string showId)
        {
            ShowRating rating = await apiLogic.GetShowEpisodeRatingsAsync(showId);
            return Json(rating, JsonRequestBehavior.AllowGet);
        }
    }
}