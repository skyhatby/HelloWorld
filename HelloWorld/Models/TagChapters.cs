using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelloWorld.Models
{
    public class TagChapters
    {
        [Key]
        public int TagChapterId { get; set; }
        public int ChapterId { get; set; }
        public int TagId { get; set; }

        public virtual Chapter Chapter { get; set; }
        public virtual Tag Tag { get; set; }
    }
}