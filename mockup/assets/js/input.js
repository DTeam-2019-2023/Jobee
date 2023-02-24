$("input").on("blur", function () {
  if ($(this).val() != "") {
    $(this).next(".inputSpan").addClass("focus");
  } else {
    $(this).next(".inputSpan").removeClass("focus");
  }
});

$("input").on("focus", function () {
  if ($(this).val() != "") {
    $(this).next(".inputSpan").addClass("no-animation");
  } else {
    $(this).next(".inputSpan").removeClass("no-animation");
    $(this).next(".inputSpan").addClass("focus");
  }
});
