$(document).ready(function () {

    $("._table_row").on("click", "._table_seat", function () {
        var table_row = $(this).closest("._table_row");
        if (table_row.find("._table_order_wrapper").attr("data-id") > 0
                && !confirm("Вы действительно хотите освободить стол?"
                                + " Текущий заказ будет отменен.")) {
            return false;
        }
        var new_free_status;
        var new_free_img;
        if ($(this).attr("data-free") == "0") {
            new_free_status = 1;
            new_free_img = "good.png";
        } else {
            new_free_status = 0;
            new_free_img = "bad.png";
        }
        $.ajax({
            type: "POST",
            url: "/Tables/SwitchSeated",
            data: {
                free: new_free_status,
                id: table_row.attr("data-id")
            },
            success: function (_response) {
                table_row.html(_response);
            }
        })
    })

})