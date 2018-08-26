namespace GuiMailCongThongTin.DAO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CapTruong")]
    public partial class CapTruong
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CapTruong()
        {
            Schools = new HashSet<School>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string TenDayDu { get; set; }

        public bool? IsActive { get; set; }

        [StringLength(50)]
        public string TenVietTat { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<School> Schools { get; set; }
    }
}
