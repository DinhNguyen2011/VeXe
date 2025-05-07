namespace Asp.netWebDatVe.Models.Payment
{
    public class PaymentInformationModel
    {
        public string OrderType { get; set; } = "bus_booking";
        public decimal Amount { get; set; }
        public string OrderDescription { get; set; }
        public string Name { get; set; }
        public int MaPhieu { get; set; } // Liên kết với phiếu đặt vé
    }
}
