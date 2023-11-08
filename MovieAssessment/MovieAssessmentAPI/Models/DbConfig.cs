using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieAssessmentAPI.Models
{
    public class DbConfig
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string MoviesCollectionName { get; set; }
    }    
}