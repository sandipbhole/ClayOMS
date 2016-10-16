using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Clay.OMS.Message
{
    public class Subject
    {
        public System.Nullable<long> subjectID { get; set; }

        public string subjectCode { get; set; }

        [Display(Name = "Subject")]
        [StringLength(100)]
        [Required(ErrorMessage = "Please provide subject.", AllowEmptyStrings = false)]
        public string subject { get; set; }

        public System.Nullable<long> departmentID { get; set; }

        [Display(Name = "Department")]
        [StringLength(50)]
        [Required(ErrorMessage = "Please provide department.", AllowEmptyStrings = false)]
        public string department { get; set; }

        public System.Nullable<long> programmeTypeID { get; set; }

        [Display(Name = "Programme Type")]
        [StringLength(50)]
        [Required(ErrorMessage = "Please provide programme type.", AllowEmptyStrings = false)]
        public string programmeType { get; set; }

        public string subjectTypeID { get; set; }

        [Display(Name = "Subject Type")]
        [StringLength(50)]
        [Required(ErrorMessage = "Please provide subject type.", AllowEmptyStrings = false)]
        public string subjectType { get; set; }

        [Display(Name = "Total Mark")]        
        [Required(ErrorMessage = "Please provide total mark.", AllowEmptyStrings = false)]
        public System.Nullable<int> totalMark { get; set; }

        public string batchFrom { get; set; }

        public string batchNo { get; set; }

        public System.Nullable<int> subjectMode { get; set; }

        public string subjectDescription { get; set; }

        public System.Nullable<bool> activated { get; set; }

        public string updateUser { get; set; }

        public string addUser { get; set; }

        public DateTime  updateDate { get; set; }

        public DateTime addDate { get; set; }
    }
}
