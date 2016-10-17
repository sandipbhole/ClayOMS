using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Clay.OMS.Message
{
    public class Designation
    {
        public long designationID { get; set; }

        [Display(Name = "Designation")]
        [StringLength(50)]
        [Required(ErrorMessage = "Please provide designation.", AllowEmptyStrings = false)]
        public string designation { get; set; }

        public System.Nullable<Boolean> activated { get; set; }

        public string addUser { get; set; }

        public DateTime addDate { get; set; }

        public string updateUser { get; set; }

        public System.Nullable<DateTime> updateDate { get; set; }
    }
}
