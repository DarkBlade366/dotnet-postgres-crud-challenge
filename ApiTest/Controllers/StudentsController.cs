using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiTest.Data;
using ApiTest.Models;

namespace ApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApiTestContext _context;

        public StudentsController (ApiTestContext context)
        {
            _context = context;
        }

        // Devuelve todos los estudiantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            // Devuelve la lista de estudiantes desde la base de datos
            return await _context.Students.ToListAsync();
        }

        // Devuelve un estudiante específico por Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            // Si no existe, devuelve 404 Not Found
            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // Devuelve una lista de estudiantes con el mismo nombre
        [HttpGet ("by-name/{name}")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents_name (string name)
        {            
            var students = await _context.Students.Where(s=>s.Name == name).ToListAsync();
            return students;
        }

        // Crea un nuevo estudiante
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            // Agrega el estudiante al DbContext
            _context.Students.Add(student);
            
            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Devuelve 201 Created (creado correctamente) con la ruta del nuevo recurso
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        // Actualiza un estudiante existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            // Validación: si el id de la ruta no coincide con el objeto, devuelve 400
            if (id != student.Id)
            {
                return BadRequest();
            }

            // Marca la entidad como modificada para EF Core
            _context.Entry(student).State = EntityState.Modified;

            try
            {
                // Guarda los cambios
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Si el estudiante no existe, devuelve 404
                if (!_context.Students.Any(e => e.Id == id))
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

        // Elimina un estudiante por Id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            // Devuelve 204 No Content
            return NoContent();
        }
    }
}
