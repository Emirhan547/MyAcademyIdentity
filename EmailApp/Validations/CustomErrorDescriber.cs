using Microsoft.AspNetCore.Identity;

namespace EmailApp.Validations
{
    public class CustomErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = "PasswordRequiresDigit",
                Description = "Parola en az bir rakam(0-9) içermelidir."
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = "PasswordRequiresLower",
                Description = "Parola en az bir küçük harf(a-z) içermelidir."
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = "PasswordRequiresUpper",
                Description = "Parola en az bir büyük harf(A-Z) içermelidir."
            };
        }
        public override IdentityError PasswordTooShort(int lenght)
        {
            return new IdentityError
            {
                Code = "PasswordTooShort",
                Description = $"Şifre en az{lenght} karakterden oluşmalıdır."
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code ="PasswordRequiresNonAlphanumeric",
                Description = "Parola en az bir özel karakter(*,!,.) içermelidir."
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = "DuplicateUserName",
                Description = $"{userName} kullanıcı adı zaten alınmış."
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = "DuplicateEmail",
                Description = $"{email} e-posta adresi zaten kullanılmaktadır."
            };
        }
    }
}
