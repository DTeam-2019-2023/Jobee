$(".manageItem").on("click", function (e) {
  e.preventDefault();
  $("a.active").removeClass("active");
  $(".show").removeClass("show");
  $(this).addClass("active");
  $(this.hash).addClass("show");
});

$("#verifyRequest tr:not(:first-child)").on("click", function (e) {
  $("tr:not(:first-child).selected").removeClass("selected");
  $(this).addClass("selected");
});

$("tr td:first-child span").on("click", function () {
  $(this).css("white-space", "inherit");
  $(this).toggleClass("moreInfo");
});
//popup

$("#btnCreate").on("click", function (e) {
  e.preventDefault();
  $("#overlayCreateAdmin").show();
});

$(".btnCancel").on("click", function (e) {
  e.preventDefault();
  //   $(".overlayPopup").hide();
  $(this).parent().parent().parent().hide();
});

//show popup confirm delete
$(".rejectAction").on("click", function () {
  verify($(this).attr("target"), false);
});

$(".btn--No").on("click", function () {
  $(".overlayNotifyContainer").hide().trigger("hide");
});
//accept
$('.acceptAction').on('click', function () {
    var parent = $(this).closest('.listItemVerify');
    var id = $(parent).attr('id');
    $("#verifyNotify").trigger("confirmVerify", { id });
})

$("#verifyNotify").on("confirmVerify", function (e, data) {
    const idVerify = $(`<input type="hidden" name="id">`);

    idVerify.val(data.id);

    $("#verifyNotify>form").append(idVerify);

    console.log($("#verifyNotify>form"));
    $("#verifyNotify").show();
}).on("submit", "form", function (e) {
    e.preventDefault();
    var data = $(this).serialize();
    var url = $(this).attr("action");
    $.post(url, data, function (data) {
        if (data.status == "success") {
            //success
            $(`#${data.id}`).parent().remove();
            $("#verifyNotify").hide().trigger("hide");
            $("#successNotify").show();
        } else {
            //fail
            $("#verifyNotify").hide().trigger("hide");
            $("#failNotify").show();
        }
    }).fail(function () {
        alert("error");
    })
});

$("#deleteNotify").on("confirmDelete", function (e, id) {
  const input = $("<input type='hidden' name='id'>");
  input.val(id);
  $("#deleteNotify>form").append(input);
  console.log($("#deleteNotify>form"));
  $("#deleteNotify").show();
});

$("tr td:nth-child(4) span").on("click", function () {
  $(this).css("white-space", "inherit");
  $(this).toggleClass("moreInfo");
});

$(".overlayNotifyContainer").on("hide", function (params) {
  $(this).find("form>input[type='hidden']").remove();
});

$(".closeIcon").on("click", function () {
  $(".overlayNotifyContainer").hide().trigger("hide");
});

$(".btn.btn--Success").on("click", function () {
  $(".overlayNotifyContainer,.overlayPopup").hide().trigger("hide");
});

$(".btnTryagain").on("click", function (e) {
  e.preventDefault();
  $(".overlayNotifyContainer").hide().trigger("hide");
});
