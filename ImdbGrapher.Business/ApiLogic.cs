using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImdbGrapher.Models.Logic;
using ImdbGrapher.Models.Api;
using ImdbGrapher.Utilities;
using System.Text.RegularExpressions;
using ImdbGrapher.Interfaces.Business;
using ImdbGrapher.Interfaces.Api;

namespace ImdbGrapher.Business
{
    /// <summary>
    /// Logic class for the API
    /// </summary>
    public class ApiLogic : IApiLogic
    {
        private IShowApi api;
        private string apiKey;

        /// <summary>
        /// Creates the logic
        /// </summary>
        public ApiLogic(IShowApi imdbApi)
        {
            var apiKey = ConfigurationManager.AppSettings["ApiKey"];
            this.apiKey = apiKey;

            this.api = imdbApi;
        }

        /// <inheritdoc />
        public async Task<string> GetShowIdFromTitleAsync(string showName)
        {
            ShowQueryResponse response = await api.GetShowByTitleAsync(showName, apiKey);

            bool foundShow = response.Response;

            // if it's able to find a show, determine whether this is actually the correct show
            if (foundShow)
            {
                int similarity = response.Title.CompareSimilarity(showName);

                // if the show found does not have a similar title, assume we didn't find the correct show
                if (similarity > 3)
                {
                    foundShow = false;
                }
            }

            // if no show is found, perform a search for all shows that match
            if (!foundShow)
            {
                var searchResults = await api.SearchForShowAsync(showName, apiKey);

                // if only one result is returned, assume this is the show that's being searched for
                if (searchResults.Response && searchResults.Search.Count() == 1)
                {
                    return searchResults.Search.First().ImdbId;
                }

                return null;
            }

            return response.ImdbId;
        }

        /// <inheritdoc />
        public async Task<ShowRating> GetShowEpisodeRatingsAsync(string showId)
        {
            ShowQueryResponse response = await api.GetShowByIdAsync(showId, apiKey);

            if (!response.Response)
            {
                return null;
            }

            double convertedRating;
            double? nullableRating = null;
            if (double.TryParse(response.ImdbRating, out convertedRating))
            {
                nullableRating = convertedRating;
            }

            ShowRating rating = new ShowRating()
            {
                ShowTitle = response.Title,
                ImdbId = response.ImdbId,
                ImdbRating = nullableRating,
                PosterUrl = response.Poster,
                Year = response.Year,
                SeasonRatings = new List<SeasonRating>()
            };

            List<Task<SeasonQueryResponse>> responseTasks = new List<Task<SeasonQueryResponse>>();

            for (int i = 1; i <= response.TotalSeasons; i++)
            {
                responseTasks.Add(api.GetSeasonByIdAsync(response.ImdbId, i, apiKey));
            }

            foreach (Task<SeasonQueryResponse> responseTask in responseTasks)
            {
                SeasonQueryResponse seasonResponse = await responseTask;

                if (!seasonResponse.Response)
                {
                    continue;
                }

                int seasonValue;
                if (!int.TryParse(seasonResponse.Season, out seasonValue))
                {
                    continue;
                }

                SeasonRating seasonRating = new SeasonRating()
                {
                    Season = seasonValue,
                    EpisodeRatings = new List<EpisodeRating>()
                };

                foreach (EpisodeQueryResponse episodeResponse in seasonResponse.Episodes)
                {
                    double ratingValue;

                    if (double.TryParse(episodeResponse.ImdbRating, out ratingValue))
                    {
                        EpisodeRating episodeRating = new EpisodeRating()
                        {
                            Title = episodeResponse.Title,
                            Episode = episodeResponse.Episode,
                            ImdbRating = ratingValue,
                            ImdbId = episodeResponse.ImdbId,
                            Released = episodeResponse.Released
                        };

                        seasonRating.EpisodeRatings.Add(episodeRating);
                    }
                }

                if (seasonRating.EpisodeRatings.Any())
                {
                    seasonRating.EpisodeRatings = seasonRating.EpisodeRatings.OrderBy(x => x.Episode).ToList();
                    rating.SeasonRatings.Add(seasonRating);
                }
            }

            rating.SeasonRatings = rating.SeasonRatings.OrderBy(x => x.Season).ToList();

            return rating;
        }

        /// <inheritdoc />
        public async Task<List<SearchResult>> SearchForShowsAsync(string showName)
        {
            SearchQueryResponse searchResponse = await api.SearchForShowAsync(showName, apiKey);

            if (!searchResponse.Response)
            {
                return new List<SearchResult>();
            }

            List<SearchResult> searchResults = new List<SearchResult>();

            foreach (SearchResultQueryResponse response in searchResponse.Search)
            {
                SearchResult result = new SearchResult()
                {
                    Title = response.Title,
                    PosterUrl = response.Poster,
                    ImdbId = response.ImdbId,
                    Year = response.Year
                };

                searchResults.Add(result);
            }

            return searchResults;
        }
    }
}
