using System.Security.Claims;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoQ3Backend.DTOs;
using ProyectoQ3Backend.Models;
using ProyectoQ3Backend.Services;

namespace ProyectoQ3Backend.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NoteController : ControllerBase
{
    private readonly NoteServices _noteServices;
    
    public NoteController(NoteServices noteServices)
    {
        _noteServices = noteServices;
    }
    
    // POST /api/Note
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] NoteDto dto)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            
            var note = await _noteServices.Create(dto, userId);
            return Ok(note);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET /api/Note
    [HttpGet]
    public async Task<IActionResult> GetMyNotes()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            
            var notes = await _noteServices.GetByUser(userId);
            
            return Ok(notes);
            
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}