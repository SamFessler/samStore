using samStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace samStore.Controllers
{
    public class StudentController : Controller
    {


        private static List<StudentModel> students = new List<StudentModel>();

        // GET: Student
        //can pass in paramaters and change output
        // /student/index/ralph
        // to include ID 2
        // /student/index/Ralph?id2=erica
        // passing in int variables the must be nulled using ? to allow no data to be passed in
        //
        // leave controller return type as ActionResult most always so that it is possible to return all types of result classes see notes
        public ActionResult Index(string id, string format = "html")
        {
            if (students.Count == 0)
            {
                students.Add(new StudentModel { Id = 1, FirstName = "Ralph", LastName = "Comb", FavoriteFood = "coffee" });
                students.Add(new StudentModel { Id = 2, FirstName = "JinSeong", LastName = "Kim", FavoriteFood = "apples" });
                students.Add(new StudentModel { Id = 3, FirstName = "Sam", LastName = "Fessler", FavoriteFood = "shrimp" });
                students.Add(new StudentModel { Id = 4, FirstName = "Erica", LastName = "wasilenko", FavoriteFood = "humus" });
                students.Add(new StudentModel { Id = 5, FirstName = "Will", LastName = "Mabry", FavoriteFood = "Ice-Cream" });
                students.Add(new StudentModel { Id = 6, FirstName = "Joe", LastName = "Johnson", FavoriteFood = "Nachos" });
            }

            if (format == "html")
            {
                return View(students);
            }
            if (format == "text")
            {
                return Content(string.Join(",", students.Select(x => x.FirstName)));
            }
            if (format == "json")
            {
                return Json(students, JsonRequestBehavior.AllowGet);
            }

            //takes model and passes it to the appopriate view document
            return View(students);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            return View(students.First(x => x.Id == id));
        }

        [HttpPost]
        public ActionResult Edit(StudentModel model)
        {
            var student = students.FirstOrDefault(x => x.Id == model.Id);

            student.FirstName = model.FirstName;
            student.LastName = model.LastName;
            student.FavoriteFood = model.FavoriteFood;

            return RedirectToAction("Index", new { edited = true });
        }
    }
}