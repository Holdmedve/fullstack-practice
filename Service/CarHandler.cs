namespace Car;

using Microsoft.AspNetCore.Http.HttpResults;

public static class CarHandler
{
    public static async Task<IResult> CreateCar(Car car, CarDb db)
    {
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
}