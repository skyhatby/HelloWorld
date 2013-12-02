
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HelloWorld.Models
{
    public class Book
    {
        private ICollection<Chapter> _chapters;

        public Book()
        {
            _chapters = new List<Chapter>();
        }

        [Key]
        public int BookId { get; set; }
        public string BookName { get; set; }
        public int UserId { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual ICollection<Chapter> Chapters
        {
            get { return _chapters; }
            set { _chapters = value; }
        }
    }
}