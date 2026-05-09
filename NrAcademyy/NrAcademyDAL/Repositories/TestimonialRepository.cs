using NrAcademyCORE.Entities;
using NrAcademyCORE.IRepositories;
using NrAcademyDAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyDAL.Repositories;

public class TestimonialRepository : GenericRepository<Testimonial>, ITestimonialRepository
{

    public TestimonialRepository(AppDbContext context) : base(context)
    {
    }
}
