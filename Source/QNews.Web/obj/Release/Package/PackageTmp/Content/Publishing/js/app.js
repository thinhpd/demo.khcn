
$(document).ready(function () {
    //var limitRight = $('.box-gallery-home').offset().top - $('.right-320').outerHeight() - 10;
    //$(".rightContaier").scrollToFixed({
    //    top:0,
    //    limit: limitRight,
    //    zIndex: 3
    //});

    $('.flexslider').flexslider({
        animation: "slide",
        animationLoop: false,
        itemWidth: 232,
        itemMargin: 0
    });

    $("#galleryHome").bind('change', function () {
        var url = $(this).val(); // get selected value
        if (url) { // require a URL
            window.location = url; // redirect
        }
        return false;
    });

});

$(function () {
    setTimeout(function () {
        $(".box-news-2cols").each(function (index, layout) {
            var maxHeight = 0;
            var maxHeight_top = 0;

            //Set chiá»u cao cho cĂ¡c báº£n tin Ä‘áº§u
            $(layout).find(".box-news-home .content").each(function (index_item, newItem) {
                maxHeight_top = Math.max(maxHeight_top, $(newItem).height());
            });
            $(layout).find(".box-news-home .content").height(maxHeight_top);

            //Set chiá»u cao cho cĂ¡c khá»‘i
            $(layout).find(".box-news-home").each(function (index_item, newItem) {
                maxHeight = Math.max(maxHeight, $(newItem).height());
            });
            $(layout).find(".box-news-home").height(maxHeight);
        });
    }, 20);
});

$(document).ready(function () {

    $(".hot-news .right ul li a").hover(function () {

        var _html = $(this).attr("data-html");
        var _image = $(this).attr("data-icon");
        $(".hot-news .right ul li a").removeClass("actived");
        $(this).addClass("actived");
        $(".hot-news .hot-news-cover").hide().html(_html).fadeIn('fast');
        $(".hot-news .hot-news-image img").hide().attr("src", _image).fadeIn('fast');
    });

    setTimeout(function () {
        getWeather(58);
    }, 300);
});


function getWeather(id) {
    var _urlAjax = "/Ajax/Weather/" + id;
    $("#weatherContainer").load(_urlAjax).hide().fadeIn('fast');
}

function siteLink(link) {
    if (link != '') {
        window.open(link, '_blank');
    }
}


function playVideo(source) {
	var video = document.getElementById("video_player");  
	
	$("#video_player source").attr("src", source);
	video.load();
	video.play();
}

function playAudio(source) {
	var video = document.getElementById("video_player");  
	
	$("#video_player source").attr("src", source);
	video.load();
	video.play();
}

$(function () {
	$(window).scroll(function () {
		if ($(this).scrollTop() != 0) {
			$('#bttop').fadeIn();
		} else {
			$('#bttop').fadeOut();
		}
	});
	$(window).load(function () {
		if ($(this).scrollTop() != 0) {
			$('#bttop').fadeIn();
		} else {
			$('#bttop').fadeOut();
		}
	});
	$('#bttop').click(function () {
		$('body,html').animate({ scrollTop: 0 }, 300);
	});


});
