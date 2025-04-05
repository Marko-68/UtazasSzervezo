using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_Library.Services
{
    public class ReviewService
    {
        private readonly UtazasSzervezoDbContext _context;
        public ReviewService(UtazasSzervezoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetAllReviews()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<Review> GetReviewById(int id)
        {
            return await _context.Reviews.FindAsync(id);
        }

        public async Task<List<Review>> GetReviewsByAccommodationId(int accommodationId)
        {
            return await _context.Reviews
                .Where(r => r.accommodation_id == accommodationId)
                .ToListAsync();
        }

        public async Task<Review> CreateReview(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;   
        }

        public async Task<bool> DeleteReview(int id)
        {
            var reviews = await _context.Reviews.FindAsync(id);
            if (reviews == null) return false;

            _context.Reviews.Remove(reviews);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
