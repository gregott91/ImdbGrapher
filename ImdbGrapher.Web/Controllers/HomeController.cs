using ImdbGrapher.Api;
using ImdbGrapher.Business;
using ImdbGrapher.Models.Home;
using ImdbGrapher.Models.Logic;
using log4net;
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
        private static readonly ILog log = LogManager.GetLogger(typeof(HomeController));
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
        public async Task<ActionResult> GraphShow(string id)
        {
            ShowRating show = null;

            try
            {
                show = await apiLogic.GetShowAsync(id);
            }
            catch (Exception ex)
            {
                log.Error("Failed while graphing show " + id, ex);
            }

            return View(new GraphShowModel()
            {
                Show = show
            });
        }

        /// <summary>
        /// The page for searching for shows
        /// </summary>
        /// <param name="showTitle">The show title</param>
        /// <returns>The view</returns>
        public async Task<ActionResult> SearchShows(string showTitle)
        {
            List<SearchResult> results = new List<SearchResult>();

            try
            {
                results = await apiLogic.SearchForShowsAsync(showTitle);
            }
            catch (Exception ex)
            {
                log.Error("Failed while searching for show " + showTitle, ex);
            }

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
            try
            {
                return await apiLogic.GetShowIdFromTitleAsync(showTitle);
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting the show ID for " + showTitle);
            }

            return null;
        }

        /// <summary>
        /// Gets the show graph data
        /// </summary>
        /// <param name="showId">The show ID</param>
        /// <param name="totalSeasons">The seasons count</param>
        /// <returns>The show data</returns>
        public async Task<JsonResult> GetShowData(string showId, int totalSeasons)
        {
            List<SeasonRating> rating = new List<SeasonRating>();

            try
            {
                rating = await apiLogic.GetShowEpisodeRatingsAsync(showId, totalSeasons);
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting the show data for " + showId + " and " + totalSeasons, ex);
            }

            return Json(rating, JsonRequestBehavior.AllowGet);
        }
    }
}