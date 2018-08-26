namespace CongThongTinDienTu.Models.DAO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DVQL")]
    public partial class DVQL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DVQL()
        {
            Schools = new HashSet<School>();
        }

        public int Id { get; set; }

        [StringLength(200)]
        public string Ten { get; set; }

        [StringLength(200)]
        public string DiaChi { get; set; }

        [StringLength(200)]
        public string TenDayDu { get; set; }

        public bool? IsActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<School> Schools { get; set; }
    }
}
