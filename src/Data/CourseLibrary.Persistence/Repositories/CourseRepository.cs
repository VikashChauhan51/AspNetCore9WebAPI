﻿using CourseLibrary.Domain.Abstraction.Repositories;
using CourseLibrary.Domain.Entities;
using CourseLibrary.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CourseLibrary.Persistence.Repositories;

public class CourseRepository : ICourseRepository
{
    protected readonly CourseLibraryContext _context;
    public CourseRepository(CourseLibraryContext context) 
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void AddCourse(Guid authorId, Course course)
    {
        if (authorId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(authorId));
        }

        if (course == null)
        {
            throw new ArgumentNullException(nameof(course));
        }

        // always set the AuthorId to the passed-in authorId
        course.AuthorId = authorId;
        _context.Courses.Add(course);
    }

    public void DeleteCourse(Course course)
    {
        _context.Courses.Remove(course);
    }

    public async Task<Course?> GetCourseAsync( Guid courseId, CancellationToken cancellationToken = default)
    {
       
        if (courseId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(courseId));
        }

        return await _context.Courses
          .Where(c => c.Id == courseId).FirstOrDefaultAsync();

    }

    public async Task<Course?> GetCourseAsync(Guid authorId, Guid courseId, CancellationToken cancellationToken = default)
    {
        if (authorId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(authorId));
        }
        if (courseId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(courseId));
        }

        return await _context.Courses
          .Where(c => c.Id == courseId && c.AuthorId == authorId).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Course>> GetCoursesAsync(Guid authorId, CancellationToken cancellationToken = default)
    {
        if (authorId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(authorId));
        }

        return await _context.Courses
                    .Where(c => c.AuthorId == authorId)
                    .OrderBy(c => c.Title).ToListAsync();
    }
    public async Task<IEnumerable<Course>> GetCoursesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Courses
                    .OrderBy(c => c.Title).ToListAsync();
    }

    public void UpdateCourse(Course course)
    {
        _context.Courses.Update(course);
    }
}
