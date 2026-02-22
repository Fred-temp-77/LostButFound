using System.ComponentModel.DataAnnotations.Schema;

namespace LostButFound.Models
{
    public class Found
    {
        public Guid FoundId { get; set; }
        public string Name { get; set; }
        public string Item { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public string ItemDesc { get; set; }
        public string ImgByte { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
