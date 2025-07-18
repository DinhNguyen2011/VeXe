namespace Asp.netWebDatVe.Models.Payment
{
    public class PaymentInformationModel
    {
        //nhận phản hồi thanh toán
        public string OrderType { get; set; } = "bus_booking";
        public decimal Amount { get; set; } //tổng tiền
        public string OrderDescription { get; set; } //chi tiết về giao dịch, ví dụ: "Thanh toan ve xe cho [Tên khách hàng]".
        public string Name { get; set; }
        public int MaPhieu { get; set; } // Liên kết với phiếu đặt vé
    }
}
