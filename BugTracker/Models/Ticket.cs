using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Slug { get; set; }
        [Required]
        public string Author { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateEdited { get; set; }
        [Required]
        public short Status { get; set; }
        public string Description { get; set; }
        public int GroupId { get; set; }
    }


    public class TicketGroup
    {

        public int Id { get; set; }
        [Required]
        [DisplayName ("Group Name")]
        public string name { get; set; }
        //public List<string> readRoles { get; set; }
        //public List<string> writeRoles { get; set; }
        [Required]
        [DisplayName("Created By")]
        public string author { get; set; }
        public DateTime dateCreated { get; set; }

    }


    public class Roles
    {
        [Required]
        public string Name { get; set; }
    }

    public class UserRoleViewModel
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public bool isSelected { get; set; }

    }
}
