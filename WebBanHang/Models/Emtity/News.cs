namespace WebBanHang.Models.Emtity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class News
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public News()
        {
            CommentNews = new HashSet<CommentNew>();
        }

        public long ID { get; set; }

        [StringLength(500)]
        public string Title { get; set; }

   
        public string Description { get; set; }

        public bool? Delete { get; set; }

        public string Img { get; set; }

        public DateTime? CreateDate { get; set; }

        public long? IdType { get; set; }

        public int? View { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommentNew> CommentNews { get; set; }

        public virtual NewType NewType { get; set; }
    }
}
