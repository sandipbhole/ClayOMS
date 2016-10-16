using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Clay.OMS.Message
{
    public class Programme
    {
      public long programmeID { get; set; }

      public System.Nullable<long> degreeID { get; set; }

        public string degree { get; set; }

        [Display(Name = "Programme")]
        [StringLength(150)]
        [Required(ErrorMessage = "Please provide programme.", AllowEmptyStrings = false)]
        public string programmeName { get; set; }

        public long facultyID { get; set; }

        [Display(Name = "Faculty")]
        [StringLength(50)]
            [Required(ErrorMessage = "Please provide faculty.", AllowEmptyStrings = false)]
        public string faculty { get; set; }

        public System.Nullable<long> programmeTypeID { get; set; }

        public string programmeType { get; set; }

        public string programmeDuration { get; set; }

        public string coordinator { get; set; }

        public string stipulatedPeriod { get; set; }

        public string programmeDescription { get; set; }

    public System.Nullable<int> programmeOrder { get; set; }

    public System.Nullable<Boolean> activated { get; set; }

    public string addUser { get; set; }

    public DateTime addDate { get; set; }

    public string updateUser { get; set; }

    public DateTime updateDate { get; set; }
}
}
