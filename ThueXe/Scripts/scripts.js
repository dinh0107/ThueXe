AOS.init({
    once: true,
});
$('.button-wrap').on("click", function () {
    $(this).toggleClass('button-active');
    $(".toDate").toggleClass('input-active');
});
function homeJs() {
    $('.service-slide').slick({
        centerMode: true,
        centerPadding: '0',
        slidesToShow: 3,
        infinite: true,
        dots: true,
        //autoplay: true,
        //autoplaySpeed: 3000,
        prevArrow: "<button type='button' aria-label='bên trái' class='slick-prev pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
        nextArrow: "<button type='button'aria-label='bên phải' class='slick-next pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",

        responsive: [
            {
                breakpoint: 900,
                settings: {
                    arrows: true,
                    centerMode: false,
                    dots: false,
                    slidesToShow: 3
                }
            },
            {
                breakpoint: 768,
                settings: {
                    arrows: true,
                    centerMode: true,
                    centerPadding: '0',
                    dots: false,
                    slidesToShow: 1
                }
            },
            {
                breakpoint: 480,
                settings: {
                    arrows: true,
                    centerMode: true,
                    centerPadding: '0', dots: false,
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
        autoplay: true,
        autoplaySpeed: 3000,
        prevArrow: "<button type='button' aria-label='bên trái' class='slick-prev pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
        nextArrow: "<button type='button'aria-label='bên phải' class='slick-next pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",

        responsive: [
            {
                breakpoint: 768,
                settings: {
                    dots: false,
                    arrows: true,
                    slidesToShow: 1
                }
            },
            {
                breakpoint: 480,
                settings: {
                    dots: false,
                    arrows: true,
                    slidesToShow: 1
                }
            }
        ]
    });
    $('.list-car').slick({
        slidesToShow: 3,
        infinite: true,
        dots: true,
        autoplay: true,
        autoplaySpeed: 3000,
        prevArrow: "<button type='button' aria-label='bên trái' class='slick-prev pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
        nextArrow: "<button type='button'aria-label='bên phải' class='slick-next pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",

        responsive: [
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 2,
                    dots: false,
                    arrows: true,
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1,
                    dots: false,
                    arrows: true,
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



    //var rowsmb = $("#priceTable tbody tr");
    //var showCountmb = 5;
    //rowsmb.slice(showCountmb).hide();

    //$("#show-more-table").click(function () {
    //    rowsmb.show();
    //    $(this).hide();
    //});

    const routes = [
        {
            "hanh_trinh": "Hà Nội - Sơn La",
            "khoang_cach_km": 198,
            "gia": { "4_cho": "2.376.000 đ", "7_cho": "2.772.000 đ", "16_cho": "4.752.000 đ" }
        },
        {
            "hanh_trinh": "Hà Nội - Điện Biên",
            "khoang_cach_km": 436,
            "gia": { "4_cho": "5.232.000 đ", "7_cho": "6.104.000 đ", "16_cho": "10.464.000 đ" }
        },
        {
            "hanh_trinh": "Hà Nội - Lai Châu",
            "khoang_cach_km": 447,
            "gia": { "4_cho": "5.364.000 đ", "7_cho": "6.258.000 đ", "16_cho": "10.728.000 đ" }
        },
        {
            "hanh_trinh": "Hà Nội - Lào Cai",
            "khoang_cach_km": 291,
            "gia": { "4_cho": "3.492.000 đ", "7_cho": "4.074.000 đ", "16_cho": "6.984.000 đ" }
        },
        {
            "hanh_trinh": "Hà Nội - Yên Bái",
            "khoang_cach_km": 160,
            "gia": { "4_cho": "1.920.000 đ", "7_cho": "2.240.000 đ", "16_cho": "3.840.000 đ" }
        },
        {
            "hanh_trinh": "Hà Nội - Phú Thọ",
            "khoang_cach_km": 102,
            "gia": { "4_cho": "1.224.000 đ", "7_cho": "1.428.000 đ", "16_cho": "2.448.000 đ" }
        },
        {
            "hanh_trinh": "Hà Nội - Hà Giang",
            "khoang_cach_km": 297,
            "gia": { "4_cho": "3.564.000 đ", "7_cho": "4.158.000 đ", "16_cho": "7.128.000 đ" }
        },
        {
            "hanh_trinh": "Hà Nội - Tuyên Quang",
            "khoang_cach_km": 146,
            "gia": { "4_cho": "1.752.000 đ", "7_cho": "2.044.000 đ", "16_cho": "3.504.000 đ" }
        },
        {
            "hanh_trinh": "Hà Nội - Cao Bằng",
            "khoang_cach_km": 285,
            "gia": { "4_cho": "3.420.000 đ", "7_cho": "3.990.000 đ", "16_cho": "6.840.000 đ" }
        },
        {
            "hanh_trinh": "Hà Nội - Thái Nguyên",
            "khoang_cach_km": 90,
            "gia": { "4_cho": "1.080.000 đ", "7_cho": "1.260.000 đ", "16_cho": "2.160.000 đ" }
        },
        {
            "hanh_trinh": "Hà Nội - Lạng Sơn",
            "khoang_cach_km": 161,
            "gia": { "4_cho": "1.932.000 đ", "7_cho": "2.254.000 đ", "16_cho": "3.864.000 đ" }
        },
        {
            "hanh_trinh": "Hà Nội - Quảng Ninh",
            "khoang_cach_km": 157,
            "gia": { "4_cho": "1.884.000 đ", "7_cho": "2.198.000 đ", "16_cho": "3.768.000 đ" }
        },
        {
            "hanh_trinh": "Hà Nội - Hải Phòng",
            "khoang_cach_km": 125,
            "gia": { "4_cho": "1.500.000 đ", "7_cho": "1.750.000 đ", "16_cho": "3.000.000 đ" }
        },
        {
            "hanh_trinh": "Hà Nội - Nam Định",
            "khoang_cach_km": 85,
            "gia": { "4_cho": "1.020.000 đ", "7_cho": "1.190.000 đ", "16_cho": "2.040.000 đ" }
        },
        {
            "hanh_trinh": "Hà Nội - Thái Bình",
            "khoang_cach_km": 105,
            "gia": { "4_cho": "1.260.000 đ", "7_cho": "1.470.000 đ", "16_cho": "2.520.000 đ" }
        },
        {
            "hanh_trinh": "Hà Nội - Ninh Bình",
            "khoang_cach_km": 95,
            "gia": { "4_cho": "1.140.000 đ", "7_cho": "1.330.000 đ", "16_cho": "2.280.000 đ" }
        },
        {
            "hanh_trinh": "Hà Nội - Thanh Hóa",
            "khoang_cach_km": 168,
            "gia": { "4_cho": "2.016.000 đ", "7_cho": "2.352.000 đ", "16_cho": "4.032.000 đ" }
        },
        {
            "hanh_trinh": "Hà Nội - Nghệ An",
            "khoang_cach_km": 345,
            "gia": { "4_cho": "4.140.000 đ", "7_cho": "4.830.000 đ", "16_cho": "8.280.000 đ" }
        },
        {
            "hanh_trinh": "Hà Nội - Hà Tĩnh",
            "khoang_cach_km": 344,
            "gia": { "4_cho": "4.128.000 đ", "7_cho": "4.816.000 đ", "16_cho": "8.256.000 đ" }
        }
    ]
    function renderTable(type) {
        let rows = "";
        routes.forEach(r => {
            rows += `
          <tr>
            <td>${r.hanh_trinh}</td>
            <td>${r.khoang_cach_km}</td>
            <td><strong>${r.gia[type]}</strong></td>
          </tr>
        `;
        });
        $("#priceTable tbody").html(rows);
    }

    $(document).ready(function () {
        // Mặc định hiển thị 4 chỗ
        renderTable("4_cho");

        // Bắt sự kiện click
        $("#btn-4cho").click(function () {
            $("button").removeClass("active"); $(this).addClass("active");
            renderTable("4_cho");
        });
        $("#btn-7cho").click(function () {
            $("button").removeClass("active"); $(this).addClass("active");
            renderTable("7_cho");
        });
        $("#btn-16cho").click(function () {
            $("button").removeClass("active"); $(this).addClass("active");
            renderTable("16_cho");
        });
    });
}
function show() {
    $(".header-mobile").addClass('active')
    $(".overflow").addClass('active')
}
function Close() {
    $(".header-mobile").removeClass('active')
    $(".overflow").removeClass('active')
}
function productDetail() {
    $('.list-car').slick({
        slidesToShow: 3,
        infinite: true,
        dots: true,
        autoplay: true,
        autoplaySpeed: 3000,
        prevArrow: "<button type='button' aria-label='bên trái' class='slick-prev pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
        nextArrow: "<button type='button'aria-label='bên phải' class='slick-next pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",
        responsive: [
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 2,
                    dots: false,
                    arrows: true,
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1,
                    dots: false,
                    arrows: true,
                }
            }
        ]
    });
}
$(".view-all").click(function () {
    let $carBody = $(".car-body");
    let $btn = $(this);

    $carBody.toggleClass("active");

    if ($carBody.hasClass("active")) {
        $btn.text("Ẩn bớt");
    } else {
        $btn.text("Xem thêm");
    }
});

$(function () {
    $(".contact-part").on("submit", function (e) {
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
                    $(".contact-part").trigger("reset");
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
