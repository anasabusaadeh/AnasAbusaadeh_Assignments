using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnasAbusaadeh_Assignments.Models
{
    public class EmployeesInformation_Validation
    {


        public bool Contains_NotValidData { get; set; }
        public bool isValid_ID { get; set; }
        public bool isValid_Name { get; set; }
        public bool isValid_Gender { get; set; }
        public bool isValid_DOB { get; set; }
        public bool isValid_DOH { get; set; }
        public bool isValid_Department { get; set; }
        public bool isValid_BasicSal { get; set; }
        public bool isValid_TotalSal { get; set; }
        public bool isValid_Mflg { get; set; }
        public int ExcelRowNumber { get; set; }




    }
}