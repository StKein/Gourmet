$(document).ready(function () {

    $("._order_wrapper").on("click", "._order_go_menu", function () {
        showPopup("/Orders/Menu");
    })

    $("._order_row").on("click", "._order_finish", function () {
        var $this = $(this);

        $.ajax({
            type: "POST",
            url: "/Orders/SetStatus",
            data: {
                id: $this.closest("._order_row").attr("data-id"),
                status: $this.attr("data-status")
            },
            success: function (_response) {
                $this.closest("._order_finish_wrapper").html(_response);
            }
        })
    })

})