using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }
    }
}
