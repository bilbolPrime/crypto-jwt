using System.ComponentModel.DataAnnotations;

namespace BilbolStack.CryptoJWT.API
{
    public class NotNegative : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is long)
            {
                return (long)value >= 0;
            }

            if (value is decimal)
            {
                return (decimal)value >= 0;
            }

            if (value is float)
            {
                return (float)value >= 0;
            }

            return (int)value >= 0;
        }
    }
}