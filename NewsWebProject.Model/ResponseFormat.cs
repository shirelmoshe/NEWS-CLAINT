using Microsoft.AspNetCore.Mvc;

namespace NewsWebProject.Model
{
    public record ResponseFormat
    {
        public string ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
        public Guid ResponseId { get; set; }
        public JsonResult Data { get; set; }
    }
}
