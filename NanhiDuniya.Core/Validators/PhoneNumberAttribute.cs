using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Validators
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        private readonly PhoneNumberUtil _phoneNumberUtil;
        private readonly PhoneNumberFormat _phoneNumberFormat;

        public PhoneNumberAttribute(PhoneNumberFormat phoneNumberFormat = PhoneNumberFormat.E164)
        {
            _phoneNumberUtil = PhoneNumberUtil.GetInstance();
            _phoneNumberFormat = phoneNumberFormat;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var phoneNumber = value as string;
            if (phoneNumber == null)
            {
                return new ValidationResult("Phone number is required.");
            }

            try
            {
                var parsedNumber = _phoneNumberUtil.Parse(phoneNumber, null);
                if (_phoneNumberUtil.IsValidNumber(parsedNumber))
                {
                    var formattedNumber = _phoneNumberUtil.Format(parsedNumber, _phoneNumberFormat);
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Invalid phone number.");
                }
            }
            catch (NumberParseException)
            {
                return new ValidationResult("Invalid phone number format.");
            }
        }
    }
}
