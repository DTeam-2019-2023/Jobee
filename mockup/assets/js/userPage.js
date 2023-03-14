//COMMON JS
$(".overlayPopup").on("click", function () {
  // $(this).hide().trigger("hide");
});

//update avatar
$(".avartar > span").on("click", function () {
  $("#avt-update").trigger("click");
});

$(".btnCancel").on("click", function (e) {
  e.preventDefault();
  //   $(".overlayPopup").hide().trigger("hide");
  $(this).parent().parent().parent().hide().trigger("hide");
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
  $(this).hide().trigger("hide");
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

$(".btn--No").on("click", function () {
  $(".overlayNotifyContainer,.overlayPopup").hide().trigger("hide");
});

$(".btnUpdate").on("click", function (e) {
  e.preventDefault();
  // $("#successNotify").show(); //=> THIS IS OPTIONAL
  $("#failNotify").show(); // OPTIONAL TOO
  // $(this).parent().parent().parent().show();
});

$(".btnTryagain").on("click", function (e) {
  e.preventDefault();
  $(".overlayNotifyContainer").hide().trigger("hide");
});
// Update Profile popup
$("#updateProfile").on("click", function (e) {
  e.preventDefault();
  $("#overlayUpdateProfile").show();
});

// SEND VERIFY REQUEST
$(".verifyIcon").on("click", function () {
  $("#verifyNotify").show();
});

//Account setting
$(".dropdownSetting>ul>li>a").on("click", function (e) {
  e.preventDefault();
  if (/^#.{1,}$/.test(this.hash)) {
    console.log(this.hash);
    $(this.hash).show();
  } else {
    //redirect
    console.log("redirect");
    console.log($(this));
    window.location.href = this.href;
  }
});

//Delete confirm
function deleteItem(id) {
  $("#deleteNotify").trigger("confirmDelete", id);
}

//confirm
$("#deleteNotify").on("confirmDelete", function (e, id) {
  const input = $("<input type='hidden' name='id'>");
  input.val(id);
  $("#deleteNotify>form").append(input);
  console.log($("#deleteNotify>form"));
  $("#deleteNotify").show();
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
//post using jquery
