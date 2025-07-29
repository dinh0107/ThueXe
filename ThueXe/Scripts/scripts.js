AOS.init({
    once: true,
});

function homeJs() {
    $('.service-slide').slick({
        centerMode: true,
        centerPadding: '0',
        slidesToShow: 3,
        infinite: true,
        dots: true,
        prevArrow: "<button type='button' aria-label='bên trái' class='slick-prev pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
        nextArrow: "<button type='button'aria-label='bên phải' class='slick-next pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",

        responsive: [
            {
                breakpoint: 768,
                settings: {
                    arrows: false,
                    centerMode: true,
                    centerPadding: '0',
                    slidesToShow: 3
                }
            },
            {
                breakpoint: 480,
                settings: {
                    arrows: false,
                    centerMode: true,
                    centerPadding: '0',
                    slidesToShow: 1
                }
            }
        ]
    });
    function toggleAccordion(button) {
        button.classList.toggle("active");
        const content = button.nextElementSibling;
        if (content.style.display === "block") {
            content.style.display = "none";
        } else {
            content.style.display = "block";
        }
    }
    $('.list-feedback').slick({
        slidesToShow: 2,
        infinite: true,
        dots: true,
        prevArrow: "<button type='button' aria-label='bên trái' class='slick-prev pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
        nextArrow: "<button type='button'aria-label='bên phải' class='slick-next pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",

        responsive: [
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 2
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1
                }
            }
        ]
    });
    $('.list-car').slick({
        slidesToShow: 3,
        infinite: true,
        dots: true,
        prevArrow: "<button type='button' aria-label='bên trái' class='slick-prev pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
        nextArrow: "<button type='button'aria-label='bên phải' class='slick-next pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",

        responsive: [
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 2
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1
                }
            }
        ]
    });
    var rows = $("#myTable tbody tr");
    var showCount = 5; 
    rows.slice(showCount).hide();

    $("#show-more-table").click(function () {
        rows.show();           
        $(this).hide();        
    });

}

function productDetail() {
    $('.list-car').slick({
        slidesToShow: 3,
        infinite: true,
        dots: true,
        prevArrow: "<button type='button' aria-label='bên trái' class='slick-prev pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
        nextArrow: "<button type='button'aria-label='bên phải' class='slick-next pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",

        responsive: [
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 2
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1
                }
            }
        ]
    });
}


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
