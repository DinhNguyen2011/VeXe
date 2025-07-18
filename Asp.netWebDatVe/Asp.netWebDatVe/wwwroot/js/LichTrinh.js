document.addEventListener("DOMContentLoaded", function () {
    // 1. Highlight dòng bảng khi hover
    const rows = document.querySelectorAll(".lt-table tbody tr");
    rows.forEach(row => {
        //khi con trỏ chuột di qua dòng, màu nền của dòng sẽ đổi thành màu xanh nhạt
        row.addEventListener("mouseover", () => {
            row.style.backgroundColor = "#f0f8ff";
        });
        //Khi con trỏ chuột rời khỏi dòng, màu nền sẽ được xóa(trở về trạng thái mặc định).
        row.addEventListener("mouseout", () => {
            row.style.backgroundColor = "";
        });
    });

    // 2. Cảnh báo nếu tìm kiếm trống
    const searchForm = document.querySelector("form[method='get']");
    const searchInput = document.querySelector(".lt-input");

    searchForm.addEventListener("submit", function (e) {
        //Kiểm tra xem ô nhập liệu tìm kiếm có trống hay không.
        if (searchInput.value.trim() === "") {
            e.preventDefault();
            alert("Vui lòng nhập điểm đi hoặc điểm đến để tìm kiếm.");

            //const errorDiv = document.createElement("div");
            //errorDiv.className = "error-message";
            //errorDiv.textContent = "Vui lòng nhập điểm đi hoặc điểm đến để tìm kiếm.";

            //// Chèn thông báo ngay sau ô nhập liệu
            //searchInput.parentElement.appendChild(errorDiv);
            
            searchInput.focus();
        }
    });
});
