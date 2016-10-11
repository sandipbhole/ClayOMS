using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Clay.OMS.Message
{
    public class ExaminationGrade
    {
        public int gradeID { get; set; }

        [Display(Name = "Grade")]
        [StringLength(100)]
        [Required(ErrorMessage = "Please provide grade.", AllowEmptyStrings = false)]
        public string grade { get; set; }

        [Display(Name = "Percentage From")]
        [StringLength(100)]
        [Required(ErrorMessage = "Please provide percentage from.", AllowEmptyStrings = false)]
        public decimal percentageFrom { get; set; }

        [Display(Name = "Percentage To")]
        [StringLength(100)]
        [Required(ErrorMessage = "Please provide percentage to.", AllowEmptyStrings = false)]
        public decimal percentageTo { get; set; }

        public string description { get; set; }

        [Display(Name = "Academic Year")]       
        [Required(ErrorMessage = "Please provide academic year.", AllowEmptyStrings = false)]
        public int academicYear { get; set; }

        public int programmeTypeID { get; set; }

        [Display(Name = "Graduation Type")]
        [StringLength(100)]
        [Required(ErrorMessage = "Please provide graduation type.", AllowEmptyStrings = false)]
        public string programmeType { get; set; }

        public string updateUser { get; set; }

        public DateTime updateDate { get; set; }

        public Boolean activated { get; set; }
}
}
