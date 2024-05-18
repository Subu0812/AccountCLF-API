using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.Exceptions
{
    public class MobileNumberAttributes : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string phoneNumber = value.ToString();

                if (phoneNumber.Length != 10)
                {
                    return new ValidationResult("Phone number must be 10 digits.");
                }
            }
            return ValidationResult.Success;
        }
    }
}

