document.addEventListener("DOMContentLoaded", function () {
    // Lọc khung giờ
    const checkboxes = document.querySelectorAll(".time-filter");
    checkboxes.forEach(cb => {
        cb.addEventListener("change", function () {
            if (this.checked) {
                checkboxes.forEach(otherCb => {
                    if (otherCb !== this) otherCb.checked = false;
                });
            }
            filterTrips();
        });
    });
    //cấu hình Swiper
    document.addEventListener('DOMContentLoaded', function () {
        var promotionSwiper = new Swiper('.promotion-swiper', {
            slidesPerView: 3, // hiển thị ? slide cùng lúc
            spaceBetween: 8, // kcach giữa các slide
            loop: true,
            pagination: {
                el: '.promotion-pagination',
                clickable: true
            },
            navigation: {
                nextEl: '.swiper-button-next',
                prevEl: '.swiper-button-prev'
            },
            breakpoints: {
                //điều chỉnh số lương s & kc slide 1, 2,3
                320: {
                    slidesPerView: 1,
                    spaceBetween: 5
                },
                768: {
                    slidesPerView: 2,
                    spaceBetween: 6
                },
                992: {
                    slidesPerView: 3,
                    spaceBetween: 8
                }
            }
        });
    });

   /* lọc và hiển thị các chuyến xe*/
    function filterTrips() {
        const selectedCb = Array.from(checkboxes).find(cb => cb.checked);
        const trips = document.querySelectorAll(".chuyen-xe");
        //duyet tung chuyen xe
        if (!selectedCb) {
            trips.forEach(trip => trip.style.display = "block");
            return;
        }

        const [start, end] = selectedCb.value.split('-').map(Number);
        trips.forEach(trip => {
            //kiem tra h khoi hanh co trong khung ko
            const gio = parseInt(trip.dataset.gioKhoihanh);
            const visible = gio >= start && gio < end;
            trip.style.display = visible ? "block" : "none";
        });
    }

    // Tìm kiếm chuyến xe
    const form = document.getElementById("searchForm");
    if (form) {
        form.addEventListener("submit", function () {
            const modal = document.getElementById("loadingModal");
            if (modal) modal.style.display = "block";
        });
    }

    // (Swiper) cho khuyến mãi
    if (typeof Swiper !== 'undefined') {
        new Swiper('.promotion-swiper', {
            slidesPerView: 1,
            spaceBetween: 10,
            loop: true,
            autoplay: {
                delay: 5000, //Tự động chuyển slide sau 5 giây
                disableOnInteraction: false,
            },
            //Hiển thị các chấm(pagination) để điều hướng, có thể nhấp vào.
            pagination: {
                el: '.promotion-pagination',
                clickable: true,
            },
            //Thêm các nút điều hướng (tiến/lùi).
            navigation: {
                nextEl: '.swiper-button-next',
                prevEl: '.swiper-button-prev',
            },
            breakpoints: {
                577: {
                    slidesPerView: 3,
                    spaceBetween: 30,
                },
            },
        });

        // Swiper cho về nhà xe
        new Swiper('.about-swiper', {
            slidesPerView: 2,
            spaceBetween: 20,
            loop: true,
            autoplay: {
                delay: 5000,
                disableOnInteraction: false,
            },
            pagination: {
                el: '.about-pagination',
                clickable: true,
            },
            breakpoints: {
                576: {
                    slidesPerView: 1,
                    spaceBetween: 10,
                },
                1024: {
                    slidesPerView: 1,
                    spaceBetween: 15,
                },
            },
        });
        //in thông báo tc
        console.log("Swiper initialized for promotion and about sections.");
    }
    else
    //in lỗi
    {
        console.error("Swiper.js not loaded.");
    }
});
//document.addEventListener("DOMContentLoaded", function () {
//    // Hàm lọc và hiển thị các chuyến xe
//    function filterTrips() {
//        const timeFilters = document.querySelectorAll('.time-filter');
//        const selectedTimes = Array.from(timeFilters)
//            .filter(cb => cb.checked)
//            .map(cb => cb.value);

//        const trips = document.querySelectorAll('.chuyen-xe');

//        // Nếu không có khung giờ nào được chọn, hiển thị tất cả chuyến xe
//        if (selectedTimes.length === 0) {
//            trips.forEach(trip => {
//                trip.style.display = 'block';
//            });
//            return;
//        }

//        // Lọc chuyến xe theo các khung giờ được chọn
//        trips.forEach(trip => {
//            const gio = parseInt(trip.dataset.gioKhoihanh);
//            const isVisible = selectedTimes.some(range => {
//                const [start, end] = range.split('-').map(Number);
//                return gio >= start && gio < end;
//            });
//            trip.style.display = isVisible ? 'block' : 'none';
//        });
//    }

//    // Gắn sự kiện cho các checkbox khung giờ
//    const checkboxes = document.querySelectorAll('.time-filter');
//    checkboxes.forEach(cb => {
//        cb.addEventListener('change', filterTrips);
//    });

//    // Tìm kiếm chuyến xe
//    const form = document.getElementById('searchForm');
//    if (form) {
//        form.addEventListener('submit', function () {
//            const modal = document.getElementById('loadingModal');
//            if (modal) modal.style.display = 'block';
//        });
//    }

//    // Cấu hình Swiper
//    
//});