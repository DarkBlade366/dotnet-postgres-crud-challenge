using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiTest.Data;
using ApiTest.Models;

namespace ApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        public readonly ApiTestContext _context;

        public CoursesController (ApiTestContext context)
        {
            _context = context;
        }

        // Devuelve todos los cursos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        // Devuelve un estudiante específico por Id
        [HttpGet ("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            // Si no existe, devuelve 404 Not Found
            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // Crea un nuevo curso
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            // Agrega el estudiante al DbContext
            _context.Courses.Add(course);

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Devuelve 201 Created (creado correctamente) con la ruta del nuevo recurso
            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
        }

        //Actualizar curso existente
        [HttpPut ("{id}")]
        public async Task<ActionResult> PutCourse(int id, Course course)
        {
            // Validación: si el id de la ruta no coincide con el objeto, devuelve 400
            if (id != course.Id)
            {
                return BadRequest();
            }

            // Marca la entidad como modificada para EF Core
            _context.Entry(course).State = EntityState.Modified;

            try
            {
                //Guardar los cambios
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!_context.Courses.Any(c => c.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            // Devuelve 204 No Content indicando éxito sin cuerpo
            return NoContent();
        }

        // Elimina un curso por Id
        [HttpDelete ("{id}")]
        public async Task<ActionResult> DeleteCourse (int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            // Devuelve 204 No Content
            return NoContent();
        }
    }
}
