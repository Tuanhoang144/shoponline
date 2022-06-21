namespace WebBanHang.Models.Emtity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommentNew")]
    public partial class CommentNew
    {
        public long IDNew { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [StringLength(500)]
        public string Message { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        
        public DateTime? Create { get; set; }

        public virtual News News { get; set; }
    }
}
