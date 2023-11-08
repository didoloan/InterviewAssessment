using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieAssessmentAPI.Models
{
    public class LoginReq
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}