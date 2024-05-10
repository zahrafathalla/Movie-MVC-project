using BLL.Interfaces;
using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class MovieRepository : GenericRepository<Movie> , IMovieRepository
    {
        private readonly ApplicationDBContext _context;

        public MovieRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Movie>> GetAll()
        {
            return await _context.Set<Movie>().OrderByDescending(x=>x.Rate).ToListAsync();

        }
    }
}
