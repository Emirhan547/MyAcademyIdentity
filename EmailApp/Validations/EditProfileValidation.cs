using System.ComponentModel.DataAnnotations;

namespace EmailApp.Validations
{
    public class EditProfileValidation
    {
        public class EditProfileViewModelValidator : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                var model = (EmailApp.Models.EditProfileViewModel)validationContext.ObjectInstance;

                if (string.IsNullOrWhiteSpace(model.FirstName))
                    return new ValidationResult("Ad alanı boş olamaz");

                if (string.IsNullOrWhiteSpace(model.LastName))
                    return new ValidationResult("Soyad alanı boş olamaz");

                if (!new EmailAddressAttribute().IsValid(model.Email))
                    return new ValidationResult("Geçerli bir email giriniz");

                return ValidationResult.Success!;
            }
        }
    }
}
