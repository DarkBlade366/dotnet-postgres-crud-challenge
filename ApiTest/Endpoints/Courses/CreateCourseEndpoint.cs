using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTest.Data;
using ApiTest.DTOs;
using ApiTest.Models;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Endpoints.Courses
{
    public class CreateCourseEndpoint : Endpoint<CreateCourseDTO, CourseDTO>
    {
        private readonly ApiTestContext _context;

        public CreateCourseEndpoint(ApiTestContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Post("/api/courses");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateCourseDTO req, CancellationToken ct)
        {
            var course = new Course
            {
                Year = req.Year
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync(ct);

            var dto = new CourseDTO
            {
                Id = course.Id,
                Year = course.Year
            };

            await Send.OkAsync(dto, ct);
        }
    }
}