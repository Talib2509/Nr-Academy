using NrAcademyCORE.Entities;
using NrAcademyDAL.Context;
using NrAcademyDAL.Repositories;

public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
{
    public EnrollmentRepository(AppDbContext context) : base(context) { }
}
