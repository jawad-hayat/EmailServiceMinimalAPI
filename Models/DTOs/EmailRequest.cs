using System.ComponentModel.DataAnnotations;

namespace EmailServiceAPI.Models.DTOs
{
    public class EmailRequest
    {
        [Required]
        [EmailAddress]
        public string? To { get; set; }

        [Required]
        public string? Subject { get; set; }

        [Required]
        public string? Body { get; set; }
    }
}
