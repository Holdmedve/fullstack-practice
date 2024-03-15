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
}