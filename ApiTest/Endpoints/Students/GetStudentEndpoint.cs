using System.Threading;
using System.Threading.Tasks;
using ApiTest.Data;
using ApiTest.DTOs;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Endpoints.Students
{
    public class GetStudentEndpoint : EndpointWithoutRequest<StudentDTO>
    {
        private readonly ApiTestContext _context;

        public GetStudentEndpoint(ApiTestContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Get("/api/students/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var id = Route<int>("id");

            var student = await _context.Students
                .Where(s => s.Id == id)
                .Select(s => new StudentDTO
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .FirstOrDefaultAsync(ct);

            if (student == null)
            {
                await Send.NotFoundAsync(ct);
                return;
            }

            await Send.OkAsync(student, ct);
        }
    }
}
