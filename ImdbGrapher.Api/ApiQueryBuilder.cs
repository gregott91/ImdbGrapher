using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Api
{
    public class ApiQueryBuilder
    {
        private const string Url = "http://www.omdbapi.com";

        private const string IdParameter = "i";

        private const string TitleParameter = "t";

        private const string SearchParameter = "s";

        private const string ApiKeyParameter = "apiKey";

        private const string SeasonParameter = "season";

        private const string TypeParameterValue = "type=series";

        public string BuildShowTitleRequest(string title, string apiKey)
        {
            return string.Format("{0}/?{1}={2}&{3}&{4}={5}",
                Url,
                TitleParameter,
                title,
                TypeParameterValue,
                ApiKeyParameter,
                apiKey);
        }

        public string BuildShowIdRequest(string id, string apiKey)
        {
            return string.Format("{0}/?{1}={2}&{3}&{4}={5}",
                Url,
                IdParameter,
                id,
                TypeParameterValue,
                ApiKeyParameter,
                apiKey);
        }

        public string BuildSeasonIdRequest(string id, int season, string apiKey)
        {
            return string.Format("{0}/?{1}={2}&{3}&{4}={5}&{6}={7}",
                Url,
                IdParameter,
                id,
                TypeParameterValue,
                SeasonParameter,
                season,
                ApiKeyParameter,
                apiKey);
        }

        public string BuildSearchRequest(string showName, string apiKey)
        {
            return string.Format("{0}/?{1}={2}&{3}&{4}={5}",
                Url,
                SearchParameter,
                showName,
                TypeParameterValue,
                ApiKeyParameter,
                apiKey);
        }
    }
}
