SELECT d.Id, d.Name, d.Breed, d.Notes, d.ImageUrl, d.OwnerId, o.Name 
                FROM Dog d
                LEFT JOIN Owner o ON o.id = d.OwnerId
                WHERE OwnerId = 1