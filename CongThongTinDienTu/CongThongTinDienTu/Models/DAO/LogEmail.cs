namespace CongThongTinDienTu.Models.DAO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LogEmail")]
    public partial class LogEmail
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string NoidungGui { get; set; }

        public bool? IsSuccessful { get; set; }
    }
}
