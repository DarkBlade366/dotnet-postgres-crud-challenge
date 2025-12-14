using System.Threading;
using System.Threading.Tasks;
using ApiTest.Data;
using ApiTest.DTOs;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Endpoints.Students
{
    public class UpdateStudentEndpoint : Endpoint<UpdateStudentDTO>
    {
        private readonly ApiTestContext _context;

        public UpdateStudentEndpoint(ApiTestContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Put("/api/students/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(UpdateStudentDTO req, CancellationToken ct)
        {
            var id = Route<int>("id");

            if (id != req.Id)
            {
                await Send.NotFoundAsync(ct);
                return;
            }

            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id, ct);

            if (student == null)
            {
                await Send.NotFoundAsync(ct);
                return;
            }

            student.Name = req.Name;
            await _context.SaveChangesAsync(ct);

            await Send.NoContentAsync(ct);
        }
    }
}
