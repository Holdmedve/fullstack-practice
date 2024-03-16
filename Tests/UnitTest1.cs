namespace Tests;
using Microsoft.EntityFrameworkCore;
using Car;
using System.Formats.Asn1;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

public class TestCarHandler
{
    private readonly ITestOutputHelper _testOutputHelper;
    public TestCarHandler(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async void TestCreateCar_GivenCar_DbSavesIt()
    {
        Car c = new Car{Brand = "Audi", IsElectric = false};
        DbContextOptionsBuilder<CarDb> dbOptionsBuilder = new DbContextOptionsBuilder<CarDb>();
        dbOptionsBuilder.UseInMemoryDatabase("TestCarDatabase");
        CarDb db = new CarDb(dbOptionsBuilder.Options);
        
        await CarHandler.CreateCar(car: c, db: db);

        Car result = db.Cars.First();
        Assert.Equal(expected: c.Brand, actual: result.Brand);
        Assert.Equal(expected: c.IsElectric, actual: result.IsElectric);
    }

    [Fact]
    public async void TestGetCar_CarWithIdExists_ReturnCar()
    {
        DbContextOptionsBuilder<CarDb> dbOptionsBuilder = new DbContextOptionsBuilder<CarDb>();
        dbOptionsBuilder.UseInMemoryDatabase("TestCarDatabase");
        CarDb db = new CarDb(dbOptionsBuilder.Options);
        int expectedId = 42;
        db.Cars.Add(new Car{Id = expectedId, Brand = "bmw", IsElectric = false});

        var result = await CarHandler.GetCar(id: expectedId, db: db);

        Assert.IsType<Results<Ok<Car>, NotFound>>(result);
        Car carResult = ((Ok<Car>) result.Result).Value;
        Assert.Equal(expected: expectedId, actual: carResult.Id);
    }

    [Fact]
    public async void TestGetCar_CarWithIdDoesNotExist_ReturnNotFound()
    {
        DbContextOptionsBuilder<CarDb> dbOptionsBuilder = new DbContextOptionsBuilder<CarDb>();
        dbOptionsBuilder.UseInMemoryDatabase("TestCarDatabase");
        CarDb db = new CarDb(dbOptionsBuilder.Options);

        var result = await CarHandler.GetCar(id: 42, db: db);

        Assert.IsType<Results<Ok<Car>, NotFound>>(result);
        var notFoundResult = (NotFound) result.Result;
        Assert.NotNull(notFoundResult);
    }
}