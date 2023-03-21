//#region View Navigation

$("a.viewContentNavAction").on("click", function (e) {
    e.preventDefault();
    const idvalue = $(this).parent().parent().attr("iditem");
    $.post(this.href, { id: idvalue }, function (data) {
        $("#ViewContentNav").html(data);
        $("#ViewContentNav").show();
    });
});
//#endregion
$(".overlayContentPopup").on("click", function () {
    $(this).hide().trigger("hide");
});