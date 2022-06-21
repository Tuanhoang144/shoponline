namespace WebBanHang.Models.Emtity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contract")]
    public partial class Contract
    {
        public long ID { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public string Message { get; set; }

        [StringLength(300)]
        public string Subject { get; set; }

        [StringLength(200)]
        public string Email { get; set; }
    }
}
