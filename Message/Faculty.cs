﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Clay.OMS.Message
{
    public class Faculty
    {
        public string facultyID {get; set;}

        [Display(Name = "Faculty Name")]
        [StringLength(50)]
        [Required(ErrorMessage = "Please provide faculty name.", AllowEmptyStrings = false)]
        public string facultyName { get; set; }

        public System.Nullable<int> yearOfEstablishment { get; set; }

        [Display(Name = "Dean")]
        [StringLength(80)]
        public string dean { get; set; }
        
        public string addUser { get; set; }

        public string updateUser { get; set; }

        public System.Nullable<DateTime> updateDate { get; set; }

        public System.Nullable<bool> activated { get; set; }
    }
}
