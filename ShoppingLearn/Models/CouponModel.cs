using System.ComponentModel.DataAnnotations;

namespace ShoppingLearn.Models
{
    public class CouponModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Yêu cầu tên coupon")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Yêu cầu mô tả")]
        public decimal Discount { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập giảm giá")]
        public string Description { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateExpried { get; set; }

      
        [Required(ErrorMessage = "Yêu cầu số lượng coupon")]
        public int Quantity { get; set; }
        public int Status { get; set; }

    }
}
