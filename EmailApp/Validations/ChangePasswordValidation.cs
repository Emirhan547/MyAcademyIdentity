using System.ComponentModel.DataAnnotations;

namespace EmailApp.Validations
{
    public class ChangePasswordValidation
    {
        public class ChangePasswordViewModelValidator : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                var model = (EmailApp.Models.ChangePasswordViewModel)validationContext.ObjectInstance;

                if (string.IsNullOrWhiteSpace(model.CurrentPassword))
                    return new ValidationResult("Mevcut şifre boş olamaz");

                if (string.IsNullOrWhiteSpace(model.NewPassword))
                    return new ValidationResult("Yeni şifre boş olamaz");

                if (model.NewPassword != model.ConfirmNewPassword)
                    return new ValidationResult("Yeni şifreler eşleşmiyor");

                return ValidationResult.Success!;
            }
        }
    }
}
