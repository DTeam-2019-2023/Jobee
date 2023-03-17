// Define variables for each parameter
var inputTypeSelect = "input, textarea";
var inputSpanOrAreaSpan = "span.inputSpan, span.areaSpan";
var inputTxt = ".inputTxt";

// Use them in the event handlers
function handleInputTypeSelectBlur(e) {
    $(this).next(inputSpanOrAreaSpan).toggleClass("focus", $(this).val() != "");
}

function handleInputTypeSelectFocus(e) {
    // Chain methods on the same selector
    $(this).next(inputSpanOrAreaSpan)
        .toggleClass("no-animation", $(this).val() != "")
        .addClass("focus");
}

function handleClickOnInputOrAreaSpan(e) {
    $(this).prev(inputTypeSelect).trigger("focus");
}

function handleInputOnInputTxt(e) {
    this.style.height = "auto";
    this.style.height = this.scrollHeight + "px";
}

function handleChangeOnAnyElement(e) {
    if ($(this).val() != "") {
        $(this).next(inputSpanOrAreaSpan).addClass("focus");
    }
}

// Pass the named functions as arguments to the .on() method
$(document).ready(function () {
    $(document)
        .on("blur", inputTypeSelect, handleInputTypeSelectBlur)
        .on("focus", inputTypeSelect, handleInputTypeSelectFocus)
        .on("click", inputSpanOrAreaSpan, handleClickOnInputOrAreaSpan)
        .on("input", inputTxt, handleInputOnInputTxt)
        .on("change", handleChangeOnAnyElement);
    $(".inputTextContainer input").each(function () {
        //check if their value is not null
        $(this).siblings("span.inputSpan").toggleClass("focus", $(this).val() != "");

        //if ($(this).val()) {
        //    //add focus class to their sibling span element
        //    $(this).siblings("span.inputSpan").toggleClass("focus", true);
        //} else {
        //    //remove focus class from their sibling span element
        //    $(this).siblings("span.inputSpan").toggleClass("focus", false);
        //}
    });
    //select all textarea elements inside inputTextareaContainer divs
    $(".inputTextareaContainer textarea").each(function () {
        //check if their value is not null
        $(this).siblings("span.areaSpan").toggleClass("focus", $(this).val() != "");

        //if ($(this).val()) {
        //    //add focus class to their sibling span element
        //    $(this).siblings("span.areaSpan").toggleClass("focus", true);
        //} else {
        //    //remove focus class from their sibling span element
        //    $(this).siblings("span.areaSpan").toggleClass("focus", false);
        //}
    });
});