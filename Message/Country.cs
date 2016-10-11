using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Clay.OMS.Message
{
    public class Country
    {
        public int countryID { get; set; }


        [Display(Name = "Country")]
        [StringLength(50)]
        [Required(ErrorMessage = "Please provide country.", AllowEmptyStrings = false)]
        public string country { get; set; }
    }
}
