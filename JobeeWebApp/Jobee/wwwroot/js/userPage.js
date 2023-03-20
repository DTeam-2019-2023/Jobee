//#region Golble
$(document).on("click", ".btnCancel", function (e) {
    e.preventDefault();
    //   $(".overlayPopup").hide().trigger("hide");
    $(this).parent().parent().parent().hide().trigger("hide");
})

$(".btn--No").on("click", function () {
    $(".overlayNotifyContainer,.overlayPopup").hide().trigger("hide");
});

$(".btnTryagain").on("click", function (e) {
    e.preventDefault();
    $(".overlayNotifyContainer").hide().trigger("hide");
});

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

$(".overlayNotifyContainer").on("hide", function (params) {
    $(this).find("form>input[type='hidden']").remove();
});

$(".closeIcon").on("click", function () {
    $(".overlayNotifyContainer").hide().trigger("hide");
});

$(".btn.btn--Success").on("click", function () {
    $(".overlayNotifyContainer,.overlayPopup").hide().trigger("hide");
});
//#endregion

//#region Avatar
$(".avartar > span").on("click", function () {
    $("#avt-update").trigger("click");
});
//#endregion

//#region Popup
$(".overlayContentPopup").on("click", function () {
    $(this).hide().trigger("hide");
});
//#endregion

//#region Add Navigation
//#region Show add popup

$("#eduBtn").on("click", function () {
    $("#overlayEdu").show();
});

//$("#eduBtn").on("click", function () {
//    $("#overlayEdu").show();
//});

//JS ACTIVITY
$("#activityBtn").on("click", function () {
    $("#overlayActivity").show();
});
//JS ACTIVITY
//$("#activityBtn").on("click", function () {
//    $("#overlayActivity").show();
//});

//JS PROJECT
$("#projectBtn").on("click", function () {
    $("#overlayProject").show();
});

//JS CERTIFICATE
$("#certBtn").on("click", function () {
    $("#overlayCert").show();
});


//JS AWARD
$("#awardBtn").on("click", function () {
    $("#overlayAward").show();
});
//#endregion
//#endregion

//#region Edit Navigation
//#region Get pupup edit

$(".editContentNavAction").on("click", function (e) {
    e.preventDefault();
    const idvalue = $(this).parent().parent().attr("iditem");

    $.get(this.href, { id: idvalue }, function (data) {
        var newComponent = $(data);
        $("#EditContentNav").html(newComponent);
        setTimeout(function () {
            newComponent.find("input").focus();
            newComponent.find("input[type='date']").each(function () {
                //check if their value is not null
                if ($(this).val()) {
                    //add focus class to their sibling span element
                    $(this).siblings("span.inputSpan").toggleClass("focus", true);
                } else {
                    //remove focus class from their sibling span element
                    $(this).siblings("span.inputSpan").toggleClass("focus", false);
                }
            });
        }, 4);
        $("#EditContentNav").show();
    });
});
//#endregion
//#region Send form Edit Navigation
$("#EditContentNav").on("submit", "form", function (e) {
    e.preventDefault();
    var data = $(this).serialize();
    var url = $(this).attr("action");
    var method = $(this).attr("method");
    $.ajax({
        url: url,
        type: method,
        data: data,

    }).done(function (rep) {
        console.log(rep);
        if (rep.status == "success") {
            //success
            $("#EditContentNav").hide();
            $("#successNotify").show();
        } else {
            //fail
            $("#EditContentNav").html(data);
            $("#failNotify").show();
        }
    }).fail(function () {
        alert("error");
    });
});
//#endregion
//#endregion

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

//#region Delete Navigation
function deleteItem(typeNav, id) {
    $("#deleteNotify").trigger("confirmDelete", { typeNav, id });
}

$("#deleteNotify").on("confirmDelete", function (e, data) {
    const InputidItem = $(`<input type="hidden" name="id">`);
    const InputtypeNav = $(`<input type="hidden" name="navType">`);

    InputidItem.val(data.id);
    InputtypeNav.val(data.typeNav);
    
    $("#deleteNotify>form").append(InputidItem);
    $("#deleteNotify>form").append(InputtypeNav);

    console.log($("#deleteNotify>form"));
       $("#deleteNotify").show();
}).on("submit", "form", function (e) {
    e.preventDefault();
    var data = $(this).serialize();
    var url = $(this).attr("action");
    $.post(url, data, function (data) {
        if (data.status == "success") {
            //success
            $(`#${data.id}`).remove();
            $("#deleteNotify").hide().trigger("hide");
            $("#successNotify").show();
        } else {
            //fail
            $("#deleteNotify").hide().trigger("hide");
            $("#failNotify").show();
        }
    }).fail(function () {
        alert("error");
    })
});
//#endregion

//#region Profile
$("#updateProfile").on("click", function (e) {
    e.preventDefault();
    $("#overlayUpdateProfile").show();
});
//#endregion

//#region Verify
$(".verifyIcon").on("click", function () {
    $("#verifyNotify").show();
});
//#endregion

