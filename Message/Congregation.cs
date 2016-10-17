using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Clay.OMS.Message
{
    public class Congregation
    {

        public long congregationID { get; set; }

        [Display(Name = "Congregation")]
        [StringLength(75)]
        [Required(ErrorMessage = "Please provide congregation.", AllowEmptyStrings = false)]
        public string congregation { get; set; }

        [Display(Name = "Congregation Code")]
        [StringLength(20)]
        [Required(ErrorMessage = "Please provide congregation code.", AllowEmptyStrings = false)]
        public string congregationCode { get; set; }

        public System.Nullable<Boolean> activated { get; set; }

        public string addUser { get; set; }

        public DateTime addDate { get; set; }

        public string updateUser { get; set; }

        public System.Nullable<DateTime> updateDate { get; set; }
    }
}
