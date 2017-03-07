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


        private List<StudentModel> students = new List<StudentModel>();

        // GET: Student
        //can pass in paramaters and change output
        // /student/index/ralph
        // to include ID 2
        // /student/index/Ralph?id2=erica
        // passing in int variables the must be nulled using ? to allow no data to be passed in
        public ActionResult Index(string id, string id2)
        {
            if(students.Count == 0 )
            {
                students.Add(new StudentModel { Id = 1, FirstName = "Ralph", LastName = "Comb", FavoriteFood = "coffee" });
                students.Add(new StudentModel { Id = 2, FirstName = "JinSeong", LastName = "Kim", FavoriteFood = "apples" });
                students.Add(new StudentModel { Id = 3, FirstName = "Sam", LastName = "Fessler", FavoriteFood = "shrimp" });
                students.Add(new StudentModel { Id = 4, FirstName = "Erica", LastName = "wasilenko", FavoriteFood = "humus" });
                students.Add(new StudentModel { Id = 5, FirstName = "Will", LastName = "Mabry", FavoriteFood = "Ice-Cream" });
                students.Add(new StudentModel { Id = 6, FirstName = "Joe", LastName = "Johnson", FavoriteFood = "Nachos" });
            }
            //takes model and passes it to the appopriate view document
            return View(students);
        }

        public ActionResult GetMoreCoffee()
        {
            return Json("Im going to get more coffee", JsonRequestBehavior.AllowGet);
        }
    }
}