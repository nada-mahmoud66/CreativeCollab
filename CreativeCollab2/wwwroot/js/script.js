
jQuery(document).ready(function ($) {

    "use strict";


    //------- Notifications Dropdowns
    $('.top-area > .setting-area > li').on("click", function () {
        $(this).siblings().children('div').removeClass('active');
        $(this).children('div').addClass('active');
        return false;
    });
    //------- remove class active on body
    $("body *").not('.top-area > .setting-area > li').on("click", function () {
        $(".top-area > .setting-area > li > div").removeClass('active');
    });


    //--- user setting dropdown on topbar	
    $('.user-img').on('click', function () {
        $('.user-setting').toggleClass("active");
        return false;
    });

    //--- side message box	
    //$('.friendz-list > li, .chat-users > li').on('click', function () {
    //    $('.chat-box').addClass("show");
    //    return false;
    //});
    //$('.close-mesage').on('click', function () {
    //    $('.chat-box').removeClass("show");
    //    return false;
    //});

    //------ scrollbar plugin
    if ($.isFunction($.fn.perfectScrollbar)) {
        $('.dropdowns, .twiter-feed, .invition, .followers, .chatting-area, .peoples, #people-list, .chat-list > ul, .message-list, .chat-users, .left-menu').perfectScrollbar();
    }

    /*--- socials menu scritp ---*/
    $('.trigger').on("click", function () {
        $(this).parent(".menu").toggleClass("active");
    });

    /*--- emojies show on text area ---*/
    $('.add-smiles > span').on("click", function () {
        $(this).parent().siblings(".smiles-bunch").toggleClass("active");
    });

    // delete notifications
    $('.notification-box > ul li > i.del').on("click", function () {
        $(this).parent().slideUp();
        return false;
    });

    /*--- socials menu scritp ---*/
    $('.f-page > figure i').on("click", function () {
        $(".drop").toggleClass("active");
    });

    //===== Search Filter =====//
    (function ($) {
        // custom css expression for a case-insensitive contains()
        jQuery.expr[':'].Contains = function (a, i, m) {
            return (a.textContent || a.innerText || "").toUpperCase().indexOf(m[3].toUpperCase()) >= 0;
        };

        function listFilter(searchDir, list) {
            var form = $("<form>").attr({ "class": "filterform", "action": "#" }),
                input = $("<input>").attr({ "class": "filterinput", "type": "text", "placeholder": "Search Contacts..." });
            $(form).append(input).appendTo(searchDir);

            $(input)
                .change(function () {
                    var filter = $(this).val();
                    if (filter) {
                        $(list).find("li:not(:Contains(" + filter + "))").slideUp();
                        $(list).find("li:Contains(" + filter + ")").slideDown();
                    } else {
                        $(list).find("li").slideDown();
                    }
                    return false;
                })
                .keyup(function () {
                    $(this).change();
                });
        }

        //search friends widget
        $(function () {
            listFilter($("#searchDir"), $("#people-list"));
        });
    }(jQuery));

    //search joined 
    (function ($) {
        // custom css expression for a case-insensitive contains()
        jQuery.expr[':'].Contains = function (a, i, m) {
            return (a.textContent || a.innerText || "").toUpperCase().indexOf(m[3].toUpperCase()) >= 0;
        };

        function listFilter(searchDir, list) {
            var form = $("<form>").attr({ "class": "filterform", "action": "#" }),
                input = $("<input>").attr({ "class": "filterinput", "type": "text", "placeholder": "Search ..." });
            $(form).append(input).appendTo(searchDir);

            $(input)
                .change(function () {
                    var filter = $(this).val();
                    if (filter) {
                        $(list).find("li:not(:Contains(" + filter + "))").slideUp();
                        $(list).find("li:Contains(" + filter + ")").slideDown();
                    } else {
                        $(list).find("li").slideDown();
                    }
                    return false;
                })
                .keyup(function () {
                    $(this).change();
                });
        }

        //search friends widget
        $(function () {

            $('.searchjoined').each(function () {
                var searchJoined = $(this);
                var nearbyContct = searchJoined.closest('.central-meta').find('.nearby-contct');
                listFilter(searchJoined, nearbyContct);
            });
        });
    }(jQuery));
    // new
    $('#filterInput').on('input', function () {
        const filter = $(this).val().toLowerCase();
        $('.send-ppl li').each(function () {
            const listItem = $(this);
            const text = listItem.text().toLowerCase();
            if (text.includes(filter)) {
                listItem.show();
            } else {
                listItem.hide();
            }
        });
    });
    //new
    //progress line for page loader
    $('body').show();
    NProgress.start();
    setTimeout(function () { NProgress.done(); $('.fade').removeClass('out'); }, 2000);

    //--- bootstrap tooltip	
    $(function () {
        $('[data-toggle="tooltip"]').tooltip();
    });

    // Sticky Sidebar & header
    if ($(window).width() < 769) {
        jQuery(".sidebar").children().removeClass("stick-widget");
    }

    if ($.isFunction($.fn.stick_in_parent)) {
        $('.stick-widget').stick_in_parent({
            parent: '#page-contents',
            offset_top: 60,
        });


        $('.stick').stick_in_parent({
            parent: 'body',
            offset_top: 0,
        });

    }

    /*--- topbar setting dropdown ---*/
    $(".we-page-setting").on("click", function () {
        $(".wesetting-dropdown").toggleClass("active");
    });

    /*--- topbar toogle setting dropdown ---*/
    $('#nightmode').on('change', function () {
        if ($(this).is(':checked')) {
            // Show popup window
            $(".theme-layout").addClass('black');
        }
        else {
            $(".theme-layout").removeClass("black");
        }
    });

    //chosen select plugin
    if ($.isFunction($.fn.chosen)) {
        $("select").chosen();
    }

    //----- add item plus minus button
    if ($.isFunction($.fn.userincr)) {
        $(".manual-adjust").userincr({
            buttonlabels: { 'dec': '-', 'inc': '+' },
        }).data({ 'min': 0, 'max': 20, 'step': 1 });
    }

    if ($.isFunction($.fn.loadMoreResults)) {
        $('.loadMore').loadMoreResults({
            displayedItems: 3,
            showItems: 1,
            button: {
                'class': 'btn-load-more',
                'text': 'Load More'
            }
        });
    }
    //===== owl carousel  =====//
    if ($.isFunction($.fn.owlCarousel)) {
        $('.sponsor-logo').owlCarousel({
            items: 6,
            loop: true,
            margin: 30,
            autoplay: true,
            autoplayTimeout: 1500,
            smartSpeed: 1000,
            autoplayHoverPause: true,
            nav: false,
            dots: false,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 3,
                },
                600: {
                    items: 3,

                },
                1000: {
                    items: 6,
                }
            }

        });
    }

    // slick carousel for detail page
    if ($.isFunction($.fn.slick)) {
        $('.slider-for-gold').slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            slide: 'li',
            fade: false,
            asNavFor: '.slider-nav-gold'
        });

        $('.slider-nav-gold').slick({
            slidesToShow: 3,
            slidesToScroll: 1,
            asNavFor: '.slider-for-gold',
            dots: false,
            arrows: true,
            slide: 'li',
            vertical: true,
            centerMode: true,
            centerPadding: '0',
            focusOnSelect: true,
            responsive: [
                {
                    breakpoint: 768,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 1,
                        infinite: true,
                        vertical: false,
                        centerMode: true,
                        dots: false,
                        arrows: false
                    }
                },
                {
                    breakpoint: 641,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 1,
                        infinite: true,
                        vertical: true,
                        centerMode: true,
                        dots: false,
                        arrows: false
                    }
                },
                {
                    breakpoint: 420,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 1,
                        infinite: true,
                        vertical: false,
                        centerMode: true,
                        dots: false,
                        arrows: false
                    }
                }
            ]
        });
    }

    //---- responsive header

    $(function () {

        //	create the menus
        $('#menu').mmenu();
        $('#shoppingbag').mmenu({
            navbar: {
                title: 'General Setting'
            },
            offCanvas: {
                position: 'right'
            }
        });

        //	fire the plugin
        $('.mh-head.first').mhead({
            scroll: {
                hide: 200
            }

        });
        $('.mh-head.second').mhead({
            scroll: false
        });


    });

    //**** Slide Panel Toggle ***//
    $("span.main-menu").on("click", function () {
        $(".side-panel").addClass('active');
        $(".theme-layout").addClass('active');
        return false;
    });

    $('.theme-layout').on("click", function () {
        $(this).removeClass('active');
        $(".side-panel").removeClass('active');


    });


    // login & register form
    $('button.signup').on("click", function () {
        $('.login-reg-bg').addClass('show');
        return false;
    });

    $('.already-have').on("click", function () {
        $('.login-reg-bg').removeClass('show');
        return false;
    });

    //----- count down timer		
    if ($.isFunction($.fn.downCount)) {
        $('.countdown').downCount({
            date: '11/12/2018 12:00:00',
            offset: +10
        });
    }

    /** Post a Comment **/
    //// Event listener for replying to a comment
    //jQuery(".coment-area").on("click", ".we-reply", function (event) {
    //    event.preventDefault();

    //    var replyTextarea = jQuery('<textarea placeholder="reply to ......"></textarea>');


    //    // Create a new comment container for the reply
    //    var replyContainer = jQuery('<div class="reply-container"></div>');

    //    // Append the reply textarea and button to the reply container
    //    replyContainer.append(replyTextarea);

    //    // Find the parent comment and append the reply container to it
    //    var parentComment = jQuery(this).closest("li");
    //    var alreadyAppended = parentComment.data('replyAppended');

    //    if (!alreadyAppended) {
    //        // Append the reply textarea to the reply container
    //        replyContainer.append(replyTextarea);

    //        // Append the reply container to the parent comment
    //        parentComment.append(replyContainer);

    //        // Set a flag to indicate that the replyContainer has been appended
    //        parentComment.data('replyAppended', true);

    //        // Focus on the reply textarea
    //        replyTextarea.focus();
    //    }

    //    replyTextarea.css({
    //        'height': '45px',
    //        'background-color': '#f3f3f3'

    //    });

    //    replyContainer.css({
    //        'background-color': '#f3f3f3', // Change the background color to a desired value
    //        'border': 'none',
    //        'margin-top': '10px'     // Change the border to a desired style
    //        // Add more styles as needed
    //    });

    //    // Focus on the reply textarea




    //});

    //// Event listener for submitting a new comment or reply
    //jQuery(".coment-area").on("keydown", ".post-comt-box textarea, .reply-container textarea", function (event) {
    //    if (event.keyCode == 13) {
    //        event.preventDefault();
    //        //text i write in box
    //        var comment = jQuery(this).val();
    //        //the comment i reply to 
    //        var parentComment = jQuery(this).closest("li");
    //        var childComment = jQuery(this).closest(".we-comet");

    //        // Check if it's a reply or a new comment
    //        if (!(parentComment.hasClass("newcommentinbox"))) {
    //            if (parentComment.find('.reply_').length > 0) {
    //                console.log("1111111");

    //                // Create HTML for the reply comment

    //                var replyHTML = '<li><div class="comet-avatar"><img src="images/resources/comet-2.jpg" alt=""></div>' +
    //                    '<div class="we-comment"><div class="coment-head"><h5><a href="time-line.html"title="">alexendra dadrio</a></h5><span>1 month ago</span><a class="we-reply" href="#"title="Reply"><i class="fa fa-reply"></i></a></div><p>' + comment + '</p></div></li>';
    //                var newreply = parentComment.find('.postreply');
    //                // Append the reply HTML to the parent comment
    //                newreply.prepend(replyHTML);
    //                jQuery(this).remove();
    //            }
    //            else if (!(parentComment.find('.reply_').length > 0)) {
    //                console.log("333");
    //                var repliesContainer = jQuery('<ul class="reply_ postreply"</ul>');

    //                // Append the new comment to the replies container
    //                repliesContainer.append('<li>' +
    //                    '<div class="comet-avatar"><img src="images/resources/comet-1.jpg" alt=""></div>' +
    //                    '<div class="we-comment">' +
    //                    '<div class="coment-head"><h5><a href="time-line.html" title="">Your Name</a></h5>' +
    //                    '<span>Just now</span>' +
    //                    '<a class="we-reply" href="#" title="Reply"><i class="fa fa-reply"></i></a></div>' +
    //                    '<p>' + comment + '</p>' +

    //                    '</div></li>');

    //                // Append the replies container to the parent comment


    //                // Insert the new comment before the parent comment
    //                parentComment.append(repliesContainer);
    //                jQuery(this).remove();

    //            }
    //        }
    //        else {

    //            if (event.keyCode == 13) {
    //                var comment = jQuery(this).val();
    //                var parent = jQuery(".showmore").parent("li");
    //                var comment_HTML = '<li><div class="comet-avatar"><img src="images/resources/comet-1.jpg" alt=""></div><div class="we-comment"><div class="coment-head"><h5><a href="time-line.html" title="">Jason borne</a></h5><span>1 year ago</span><a class="we-reply" href="#" title="Reply"><i class="fa fa-reply"></i></a></div><p>' + comment + '</p></div></li>';
    //                $(comment_HTML).insertBefore(parent);
    //                jQuery(this).val('');
    //            }
    //        }


    //        // Clear the textarea

    //    }
    //});

    //inbox page 	
    //***** Message Star *****//  
    $('.message-list > li > span.star-this').on("click", function () {
        $(this).toggleClass('starred');
    });


    //***** Message Important *****//
    $('.message-list > li > span.make-important').on("click", function () {
        $(this).toggleClass('important-done');
    });



    // Listen for click on toggle checkbox
    $('#select_all').on("click", function (event) {
        if (this.checked) {
            // Iterate each checkbox
            $('input:checkbox.select-message').each(function () {
                this.checked = true;
            });
        }
        else {
            $('input:checkbox.select-message').each(function () {
                this.checked = false;
            });
        }
    });


    $(".delete-email").on("click", function () {
        $(".message-list .select-message").each(function () {
            if (this.checked) {
                $(this).parent().slideUp();
            }
        });
    });

    // change background color on hover
    $('.category-box').hover(function () {
        $(this).addClass('selected');
        $(this).parent().siblings().children('.category-box').removeClass('selected');
    });


    //------- offcanvas menu 

    const menu = document.querySelector('#toggle');
    const menuItems = document.querySelector('#overlay');
    const menuContainer = document.querySelector('.menu-container');
    const menuIcon = document.querySelector('.canvas-menu i');

    function toggleMenu(e) {
        menuItems.classList.toggle('open');
        menuContainer.classList.toggle('full-menu');
        menuIcon.classList.toggle('fa-bars');
        menuIcon.classList.add('fa-times');
        e.preventDefault();
    }

    if (menu) {
        menu.addEventListener('click', toggleMenu, false);
    }

    // Responsive nav dropdowns
    $('.offcanvas-menu li.menu-item-has-children > a').on('click', function () {
        $(this).parent().siblings().children('ul').slideUp();
        $(this).parent().siblings().removeClass('active');
        $(this).parent().children('ul').slideToggle();
        $(this).parent().toggleClass('active');
        return false;
    });

    ///////////////////////////////////




    // Prepare the preview for profile picture
    $("#wizard-picture").change(function () {
        readURL(this);
    });

    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#wizardPicturePreview').attr('src', e.target.result).fadeIn('slow');
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

    //////////////////////////////////////




    //new

    $('#checkAll').change(function () {
        var checkboxes = $('ul li input[type="checkbox"]');
        checkboxes.prop('checked', $('#checkAll').prop('checked'));
    });

    // update edited text 



    // Event listener for confirm delete button in modal

    ///////////upload image in post ///////////


    //############################################

    /////////add post///////////


});//document ready end


var animateButton = function (e) {

    e.preventDefault;
    //reset animation
    e.target.classList.remove('animate');



    e.target.classList.add('animate');
    setTimeout(function () {
        e.target.classList.remove('animate');
    }, 4000);
};

var classname = document.getElementsByClassName("button");

for (var i = 0; i < classname.length; i++) {
    classname[i].addEventListener('click', animateButton, false);
}
document.addEventListener("DOMContentLoaded", function () {
    const checkboxes = document.querySelectorAll('input[type="checkbox"]');
    const selectedInterestsDiv = document.getElementById('selectedInterests');
    const selectedInterests = [];

    checkboxes.forEach(function (checkbox) {
        checkbox.addEventListener('change', function () {
            const label = this.nextElementSibling.textContent.trim();
            if (this.checked) {
                if (!selectedInterests.includes(label)) {
                    selectedInterests.push(label);
                }
            } else {
                const index = selectedInterests.indexOf(label);
                if (index !== -1) {
                    selectedInterests.splice(index, 1);
                }
            }
            updateSelectedInterests();
        });
    });

    function updateSelectedInterests() {
        selectedInterestsDiv.textContent = selectedInterests.join(', ');


    }



    //DropDown input - select
    $('.t-dropdown-input').on('click', function () {
        $(this).parent().next().slideDown('fast');
    });

    $('.t-select-btn').on('click', function () {
        $('.t-dropdown-list').slideUp('fast');

        if (!$(this).prev().attr('disabled')) {
            $(this).prev().trigger('click');
        }
    });



    $('.t-dropdown-input').val('');

    $('li.t-dropdown-item').on('click', function () {
        var text = $(this).html();
        $(this).parent().prev().find('.t-dropdown-input').val(text);
        $('.t-dropdown-list').slideUp('fast');
    });

    $(document).on('click', function (event) {
        if ($(event.target).closest(".t-dropdown-input, .t-select-btn").length)
            return;
        $('.t-dropdown-list').slideUp('fast');
        event.stopPropagation();
    });




    (function ($) {
        // custom css expression for a case-insensitive contains()
        jQuery.expr[':'].Contains = function (a, i, m) {
            return (a.textContent || a.innerText || "").toUpperCase().indexOf(m[3].toUpperCase()) >= 0;
        };

        function listFilter(searchDir, list) {
            var form = $("<form>").attr({ "class": "filterform", "action": "#" }),
                input = $("<input>").attr({ "class": "filterinput", "type": "text", "placeholder": "Search..." });
            $(form).append(input).appendTo(searchDir);

            $(input)
                .change(function () {
                    var filter = $(this).val();
                    if (filter) {
                        $(list).find("li:not(:Contains(" + filter + "))").slideUp();
                        $(list).find("li:Contains(" + filter + ")").slideDown();
                    } else {
                        $(list).find("li").slideDown();
                    }
                    return false;
                })
                .keyup(function () {
                    $(this).change();
                });
        }

        //search friends widget
        $(function () {

            $('.searchjoine').each(function () {
                var searchJoined = $(this);
                var nearbyContct = searchJoined.closest('.tab-pane').find('.nearby-contct');
                listFilter(searchJoined, nearbyContct);
            });
        });
    }(jQuery));


    // ola


    $('.follow-btn').on('click', function (event) {
        // Prevent the default behavior of the anchor tag
        event.preventDefault();
        var spanElement = $(this).closest('.pepl-info').find('.follow-status');
        var removedLi = $(this).closest('.unique-li-class').detach();

        // Find the <span> element inside the parent <div> and toggle its text

        spanElement.text(spanElement.text() == 'Following' ? 'Follow' : 'Following');

        if (spanElement.text() == 'Follow') {
            $('.follwers_ul').append(removedLi);
            $('.remove-btn').hide();
        } else {
            $('.friend_ul').append(removedLi);
            $('.remove-btn').show();
        }
    });
    // ola

    //newwwwwwwwwwwww


    $("#fileInputContainer").click(function () {
        $("#fileInput").click();
    });

    // Optionally, handle the file input change event
    $("#fileInput").change(function () {
        // Do something when a file is selected
        console.log("File selected:", $(this).val());
    });

});
// more info: https://github.com/nolanlawson/emoji-picker-element


//upload photo  in post 

const inputFile = document.querySelector("#picture__input");
const pictureImage = document.querySelector(".picture__image");
const pictureImageTxt = "Choose an image";
pictureImage.innerHTML = pictureImageTxt;

inputFile.addEventListener("change", function (e) {
    const inputTarget = e.target;
    const file = inputTarget.files[0];

    if (file) {
        const reader = new FileReader();

        reader.addEventListener("load", function (e) {
            const readerTarget = e.target;

            const img = document.createElement("img");
            img.src = readerTarget.result;
            img.classList.add("picture__img");

            pictureImage.innerHTML = "";
            pictureImage.appendChild(img);
        });

        reader.readAsDataURL(file);
    } else {
        pictureImage.innerHTML = pictureImageTxt;
    }



});


////newwwwwwwwwwwwww
//document.getElementById("em---1").addEventListener('click', function () {
//    document.getElementById('comment-area').value = document.getElementById('comment-area').value + "👍🏻";
//});

//document.getElementById("em-smiley").addEventListener('click', function () {
//    document.getElementById('comment-area').value = document.getElementById('comment-area').value + "😃";
//});
//document.getElementById(" em-anguished").addEventListener('click', function () {
//    document.getElementById('comment-area').value = document.getElementById('comment-area').value + "😯";
//});
//document.getElementById("em-laughing").addEventListener('click', function () {
//    document.getElementById('comment-area').value = document.getElementById('comment-area').value + "😆";
//});
//document.getElementById("em-angry").addEventListener('click', function () {
//    document.getElementById('comment-area').value = document.getElementById('comment-area').value + "😠";
//});
//document.getElementById("em-astonished").addEventListener('click', function () {
//    document.getElementById('comment-area').value = document.getElementById('comment-area').value + "😵";
//});
//document.getElementById("em-heart").addEventListener('click', function () {
//    document.getElementById('comment-area').value = document.getElementById('comment-area').value + "❤️";
//});
//document.getElementById("em-disappointed").addEventListener('click', function () {
//    document.getElementById('comment-area').value = document.getElementById('comment-area').value + "😔";
//});
//document.getElementById("em-worried").addEventListener('click', function () {
//    document.getElementById('comment-area').value = document.getElementById('comment-area').value + "😟";
//});
//document.getElementById("em-kissing_heart").addEventListener('click', function () {
//    document.getElementById('comment-area').value = document.getElementById('comment-area').value + "😘";
//});
//document.getElementById("em-rage").addEventListener('click', function () {
//    document.getElementById('comment-area').value = document.getElementById('comment-area').value + "😡";
//});
//document.getElementById("em-stuck_out_tongue").addEventListener('click', function () {
//    document.getElementById('comment-area').value = document.getElementById('comment-area').value + "😛";
//});
