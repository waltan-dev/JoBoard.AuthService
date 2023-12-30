// using Microsoft.AspNetCore.Mvc;
//
// namespace JoBoard.AuthService.Controllers;
//
// [Route("api/v1/account")]
// public class AccountController : Controller
// {
//     [HttpPost]
//     [Route("login")]
//     public async Task<IActionResult> Login([FromBody]LoginParams model)
//     {
//         return Ok();
//     }
//     
//     [HttpPost]
//     [Route("register")]
//     public async Task<IActionResult> Register([FromBody]RegistrationViewModel user)
//     {
//         return Ok();
//     }
//     
//     [HttpPost]
//     [Route("refresh-token")]
//     public async Task<IActionResult> RefreshToken([FromBody]RefreshTokenParams pars)
//     {
//         return Ok();
//     }
//     
//     [HttpGet]
//     [Route("logout")]
//     public async Task<IActionResult> Logout()
//     {
//         return Ok();
//     }
// }