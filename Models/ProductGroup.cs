using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AgreementManagement.Models
{
    public class ProductGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string GroupDescription { get; set; }

        [Required]
        [MaxLength(50)] 
        public string GroupCode { get; set; }

        public bool Active { get; set; }
    }
}
