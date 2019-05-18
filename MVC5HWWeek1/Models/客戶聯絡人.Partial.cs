namespace MVC5HWWeek1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    [MetadataType(typeof(客戶聯絡人MetaData))]
    public partial class 客戶聯絡人 : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var db = new 客戶資料Entities();

            if (this.Id == 0)
            {
                //新增
                if (db.客戶聯絡人.Where(c => c.Email == this.Email && c.客戶Id == this.客戶Id).Any())
                {
                    yield return new ValidationResult("Email 重複", new string[] { "Email"});
                }
            }
            else
            {
                if (this.是否被刪除 == false)
                {
                    //修改
                    if (db.客戶聯絡人.Where(c => c.Email == this.Email && c.Id != this.Id).Any())
                    {
                        yield return new ValidationResult("Email 重複");
                    }
                }
                else
                {
                    //刪除不做判斷
                }
            }

            yield return ValidationResult.Success;
        }
    }

    public partial class 客戶聯絡人MetaData
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int 客戶Id { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 職稱 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 姓名 { get; set; }
        
        [StringLength(250, ErrorMessage="欄位長度不得大於 250 個字元")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [RegularExpression(@"\d{4}-\d{6}", ErrorMessage = "格式錯誤(e.g.0911-111111)")]
        public string 手機 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string 電話 { get; set; }
        public Nullable<bool> 是否被刪除 { get; set; }
    
        public virtual 客戶資料 客戶資料 { get; set; }
    }
}
