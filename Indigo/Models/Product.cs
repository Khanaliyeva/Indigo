using System.ComponentModel.DataAnnotations.Schema;

namespace Indigo.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        //public string? Information { get; set; }
        public string? ImgUrl { get; set; }       

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
