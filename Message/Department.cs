using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Clay.OMS.Message
{
    public class Department
    {
        public long departmentID { get; set; }

        public long facultyID { get; set; }

        [Display(Name = "Department Code")]
        [StringLength(20)]
        public string code { get; set; }

        [Display(Name = "Faculty Name")]
        [StringLength(50)]
        [Required(ErrorMessage = "Please provide faculty name.", AllowEmptyStrings = false)]
        public string facultyName { get; set; }

        [Display(Name = "Department Name")]
        [StringLength(100)]
        [Required(ErrorMessage = "Please provide department name.", AllowEmptyStrings = false)]
        public string department { get; set; }

        [Display(Name = "Department Order")]        
        public int departmentOrder { get; set; }

        [Display(Name = "Department Type")]
        public System.Nullable <int> departmentType {get; set;}

        [Display(Name = "Year Of Establishment")]
        public System.Nullable<int> yearOfEstablishment { get; set; }

        public string addUser { get; set; }

        public string updateUser { get; set; }

        public System.Nullable<DateTime> updateDate { get; set; }

        public System.Nullable<bool> activated { get; set; }
    }
}
