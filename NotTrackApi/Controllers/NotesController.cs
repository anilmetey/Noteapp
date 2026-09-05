using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotTrackApi.Data;
using NotTrackApi.DTOs;
using NotTrackApi.Models;
using System.Security.Claims;

namespace NotTrackApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<NotesController> _logger;

        public NotesController(AppDbContext context, ILogger<NotesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create(NoteDto dto)
        {
            try
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.Title))
                {
                    _logger.LogWarning("Geçersiz veri.");
                    return BadRequest("Geçersiz not verisi.");
                }

                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(userEmail))
                {
                    _logger.LogWarning("Email alınamadı.");
                    return Unauthorized("Kullanıcı doğrulanamadı.");
                }

                var note = new Note
                {
                    Title = dto.Title.Trim(),
                    Content = dto.Content?.Trim(),
                    UserEmail = userEmail,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Notes.Add(note);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Not oluşturuldu: {@Note}", note);
                return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Not oluşturulurken beklenmeyen bir hata oluştu.");
                return StatusCode(500, "Bir hata oluştu.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
            {
                _logger.LogWarning("Kullanıcı email alınamadı.");
                return Unauthorized();
            }

            var notes = await _context.Notes
                .Where(n => n.UserEmail == userEmail)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            _logger.LogInformation("{Email} kullanıcısı {Count} adet not aldı.", userEmail, notes.Count);

            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var note = await _context.Notes.FindAsync(id);

            if (note == null)
            {
                _logger.LogWarning("ID'si {Id} olan not bulunamadı.", id);
                return NotFound();
            }

            if (note.UserEmail != userEmail)
            {
                _logger.LogWarning("Kullanıcı {Email}, kendisine ait olmayan nota erişmeye çalıştı (ID: {Id})", userEmail, id);
                return Forbid();
            }

            _logger.LogInformation("ID'si {Id} olan not getirildi.", id);
            return Ok(note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, NoteDto dto)
        {
            try
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.Title))
                {
                    _logger.LogWarning("Güncelleme için geçersiz veri alındı (ID: {Id}).", id);
                    return BadRequest("Geçersiz not verisi.");
                }

                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var note = await _context.Notes.FindAsync(id);

                if (note == null)
                {
                    _logger.LogWarning("Güncellenmek istenen not bulunamadı (ID: {Id}).", id);
                    return NotFound();
                }

                if (note.UserEmail != userEmail)
                {
                    _logger.LogWarning("Kullanıcı {Email}, başkasına ait notu güncellemeye çalıştı (ID: {Id})", userEmail, id);
                    return Forbid();
                }

                note.Title = dto.Title.Trim();
                note.Content = dto.Content?.Trim();
                note.UpdatedAt = DateTime.UtcNow;

                _context.Notes.Update(note);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Not güncellendi: {@Note}", note);
                return Ok(note);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Not güncellenirken beklenmeyen bir hata oluştu (ID: {Id}).", id);
                return StatusCode(500, "Bir hata oluştu.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var userRole = User.FindFirstValue(ClaimTypes.Role);
                var note = await _context.Notes.FindAsync(id);

                if (note == null)
                {
                    _logger.LogWarning("Silinmek istenen not bulunamadı (ID: {Id})", id);
                    return NotFound();
                }

                if (note.UserEmail != userEmail && !string.Equals(userRole, "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Kullanıcı {Email} (rol: {Role}), başkasına ait notu silmeye çalıştı (ID: {Id})", userEmail, userRole, id);
                    return Forbid();
                }

                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Not silindi (ID: {Id})", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Not silinirken hata oluştu (ID: {Id})", id);
                return StatusCode(500, "Bir hata oluştu.");
            }
        }

    }
}
