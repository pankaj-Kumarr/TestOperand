using Microsoft.AspNetCore.Mvc;
using TestOperand.Models;

namespace TestOperand.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpPost("GetResult")]
        public IActionResult GetResult(OperationRequest request)
        {
            try
            {
                var flag = ValidateRequest(request);
                if (flag)
                {
                    decimal result;
                    switch (request.Operand)
                    {
                        case "+":
                            result = request.Numbers.Sum();
                            break;
                        case "-":
                            result = request.Numbers.Aggregate((a, b) => a - b);
                            break;
                        case "*":
                            result = request.Numbers.Aggregate((a, b) => a * b);
                            break;
                        case "/":
                            if (request.Numbers.Skip(1).Any(n => n == 0))
                            {
                                return BadRequest("Division by zero is not allowed.");
                            }
                            result = request.Numbers.Aggregate((a, b) => a / b);
                            break;
                        default:
                            return BadRequest();

                    }
                    return Ok(new { result });
                }
            }
            catch (Exception ex)
            {
                //log exceptios
            }
            return BadRequest();
        }

        private bool ValidateRequest(OperationRequest request) {
            if (request == null || String.IsNullOrEmpty(request.Operand) || request.Numbers.Length == 0)
            {
               return false;
            }
            return true;
        }
    }
}
