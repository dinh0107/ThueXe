AOS.init({
    once: true,
});

function homeJs() {
    $(".banner").slick({
        dots: true,
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1,
        speed: 1000,
        autoplay: false,
        autoplaySpeed: 5000,
        prevArrow: '<button class="chevron-prev"><i class="fa-solid fa-angle-left"></i></button>',
        nextArrow: '<button class="chevron-next"><i class="fa-solid fa-angle-right"></i></button>',
    });

    $(".product-list").slick({
        dots: false,
        infinite: true,
        slidesToShow: 4,
        slidesToScroll: 1,
        speed: 1000,
        autoplay: false,
        autoplaySpeed: 3000,
        prevArrow: '<button class="chevron-prev"><i class="far fa-chevron-left"></i></button>',
        nextArrow: '<button class="chevron-next"><i class="far fa-chevron-right"></i></button>',
        responsive: [
            {
                breakpoint: 992,
                settings: {
                    slidesToShow: 3,
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 1,
                }
            }
        ]
    });

    $(".feedback-list").slick({
        dots: false,
        infinite: true,
        slidesToShow: 3,
        slidesToScroll: 1,
        speed: 1000,
        autoplay: false,
        autoplaySpeed: 3000,
        centerMode: true,
        centerPadding: '0',
        prevArrow: '<button class="chevron-prev"><i class="far fa-chevron-left"></i></button>',
        nextArrow: '<button class="chevron-next"><i class="far fa-chevron-right"></i></button>',
        responsive: [
            {
                breakpoint: 992,
                settings: {
                    slidesToShow: 3,
                }
            },
            {
                breakpoint: 600,
                centerMode: false,
                settings: {
                    slidesToShow: 1,
                }
            }
        ]
    });

    $(".btn-reverse").click(function () {
        var from = $("#Contact_From").val();
        var to = $("#Contact_To").val();

        $("#Contact_From").val(to);
        $("#Contact_To").val(from);
    });
}

function productDetail() {
    $('.slider-for').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        fade: true,
        asNavFor: '.slider-nav',
        prevArrow: '<button class="chevron-prev"><i class="fas fa-chevron-left"></i></button>',
        nextArrow: '<button class="chevron-next"><i class="fas fa-chevron-right"></i></button>',
    });

    $('.slider-nav').slick({
        slidesToShow: 4,
        slidesToScroll: 1,
        asNavFor: '.slider-for',
        speed: 1000,
        dots: false,
        focusOnSelect: true,
        prevArrow: '<button class="chevron-prev"><i class="fas fa-chevron-left"></i></button>',
        nextArrow: '<button class="chevron-next"><i class="fas fa-chevron-right"></i></button>',
        responsive: [
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1,
                    vertical: false,
                }
            },
        ]
    });

    $(".feedback-product").slick({
        dots: false,
        infinite: true,
        slidesToShow: 6,
        slidesToScroll: 1,
        speed: 1000,
        autoplay: false,
        autoplaySpeed: 3000,
        prevArrow: '<button class="chevron-prev"><i class="far fa-chevron-left"></i></button>',
        nextArrow: '<button class="chevron-next"><i class="far fa-chevron-right"></i></button>',
        responsive: [
            {
                breakpoint: 992,
                settings: {
                    slidesToShow: 2,
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 1,
                }
            }
        ]
    });
}

$(document).ready(function () {
    $(window).scroll(function () {
        if ($(this).scrollTop() > 200) {
            $(".btn-scroll").fadeIn(200);
        } else {
            $(".btn-scroll").fadeOut(200);
        }

        if ($(this).scrollTop() > 350) {
            $(".header-stuck").addClass('show');
        }
        else {
            $(".header-stuck").removeClass('show');
        }
    });

    $(".btn-scroll").click(function (event) {
        event.preventDefault();
        $("html, body").animate({ scrollTop: 0 }, 300);
    });

    $(".hamburger").click(function () {
        $(this).toggleClass("active");
        $(".menu-landing-mb").toggleClass("active");
        $(".overlay").toggleClass('active');
    });

    $(".expand-bar").click(function () {
        $(this).toggleClass('open');
        $(this).siblings('.sub-nav-mb').slideToggle();
    });

    $(".overlay").click(function () {
        $(this).removeClass("active");
        $(".menu-landing-mb").removeClass("active");
        $(".hamburger").toggleClass('active');
    });

    $(".menu-landing-mb").click(function () {
        $(this).removeClass("active");
        $(".overlay").removeClass("active");
        $(".hamburger").removeClass('active');
    });

    $('.btn-toc').click(function () {
        $('.toc-body').slideToggle();
    });

    $(".btn-search").click(function () {
        $(".body-overlay").addClass('active');
        $(".site-search").addClass('active');
        function delay() {
            $(".site-search .form-control").focus();
        }

        setTimeout(delay, 300);
    });

    $(".body-overlay, .site-search-close").click(function () {
        $(".body-overlay").removeClass('active');
        $(".site-search").removeClass('active');
    });

    $('.button-wrap').on("click", function () {
        $(this).toggleClass('button-active');
        $("#toDate").toggleClass('input-active');
    });
});

$(function () {
    $("#bookingForm").on("submit", function (e) {
        e.preventDefault();
        if ($(this).valid()) {
            $.post("/Home/ContactForm", $(this).serialize(), function (data) {
                if (data.status) {
                    $.toast({
                        heading: 'Liên hệ đặt xe thành công',
                        text: data.msg,
                        icon: 'success',
                        position: "bottom-right"
                    })
                    $("#bookingForm").trigger("reset");
                } else {
                    $.toast({
                        heading: 'Liên hệ không thành công',
                        text: data.msg,
                        icon: 'error',
                        position: "bottom-right"
                    })
                }
            });
        }
    });
});

//function initAutocomplete() {
//    const startInput = document.getElementById('Contact_From');
//    const endInput = document.getElementById('Contact_To');

//    const startAutocomplete = new google.maps.places.Autocomplete(startInput, {
//        types: ['geocode'],
//        componentRestrictions: { country: 'vn' }
//    });

//    const endAutocomplete = new google.maps.places.Autocomplete(endInput, {
//        types: ['geocode'],
//        componentRestrictions: { country: 'vn' }
//    });

//    startAutocomplete.addListener('place_changed', function () {
//        const place = startAutocomplete.getPlace();
//        console.log('Điểm đi:', place);
//    });

//    endAutocomplete.addListener('place_changed', function () {
//        const place = endAutocomplete.getPlace();
//        console.log('Điểm đến:', place);
//    });
//}

//window.onload = initAutocomplete;