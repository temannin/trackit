using System.Diagnostics;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PythonController : ControllerBase
    {
        private readonly ILogger<PythonController> _logger;

        public PythonController(ILogger<PythonController> logger)
        {
            _logger = logger;
        }

        [HttpPost("run")]
        public async Task<IActionResult> RunPython([FromBody] PythonCodeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Code))
                return BadRequest("No code provided.");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "docker",
                    Arguments = "run -i --init --cap-add=SYS_ADMIN --rm ghcr.io/puppeteer/puppeteer:latest node -",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            // Send the raw JavaScript script to stdin
            await process.StandardInput.WriteAsync(request.Code);
            process.StandardInput.Close(); // Important: signal EOF to node

            // Capture output and error
            string stdout = await process.StandardOutput.ReadToEndAsync();
            string stderr = await process.StandardError.ReadToEndAsync();
            process.WaitForExit();

            // Log or return the result
            _logger.LogInformation("STDOUT:\n{Stdout}", stdout);
            _logger.LogError("STDERR:\n{Stderr}", stderr);

            return Ok(new { output = stdout, error = stderr });
        }
    }

    public class PythonCodeRequest
    {
        public required string Code { get; set; }
    }
}
