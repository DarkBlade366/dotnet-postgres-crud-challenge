using System.Threading;
using System.Threading.Tasks;
using ApiTest.Data;
using ApiTest.DTOs;
using ApiTest.Models;
using FastEndpoints;

namespace ApiTest.Endpoints.Students
{
    public class CreateStudentEndpoint : Endpoint<CreateStudentDTO, StudentDTO>
    {
        private readonly ApiTestContext _context;

        public CreateStudentEndpoint(ApiTestContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Post("/api/students");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateStudentDTO req, CancellationToken ct)
        {
            var student = new Student
            {
                Name = req.Name
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync(ct);

            var dto = new StudentDTO
            {
                Id = student.Id,
                Name = student.Name
            };

            await Send.OkAsync(dto, ct);
        }
    }
}
