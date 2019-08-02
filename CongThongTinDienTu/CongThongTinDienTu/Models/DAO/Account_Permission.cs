namespace CongThongTinDienTu.Models.DAO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Account_Permission
    {
        public int Id { get; set; }

        public int? PermissionId { get; set; }

        public bool? IsActive { get; set; }

        public int? AccountId { get; set; }

        public virtual Account Account { get; set; }

        public virtual Permission Permission { get; set; }
    }
}
