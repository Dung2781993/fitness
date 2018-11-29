namespace FIT5032_fitness.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("GroupTraining")]
    public partial class GroupTraining
    {
        [Key]
        public int GroupId { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Booking Date")]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

    }

}
