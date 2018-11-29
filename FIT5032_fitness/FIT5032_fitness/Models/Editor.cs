namespace FIT5032_fitness.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Editor")]
    public partial class Editor
    {
        [Key]
        public int ForumId { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [StringLength(50)]
        public string authorName { get; set; }

        public string path { get; set; }
    }
}
