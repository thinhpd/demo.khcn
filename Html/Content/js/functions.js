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
	$(window).scroll(function (event) {
		var scroll = $(window).scrollTop();
		console.log(scroll);
		if(scroll>150){
			$('#menutop').addClass('menutopfix');
		}else{
			$('#menutop').removeClass('menutopfix');
		}
		// Do something
	});
	console.log('page loaded');

};