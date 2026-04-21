using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Exceptions.AuthException
{
    public class CustomErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Description = "Şifrə ən azı bir rəqəm (0-9) ehtiva etməlidir."
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Description = "Şifrə ən azı bir kiçik hərf (a-z) ehtiva etməlidir."
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Description = "Şifrə ən azı bir böyük hərf (A-Z) ehtiva etməlidir."
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Description = "Şifrə ən azı bir xüsusi simvol (.,-_*+ və s.) ehtiva etməlidir."
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Description = $"Şifrə ən azı {length} simvol olmalıdır."
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Description = $"'{userName}' istifadəçi adı artıq mövcuddur."
            };
        }


        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Description = $"'{email}' e-poçt ünvanı artıq qeydiyyatdan keçib."
            };
        }


        public override IdentityError PasswordMismatch()
        {
            return new IdentityError
            {
                Description = "Şifrələr uyğun gəlmir."
            };
        }


        public override IdentityError PasswordRequiresUniqueChars(int length)
        {
            return new IdentityError
            {
                Description = $"Şifrə ən azı {length} fərqli simvol ehtiva etməlidir."
            };
        }

        public override IdentityError RecoveryCodeRedemptionFailed()
        {
            return new IdentityError
            {
                Description = "Bərpa kodu səhvdir və ya istifadə olunub."
            };
        }

        public override IdentityError InvalidEmail(string? email)
        {
            return new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = string.IsNullOrWhiteSpace(email) || !email.Contains('@')
                    ? "Düzgün e-poçt ünvanı daxil edin (nümunə: ad@example.com)."
                    : $"'{email}' düzgün e-poçt ünvanı deyil."
            };
        }

    }
}
