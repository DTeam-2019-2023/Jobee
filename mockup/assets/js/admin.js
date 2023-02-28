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
