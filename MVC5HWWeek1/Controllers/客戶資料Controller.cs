using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5HWWeek1.Models;
using ClosedXML.Excel;
using System.IO;
using System.Reflection;

namespace MVC5HWWeek1.Controllers
{

    public class 客戶資料Controller : Controller
    {
        //private 客戶資料Entities db = new 客戶資料Entities();
        客戶資料Repository repo;
        view客戶相關資訊Repository repoView;
        IEnumerable<string> 客戶分類 = new List<string>() { "", "個人", "法人", "公司" };

        public 客戶資料Controller()
        {
            repo = RepositoryHelper.Get客戶資料Repository();
            repoView = RepositoryHelper.Getview客戶相關資訊Repository();
            ViewBag.客戶分類 = new SelectList(客戶分類);
        }
        // GET: 客戶資料
        public ActionResult Index(string name, string type)
        {
            //var data = db.客戶資料.Where(c => !c.是否被刪除).AsQueryable();
            var data = repo.Search(name, type);
            //return View(db.客戶資料.Where(c => !c.是否被刪除).ToList());
            return View(data.ToList());
        }
        public ActionResult 客戶相關檢視表()
        {
            return View(repoView.All().ToList());
        }
        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            客戶資料 客戶資料 = repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: 客戶資料/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類,是否被刪除")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                //db.客戶資料.Add(客戶資料);
                //db.SaveChanges();
                repo.Add(客戶資料);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            客戶資料 客戶資料 = repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類,是否被刪除")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(客戶資料).State = EntityState.Modified;
                //db.SaveChanges();
                repo.UnitOfWork.Context.Entry(客戶資料).State = EntityState.Modified;
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            客戶資料 客戶資料 = repo.Find(id.Value);
            if (客戶資料 == null)
            {
            ViewBag.客戶分類 = new SelectList(客戶分類);
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            ////db.客戶資料.Remove(客戶資料);
            //客戶資料.是否被刪除 = true;
            //db.SaveChanges();
            客戶資料 客戶資料 = repo.Find(id);
            repo.Delete(客戶資料);
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }
        public FileResult Export()
        {
            //ClosedXML的用法 先new一個Excel Workbook
            using (XLWorkbook wb = new XLWorkbook())
            {
                var data = repo.All().Select(c => new { c.客戶名稱, c.統一編號, c.電話, c.傳真, c.地址, c.Email, c.客戶分類 });
                var ws = wb.Worksheets.Add("客戶資料", 1);
                //ws.Cell(1, 1).InsertData(ConvertObjectsToDataTable(data));
                ws.Cell(1, 1).InsertData(data);

                //因為是用Query的方式,這個地方要用串流的方式來存檔
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    //請注意 一定要加入這行,不然Excel會是空檔
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    //return this.File(memoryStream.ToArray(), "application/vnd.ms-excel", "Download.xlsx");
                    return this.File(memoryStream.ToArray(), "application/vnd.ms-excel", DateTime.Now.ToString() + ".xlsx");
                }
            }
        }
        public static DataTable ConvertObjectsToDataTable(IEnumerable<object> objects)
        {
            DataTable dt = null;

            if (objects != null && objects.Count() > 0)
            {
                Type type = objects.First().GetType();
                dt = new DataTable(type.Name);

                foreach (PropertyInfo property in type.GetProperties())
                {
                    dt.Columns.Add(new DataColumn(property.Name));
                }

                foreach (FieldInfo field in type.GetFields())
                {
                    dt.Columns.Add(new DataColumn(field.Name));
                }

                foreach (object obj in objects)
                {
                    DataRow dr = dt.NewRow();
                    foreach (DataColumn column in dt.Columns)
                    {
                        PropertyInfo propertyInfo = type.GetProperty(column.ColumnName);
                        if (propertyInfo != null)
                        {
                            dr[column.ColumnName] = propertyInfo.GetValue(obj, null);
                        }

                        FieldInfo fieldInfo = type.GetField(column.ColumnName);
                        if (fieldInfo != null)
                        {
                            dr[column.ColumnName] = fieldInfo.GetValue(obj);
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
                repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
