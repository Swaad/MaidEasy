﻿using MaidEasy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaidEasy.Controllers
{
    public class AdminWorkerController : Controller
    {
        // GET: AdminWorker
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add_worker()
        {
            return View();
        }
        public ActionResult WorkerList()
        {
            DBHelper db = DBHelper.getDB();
            string sql = "SELECT count(WorkerId) FROM worker";
            var table = db.getData(sql);
            table.Read();
            int count = Int32.Parse(table.GetString(0));
            table.Close();


            string[,] data = new string[count, 8];
            sql = "select WorkerId,Name,gender,Area,type,status,experience,rating from Worker";
            table = db.getData(sql);
            int i = 0;
            while (table.Read())
            {
                data[i, 0] = table.GetString(0);
                data[i, 1] = table.GetString(1);
                data[i, 2] = table.GetString(2);
                data[i, 3] = table.GetString(3);
                data[i, 4] = table.GetString(4);
                data[i, 5] = table.GetString(5);
                data[i, 6] = table.GetString(6);
                data[i, 7] = table.GetString(7);

                i++;
            }
            table.Close();
            i = 0;
            while (i < count)
            {
                data[i, 3] = getThanaList(data[i, 3]);
                data[i, 4] = getWorkerTypeList(data[i, 4]);
                i++;
            }

            System.Diagnostics.Debug.WriteLine("---------END Worker info-----------");
            Session["WorkerList"] = data;
            Session["WorkerList_Count"] = count;
            return View();
        }
        public ActionResult Edit_Worker()
        {
            if (Request["workerID"] != null) Session["workerID"] = Request["workerID"];

            var id = Session["workerID"];
            string sql = "SELECT Name,fatherName,mobile,PresentAddress,PermanentAddress,gender,type,Area,image from worker where WorkerId = '" + id + "'";

            DBHelper db = DBHelper.getDB();
            var table = db.getData(sql);
            table.Read();
            string[] data = new string[10];
            data[0] = table.GetString(0);
            data[1] = table.GetString(1);
            data[2] = table.GetString(2);
            data[3] = table.GetString(3);
            data[4] = table.GetString(4);
            data[5] = table.GetString(5);
            data[6] = table.GetString(6);
            data[7] = table.GetString(7);
            data[8] = table.GetString(8);
            table.Close();

            data[9] = data[6];
            data[6] = getWorkerTypeList(data[6]);

            int cnt = data[7].Length;
            sql = "SELECT Name from thana";
            string[] thanaList = new string[cnt];
            int i = 0;
            table = db.getData(sql);
            while (table.Read())
            {
                thanaList[i++] = table.GetString(0);
            }
            table.Close();

            ViewData["thanaList"] = thanaList;
            ViewData["WorkerData"] = data;
            return View();
        }

        public ActionResult AddNewWorker()
        {
            var name = Request["name"];
            var fatherName = Request["fathername"];
            var phone = Request["Phone"];
            var presentAddress = Request["presentAddress"];
            var permanentAddress = Request["permanentAddress"];
            var gender = Request["gender"];
            return View();
        }

        public ActionResult SaveWorkerData()
        {
            var name = Request["name"];
            System.Diagnostics.Debug.WriteLine("-----Name-------------------------------");
            System.Diagnostics.Debug.WriteLine(name);
            System.Diagnostics.Debug.WriteLine("-----Name-------------------------------");
            return RedirectToAction("Edit_Worker", "AdminWorker");
        }
        private string getThanaList(string thanaString)
        {
            string ret = "", sql;
            DBHelper db = DBHelper.getDB();
            int length = thanaString.Length, i = 0, ind = 0;
            while (i < length)
            {
                if (thanaString[i] == '1')
                {
                    sql = "select Name from Thana where ThanaId = '" + (i + 1) + "' ";
                    var table = db.getData(sql);
                    table.Read();
                    string tName = table.GetString(0);
                    if (ind != 0) ret += ",\n";
                    else ind = 1;
                    ret += tName;
                    //System.Diagnostics.Debug.WriteLine(table[0]);
                    table.Close();
                }
                i++;
            }
            return ret;
        }

        private string getWorkerTypeList(string typeString)
        {
            string ret = "";
            int length = typeString.Length, i = 0, ind = 0;
            while (i < length)
            {
                if (typeString[i] == '1')
                {
                    if (ind != 0) ret += ",\n";
                    else ind = 1;
                    if (i == 0) ret += "temporary";
                    else if (i == 1) ret += "permanent";
                    else if (i == 2) ret += "babycare";
                    else ret += "elderlycare";
                }
                i++;
            }
            return ret;
        }
    }
}
