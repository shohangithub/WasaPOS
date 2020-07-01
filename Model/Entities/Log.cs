using System;
using System.ComponentModel.DataAnnotations;

namespace Model.Entities
{
    public class Log
    {
        public Log()
        {
            AddedDate = DateTime.Now;
        }
        public long Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public LogScreen LogScreen { get; set; }
        public string Message { get; set; }
        [Required]
        public string IpAddress { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
