using System.ComponentModel.DataAnnotations;

namespace BilbolStack.CryptoJWT.API
{
    public class ChallengeRequest
    {
        [Required]
        public string? Account { get; set; }
        [NotNegative]
        public long NFTId { get;set; }
    }
}
