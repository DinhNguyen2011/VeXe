function validateForm() {
    // Xóa thông báo lỗi cũ
    document.querySelectorAll('.error-message').forEach(error => error.textContent = '');

    const tenKhachHang = document.getElementById('tenKhachHang').value.trim();
    const soDienThoai = document.getElementById('soDienThoai').value.trim();
    const email = document.getElementById('email').value.trim();
    const selectedSeats = document.getElementById('selectedSeatsInput').value;
    let isValid = true;

    // Kiểm tra tên khách hàng
    if (!tenKhachHang) {
        document.getElementById('tenKhachHang-error').textContent = 'Vui lòng nhập tên khách hàng.';
        isValid = false;
    }

    // Kiểm tra số điện thoại (bắt đầu từ 0, 10 chữ số)
    if (!soDienThoai) {
        document.getElementById('soDienThoai-error').textContent = 'Vui lòng nhập số điện thoại.';
        isValid = false;
    } else if (!/^[0][0-9]{9}$/.test(soDienThoai)) {
        document.getElementById('soDienThoai-error').textContent = 'Số điện thoại phải bắt đầu từ 0 và có 10 chữ số.';
        isValid = false;
    }

    // Kiểm tra email
    if (!email) {
        document.getElementById('email-error').textContent = 'Vui lòng nhập email.';
        isValid = false;
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
        document.getElementById('email-error').textContent = 'Email không hợp lệ.';
        isValid = false;
    }

    // Kiểm tra ghế được chọn
    if (!selectedSeats) {
        document.getElementById('tenKhachHang-error').textContent = 'Vui lòng chọn ít nhất một ghế.'; // Hiển thị lỗi chung
        isValid = false;
    }

    return isValid;
}