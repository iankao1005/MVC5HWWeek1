using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5HWWeek1.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
        public 客戶聯絡人 Find(int id)
        {
            return this.All().Where(c => c.Id == id).FirstOrDefault();
        }
        public override IQueryable<客戶聯絡人> All()
        {
            return base.All().Where(c => !c.是否被刪除);
        }
    }

	public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}