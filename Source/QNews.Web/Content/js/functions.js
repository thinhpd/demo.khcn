// Browser detection for when you get desperate. A measure of last resort.

// http://rog.ie/post/9089341529/html5boilerplatejs
// sample CSS: html[data-useragent*='Chrome/13.0'] { ... }

// Uncomment the below to use:
// var b = document.documentElement;
// b.setAttribute('data-useragent',  navigator.userAgent);
// b.setAttribute('data-platform', navigator.platform);


function initPage(){

	// your functions go here
	$(function() {
		$('.newsticker').newsTicker({
			row_height: 20,
			max_rows: 1,
			speed: 600,
			direction: 'up',
			duration: 4000,
			autostart: 1,
			pauseOnHover: 1
		});
	});
	console.log('page loaded');

    $('.slideranh').owlCarousel({
        loop: false,
            margin: 10,
            autoplay: true,
            autoplayTimeout: 5000,
            autoplayHoverPause: true,
            responsiveClass: true,
            nav: false,
            dots: false,
            responsive: {
                0: {
                    items: 1,
                    nav: true
                },
                600: {
                    items: 3,
                    nav: false
                },
                1000: {
                    items: 4,
                    nav: false,
                    loop: true,
                    margin: 10,
                    dots: false
                }
            }
    });
    $('.sliderdvtg').owlCarousel({
        items: 4,
        loop: false,
        margin: 20,
        padding: 30,
        autoplay: true,
        autoplayTimeout: 5000
    });
};