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
    public class DeleteCourseEndpoint : EndpointWithoutRequest
    {
        private readonly ApiTestContext _context;

        public DeleteCourseEndpoint(ApiTestContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Delete("/api/courses/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var id = Route<int>("id");

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                await Send.NotFoundAsync(ct);
                return;
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync(ct);

            await Send.NoContentAsync(ct);
        }
    }
}