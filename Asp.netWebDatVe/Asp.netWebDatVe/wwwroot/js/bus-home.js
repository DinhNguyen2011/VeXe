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

    function filterTrips() {
        const selectedCb = Array.from(checkboxes).find(cb => cb.checked);
        const trips = document.querySelectorAll(".chuyen-xe");

        if (!selectedCb) {
            trips.forEach(trip => trip.style.display = "block");
            return;
        }

        const [start, end] = selectedCb.value.split('-').map(Number);
        trips.forEach(trip => {
            const gio = parseInt(trip.dataset.gioKhoihanh);
            const visible = gio >= start && gio < end;
            trip.style.display = visible ? "block" : "none";
        });
    }

    // Modal loading
    const form = document.getElementById("searchForm");
    if (form) {
        form.addEventListener("submit", function () {
            const modal = document.getElementById("loadingModal");
            if (modal) modal.style.display = "block";
        });
    }

    // Swiper cho khuyến mãi
    if (typeof Swiper !== 'undefined') {
        new Swiper('.promotion-swiper', {
            slidesPerView: 1,
            spaceBetween: 10,
            loop: true,
            autoplay: {
                delay: 5000,
                disableOnInteraction: false,
            },
            pagination: {
                el: '.promotion-pagination',
                clickable: true,
            },
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
        console.log("Swiper initialized for promotion and about sections.");
    } else {
        console.error("Swiper.js not loaded.");
    }
});