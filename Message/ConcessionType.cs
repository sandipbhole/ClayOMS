using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Clay.OMS.Message
{
    public class ConcessionType
    {
        public long concessionTypeID { get; set; }

        [Display(Name = "Concession type")]
        [StringLength(50)]
        [Required(ErrorMessage = "Please provide concession type.", AllowEmptyStrings = false)]
        public string concessionType { get; set; }

        public System.Nullable<Boolean> activated { get; set; }

        public string addUser { get; set; }

        public DateTime addDate { get; set; }

        public string updateUser { get; set; }

        public System.Nullable<DateTime> updateDate { get; set; }
    }
}
