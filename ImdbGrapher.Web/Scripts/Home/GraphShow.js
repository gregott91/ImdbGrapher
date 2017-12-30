$(document).ready(function () {
    var chart;

    var graphResult;

    var showDatasets;
    var seasonTrendlines;
    var graphLabels;

    var imdbUrl = 'http://www.imdb.com/title/';

    // download and format the data on page load
    graphShow();

    // if the grid options are changed, refresh the graph
    $('#startFromZero').change(displayShowGraph);
    $('#showSeriesTrendline').change(displayShowGraph);
    $('#filterSeasons').change(displayShowGraph);
    $('#seasonStartDropdown').change(function () {
        updateSeasonFilter(true);
        displayShowGraph();
    });
    $('#seasonEndDropdown').change(function () {
        updateSeasonFilter(false);
        displayShowGraph();
    });

    // handle a click on the chart
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

        // generate the url for the show
        var url = imdbUrl + graphResult.SeasonRatings[episodeIndex.Season].EpisodeRatings[episodeIndex.Episode].ImdbId;

        // open in current window or new tab, depending on click type
        if (e.which == 2) {
            window.open(url, '_blank');
        } else {
            window.location.href = url;
        }
    });

    // updates the filter dropdowns for the season
    function updateSeasonFilter(startChanged) {
        $('#filterSeasons').prop('checked', true);

        var startVal = $('#seasonStartDropdown').val();
        var endVal = $('#seasonEndDropdown').val();

        if (endVal != -1 && parseInt(startVal) > parseInt(endVal)) {
            if (startChanged) {
                $('#seasonEndDropdown').val(-1);
            } else {
                $('#seasonStartDropdown').val(-1);
            }
        }
    }

    // gets the show data and graphs the show
    function graphShow() {
        $.ajax({
            url: '/imdbgraph/Home/GetShowData?showId=' + $('#showId').val(),
            success: function (result) {
                $('#loaderContainer').hide();

                if (result.ShowTitle) {
                    graphResult = result;

                    initializeGraphData();
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

    // initializes the data for the graph
    function initializeGraphData() {
        $('#graphContainer').show();

        graphLabels = {};
        showDatasets = [];
        seasonTrendlines = [];

        var episodeCount = 0;

        for (var seasonIndex in graphResult.SeasonRatings) {
            var season = graphResult.SeasonRatings[seasonIndex];
            var seasonColor = getSeasonColor(seasonIndex);
            var seasonDisplay = parseInt(seasonIndex) + 1;
            graphLabels[seasonDisplay] = [];
            var dataset = [];

            for (var episodeIndex in season.EpisodeRatings) {
                var episodeDisplay = parseInt(episodeIndex) + 1;

                graphLabels[seasonDisplay].push('S' + seasonDisplay + 'E' + episodeDisplay);

                var episode = season.EpisodeRatings[episodeIndex];
                dataset.push(episode.ImdbRating);
            }

            showDatasets.push({
                data: dataset,
                season: season.Season,
                seasonColor: seasonColor
            });

            seasonTrendlines.push(generateTrendlineDataset(dataset));

            episodeCount += season.EpisodeRatings.length;

            $('#seasonStartDropdown').append('<option value="' + season.Season + '">' + season.Season + '</option>');
            $('#seasonEndDropdown').append('<option value="' + season.Season + '">' + season.Season + '</option>');
        }

        $('#showTitleDisplayContent').text(graphResult.ShowTitle + " (" + graphResult.Year + ")");
        $('#showTitleDisplayContent').attr('href', imdbUrl + graphResult.ImdbId);
        $('#showTitleDisplayContent').attr('data-poster', graphResult.PosterUrl);
        $('#showTitleDisplayContent').attr('data-rating', graphResult.ImdbRating);
        $('#similarShowsLink').attr('href', '/imdbgraph/Home/SearchShows?showTitle=' + graphResult.ShowTitle);
    }

    // initializes the graph
    function initializeChart() {
        var ctx = $('#showChart')[0];
        ctx.height = 500;
        ctx.width = 1110;
        chart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: [],
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

    // gets the epsiode index from the dataset indices
    function getEpisodeIndex(index, datasetIndex) {
        var unfilteredDatasetIndex = datasetIndex;
        var filterSeasons = $('#filterSeasons').prop('checked');
        var startVal = $('#seasonStartDropdown').val();
        var endVal = $('#seasonEndDropdown').val();
        var startIndex = 0;

        if (filterSeasons && startVal != -1) {
            startIndex = startVal - 1;
            datasetIndex += startIndex;
        }

        if (datasetIndex >= graphResult.SeasonRatings.length) {
            return null;
        }

        for (var i = startIndex; i < datasetIndex; i++) {
            var datasetLength = graphResult.SeasonRatings[i].EpisodeRatings.length;
            index = index - datasetLength;
        }

        return {
            Season: datasetIndex,
            Episode: index
        }
    }

    // gets the episode tooltip label
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

    // displays the graph from the UI options
    function displayShowGraph() {
        var startAtZero = $('#startFromZero').prop('checked');
        var generateSeasonTrendline = true;
        var generateShowTrendline = $('#showSeriesTrendline').prop('checked');
        var filterSeasons = $('#filterSeasons').prop('checked');
        var startVal = $('#seasonStartDropdown').val();
        var endVal = $('#seasonEndDropdown').val();

        var dataSets = [];
        var trendlineDatasets = [];
        var allEpisodes = [];
        var episodeCount = 0;
        var labels = [];

        var episodeCount = 0;
        for (var datasetIndex in showDatasets) {
            var dataset = showDatasets[datasetIndex];
            var displaySeason = true;

            if (filterSeasons) {
                var isFiltered = false;
                if (startVal != -1 && startVal > dataset.season) {
                    isFiltered = true;
                } else if (endVal != -1 && endVal < dataset.season) {
                    isFiltered = true;
                }

                displaySeason = !isFiltered;
            }

            if (displaySeason) {
                var nullDataset = [];
                for (var nullEpisode = 0; nullEpisode < episodeCount; nullEpisode++) {
                    nullDataset.push(null);
                }

                allEpisodes = allEpisodes.concat(dataset.data);

                var resultDataset = nullDataset.concat(dataset.data);

                dataSets.push({
                    pointRadius: 4,
                    pointHoverRadius: 7,
                    borderColor: formatColor(dataset.seasonColor, 0.1),
                    backgroundColor: formatColor(dataset.seasonColor, 0.05),
                    pointBorderColor: formatColor(dataset.seasonColor, 1),
                    pointBackgroundColor: formatColor(dataset.seasonColor, 0.5),
                    data: resultDataset
                });

                if (generateSeasonTrendline) {
                    var trendlineDataset = nullDataset.concat(seasonTrendlines[datasetIndex].data);

                    trendlineDatasets.push(convertTrendlineToDataset(trendlineDataset, dataset.seasonColor));
                }

                var seasonLabels = graphLabels[dataset.season];
                labels = labels.concat(seasonLabels);

                episodeCount += dataset.data.length;
            }
        }

        dataSets = dataSets.concat(trendlineDatasets);

        if (generateShowTrendline) {
            var seriesTrendline = generateTrendlineDataset(allEpisodes);
            dataSets.push(convertTrendlineToDataset(seriesTrendline.data, {
                r: 255,
                g: 255,
                b: 255
            }));
        }

        chart.options.scales.yAxes[0].ticks.beginAtZero = startAtZero;
        chart.data.datasets = dataSets;
        chart.data.labels = labels;
        chart.update(0);
    }

    function convertTrendlineToDataset(trendlineDataset, color) {
        return {
            pointRadius: 0,
            pointHoverRadius: 0,
            borderColor: formatColor(color, 1),
            backgroundColor: 'transparent',
            pointBorderColor: 'transparent',
            pointBackgroundColor: 'transparent',
            data: trendlineDataset
        };
    }

    // generates a trendline dataset
    function generateTrendlineDataset(ratings) {
        var trendLine = calculateTrendLine(ratings);

        var dataset = [];

        var lastValue = trendLine.yIntercept;
        for (var i = 0; i < ratings.length; i++) {
            dataset.push(lastValue);

            lastValue += trendLine.slope;
        }

        return {
            data: dataset
        };
    }

    // calculates a trendline off of a series of points
    function calculateTrendLine(episodeRatings) {
        var n = episodeRatings.length;

        var a = 0;
        for (var index in episodeRatings) {
            var x = parseInt(index) + 1;
            var y = episodeRatings[index];

            a += (x * y);
        }
        a *= n;

        var bY = 0;
        var bX = 0;
        for (var index in episodeRatings) {
            var x = parseInt(index) + 1;
            var y = episodeRatings[index];

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
            var y = episodeRatings[index];

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

    // gets the color of the points for a given season
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

    // formats a color as an rgba string
    function formatColor(color, alpha) {
        return 'rgba(' + color.r + ',' + color.g + ',' + color.b + ',' + alpha + ')';
    }
})