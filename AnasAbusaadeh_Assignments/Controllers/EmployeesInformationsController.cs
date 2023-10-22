using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AnasAbusaadeh_Assignments.Models;
using OfficeOpenXml;

namespace AnasAbusaadeh_Assignments.Controllers
{
    public class EmployeesInformationController : Controller
    {
        private AnasAbusaadehAssignmentEntities db = new AnasAbusaadehAssignmentEntities();

        // GET: EmployeesInformation
        public ActionResult Index()
        {
            return View(db.EmployeesInformations.ToList());
        }

        // GET: EmployeesInformation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeesInformation employeesInformation = db.EmployeesInformations.Find(id);
            if (employeesInformation == null)
            {
                return HttpNotFound();
            }
            return View(employeesInformation);
        }

        // GET: EmployeesInformation/Create
        public ActionResult Create()
        {
            return View();
        }


        public ActionResult UploadEmployeesInfoExcel()
        {
            return View();
        }


        ////Validate if employee is already Exsist
        //public JsonResult IsEmployeeIDUnique(int EmployeeNumber, int ID)
        //{
        //    // Check if the number is unique in the database
        //    bool isUnique = !EmployeesInformation.Any(e => e.EmployeeNumber == EmployeeNumber && e.ID != ID);

        //    return Json(isUnique, JsonRequestBehavior.AllowGet);
        //}

        // POST: EmployeesInformation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadEmployeesInfoExcel(HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    // Process the uploaded Excel file
                    var employees = ProcessExcelFile(file);
                    if (employees.ListOf_NotValidEmpInfo.Count > 0)
                    {
                        // display Not Valid Data Format with Rows message 
                        var errorMsg = GenerateErrorMessage(employees.ListOf_NotValidEmpInfo);
                        return View("ErrorView", employees.ListOf_NotValidEmpInfo);
                    }
                    else  // insert data
                    {
                        // Insert the employees into the database
                        db.EmployeesInformations.AddRange(employees.ListOf_ValidEmpInfo);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                return View();
            }
            catch
            {
                return View("Shared/Error");
            }

           
        }

        private (List<EmployeesInformation> ListOf_ValidEmpInfo, List<EmployeesInformation> ListOf_NotValidEmpInfo) ProcessExcelFile(HttpPostedFileBase file)
        {

            StringBuilder sbRowsOfNotValidData = new StringBuilder();
            var ListOfEmployees_Validation_Sucess = new List<EmployeesInformation>();
            var ListOfEmployees_ValidationFailed = new List<EmployeesInformation>();


            using (var package = new ExcelPackage(file.InputStream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    //validate Employee Information 
                    var Employee = FormatUtility.ValidateEmployeeInformation
                           (
                           worksheet.Cells[row, 1].Text, // ID
                           worksheet.Cells[row, 2].Text, // Name
                           worksheet.Cells[row, 3].Text, // Gender
                           worksheet.Cells[row, 4].Text, //DOB
                           worksheet.Cells[row, 5].Text, //DOH
                           worksheet.Cells[row, 6].Text, //Department
                           worksheet.Cells[row, 7].Text, // BasicSal
                           worksheet.Cells[row, 8].Text, // TotalSal
                           worksheet.Cells[row, 9].Text, // Mflg
                           row
                           );


                    if (Employee.Info_Validation.Contains_NotValidData)
                    {
                        ListOfEmployees_ValidationFailed.Add(Employee);
                    }
                    else
                    {
                        ListOfEmployees_Validation_Sucess.Add(Employee);
                    }
                }


                return (ListOfEmployees_Validation_Sucess, ListOfEmployees_ValidationFailed);
            }





        }


        // POST: EmployeesInformation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Gender,DOB,DOH,Department,BasicSal,TotalSal,Mflg")] EmployeesInformation employeesInformation)
        {
            if (ModelState.IsValid)
            {
                db.EmployeesInformations.Add(employeesInformation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employeesInformation);
        }







        // GET: EmployeesInformation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeesInformation employeesInformation = db.EmployeesInformations.Find(id);
            if (employeesInformation == null)
            {
                return HttpNotFound();
            }
            return View(employeesInformation);
        }

        // POST: EmployeesInformation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Gender,DOB,DOH,Department,BasicSal,TotalSal,Mflg")] EmployeesInformation employeesInformation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeesInformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employeesInformation);
        }

        // GET: EmployeesInformation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeesInformation employeesInformation = db.EmployeesInformations.Find(id);
            if (employeesInformation == null)
            {
                return HttpNotFound();
            }
            return View(employeesInformation);
        }

        // POST: EmployeesInformation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeesInformation employeesInformation = db.EmployeesInformations.Find(id);
            db.EmployeesInformations.Remove(employeesInformation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public string GenerateErrorMessage(List<EmployeesInformation> employees)
        {

            return "";
        }
    }



    public static class FormatUtility
    {
        // Standard date formats
        public static string ShortDate = "MM/dd/yyyy";
        public static string LongDate = "dddd, MMMM dd, yyyy";
        // Add more date formats as needed



        public static EmployeesInformation ValidateEmployeeInformation(string ID, string Name, string Gender, string DOB, string DOH, string Department, string BasicSal, string TotalSal, string Mflg, int excelRowNumber)
        {
            try
            {
                #region start validate employees information format

                var Validate_DOB = FormatUtility.ValidateDateFormat(DOB, FormatUtility.LongDate);
                var Validate_DOH = FormatUtility.ValidateDateFormat(DOH, FormatUtility.ShortDate);

                var EmpInfoValidation = new EmployeesInformation_Validation
                {
                    isValid_DOB = Validate_DOB.isValidDate,
                    isValid_DOH = Validate_DOH.isValidDate,
                    isValid_ID = int.TryParse(ID, out int oID),
                    isValid_Gender = int.TryParse(Gender, out int oGender),
                    isValid_Mflg = int.TryParse(Mflg, out int oMflg),
                    isValid_BasicSal = decimal.TryParse(BasicSal, out decimal oBasicSal),
                    isValid_TotalSal = decimal.TryParse(TotalSal, out decimal oTotalSal),
                    isValid_Name = !string.IsNullOrEmpty(Name),
                    isValid_Department = !string.IsNullOrEmpty(Department),
                    ExcelRowNumber = excelRowNumber,
                };
                if (EmpInfoValidation.isValid_DOB && EmpInfoValidation.isValid_DOH && EmpInfoValidation.isValid_BasicSal && EmpInfoValidation.isValid_Department
                 && EmpInfoValidation.isValid_Gender && EmpInfoValidation.isValid_ID && EmpInfoValidation.isValid_Mflg && EmpInfoValidation.isValid_Name && EmpInfoValidation.isValid_TotalSal)
                {
                    EmpInfoValidation.Contains_NotValidData = false;

                }
                else
                {
                    EmpInfoValidation.Contains_NotValidData = true;
                }
                #endregion




                var EmpInfo = new EmployeesInformation
                {
                    DOB = Validate_DOB.outDate,
                    DOH = Validate_DOH.outDate,
                    ID = oID,
                    Name = Name,
                    Department = Department,
                    BasicSal = oBasicSal,
                    TotalSal = oTotalSal,
                    Gender = oGender,
                    Mflg = oMflg,
                    Info_Validation = EmpInfoValidation
                };

                return EmpInfo;
            }

            catch
            {
                return null;
            }
        }
        public static (bool isValidDate, DateTime outDate) ValidateDateFormat(string date, string FromFormat)
        {
            try
            {
                var _isValidDate = DateTime.TryParseExact(date, FromFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime oDate);

                if (_isValidDate)
                {
                    return (_isValidDate, oDate);
                }
                else
                {
                    return (_isValidDate, oDate);
                }

            }
            catch
            {
                return (false, DateTime.MinValue);
            }
        }




    }





}
