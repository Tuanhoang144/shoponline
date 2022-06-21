using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAdmin.Models
{
    [Table("CommentNew")]
    public partial class CommentNew
    {
        public long IDNew { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [StringLength(500)]
        public string Message { get; set; }

        public DateTime? Create { get; set; }

        public virtual News News { get; set; }
    }
}