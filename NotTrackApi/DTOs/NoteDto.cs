using System.ComponentModel.DataAnnotations;

namespace NotTrackApi.DTOs
{
    public class NoteDto
    {
        [Required(ErrorMessage = "Başlık zorunludur.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "İçerik zorunludur.")]
        public string Content { get; set; }
    }
}
