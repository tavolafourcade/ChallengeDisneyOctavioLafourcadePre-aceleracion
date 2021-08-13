using ChallengeDisney.Entities;
using ChallengeDisneyOctavioLafourcadePre_Aceleracion.Context;
using ChallengeDisneyOctavioLafourcadePre_Aceleracion.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChallengeDisneyOctavioLafourcadePre_Aceleracion.Controllers
{ 
    [ApiController]
    [Route("characters")]
    public class CharacterController : ControllerBase
    {
        private readonly ChallengeContext _challengeContext;
        public CharacterController(ChallengeContext ctx)
        {
            _challengeContext = ctx;

        }

        [HttpGet]
        public List<Character> GetCharacters(int movieId, string name, int age, float weight)
        {
            // Levantamos del contexto la entidad de Characters y le indicamos que incluya la entidad relacionada Movies
            IQueryable<Character> list = _challengeContext.Characters.Include(x => x.Movies);

            if (movieId != 0) list = list.Where(x => x.Movies.FirstOrDefault(x => x.Id == movieId) != null);

            if (name != null) list = list.Where(x => x.Name == name);

            if (age != 0) list = list.Where(x => x.Age == age);

            if (weight != 0) list = list.Where(x => x.Weight == weight);

            return list.Select(x => new Character
            {
                Name = x.Name,
                Image = x.Image
            }).ToList();
        }

        [HttpGet("details")]
        public IActionResult GetCharacter(string name)
        {
            var character = _challengeContext.Characters
                .Include(x => x.Movies)
                .FirstOrDefault(x => x.Name == name);

            if (character == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "The character doesn't exist");
            }

            return StatusCode(StatusCodes.Status200OK, character);
        }


        [HttpPost("add")]
        public IActionResult Post(CharacterRequestModel character)
        {
            // Armado de la entidad
            var newCharacter = new Character
            {
                Name = character.Name,
                Image = character.Image,
                Age = character.Age,
                Weight = character.Weight,
                History = character.History
            };

            // Chequeando si se tiene que agregar una relación con una pelicula
            if (character.MovieId != 0)
            {
                // Verificando que tenga el mismo Id que nos enviaron
                var movie = _challengeContext.Movies.FirstOrDefault(x => x.Id == character.MovieId);

                // Si la pelicula existe
                if (movie != null)
                {
                    // Evitando que Movies sea una entidad vacia (Instanciando Movies)
                    if (newCharacter.Movies == null) newCharacter.Movies = new List<Movie>();
                    // Agregando Movies
                    newCharacter.Movies.Add(movie);
                }
            }

            // Agregamos el nuevo character
            _challengeContext.Characters.Add(newCharacter);

            _challengeContext.SaveChanges();

            // Devolvemos un ViewModel nuevo con los valores de Character
            return StatusCode(StatusCodes.Status201Created, new CharacterResponseModel
            {
                Id = newCharacter.Id,
                Name = newCharacter.Name,
                Image = newCharacter.Image,
                Age = newCharacter.Age,
                Weight = newCharacter.Weight,
                History = newCharacter.History,
                MovieId = character.MovieId
            });
        }


        [HttpPut("update")]
        public IActionResult Put(CharacterUpdateRequestModel character)
        {
            var newCharacter = _challengeContext.Characters.Include(x => x.Movies).FirstOrDefault(x => x.Id == character.Id);

            // Si no existe devuelve un error 404
            if (newCharacter == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "The character doesn't exist.");
            }

            // Mapeando los valores que queremos
            newCharacter.Id = character.Id;
            newCharacter.Name = character.Name;
            newCharacter.Image = character.Image;
            newCharacter.Age = character.Age;
            newCharacter.Weight = character.Weight;
            newCharacter.History = character.History;

            _challengeContext.Characters.Update(newCharacter);

            _challengeContext.SaveChanges();

            return StatusCode(StatusCodes.Status201Created, new CharacterUpdateResponseModel
            {
                Id = character.Id,
                Name = character.Name,
                Image = character.Image,
                Age = character.Age,
                Weight = character.Weight,
                History = character.History,
                MovieId = character.MovieId
            });
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            var deleteCharacter = _challengeContext.Characters.Find(id);

            if (deleteCharacter == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "The character doesn't exist.");
            }

            _challengeContext.Characters.Remove(deleteCharacter);

            _challengeContext.SaveChanges();

            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
