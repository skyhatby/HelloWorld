using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HelloWorld.Models
{
    public class Tag
    {
        private ICollection<TagChapters> _tagchapters;

        public Tag()
        {
            _tagchapters = new List<TagChapters>();
        }

        [Key]
        public int TagId { get; set; }
        public string TagContent { get; set; }

        public virtual ICollection<TagChapters> TagChapterses
        {
            get { return _tagchapters; }
            set { _tagchapters = value; }
        }

    }
}