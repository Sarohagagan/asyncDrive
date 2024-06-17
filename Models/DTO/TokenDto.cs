using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class TokenDto
    {
        public class LoginRequest
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            public string UserName { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

        }
        public class LoginResponse
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
        }
        public class RefreshRequest
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
        }
        public class RefreshResponse
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
        }
        public class ValidateRequest
        {
            public string AccessToken { get; set; }
        }
    }
}
