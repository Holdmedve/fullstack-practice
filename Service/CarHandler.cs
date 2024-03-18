namespace Car;

using Microsoft.AspNetCore.Http.HttpResults;

public static class CarHandler
{
    public static async Task<Created<Car>> CreateCar(
        Car car, 
        CarDb db,
        IDateTime dateTime,
        ILogger logger,
        DbConfig dbConfig        
    )
    {
        logger.LogDebug($"the current datetime is: {dateTime.Now}");
        logger.LogInformation($"the current datetime is: {dateTime.Now}");
        logger.LogWarning($"the current datetime is: {dateTime.Now}");
        logger.LogError($"the current datetime is: {dateTime.Now}");
        logger.LogInformation($"The database name is: {dbConfig.Name}");

        db.Cars.Add(car);
        await db.SaveChangesAsync();
        return TypedResults.Created($"/cars/{car.Id}", car);
    }

    public static async Task<Results<Ok<Car>, NotFound>> GetCar(int id, CarDb db)
    {
        return await db.Cars.FindAsync(id)
            is Car car
                ? TypedResults.Ok(car)
                : TypedResults.NotFound();
    }

    public static async Task<Results<NoContent, NotFound>> DeleteCar(int id, CarDb db)
    {
        var car = await db.Cars.FindAsync(id);
        if (car == null)
        {
            return TypedResults.NotFound();
        }

        db.Remove(car);
        return TypedResults.NoContent();
    }

    public static async Task<Results<Created<Car>, NotFound>> UpdateCar(Car carUpdate, CarDb db)
    {
        var car = await db.Cars.FindAsync(carUpdate.Id);
        if (car == null)
        {
            return TypedResults.NotFound();
        }

        car.Id = carUpdate.Id;
        car.IsElectric = carUpdate.IsElectric;
        car.Brand = carUpdate.Brand;

        db.Cars.Update(car);
        return TypedResults.Created($"/cars/{car.Id}", car);

    }
}