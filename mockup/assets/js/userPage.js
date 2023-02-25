//COMMON JS
$(".overlayAdd").on("click", function () {
  $(this).hide();
});

$(".btnCancel").on("click", function (e) {
  e.preventDefault();
  //   $(".overlayAdd").hide();
  $(this).parent().parent().parent().hide();
});

//==========================//

// JS EDU
$("#eduBtn").on("click", function () {
  $("#overlayEdu").show();
  $("#eduContainer").css("top", "50%");
  $("#eduContainer").css("left", "50%");
});

//JS PROJECT
$("#projectBtn").on("click", function () {
  $("#overlayProject").show();
  $("#projContainer").css("top", "50%");
  $("#projContainer").css("left", "50%");
});

//JS CERTIFICATE
$("#certBtn").on("click", function () {
  $("#overlayCert").show();
  $("#certContainer").css("top", "50%");
  $("#certContainer").css("left", "50%");
});
//JS ACTIVITY
$("#activityBtn").on("click", function () {
  $("#overlayActivity").show();
  $("#activityContainer").css("top", "50%");
  $("#activityContainer").css("left", "50%");
});

//JS AWARD
$("#awardBtn").on("click", function () {
  $("#overlayAward").show();
  $("#awardContainer").css("top", "50%");
  $("#awardContainer").css("left", "50%");
});
