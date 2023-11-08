using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieAssessmentAPI.Models
{
    public class MoviesReq
    {
        public int PageSize {get;set;} = 50;
        public int page {get;set;} = 1;
    }
}