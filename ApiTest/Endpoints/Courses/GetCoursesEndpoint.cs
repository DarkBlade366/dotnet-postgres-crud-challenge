using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApiTest.Data;
using ApiTest.DTOs;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Endpoints.Courses
{
    public class GetCoursesEndpoint : EndpointWithoutRequest<List<CourseDTO>>
    {
        private readonly ApiTestContext _context;

        public GetCoursesEndpoint(ApiTestContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Get("/api/courses");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var courses = await _context.Courses
                .Select(c => new CourseDTO
                {
                    Id = c.Id,
                    Year = c.Year
                })
                .ToListAsync(ct);

            await Send.OkAsync(courses, ct);
        }
    }
}
