using System.Diagnostics;
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
                    Arguments = "run --rm -i python:3.11 python",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            _logger.LogInformation("Running Docker container with Python code...");

            process.Start();

            // Write code into Docker container's stdin
            await process.StandardInput.WriteAsync(request.Code);
            process.StandardInput.Close();

            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();

            process.WaitForExit();

            _logger.LogInformation("Docker output: {Output}", output);
            _logger.LogInformation("Docker error: {Error}", error);

            return Ok(new { output, error });
        }
    }

    public class PythonCodeRequest
    {
        public required string Code { get; set; }
    }
}
