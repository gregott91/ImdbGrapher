$(document).ready(function () {
    $('#graphButton').click(function () {
        graphShow($('#idOrTitle').val());
    })

    $('#idOrTitle').keyup(function (event) {
        if (event.keyCode === 13) {
            $('#graphButton').click();
        }
    });

    function graphShow(showValue) {
        if (showValue == null || showValue == undefined || showValue == '') {
            $('#idOrTitleGroup').addClass('has-error');
            $('#helpMessage').text('Please enter a value');
            $('#helpMessage').show();
            return;
        }

        $('#buttonText').hide();
        $('#buttonLoader').show();
        $('#graphButton').attr('disabled', true);
        $('#helpMessage').text('');
        $('#helpMessage').hide();
        $('#idOrTitleGroup').removeClass('has-error');

        var regex = /tt[0-9]{7}/;

        if (regex.test(showValue) && showValue.length == 9) {
            window.location.href = '/imdbgraph/Home/GraphShow?id=' + showValue;
        }

        $.ajax({
            url: '/imdbgraph/Home/GetShowId?showTitle=' + showValue,
            success: function (result) {
                if (result == '') {
                    window.location.href = '/imdbgraph/Home/SearchShows?showTitle=' + showValue;
                } else {
                    window.location.href = '/imdbgraph/Home/GraphShow?id=' + result;
                }
            }, complete: function () {
                $('#graphButton').removeAttr('disabled');
                $('#buttonText').show();
                $('#buttonLoader').hide();
            }
        });
    }
});