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
  $("#deleteNotify").show();
});

$(".btn--No").on("click", function () {
  $(".overlayNotifyContainer").hide();
});

$(".acceptAction").on("click", function () {
  $("#verifyNotify").show();
});

$("tr td:nth-child(4) span").on("click", function () {
  $(this).css("white-space", "inherit");
  $(this).toggleClass("moreInfo");
});
