using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Clay.OMS.Message
{
    public class AddressType
    {
        public long addressTypeID { get; set; }

        [Display(Name = "Address type")]
        [StringLength(50)]
        [Required(ErrorMessage = "Please provide address type.", AllowEmptyStrings = false)]
        public string addressType { get; set; }
    }
}
