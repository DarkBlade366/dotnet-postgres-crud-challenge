using System.Threading;
using System.Threading.Tasks;
using ApiTest.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Endpoints.Students
{
    public class DeleteStudentEndpoint : EndpointWithoutRequest
    {
        private readonly ApiTestContext _context;

        public DeleteStudentEndpoint(ApiTestContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Delete("/api/students/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var id = Route<int>("id");

            var student = await _context.Students.FindAsync(new object?[] { id }, ct);
            if (student == null)
            {
                await Send.NotFoundAsync(ct);
                return;
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync(ct);

            await Send.NoContentAsync(ct);
        }
    }
}
