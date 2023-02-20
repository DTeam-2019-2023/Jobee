var currentId = null;
var containerHeight = $(".container").height();
var heightScroll = $(".container").scrollTop();
$(".container").on("scroll", function (e) {
  heightScroll = $(this).scrollTop();
  var index = Math.floor(heightScroll / containerHeight);
  currentId = $(".panel").eq(index).attr("id");
  $(".active").removeClass("active");
  $(".slider li").eq(index).addClass("active");
  window.location.hash = currentId;
});
// Add smooth scrolling to all links
$("li").on("click", "a", function (event) {
  $(".active").removeClass("active");
  $(this).parent().addClass("active");
  // Make sure this.hash has a value before overriding default behavior
  if (this.hash !== "") {
    // Prevent default anchor click behavior
    event.preventDefault();
    // Store hash
    var hash = this.hash;

    // Using jQuery's animate() method to add smooth page scroll
    // The optional number (800) specifies the number of milliseconds it takes to scroll to the specified area
    $("html, body").animate(
      {
        scrollTop: $(hash).offset().top,
      },
      100,
      function () {
        // Add hash (#) to URL when done scrolling (default click behavior)
        window.location.hash = hash;
      }
    );
  } // End if
});
