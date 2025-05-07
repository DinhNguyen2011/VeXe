document.addEventListener("DOMContentLoaded", function () {
    // 1. Highlight dòng bảng khi hover
    const rows = document.querySelectorAll(".lt-table tbody tr");
    rows.forEach(row => {
        row.addEventListener("mouseover", () => {
            row.style.backgroundColor = "#f0f8ff";
        });
        row.addEventListener("mouseout", () => {
            row.style.backgroundColor = "";
        });
    });

    // 2. Cảnh báo nếu tìm kiếm trống
    const searchForm = document.querySelector("form[method='get']");
    const searchInput = document.querySelector(".lt-input");

    searchForm.addEventListener("submit", function (e) {
        if (searchInput.value.trim() === "") {
            e.preventDefault();
            alert("Vui lòng nhập điểm đi hoặc điểm đến để tìm kiếm.");
            searchInput.focus();
        }
    });
});
