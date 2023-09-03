using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgreementManagement.Models
{
    public class Agreement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } 

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime EffectiveDate { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ExpirationDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")] 
        public decimal NewPrice { get; set; }

        public bool Active { get; set; }
        public virtual Product Product { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}
