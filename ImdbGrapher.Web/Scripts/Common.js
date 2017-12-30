$(document).ready(function () {
    // handles displaying the poster on hover
    $('.displayPosterLink').hover(function () {
        displayPoster(this, $(this).data('poster'), $(this).data('rating'));
    }, function () {
        hidePoster();
    });

    // hides the poster on unhover
    function hidePoster() {
        $('#posterDisplay').hide();
        $('#imageDisplay').off('load');
        $('#posterDisplayLoader').show();
        $('#imageDisplay').hide();
        $('#ratingContent').hide();
        $('#tooltipContent').hide();
    }

    // displays the poster in a tooltip
    function displayPoster(item, posterUrl, rating) {
        var showPoster = posterUrl != "N/A";
        var showRating = rating != undefined;

        // if there's no show data, don't show the tooltip
        if (!(showPoster || showRating)) {
            return;
        }

        // position the tooltip
        var position = $(item).offset();
        var width = $(item).width();

        var y = position.top;
        var x = position.left + width + 10;

        $('#posterDisplay').css('top', y);
        $('#posterDisplay').css('left', x);

        $('#posterDisplay').show();

        // if a rating is present, show the rating
        if (showRating) {
            $('#ratingValue').text(rating);
            $('#ratingContent').show();
        }

        // if a poster is present, show the poster
        if (showPoster) {
            $('#imageDisplay').on('load', function () {
                $('#posterDisplayLoader').hide();
                $('#imageDisplay').show();
                $('#tooltipContent').show();
            });

            $('#imageDisplay').attr('src', posterUrl);
        } else {
            $('#tooltipContent').show();
        }
    }
});