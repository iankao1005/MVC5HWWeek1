using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5HWWeek1.Models
{   
	public  class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
	{
        public 客戶銀行資訊 Find(int id)
        {
            return this.All().Where(c => c.Id == id).FirstOrDefault();
        }
        public override IQueryable<客戶銀行資訊> All()
        {
            return base.All().Where(c => !c.是否被刪除);
        }
    }

	public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
	{

	}
}