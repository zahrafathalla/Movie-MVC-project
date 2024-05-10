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
    public class GenreRepository : GenericRepository<Genre>  , IGenreRepository
    {
        private readonly ApplicationDBContext _context;

        public GenreRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Genre>> GetAll()
        {

            return await _context.Set<Genre>().OrderBy(x => x.Name).ToListAsync(); ;

        }
    }
}
