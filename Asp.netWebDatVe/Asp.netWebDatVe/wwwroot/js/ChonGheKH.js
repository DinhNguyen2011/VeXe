function validateForm() {
    let isValid = true;

    const tenKhachHang = document.getElementById("tenKhachHang").value.trim();
    const soDienThoai = document.getElementById("soDienThoai").value.trim();
    const email = document.getElementById("email").value.trim();
    const phuongThuc = document.getElementById("phuongThuc").value;
    const selectedSeatsInput = document.getElementById("selectedSeatsInput").value;
    const maChuyen = document.querySelector('input[name="maChuyen"]').value;

    console.log("Form data:", { maChuyen, tenKhachHang, soDienThoai, email, phuongThuc, selectedSeatsInput });

    // xóa thông báo lỗi cũ
    document.getElementById("tenKhachHang-error").innerText = "";
    document.getElementById("soDienThoai-error").innerText = "";
    document.getElementById("email-error").innerText = "";
    document.getElementById("phuongThuc-error").innerText = "";

    //ktra tên, sdt, email, phương thức thanh toán, ghế, mã chuyến 
    if (!tenKhachHang) {
        document.getElementById("tenKhachHang-error").innerText = "Vui lòng nhập họ và tên.";
        isValid = false;
        //if (!tenKhachHang || tenKhachHang.length < 2) {
        //    document.getElementById("tenKhachHang-error").innerText = "Họ và tên phải có ít nhất 2 ký tự.";
        //    isValid = false;
        //}
    }

    const phoneRegex = /^[0][0-9]{9}$/;
    if (!phoneRegex.test(soDienThoai)) {
        document.getElementById("soDienThoai-error").innerText = "Vui lòng nhập số điện thoại hợp lệ (10 chữ số).";
        isValid = false;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
        document.getElementById("email-error").innerText = "Vui lòng nhập email hợp lệ.";
        isValid = false;
    }

    if (!phuongThuc) {
        document.getElementById("phuongThuc-error").innerText = "Vui lòng chọn phương thức thanh toán.";
        isValid = false;
    }

    if (!selectedSeatsInput) {
        alert("Vui lòng chọn ít nhất một ghế.");
        isValid = false;
    }

    if (!maChuyen) {
        alert("Mã chuyến xe không hợp lệ.");
        isValid = false;
    }

    return isValid;
}