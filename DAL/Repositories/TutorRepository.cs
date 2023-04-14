using AutoMapper;
using DAL.Core;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class TutorRepository : Repository<Tutor>, ITutorRepository
    {
        private readonly IMapper mapper;

        public TutorRepository(DataContext dataContext, IMapper mapper) : base(dataContext)
        {
            this.mapper = mapper;
        }

        public async Task<Tutor?> GetByLoginAsync(string login)
        {
            return await Set.FirstOrDefaultAsync(e => e.Login == login);
        }

        public async Task AddAsync(Tutor tutor)
        {
            await Set.AddAsync(tutor);
        }

        public Tutor? GetByLogin(string tutorLogin)
        {
            return Set.FirstOrDefault(e => e.Login == tutorLogin);
        }
    }
}
