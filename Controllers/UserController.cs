﻿using MaidEasy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaidEasy.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult feedback()
        {
            int id = Int32.Parse(Request["maid"].ToString());
            ViewData["maid"] = id;
            DBHelper db = DBHelper.getDB();
            string sql = "SELECT Name from Worker where WorkerId = '" + id + "'";
            var table = db.getData(sql);
            table.Read();
            string name = table.GetString(0);
            table.Close();
            ViewData["maidName"] = name;
            return View();
        }

        private string getThana(string s)
        {
            int id = 0, len = s.Length;
            for (int i = 0; i < len; i++)
            {
                if (s[i] == '1')
                { id = i + 1; break; }
            }
            DBHelper db = DBHelper.getDB();
            string sql = "SELECT Name from Thana where ThanaId  = '" + id + "'";
            var table = db.getData(sql);
            table.Read();
            string ret = table.GetString(0);
            table.Close();
            return ret;
        }

        [HttpGet]
        public ActionResult user_profile()
        {
            var id = Session["username"];
            DBHelper db = DBHelper.getDB();
            string sql = "SELECT Name ,mobile ,PresentAddress,PermanentAddress,thana  from Users where username = '" + id + "'";
            var table = db.getData(sql);
            table.Read();
            ViewData["Name"]             =  table.GetString(0);
            ViewData["mobile"]           =  table.GetString(1);
            ViewData["PresentAddress"]   =  table.GetString(2);
            ViewData["PermanentAddress"] =  table.GetString(3);
            string s                     =  table.GetString(4);
            table.Close();

            /*System.Diagnostics.Debug.WriteLine("--------thana id------------");
            System.Diagnostics.Debug.WriteLine(t);
            System.Diagnostics.Debug.WriteLine("--------------------");*/

            ViewData["thana"] = getThana(s);
            return View();
        }

        public ActionResult hired_workers_profile()
        {
            DBHelper db = DBHelper.getDB();
            int id = Int32.Parse(Session["userID"].ToString());
            string sql = "SELECT count(WorkerId) from Contracts where UserId = '" + id + "' and status = 'current'";
            var table = db.getData(sql);
            table.Read();
            int cnt1 = Int32.Parse(table.GetString(0));
            table.Close();
            string[,] data1 = new string[cnt1, 8];
            sql = "SELECT WorkerName, StartMonth, EndMonth, StartTime, EndTime, Amount, Worklist, WorkerId  from Contracts where UserId = '" + id + "' and status = 'current'";
            table = db.getData(sql);
            int i = 0;
            while(table.Read())
            {
                data1[i, 0] = table.GetString(0);
                data1[i, 1] = table.GetString(1);
                data1[i, 2] = table.GetString(2);
                data1[i, 3] = table.GetString(3);
                data1[i, 4] = table.GetString(4);
                data1[i, 5] = table.GetString(5);
                data1[i, 6] = table.GetString(6);
                data1[i, 7] = table.GetString(7);
                i++;
            }
            table.Close();


            sql = "SELECT count(WorkerId) from Contracts where UserId = '" + id + "' and status = 'previous'";
            table = db.getData(sql);
            table.Read();
            int cnt2 = Int32.Parse(table.GetString(0));
            table.Close();
            string[,] data2 = new string[cnt2, 8];
            sql = "SELECT WorkerName, StartMonth, EndMonth, StartTime, EndTime, Amount, Worklist, WorkerId from Contracts where UserId = '" + id + "' and status = 'previous'";
            table = db.getData(sql);
            i = 0;
            while (table.Read())
            {
                data2[i, 0] = table.GetString(0);
                data2[i, 1] = table.GetString(1);
                data2[i, 2] = table.GetString(2);
                data2[i, 3] = table.GetString(3);
                data2[i, 4] = table.GetString(4);
                data2[i, 5] = table.GetString(5);
                data2[i, 6] = table.GetString(6);
                data2[i, 7] = table.GetString(7);
                i++;
            }
            table.Close();


            ViewData["cnt1"] = cnt1;        ViewData["cnt2"] = cnt2;
            ViewData["data1"] = data1;      ViewData["data2"] = data2;
            return View();
        }
        public ActionResult Edit_profile()
        {
            if (TempData["message"] != null) //It will true when Password not match with DB password 
                ViewBag.Error = TempData["message"].ToString();


            DBHelper db = DBHelper.getDB();
            string sql = "SELECT Name ,  mobile , PresentAddress , PermanentAddress , thana , image  from Users where username  = '" + Session["username"] + "' ";
            var table = db.getData(sql);
            table.Read();
            TempData["Ename"] =              table.GetString(0);
            TempData["Ephone"] =             table.GetString(1);
            TempData["EpresentAddress"] =    table.GetString(2);
            TempData["EpermanentAddress"] =  table.GetString(3);
            string s =             table.GetString(4);
            table.Close();

            TempData["Ethana"] = getThana(s);


            return View();
        }
        [HttpPost]
        public ActionResult EntryFeedback()
        {
            var comment = Request["comment"];
            var rating = Request["rating"];
            var wID = Request["maid"];

            DBHelper db = DBHelper.getDB();
            string sql = "INSERT into WorkerReview (WorkerId, rating , username , description )VALUES( '" + wID + " ', ' " + rating + "', '" + Session["username"] + "', '" + comment + " ');";
            db.setData(sql);
            sql = "SELECT sum(rating),COUNT(Id) from workerreview WHERE WorkerId = '" + wID + "' ";
            var table = db.getData(sql);
            table.Read();
            double rat = Convert.ToDouble(table.GetString(0));
            int c = Int32.Parse(table.GetString(1));
            table.Close();

            rat = rat / c;

            sql = "UPDATE Worker SET rating  = '" + rat + "' where WorkerId = '" + wID + "'";
            db.setData(sql);
            //return View("~/Views/User/user_profile.cshtml");
            return RedirectToAction("user_profile", "User");
        }
    }
}