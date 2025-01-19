using Deepin.Storage.API.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Deepin.Storage.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class FilesController(IFileService fileService) : ControllerBase
    {
        private readonly IFileService _fileService = fileService;

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            if (file is null)
            {
                return BadRequest("File is required");
            }
            await using var stream = file.OpenReadStream();
            var result = await _fileService.UploadAsync(stream, file.FileName, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _fileService.GetByIdAsync(id, cancellationToken);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> GetStreamAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var file = await _fileService.GetByIdAsync(id, cancellationToken);
            if (file is null)
            {
                return NotFound();
            }
            var stream = await _fileService.GetStreamAsync(id, cancellationToken);
            if (stream is null)
            {
                return NotFound();
            }
            return File(stream, file.MimeType, file.Name);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _fileService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
