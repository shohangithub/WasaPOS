using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            AddedDate = DateTime.UtcNow;          
            Logs = new List<Log>();

        }
        [MaxLength(13)]
       // [Index("IX_PhoneNumber", IsUnique = true)]
        public override string PhoneNumber { get; set; }

        [StringLength(30)]
        public string FirstName { get; set; }
      
        [StringLength(30)]
        public string LastName { get; set; }

        public string FullName { get { return (FirstName + " " + LastName).TrimStart().TrimEnd(); } set { } }

        public Gender Gender { get; set; }
 

        [DisplayFormat(DataFormatString = "{0:MMMM d, yyyy}", ApplyFormatInEditMode = true)]
  
        public DateTime? DateOfBirth { get; set; }


        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public byte[] Image { get; set; }
        public string ContentType { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Log> Logs { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
