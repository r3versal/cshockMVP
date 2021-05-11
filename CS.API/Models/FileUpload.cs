using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace CS.API.Models
{
    public class FileUpload
    {
        public int Id { get; set; }
        public IFormFileCollection files { get; set; }
        public string users { get; set; }
    }
}
