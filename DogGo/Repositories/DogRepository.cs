using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class DogRepository : IDogRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public DogRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Dog> GetAllDogs()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT d.Id, 
                               d.Name, 
                               d.OwnerId, 
                               d.Breed, 
                               d.Notes, 
                               d.ImageUrl, 
                               o.Name AS 'Owner Name'
                            FROM Dog d
                            Join Owner o On d.OwnerId = o.Id
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();
                    while (reader.Read())
                    {
                        Owner owner = new Owner();
                        owner.Name = reader.GetString(reader.GetOrdinal("Owner Name"));

                        string notes = null;
                        string image = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                        {
                            notes = reader.GetString(reader.GetOrdinal("Notes"));
                        };
                        if (!reader.IsDBNull(reader.GetOrdinal("ImageUrl")))
                        {
                            image = reader.GetString(reader.GetOrdinal("ImageUrl"));
                        };
                        Dog aDog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Notes = notes,
                            ImageUrl = image,
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Owner = owner
                        };

                        dogs.Add(aDog);
                    }

                    reader.Close();

                    return dogs;
                }
            }
        }

        public Dog GetDogById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    
                    cmd.CommandText = @"
                        SELECT d.Id, 
                               d.Name, 
                               d.OwnerId, 
                               d.Breed, 
                               d.Notes, 
                               d.ImageUrl, 
                               o.Name AS 'Owner Name'
                            FROM Dog d
                            Join Owner o On d.OwnerId = o.Id                        
                           Where d.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Owner owner = new Owner();
                        owner.Name = reader.GetString(reader.GetOrdinal("Owner Name"));

                        string notes = null;
                        string image = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                        {
                            notes = reader.GetString(reader.GetOrdinal("Notes"));
                        };
                        if (!reader.IsDBNull(reader.GetOrdinal("ImageUrl")))
                        {
                            image = reader.GetString(reader.GetOrdinal("ImageUrl"));
                        };
                        Dog aDog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Notes = notes,
                            ImageUrl = image,
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Owner = owner
                        };

                        reader.Close();
                        return aDog;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }


            }
        }
        public List<Dog> GetDogByBreed(string breed)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, Name, OwnerId, Breed, Notes, ImageUrl FROM Dogr
                        WHERE Breed = @breed";

                    cmd.Parameters.AddWithValue("@breed", breed);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();
                    while (reader.Read())
                    {
                        string notes = null;
                        string image = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                        {
                            notes = reader.GetString(reader.GetOrdinal("Notes"));
                        };
                        if (!reader.IsDBNull(reader.GetOrdinal("ImageUrl")))
                        {
                            image = reader.GetString(reader.GetOrdinal("ImageUrl"));
                        };
                        Dog aDog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Notes = notes,
                            ImageUrl = image,
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId"))
                        };

                        dogs.Add(aDog);
                    }

                    reader.Close();
                    return dogs;
                }
            }
        }

        public void AddDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Dog ([Name], OwnerId, Breed, Notes, ImageUrl)
                    OUTPUT INSERTED.ID
                    VALUES (@name, @OwnerId, @Breed, @Notes, @ImageUrl);
                ";

                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@OwnerId", dog.OwnerId);
                    cmd.Parameters.AddWithValue("@Breed", dog.Breed);
                    cmd.Parameters.AddWithValue("@Notes", dog.Notes);
                    cmd.Parameters.AddWithValue("@ImageUrl", dog.ImageUrl);

                    int id = (int)cmd.ExecuteScalar();

                    dog.Id = id;
                }
            }
        }

        public void UpdateDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Dog
                            SET 
                                [Name] = @name, 
                                OwnerId = @OwnerId, 
                                Breed = @Breed, 
                                Notes = @Notes, 
                                ImageUrl = @ImageUrl
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@OwnerId", dog.OwnerId);
                    cmd.Parameters.AddWithValue("@Breed", dog.Breed);
                    cmd.Parameters.AddWithValue("@Notes", dog.Notes);
                    cmd.Parameters.AddWithValue("@ImageUrl", dog.ImageUrl);
                    cmd.Parameters.AddWithValue("@id", dog.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteDog(int DogId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Dog
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", DogId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}