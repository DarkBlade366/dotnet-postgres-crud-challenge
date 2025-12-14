using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTest.Data;
using ApiTest.DTOs;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Endpoints.Courses
{
    public class GetCourseEndpoint : EndpointWithoutRequest<CourseDTO>
    {
        private readonly ApiTestContext _context;

        public GetCourseEndpoint(ApiTestContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Get("/api/courses/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var id = Route<int>("id");

            var course = await _context.Courses
                .Where(c => c.Id == id)
                .Select(c => new CourseDTO
                {
                    Id = c.Id,
                    Year = c.Year
                })
                .FirstOrDefaultAsync(ct);

            if (course == null)
            {
                await Send.NotFoundAsync(ct);
                return;
            }

            await Send.OkAsync(course, ct);
        }
    }
}