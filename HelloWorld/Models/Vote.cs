using System.ComponentModel.DataAnnotations;

namespace HelloWorld.Models
{
    public class Vote
    {
        [Key]
        public int VoteId { get; set; }
        public byte Like { get; set; }
        public byte Dislike { get; set; }
        public int ChapterId { get; set; }
        public string UserName { get; set; }

        public virtual Chapter Chapter { get; set; }
    }
}