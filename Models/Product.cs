using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AgreementManagement.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("ProductGroup")]
        public int ProductGroupId { get; set; }

        [Required]
        public string ProductDescription { get; set; }

        [Required]
        [MaxLength(50)] 
        public string ProductNumber { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")] 
        public decimal Price { get; set; }

        public bool Active { get; set; }
        public virtual ProductGroup ProductGroup { get; set; }
    }
}
