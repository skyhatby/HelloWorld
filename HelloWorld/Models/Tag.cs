using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelloWorld.Models
{
    public class Tag
    {
        private ICollection<Chapter> _chapters;

        public Tag()
        {
            _chapters = new List<Chapter>();
        }

        [Key]
        public int TagId { get; set; }
        public string TagContent { get; set; }

        public virtual ICollection<Chapter> Chapters
        {
            get { return _chapters; }
            set { _chapters = value; }
        }

    }
}