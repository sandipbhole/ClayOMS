using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Clay.OMS.Message
{
    public class StaffType
    {
        public long staffTypeID { get; set; }

        [Display(Name = "Staff Type")]
        [StringLength(50)]
        [Required(ErrorMessage = "Please provide staff type.", AllowEmptyStrings = false)]
        public string staffType { get; set; }

        public Boolean activated { get; set; }

        public string addUser { get; set; }

        public DateTime  addDate { get; set; }

        public DateTime updateDate { get; set; }

        public string updateUser { get; set; }
    }
}
