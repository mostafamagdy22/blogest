using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogest.application.DTOs.responses.Auth
{
    public class ForgetPasswordCallBackResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public bool isAuth { get; set; }
    }
}