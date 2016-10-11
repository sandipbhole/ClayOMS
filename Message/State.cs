using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Clay.OMS.Message
{
    public class State
    {
        public int stateID { get; set; }

        [Display(Name = "State")]
        [StringLength(150)]
        [Required(ErrorMessage = "Please provide state.", AllowEmptyStrings = false)]
        public string state { get; set; }

        public int countryID { get; set; }

        [Display(Name = "Country")]
        [StringLength(150)]
        [Required(ErrorMessage = "Please provide country.", AllowEmptyStrings = false)]
        public string country { get; set; }

        public System.Nullable<Boolean> activated { get; set; }

        public string addUser { get; set; }

        public DateTime addDate { get; set; }

        public string updateUser { get; set; }

        public System.Nullable<DateTime> updateDate { get; set; }
    }
}
