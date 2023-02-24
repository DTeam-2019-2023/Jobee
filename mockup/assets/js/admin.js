$(".manageItem").on("click", function (e) {
  e.preventDefault();
  $("a.active").removeClass("active");
  $(".show").removeClass("show");
  $(this).addClass("active");
  $(this.hash).addClass("show");
});

$("#verifyRequest tr").on("click", function (e) {
  $("tr.selected").removeClass("selected");
  $(this).addClass("selected");
});

$("tr td:first-child span").on("click", function () {
  $(this).css("white-space", "inherit");
  $(this).toggleClass("moreInfo");
});
