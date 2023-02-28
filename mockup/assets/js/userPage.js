//COMMON JS
$(".overlayPopup").on("click", function () {
  // $(this).hide();
});

$(".btnCancel").on("click", function (e) {
  e.preventDefault();
  //   $(".overlayPopup").hide();
  $(this).parent().parent().parent().hide();
});

//==========================//

// JS EDU
$("#eduBtn").on("click", function () {
  $("#overlayEdu").show();
});

//JS PROJECT
$("#projectBtn").on("click", function () {
  $("#overlayProject").show();
});

//JS CERTIFICATE
$("#certBtn").on("click", function () {
  $("#overlayCert").show();
});
//JS ACTIVITY
$("#activityBtn").on("click", function () {
  $("#overlayActivity").show();
});

//JS AWARD
$("#awardBtn").on("click", function () {
  $("#overlayAward").show();
});

//=================================//
//COMMON VIEW POPUP CLOSE
$(".overlayContentPopup").on("click", function () {
  $(this).hide();
});

//VIEW EDU
$("#expandEdu").on("click", function () {
  $("#overlayViewEducation").show();
});

//VIEW PROJECT
$("#expandProject").on("click", function () {
  $("#overlayViewProject").show();
});
//VIEW CERTIFICATE
$("#expandCertificate").on("click", function () {
  $("#overlayViewCertificate").show();
});
//VIEW ACTIVITY
$("#expandActivity").on("click", function () {
  $("#overlayViewActivity").show();
});

//VIEW AWARD
$("#expandAward").on("click", function () {
  $("#overlayViewAward").show();
});

//COMMON EDIT POPUP
$("#editEdu").on("click", function () {
  $("#overlayEditEdu").show();
});

$("#editProject").on("click", function () {
  $("#overlayEditProject").show();
});

$("#editCertificate").on("click", function () {
  $("#overlayEditCert").show();
});

$("#editActivity").on("click", function () {
  $("#overlayEditActivity").show();
});

$("#editAward").on("click", function () {
  $("#overlayEditAward").show();
});

//=============POPUP DELETE=============
$(".overlayNotifyContainer").on("click", function () {
  $(this).hide();
});

$(".btn--No").on("click", function () {
  $(".overlayNotifyContainer").hide();
});

$(".delContent").on("click", function () {
  $("#deleteNotify").show();
});

$(".btnUpdate").on("click", function (e) {
  e.preventDefault();
  // $("#successNotify").show(); //=> THIS IS OPTIONAL
  $("#failNotify").show(); // OPTIONAL TOO
  // $(this).parent().parent().parent().show();
});

$(".btnTryagain").on("click", function (e) {
  e.preventDefault();
});

$("#updateProfile").on("click", function (e) {
  e.preventDefault();
  $("#overlayUpdateProfile").show();
});
