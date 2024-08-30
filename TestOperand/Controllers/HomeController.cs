using Microsoft.AspNetCore.Mvc;
using TestOperand.Models;

namespace TestOperand.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// Method to get result after performing the given operation.
        /// Given method accepts any one operand from '+,-,*,/' and perform action on the given array.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetResult")]
        public IActionResult GetResult(OperationRequest request)
        {
            try
            {
                var flag = ValidateRequest(request);
                if (flag)
                {
                    decimal result;
                    var arr = request.Numbers;
                    switch (request.Operand)
                    {
                        case "+":
                            result = arr.Sum();
                            break;
                        case "-":
                            result = arr.Aggregate((a, b) => a - b);
                            break;
                        case "*":
                            result = arr.Aggregate((a, b) => a * b);
                            break;
                        case "/":
                            if (arr.Skip(1).Contains(0))
                            {
                                return BadRequest("Division by zero is not allowed.");
                            }
                            result = request.Numbers.Aggregate((a, b) => a / b);
                            break;
                        default:
                            return BadRequest("Some Error Occured.");

                    }
                    return Ok(" result : " + result);
                }
            }
            catch (Exception ex)
            {
                //log exceptios
            }
            return BadRequest("Some Error Occured.");
        }

        /// <summary>
        /// Method to validate the given request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool ValidateRequest(OperationRequest request) {
            if (request == null || String.IsNullOrEmpty(request.Operand) || request.Numbers == null || request.Numbers.Length == 0)
            {
               return false;
            }
            return true;
        }
    }
}
