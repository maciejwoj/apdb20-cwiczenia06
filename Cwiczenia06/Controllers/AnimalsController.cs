using Cwiczenia06.Models;
using Cwiczenia06.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Cwiczenia06.Controllers;

[ApiController]
// [Route("api/animals")]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public AnimalsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpGet]
    public IActionResult GetAnimals()
    {
        // Otwieramy połączenie
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        
        // Defincja command
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM Animal";
        
        // Wykonanie zapytania
        var reader = command.ExecuteReader();

        List<Animal> animals = new List<Animal>();

        int idAnimalOrdinal = reader.GetOrdinal("IdAnimal");
        int nameOrdinal = reader.GetOrdinal("Name");
        int DescriptionOrdinal = reader.GetOrdinal("Description");
        int CategoryOrdinal = reader.GetOrdinal("Category");
        int AreaOrdinal = reader.GetOrdinal("Area");
        
        while (reader.Read())
        {
            animals.Add(new Animal()
            {
                IdAnimal = reader.GetInt32(idAnimalOrdinal),
                Name = reader.GetString(nameOrdinal),
                Description = reader.GetString(DescriptionOrdinal),
                Category = reader.GetString(CategoryOrdinal),
                Area = reader.GetString(AreaOrdinal)
            });
        }

        // var animals = _repository.GetAnimals();

        return Ok(animals);
    }


    [HttpPost]
    public IActionResult AddAnimal(AddAnimal addAnimal)
    {
        // Otwieramy połączenie
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        
        // Defincja command
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "INSERT INTO Animal VALUES(@animalName,@animalDescription,@animalCategory,@animalArea)";
        command.Parameters.AddWithValue("@animalName", addAnimal.Name);
        command.Parameters.AddWithValue("@animalDescription", addAnimal.Description);
        command.Parameters.AddWithValue("@animalCategory", addAnimal.Category);
        command.Parameters.AddWithValue("@animalArea", addAnimal.Area);
        
        // Wykonanie zapytania
        command.ExecuteNonQuery();
        
        // _repository.AddAnimal(addAnimal);
        
        return Created();
    }

    [HttpPut("{idAnimal}")]
    public IActionResult UpdateAnimal(int idAnimal, UpdateAnimal updatedAnimal)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));

        string queryString = "UPDATE Animal SET Name = @Name, Description = @Description, Category = @Category, Area = @Area WHERE IdAnimal = @IdAnimal";
        using SqlCommand command = new SqlCommand(queryString, connection);
        command.Parameters.AddWithValue("@IdAnimal", idAnimal);
        command.Parameters.AddWithValue("@Name", updatedAnimal.Name);
        command.Parameters.AddWithValue("@Description", updatedAnimal.Description);
        command.Parameters.AddWithValue("@Category", updatedAnimal.Category);
        command.Parameters.AddWithValue("@Area", updatedAnimal.Area);
        
        return NoContent();
    }
    
    [HttpDelete("{idAnimal}")]
    public IActionResult DeleteAnimal(int idAnimal, UpdateAnimal deleteAnimal)
    {
        
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));

        string queryString = "DELETE FROM Animal WHERE IdAnimal = @IdAnimal";
        using SqlCommand command = new SqlCommand(queryString, connection);
        command.Parameters.AddWithValue("@IdAnimal", idAnimal);
        
        return NoContent();
    }
}