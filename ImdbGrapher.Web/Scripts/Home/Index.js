$(document).ready(function () {
    // handles click of the "Graph It" button
    $('#graphButton').click(function () {
        graphShow($('#idOrTitle').val());
    })

    // clicks the "Graph It" button if the user types "Enter"
    $('#idOrTitle').keyup(function (event) {
        if (event.keyCode === 13) {
            $('#graphButton').click();
        }
    });

    // makes the graph show request
    function graphShow(showValue) {
        // validates that a show was entered
        if (showValue == null || showValue == undefined || showValue == '') {
            $('#idOrTitleGroup').addClass('has-error');
            $('#helpMessage').text('Please enter a value');
            $('#helpMessage').show();
            return;
        }

        // disables the button to prevent multiple requests
        $('#buttonText').hide();
        $('#buttonLoader').show();
        $('#graphButton').attr('disabled', true);
        $('#helpMessage').text('');
        $('#helpMessage').hide();
        $('#idOrTitleGroup').removeClass('has-error');

        var searchUrl = '/imdbgraph/Home/SearchShows?showTitle=';
        var graphUrl = '/imdbgraph/Home/GraphShow?id=';

        // if an ID was typed, navigate to that ID
        var regex = /tt[0-9]{7}/;

        if (regex.test(showValue) && showValue.length == 9) {
            window.location.href = graphUrl + showValue;
        }

        // otherwise, find the ID and graph that show
        $.ajax({
            url: '/imdbgraph/Home/GetShowId?showTitle=' + showValue,
            success: function (result) {
                if (result == '') {
                    // if no ID was found, redirect to the search page URL
                    window.location.href = searchUrl + showValue;
                } else {
                    window.location.href = graphUrl + result;
                }
            }, complete: function () {
                $('#graphButton').removeAttr('disabled');
                $('#buttonText').show();
                $('#buttonLoader').hide();
            }
        });
    }
});