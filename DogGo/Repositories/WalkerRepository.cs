using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DogGo.Repositories
{
    public class WalkerRepository : IWalkerRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalkerRepository(IConfiguration config)
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

        public List<Walker> GetAllWalkers()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT w.Id, w.[Name], w.ImageUrl, n.Name AS Neighborhood FROM Walker W
                            LEFT JOIN Neighborhood n ON n.Id = w.NeighborhoodId
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walker> walkers = new List<Walker>();
                    while (reader.Read())
                    {
                        Neighborhood n = new Neighborhood
                        {
                            Name = reader.GetString(reader.GetOrdinal("Neighborhood"))
                        };

                        Walker walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            Neighborhood = n
                        };

                        walkers.Add(walker);
                    }

                    reader.Close();

                    return walkers;
                }
            }
        }

        public Walker GetWalkerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT w.Id, w.[Name], w.ImageUrl, w.NeighborhoodId, n.Name AS Neighborhood FROM Walker W
                            LEFT JOIN Neighborhood n ON n.Id = w.NeighborhoodId
                        WHERE w.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Neighborhood n = new Neighborhood
                        {
                            Name = reader.GetString(reader.GetOrdinal("Neighborhood"))
                        };
                        Walker walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                            Neighborhood = n
                        };

                        reader.Close();
                        return walker;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }

        public List<Walker> GetWalkersInNeighborhood(int neighborhoodId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT w.Id, w.[Name], w.ImageUrl, w.NeighborhoodId, n.Name AS NeighborhoodName
                        FROM Walker w
                    LEFT JOIN Neighborhood n ON n.Id = w.NeighborhoodId 
                WHERE w.NeighborhoodId = @neighborhoodId
            ";

                    cmd.Parameters.AddWithValue("@neighborhoodId", neighborhoodId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walker> walkers = new List<Walker>();
                    while (reader.Read())
                    {
                        Walker walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                            Neighborhood = new Neighborhood()
                            {
                                Name = reader.GetString(reader.GetOrdinal("NeighborhoodName"))
                            }
                        };

                        walkers.Add(walker);
                    }

                    reader.Close();

                    return walkers;
                }
            }
        }

        public void AddWalker(Walker walker)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO Walker (Name, ImageUrl, NeighborhoodId)
                                        OUTPUT INSERTED.Id
                                        VALUES (@Name, @ImageUrl, @NeighborhoodId);
                                        ";

                    cmd.Parameters.AddWithValue("@Name", walker.Name);
                    cmd.Parameters.AddWithValue("@ImageUrl", walker.ImageUrl);
                    cmd.Parameters.AddWithValue("@NeighborhoodId", walker.NeighborhoodId);

                    int id = (int)cmd.ExecuteScalar();
                    walker.Id = id;
                }
            }
        }

        public void EditWalker(Walker walker)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    UPDATE Walker
                                    SET
                                        [Name] = @Name,
                                        ImageUrl = @ImageUrl,
                                        NeighborhoodId = @NeighborhoodId
                                    WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@Name", walker.Name);
                    cmd.Parameters.AddWithValue("@ImageUrl", walker.ImageUrl);
                    cmd.Parameters.AddWithValue("@NeighborhoodId", walker.NeighborhoodId);
                    cmd.Parameters.AddWithValue("@id", walker.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}