using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5HWWeek1.Models
{   
	public  class view客戶相關資訊Repository : EFRepository<view客戶相關資訊>, Iview客戶相關資訊Repository
	{
        public override IQueryable<view客戶相關資訊> All()
        {
            return base.All();
        }
    }

	public  interface Iview客戶相關資訊Repository : IRepository<view客戶相關資訊>
	{

	}
}