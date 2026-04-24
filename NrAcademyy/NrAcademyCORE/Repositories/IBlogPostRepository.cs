using NrAcademyCORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyCORE.Repositories
{

    public interface IBlogPostRepository : IGenericRepository<BlogPost>
    {
        object GetAll(Func<object, object> value);
    }
}
