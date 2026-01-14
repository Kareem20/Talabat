using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class OrderDTO
    {
        [Required]
        public string BasketID { get; set; }
        [Required]
        public int DeliveryMethodID { get; set; }
        [Required]
        public AddressDTO Address { get; set; }

    }
}
