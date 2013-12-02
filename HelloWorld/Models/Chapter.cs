﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HelloWorld.Models
{
    public class Chapter
    {
        private ICollection<Tag> _tags;
        private ICollection<Vote> _votes; 

        public Chapter()
        {
            _tags = new List<Tag>();
            _votes = new List<Vote>();
        }

        [Key]
        public int ChapterId { get; set; }
        public string ChapterName { get; set; }
        public string ChapterContent { get; set; }
        public int BookId { get; set; }

        public virtual ICollection<Tag> Tags
        {
            get { return _tags; }
            set { _tags = value; }
        }

        public virtual ICollection<Vote> Votes
        {
            get { return _votes; }
            set { _votes = value; }
        }

    }
}