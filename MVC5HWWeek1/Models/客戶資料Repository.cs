using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5HWWeek1.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
        public 客戶資料 Find(int id)
        {
            return this.All().Where(c => c.Id == id).FirstOrDefault();
        }
        public override IQueryable<客戶資料> All()
        {
            return base.All().Where(c => !c.是否被刪除);
        }
        public IQueryable<客戶資料> Search(string name, string type)
        {
            var data = this.All();
            if (!string.IsNullOrEmpty(name))
            {
                data = data.Where(c => c.客戶名稱.Contains(name));
            }
            if (!string.IsNullOrEmpty(type))
            {
                data = data.Where(c => c.客戶分類 == type);
            }
            return data;
        }
        public override void Delete(客戶資料 客戶資料)
        {
            客戶資料.是否被刪除 = true;
        }
    }

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}