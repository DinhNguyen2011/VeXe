namespace Asp.netWebDatVe.Models.Payment
{
    public class PaymentResponse
    {
        //thông tin phản hồi từ cổng thanh toán
        public bool Success { get; set; } //thanh toán có thành công hay ko
        public string PaymentMethod { get; set; }
        public string OrderDescription { get; set; }
        public string OrderId { get; set; }
        public string TransactionId { get; set; }//Mã giao dịch
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; } //Mã phản hồi từ VNPay
        public string MoMoResponseCode { get; set; }//Mã phản hồi từ MoMo
        public decimal Amount { get; set; }
    }
}
