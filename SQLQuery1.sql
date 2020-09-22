SELECT w.Id, w.[Name], w.ImageUrl, w.NeighborhoodId, n.Name AS NeighborhoodName
                        FROM Walker w
                    LEFT JOIN Neighborhood n ON n.Id = w.NeighborhoodId 
                WHERE w.NeighborhoodId = 7
