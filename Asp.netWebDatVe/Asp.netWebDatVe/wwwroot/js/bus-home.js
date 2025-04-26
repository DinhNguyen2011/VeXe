// Bắt sự kiện submit form
document.addEventListener("DOMContentLoaded", function () {
    var form = document.getElementById("searchForm");
    form.addEventListener("submit", function (e) {
        // Hiện modal loading
        var modal = document.getElementById("loadingModal");
        modal.style.display = "block";
    });
});
