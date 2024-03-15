namespace Tests;
using Microsoft.EntityFrameworkCore;
using Car;

public class UnitTest1
{
    [Fact]
    public async void Test1()
    {
        Car c = new Car{Brand = "Audi", IsElectric = false};
        DbContextOptionsBuilder<CarDb> dbOptionsBuilder = new DbContextOptionsBuilder<CarDb>();
        dbOptionsBuilder.UseInMemoryDatabase("TestCarDatabase");
        CarDb db = new CarDb(dbOptionsBuilder.Options);
        
        await CarHandler.CreateCar(car: c, db: db);
    }
}