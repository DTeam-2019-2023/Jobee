//COMMON JS

//update avatar
$(".avartar > span").on("click", function () {
    $("#avt-update").trigger("click");
});

$(document).on("click", ".btnCancel", function (e) {
    e.preventDefault();
    //   $(".overlayPopup").hide().trigger("hide");
    $(this).parent().parent().parent().hide().trigger("hide");
})

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

$(".editContentNavAction").on("click", function (e) {
    e.preventDefault();
    const idvalue = $(this).parent().attr("iditem");

    $.get(this.href, { id: idvalue }, function (data) {
        $("#EditContentNav").html(data);
        $("#EditContentNav").show();
    });

});

$("#EditContentNav").on("submit", "form", function (e) {
    e.preventDefault();
    var data = $(this).serialize();
    var url = $(this).attr("action");
    var method = $(this).attr("method");
    $.ajax({
        url: url,
        type: method,
        data: data,
        success: function (data, res) {
            console.log(res);
            if (res == "success") {
                //success
                $("#successNotify").show();
            } else {
                //fail
                $("#EditContentNav").html(data);
                $("#failNotify").show();
            }
        },
        error: function (err) {
            console.log(err);
        },
    });
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

$("a.viewContentNavAction").on("click", function (e) {
    e.preventDefault();
    const idvalue = $(this).parent().attr("iditem");

    $.post(this.href, { id: idvalue }, function (data) {
        $("#ViewContentNav").html(data);
        $("#ViewContentNav").show();
    });
});

//post using jquery

//$("form").on("submit", function (e) {
//    //empty data affter send form
//    e.preventDefault();
//    var data = $(this).serialize();
//    var url = $(this).attr("action");
//    var method = $(this).attr("method");
//    $.ajax({
//        url: url,
//        type: method,
//        data: data,
//        success: function (res) {
//            console.log(res);
//            if (res.status == 200) {
//                //success
//                $("#successNotify").show();
//            } else {
//                //fail
//                $("#failNotify").show();
//            }
//        },
//        error: function (err) {
//            console.log(err);
//        },
//    });

//});
