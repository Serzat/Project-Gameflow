using MyFilesGameflow.Data;

namespace MyFilesGameflow.Service;

public class BeslissingenService {
    private readonly ApplicationDbContext _context;

    public BeslissingenService(ApplicationDbContext context) {
        _context = context;
    }
    
    
}