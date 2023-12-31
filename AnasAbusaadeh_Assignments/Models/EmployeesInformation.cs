//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AnasAbusaadeh_Assignments.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class EmployeesInformation
    {
       


            [Required]
            //[Remote("IsEmployeeIDUnique", "Employee", ErrorMessage = "This Employee number is already in use.")]
            public int ID { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
            public Nullable<int> Gender { get; set; }

            [Required]
            public Nullable<System.DateTime> DOB { get; set; }

            [Required]
            public Nullable<System.DateTime> DOH { get; set; }

            [Required]
            public string Department { get; set; }

            [Required]
            public Nullable<decimal> BasicSal { get; set; }

            [Required]
            public Nullable<decimal> TotalSal { get; set; }

            [Required]
            public Nullable<int> Mflg { get; set; }

            public int PK { get; set; }

            public EmployeesInformation_Validation Info_Validation { get; set; }
     
    }
}
