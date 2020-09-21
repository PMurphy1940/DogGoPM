﻿using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class WalksRepository : IWalksRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalksRepository(IConfiguration config)
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

        public List<Walk> GetWalksByWalkerId(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT w.Id,
                               w.Date, 
                               w.Duration, 
                               d.Name AS DogName, 
                               o.Name AS OwnerName
	                         FROM Walks w
                          LEFT JOIN Dog d ON d.id = w.DogId
                          LEFT JOIN Owner o ON d.OwnerId = o.Id
                        WHERE w.WalkerId = @id
                    ";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walk> walks = new List<Walk>();
                    while (reader.Read())
                    {
                        Owner anOwner = new Owner()
                        {
                            Name = reader.GetString(reader.GetOrdinal("OwnerName"))
                        };
                        Dog dog = new Dog()
                        {
                            Owner = anOwner,
                            Name = reader.GetString(reader.GetOrdinal("DogName"))
                        };
                        Walk walk = new Walk
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            Dog = dog
                        };

                        walks.Add(walk);
                    }

                    reader.Close();

                    return walks;
                }
            }
        }

       
    }
}