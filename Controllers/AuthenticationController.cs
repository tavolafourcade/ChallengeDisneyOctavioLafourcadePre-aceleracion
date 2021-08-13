using ChallengeDisneyOctavioLafourcadePre_Aceleracion.Entities;
using ChallengeDisneyOctavioLafourcadePre_Aceleracion.ViewModels.Auth.Register;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChallengeDisneyOctavioLafourcadePre_Aceleracion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        // Funcionalidad inyectable llamada UserManager
        private readonly UserManager<User> _userManager;

        public AuthenticationController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("register")]
        //Del cuerpo de la peticion trae RegisterRequestModel
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);

            //Validamos que usuario exista
            if (userExists != null)
            {
                return BadRequest();
            }

            //Creamos el usuario
            var user = new User
            {
                Email = model.Email,
                UserName = model.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            
            //Si no se puede crear se dice porque
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        Status = "Error",
                        Message = $"User Creation Failed! Errors: {string.Join(',', result.Errors.Select(x => x.Description))}"
                    });
            }

            //Si se pudo crear decimos Ok
            return Ok(new
            {
                Status = "Success",
                Message = "Usser created Successfully"
            });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult>() Login()
        {
            return Ok();
        }
    }
}
