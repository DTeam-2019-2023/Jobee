$("input, textarea")
  .on("blur", function () {
    if ($(this).val() != "") {
      $(this).next(".inputSpan, .areaSpan").addClass("focus");
    } else {
      $(this).next(".inputSpan, .areaSpan").removeClass("focus");
    }
  })
  .on("focus", function () {
    if ($(this).val() != "") {
      $(this).next(".inputSpan, .areaSpan").addClass("no-animation");
    } else {
      $(this).next(".inputSpan, .areaSpan").removeClass("no-animation");
      $(this).next(".inputSpan, .areaSpan").addClass("focus");
    }
  });

$("span.inputSpan, span.areaSpan").on("click", function () {
  $(this).prev("input, textarea").trigger("focus");
});

$(".inputTxt").on("input", function () {
  this.style.height = "auto";
  this.style.height = this.scrollHeight + "px";
});
