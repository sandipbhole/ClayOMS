using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Clay.OMS.Message
{
    public class Faculty
    {
        public long facultyID {get; set; }

        [Display(Name = "Faculty Code")]
        [Required(ErrorMessage = "Please provide code.", AllowEmptyStrings = false)]
        [StringLength(20)]
        public string code { get; set; }

        [Display(Name = "Faculty Name")]
        [StringLength(50)]
        [Required(ErrorMessage = "Please provide faculty name.", AllowEmptyStrings = false)]
        public string facultyName { get; set; }


      
        [Display(Name = "Year Of Establishment")]
        public System.Nullable<int> yearOfEstablishment { get; set; }

        [Display(Name = "Dean")]
        [StringLength(80)]
        public string dean { get; set; }
        
        public string addUser { get; set; }

        public string updateUser { get; set; }

        public System.Nullable<DateTime> updateDate { get; set; }

        public bool activated { get; set; }
    }
}
