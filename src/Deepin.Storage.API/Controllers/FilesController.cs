using Deepin.Storage.API.Application.Models;
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
        public async Task<IActionResult> UploadAsync([FromForm] IFormFileCollection files, CancellationToken cancellationToken = default)
        {
            if (files is null || files.Count == 0)
            {
                return BadRequest("Files are required");
            }
            var result = new List<FileModel>();
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    await using var stream = file.OpenReadStream();
                    var uploadResult = await _fileService.UploadAsync(stream, file.FileName, cancellationToken);
                    result.Add(uploadResult);
                }
            }
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
        [HttpGet("{id}/download-token")]
        public async Task<IActionResult> GetTemporaryDownloadTokenAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var file = await _fileService.GetByIdAsync(id, cancellationToken);
            if (file is null)
            {
                return NotFound();
            }
            var token = await _fileService.GetTemporaryDownloadTokenAsync(id, DateTime.UtcNow.AddHours(2), cancellationToken);
            if (token is null)
            {
                return NotFound();
            }
            return Ok(new { token });
        }
        [HttpGet("{id}/download/{token}")]
        [AllowAnonymous]
        public async Task<IActionResult> DownloadByTemporaryTokenAsync(Guid id, string token, CancellationToken cancellationToken = default)
        {
            var file = await _fileService.GetByTemporaryDownloadTokenAsync(id, token, cancellationToken);
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
