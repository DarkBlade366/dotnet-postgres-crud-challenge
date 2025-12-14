using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiTest.Data;
using ApiTest.Models;
using ApiTest.DTOs;

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
        public async Task<ActionResult<IEnumerable<CourseDTO>>> GetCourses()
        {
            var courses = await _context.Courses
                .Select(c => new CourseDTO
                {
                    Id = c.Id,
                    Year = c.Year
                })
                .ToListAsync();

            return courses;
        }

        // Devuelve un estudiante específico por Id
        [HttpGet ("{id}")]
        public async Task<ActionResult<CourseDTO>> GetCourse(int id)
        {
            var course = await _context.Courses
                .Where(c => c.Id == id)
                .Select(c => new CourseDTO
                {
                    Id = c.Id,
                    Year = c.Year
                })
                .FirstOrDefaultAsync();

            // Si no existe, devuelve 404 Not Found
            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // Crea un nuevo curso
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(CreateCourseDTO courseDTO)
        {
            var course = new Course
            {
                Year = courseDTO.Year
            };

            // Agrega el estudiante al DbContext
            _context.Courses.Add(course);

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            var dto = new CourseDTO
            {
                Id = course.Id,
                Year = course.Year
            };

            // Devuelve 201 Created (creado correctamente) con la ruta del nuevo recurso
            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, dto);
        }

        //Actualizar curso existente
        [HttpPut ("{id}")]
        public async Task<ActionResult> PutCourse(int id, UpdateCourseDTO courseDTO)
        {
            // Validación: si el id de la ruta no coincide con el objeto, devuelve 400
            if (id != courseDTO.Id)
            {
                return BadRequest();
            }

            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            //Mapear los campos del DTO a la entidad
            course.Year = courseDTO.Year;

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
