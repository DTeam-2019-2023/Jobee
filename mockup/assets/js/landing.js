var currentId = null;
var containerHeight = $(".container").height();
var heightScroll = $(".container").scrollTop();
$(document).on("scroll", function (e) {
  //get offest class container scroll top
  heightScroll = $(this).scrollTop();
  console.log(heightScroll);
  // var index = Math.floor(heightScroll / containerHeight);
  // currentId = $(".panel").eq(index).attr("id");
  // $(".active").removeClass("active");
  // $(".slider li").eq(index).addClass("active");
  // window.location.hash = currentId;
});
// Add smooth scrolling to all links
$("li").on("click", "a", function (event) {
  // Make sure this.hash has a value before overriding default behavior
  console.log(this.hash);

  if (this.hash !== "" && currentId != this.hash) {
    currentId = this.hash;
    // Prevent default anchor click behavior
    event.preventDefault();
    // Store hash
    var hash = this.hash;

    // Using jQuery's animate() method to add smooth page scroll
    // The optional number (800) specifies the number of milliseconds it takes to scroll to the specified area
    console.log($(hash).offset().top);
    $("html, body").animate(
      {
        scrollTop: $(hash).offset().top,
      },
      600,
      function () {
        // Add hash (#) to URL when done scrolling (default click behavior)
        window.location.hash = hash;
      }
    );
    $(".active").removeClass("active");
    $(this).parent().addClass("active");
  } // End if
});
