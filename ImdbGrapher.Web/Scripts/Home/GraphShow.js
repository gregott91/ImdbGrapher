$(document).ready(function () {
    var chart;

    var graphResult;

    var showDatasets;
    var seasonTrendlines;
    var seriesTrendline;

    graphShow();

    $('#startFromZero').change(displayShowGraph);

    $('#showSeriesTrendline').change(displayShowGraph);

    $('#showChart').mousedown(function (e) {
        if (e.which != 1 && e.which != 2) {
            return;
        }

        var elements = chart.getElementAtEvent(e);

        if (elements.length < 1) {
            return;
        }

        var index = elements[0]._index;
        var datasetIndex = elements[0]._datasetIndex;

        var episodeIndex = getEpisodeIndex(index, datasetIndex);

        if (episodeIndex == null) {
            return;
        }

        var url = 'http://www.imdb.com/title/' + graphResult.SeasonRatings[episodeIndex.Season].EpisodeRatings[episodeIndex.Episode].ImdbId;

        if (e.which == 2) {
            window.open(url, '_blank');
        } else {
            window.location.href = url;
        }
    });

    function graphShow() {
        $.ajax({
            url: '/imdbgraph/Home/GetShowData?showId=' + $('#showId').val(),
            success: function (result) {
                $('#loaderContainer').hide();

                if (result.ShowTitle) {
                    graphResult = result;

                    initializeChart();
                    displayShowGraph();
                } else {
                    $('#errorContainer').show();
                }
            },
            error: function (result) {
                $('#loaderContainer').hide();
                $('#errorContainer').show();
            }
        });
    }

    function initializeChart() {
        $('#graphContainer').show();

        var graphLabels = [];

        showDatasets = [];
        seasonTrendlines = [];
        seriesTrendline = [];

        var allEpisodes = [];
        var episodeCount = 0;

        for (var seasonIndex in graphResult.SeasonRatings) {
            var season = graphResult.SeasonRatings[seasonIndex];
            var seasonColor = getSeasonColor(seasonIndex);
            var seasonDisplay = parseInt(seasonIndex) + 1;
            var dataset = [];

            for (var nullEpisode = 0; nullEpisode < episodeCount; nullEpisode++) {
                dataset.push(null);
            }

            for (var episodeIndex in season.EpisodeRatings) {
                var xIndex = episodeCount + parseInt(episodeIndex);
                var episodeDisplay = parseInt(episodeIndex) + 1;

                graphLabels.push('S' + seasonDisplay + 'E' + episodeDisplay);

                var episode = season.EpisodeRatings[episodeIndex];
                dataset.push(episode.ImdbRating);

                allEpisodes.push(episode);
            }

            showDatasets.push({
                pointRadius: 4,
                pointHoverRadius: 7,
                borderColor: formatColor(seasonColor, 0.1),
                backgroundColor: formatColor(seasonColor, 0.05),
                pointBorderColor: formatColor(seasonColor, 1),
                pointBackgroundColor: formatColor(seasonColor, 0.5),
                data: dataset
            });

            seasonTrendlines.push(generateTrendlineDataset(season.EpisodeRatings, episodeCount, seasonColor));

            episodeCount += season.EpisodeRatings.length;
        }

        seriesTrendline = generateTrendlineDataset(allEpisodes, 0, {
            r: 255,
            g: 255,
            b: 255
        });

        $('#showTitleDisplayContent').text(graphResult.ShowTitle + " (" + graphResult.Year + ")");
        $('#showTitleDisplayContent').attr('href', 'http://www.imdb.com/title/' + graphResult.ImdbId);
        $('#showTitleDisplayContent').attr('data-poster', graphResult.PosterUrl);
        $('#showTitleDisplayContent').attr('data-rating', graphResult.ImdbRating);
        $('#similarShowsLink').attr('href', '/imdbgraph/Home/SearchShows?showTitle=' + graphResult.ShowTitle);

        var ctx = $('#showChart')[0];
        ctx.height = 500;
        ctx.width = 1110;
        chart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: graphLabels,
                datasets: []
            },
            options: {
                responsive: false,
                maintainAspectRatio: true,
                legend: {
                    display: false
                },
                hover: {
                    onHover: function (e, el) {
                        $("#showChart").css("cursor", el[0] ? "pointer" : "default");
                    }
                },
                tooltips: {
                    titleFontSize: 0,
                    displayColors: false,
                    callbacks: {
                        label: function (tooltipItem, data) {
                            var index = tooltipItem.index;
                            var datasetIndex = tooltipItem.datasetIndex;

                            var episodeIndex = getEpisodeIndex(index, datasetIndex);

                            if (episodeIndex == null) {
                                throw new Error();
                            }

                            return getLabel(episodeIndex.Season, episodeIndex.Episode);
                        }
                    },
                },
                title: {
                    display: false
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            fontColor: 'white'
                        },
                        gridLines: {
                            color: '#646464'
                        }
                    }],
                    xAxes: [{
                        ticks: {
                            fontColor: 'white',
                            autoSkip: true,
                            maxTicksLimit: 45
                        },
                        gridLines: {
                            color: '#646464'
                        }
                    }]
                }

            }
        });
    }

    function getEpisodeIndex(index, datasetIndex) {
        if (datasetIndex >= graphResult.SeasonRatings.length) {
            return null;
        }

        for (var i = 0; i < datasetIndex; i++) {
            var datasetLength = graphResult.SeasonRatings[i].EpisodeRatings.length;
            index = index - datasetLength;
        }

        return {
            Season: datasetIndex,
            Episode: index
        }
    }

    function getLabel(datasetIndex, dataIndex) {
        var seasonIndex = datasetIndex + 1;
        var episodeIndex = dataIndex + 1;

        var season = graphResult.SeasonRatings[datasetIndex];

        var episode = season.EpisodeRatings[dataIndex];
        return [
            episode.Title + " - " + episode.ImdbRating + "/10",
            "Season " + seasonIndex + ", Episode " + episodeIndex,
            episode.Released
        ];
    }

    function displayShowGraph() {
        var startAtZero = $('#startFromZero').prop('checked');
        var generateSeasonTrendline = true;
        var generateShowTrendline = $('#showSeriesTrendline').prop('checked');

        var dataSets = [];
        var allEpisodes = [];
        var episodeCount = 0;

        for (var datasetIndex in showDatasets) {
            dataSets.push(showDatasets[datasetIndex]);
        }

        if (generateSeasonTrendline) {
            for (var datasetIndex in seasonTrendlines) {
                dataSets.push(seasonTrendlines[datasetIndex]);
            }
        }

        if (generateShowTrendline) {
            dataSets.push(seriesTrendline);
        }

        chart.options.scales.yAxes[0].ticks.beginAtZero = startAtZero;
        chart.data.datasets = dataSets;
        chart.update(0);
    }

    function generateTrendlineDataset(ratings, offset, seasonColor) {
        var trendLine = calculateTrendLine(ratings);

        var dataset = [];
        for (var i = 0; i < offset; i++) {
            dataset.push(null);
        }

        var lastValue = trendLine.yIntercept;
        for (var i = 0; i < ratings.length; i++) {
            dataset.push(lastValue);

            lastValue += trendLine.slope;
        }

        return {
            pointRadius: 4,
            pointHoverRadius: 7,
            borderColor: formatColor(seasonColor, 1),
            backgroundColor: 'transparent',
            pointBorderColor: 'transparent',
            pointBackgroundColor: 'transparent',
            data: dataset
        };
    }

    function calculateTrendLine(episodeRatings) {
        var n = episodeRatings.length;

        var a = 0;
        for (var index in episodeRatings) {
            var x = parseInt(index) + 1;
            var y = episodeRatings[index].ImdbRating;

            a += (x * y);
        }
        a *= n;

        var bY = 0;
        var bX = 0;
        for (var index in episodeRatings) {
            var x = parseInt(index) + 1;
            var y = episodeRatings[index].ImdbRating;

            bY += y;
            bX += x;
        }
        var b = bY * bX;

        var c = 0;
        for (var index in episodeRatings) {
            var x = parseInt(index) + 1;

            c += (x * x);
        }
        c *= n;

        var dX = 0;
        for (var index in episodeRatings) {
            var x = parseInt(index) + 1;

            dX += x;
        }
        var d = dX * dX;

        var slope = (a - b) / (c - d);

        var e = 0;
        for (var index in episodeRatings) {
            var y = episodeRatings[index].ImdbRating;

            e += y;
        }

        var fX = 0;
        for (var index in episodeRatings) {
            var x = parseInt(index) + 1;

            fX += x;
        }
        var f = slope * fX;

        var yIntercept = (e - f) / n;

        return {
            slope: slope,
            yIntercept: yIntercept
        }
    }

    function getSeasonColor(season) {
        var r = 0;
        var g = 0;
        var b = 0;

        switch (season % 6) {
            case 0:
                r = 240;
                g = 225;
                b = 110;
                break;
            case 1:
                r = 200;
                g = 120;
                b = 250;
                break;
            case 2:
                r = 130;
                g = 240;
                b = 160;
                break;
            case 3:
                r = 240;
                g = 120;
                b = 120;
                break;
            case 4:
                r = 120;
                g = 160;
                b = 240;
                break;
            case 5:
                r = 200;
                g = 240;
                b = 120;
                break;
        }

        return {
            r: r,
            g: g,
            b: b
        }
    }

    function formatColor(color, alpha) {
        return 'rgba(' + color.r + ',' + color.g + ',' + color.b + ',' + alpha + ')';
    }
})