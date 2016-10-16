using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Clay.OMS.Message
{
    public class AcademicClass
    {
        public long classID { get; set; }

        public string classCode   { get; set; }

        [Display(Name = "Class")]
        [StringLength(50)]
        [Required(ErrorMessage = "Please provide class.", AllowEmptyStrings = false)]
        public string academicClass { get; set; }

        public long programmeID  { get; set; }

        [Display(Name = "Programme")]
        [StringLength(50)]
        [Required(ErrorMessage = "Please provide programme.", AllowEmptyStrings = false)]
        public string programme { get; set; }

        public int yearPosition { get; set; }

        [StringLength(50)]
        public string section { get; set; }

        public Boolean activated { get; set; }

        public DateTime addDate { get; set; }

        public string addUser { get; set; }

        public DateTime updateDate { get; set; }

        public string updateUser { get; set; }
    }
}
