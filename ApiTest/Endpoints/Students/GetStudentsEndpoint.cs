using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApiTest.Data;
using ApiTest.DTOs;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Endpoints.Students
{
    public class GetStudentsEndpoint : EndpointWithoutRequest<List<StudentDTO>>
    {
        private readonly ApiTestContext _context;

        public GetStudentsEndpoint(ApiTestContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Get("/api/students");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var students = await _context.Students
                .Select(s => new StudentDTO
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToListAsync(ct);

            await Send.OkAsync(students, ct);
        }
    }
}
