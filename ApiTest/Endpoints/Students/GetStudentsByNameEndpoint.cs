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
    public class GetStudentsByNameEndpoint : Endpoint<string, List<StudentDTO>>
    {
        private readonly ApiTestContext _context;

        public GetStudentsByNameEndpoint(ApiTestContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Get("/api/students/by-name/{name}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(string name, CancellationToken ct)
        {
            var students = await _context.Students
                .Where(s => s.Name == name)
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
