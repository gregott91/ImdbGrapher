$(document).ready(function () {
    $('.displayPosterLink').hover(function () {
        displayPoster(this, $(this).data('poster'), $(this).data('rating'));
    }, function () {
        hidePoster();
    });

    function hidePoster() {
        $('#posterDisplay').hide();
        $('#imageDisplay').off('load');
        $('#posterDisplayLoader').show();
        $('#imageDisplay').hide();
        $('#ratingContent').hide();
        $('#tooltipContent').hide();
    }

    function displayPoster(item, posterUrl, rating) {
        var showPoster = posterUrl != "N/A";
        var showRating = rating != undefined;

        if (!(showPoster || showRating)) {
            return;
        }

        var position = $(item).offset();
        var width = $(item).width();

        var y = position.top;
        var x = position.left + width + 10;

        $('#posterDisplay').css('top', y);
        $('#posterDisplay').css('left', x);

        $('#posterDisplay').show();

        if (showRating) {
            $('#ratingValue').text(rating);
            $('#ratingContent').show();
        }

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