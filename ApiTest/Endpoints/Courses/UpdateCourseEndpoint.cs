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
    public class UpdateCourseEndpoint : Endpoint<UpdateCourseDTO>
    {
        private readonly ApiTestContext _context;

        public UpdateCourseEndpoint(ApiTestContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Put("/api/courses/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(UpdateCourseDTO req, CancellationToken ct)
        {
            var id = Route<int>("id");

            if (id != req.Id)
            {
                await Send.NotFoundAsync(ct);
                return;
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == id, ct);

            if (course == null)
            {
                await Send.NotFoundAsync(ct);
                return;
            }

            course.Year = req.Year;
            await _context.SaveChangesAsync(ct);

            await Send.NoContentAsync(ct);
        }
    }
}