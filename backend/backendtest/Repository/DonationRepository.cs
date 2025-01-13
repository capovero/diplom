using backendtest.Data;
using backendtest.Interfaces;

namespace backendtest.Repository;

public class DonationRepository : IDonationRepository
{
    private readonly ApplicationContext _context;

    public DonationRepository(ApplicationContext context)
    {
        _context = context;
    }
}