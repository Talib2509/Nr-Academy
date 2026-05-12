using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using NrAcademyDAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyDAL.Repositories
{
    public class UserAnswerRepository : GenericRepository<UserAnswer>, IUserAnswerRepository
    {
        public UserAnswerRepository(AppDbContext context) : base(context)
        {
        }
    }
}