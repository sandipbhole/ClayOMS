using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Clay.OMS.Message
{
    public class Degree
    {

        public int degreeID { get; set; }

        [Display(Name = "Degree")]
        [StringLength(100)]
        [Required(ErrorMessage = "Please provide degree.", AllowEmptyStrings = false)]
        public string degree { get; set; }

        public int programmeTypeID { get; set; }

        [Display(Name = "Programme Type")]
        [Required(ErrorMessage = "Please provide programme type.", AllowEmptyStrings = false)]
        public string programmeType { get; set; }

        public string seperator { get; set; }

        public System.Nullable<Boolean> activated { get; set; }

        public string addUser {get; set;}

        public DateTime addDate { get; set; }

        public string updateUser { get; set;}

        public System.Nullable<DateTime> updateDate { get; set; }
    }
}
