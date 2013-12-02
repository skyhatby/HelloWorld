using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HelloWorld.Models
{
    [Table("UserProfile")]
    public class UserProfile
    {
        private ICollection<Book> _books;
        //private ICollection<Vote> _votes; 

        public UserProfile()
        {
            _books = new List<Book>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Book> Books
        {
            get { return _books; }
            set { _books = value; }
        }
        //public virtual ICollection<Vote> Votes
        //{
        //    get { return _votes; }
        //    set { _votes = value; }
        //}
    }
}