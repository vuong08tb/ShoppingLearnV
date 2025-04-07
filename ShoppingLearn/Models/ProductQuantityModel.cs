using System.ComponentModel.DataAnnotations;

namespace ShoppingLearn.Models
{
    public class ProductQuantityModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Yêu cầu không được bỏ trống số lượng sản phẩm")]
        public int Quantity { get; set; }

        public int ProductId { get; set; }
        public DateTime  DateCreated { get; set; }
    }
}
