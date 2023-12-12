using System.ComponentModel.DataAnnotations;

namespace BilbolStack.CryptoJWT.API
{
    public class LoginRequest
    {
        [Required]
        public string? Account { get; set; }
        [NotNegative]
        public long NFTId { get;set; }
        [Required]
        public string? Signature { get; set; }
    }
}
