using Dhbw_positioning_System_Backend;
using Microsoft.EntityFrameworkCore;

namespace backend_test.TestData;

public class AnalyseData
{
    private DhbwPositioningSystemDBContext _context;
    [SetUp]
    public void SetUp()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DhbwPositioningSystemDBContext>()
            .UseSqlite("Data Source=..\\..\\..\\..\\Dhbw positioning System Backend\\DhbwPositioningSystemDB.db;Mode=ReadOnly")//Standart Path is in dhbw-positioning-system\backend\backend-test\bin\Debug\net6.0 which is fucked, Idk how to fix so thats why weird navigation.
            .Options;
        _context = new DhbwPositioningSystemDBContext(optionsBuilder);
        _context.Database.OpenConnection();
    } 
    
    
}