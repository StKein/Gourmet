function showPopup(url, data, callback) {
    if (!data) {
        data = {};
    }
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        success: function (_response) {
            $("._popup_background").show();
            $("._popup_window").html(_response).show();
        }
    })
}

function closePopup() {
    $("._popup_background").hide();
    $("._popup_window").html("").hide();
}


$(document).ready(function () {

    $("._popup_window").on("click", "._popup_close", closePopup);

})