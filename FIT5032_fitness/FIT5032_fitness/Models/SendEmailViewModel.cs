using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FIT5032_fitness.Models
{
    public class SendEmailViewModel
    {
        public string ToEmail { get; set; }

        public string Subject { get; set; }

        public string Contents { get; set; }
    }
}