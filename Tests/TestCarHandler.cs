namespace Tests;
using Microsoft.EntityFrameworkCore;
using Car;
using Xunit.Abstractions;
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
        CarDb db = CreateInMemoryDb();
        Car c = new Car{Brand = "Audi", IsElectric = false};
        
        await CarHandler.CreateCar(car: c, db: db);

        Car result = db.Cars.First();
        Assert.Equal(expected: c.Brand, actual: result.Brand);
        Assert.Equal(expected: c.IsElectric, actual: result.IsElectric);
    }

    [Fact]
    public async void TestGetCar_CarWithIdExists_ReturnCar()
    {
        CarDb db = CreateInMemoryDb();
        int expectedId = 42;
        db.Cars.Add(new Car{Id = expectedId, Brand = "bmw", IsElectric = false});

        var result = await CarHandler.GetCar(id: expectedId, db: db);

        Assert.IsType<Results<Ok<Car>, NotFound>>(result);
        var carResult = ((Ok<Car>) result.Result).Value;
        Assert.Equal(expected: expectedId, actual: carResult?.Id);
    }

    [Fact]
    public async void TestGetCar_CarWithIdDoesNotExist_ReturnNotFound()
    {
        CarDb db = CreateInMemoryDb();

        var result = await CarHandler.GetCar(id: 42, db: db);

        Assert.IsType<Results<Ok<Car>, NotFound>>(result);
        var notFoundResult = (NotFound) result.Result;
        Assert.NotNull(notFoundResult);
    }

    [Fact]
    public async void TestDeleteCar_CarWithIdDoesNotExist_ReturnsNotFound()
    {
        CarDb db = CreateInMemoryDb();

        var result = await CarHandler.DeleteCar(id: 42, db: db);

        Assert.IsType<Results<NoContent, NotFound>>(result);
        var notFoundResult = (NotFound) result.Result;
        Assert.NotNull(notFoundResult);
    }

    [Fact]
    public async void TestDeleteCar_CarWithIdExists_CarIsDeletedFromDb()
    {
        CarDb db = CreateInMemoryDb();
        int id = 42;
        db.Cars.Add(new Car{Id = id, Brand = "bmw", IsElectric = false});

        await CarHandler.DeleteCar(id: id, db: db);

        var carResult = db.Cars.Find(id);
        Assert.Null(carResult);
    }

    [Fact]
    public async void TestUpdateCar_CarWithIdExists_BrandIsUpdated()
    {
        CarDb db = CreateInMemoryDb();
        int id = 42;
        db.Cars.Add(new Car{Id = id, Brand = "bmw", IsElectric = false});
        string expectedBrand = "honda";
        var updatedCar = new Car{Id = id, Brand = expectedBrand, IsElectric = false};

        var result = await CarHandler.UpdateCar(updatedCar, db);

        Assert.IsType<Results<Created<Car>, NotFound>>(result);
        var carResult = ((Created<Car>) result.Result).Value;
        Assert.Equal(expected: expectedBrand, actual: carResult?.Brand);
    }

    [Fact]
    public async void TestUpdateCar_CarWithIdDoesNotExist_ReturnsNotFound()
    {
        CarDb db = CreateInMemoryDb();

        var result = await CarHandler.UpdateCar(new Car{Id = 42, Brand = "bmw", IsElectric = false}, db);

        Assert.IsType<Results<Created<Car>, NotFound>>(result);
        var carResult = (NotFound) result.Result;
        Assert.NotNull(carResult);
    }

    private static CarDb CreateInMemoryDb()
    {
        DbContextOptionsBuilder<CarDb> dbOptionsBuilder = new DbContextOptionsBuilder<CarDb>();
        dbOptionsBuilder.UseInMemoryDatabase("TestCarDatabase");
        return new CarDb(dbOptionsBuilder.Options);
    }
}