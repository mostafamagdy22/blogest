using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogest.application.DTOs.responses
{
    public class SignUpResponseDto
    {
        public bool SignUpSuccessfully { get; set; }
        public Guid? UserId { get; set; }
    }
}