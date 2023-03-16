using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Entities
{
    [Table("UserLogin")]
    [Index(nameof(Email), IsUnique = true)]
    public class UserLogin : BaseModel<Guid>
    {
        [Required, StringLength(50)]
        public string? FullName { get; set; }

        [Required, StringLength(100)]
        public string Email { get; set; }

        [Required, StringLength(100)]
        public string Password { get; set; }

        [Required,Display(Name = "RoleType")]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual RoleType RoleType { get; set; }
    }
}
