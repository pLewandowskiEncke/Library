using Library.Domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Library.Application.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public BookStatus Status { get; set; }
    }
}
