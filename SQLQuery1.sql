SELECT w.Id,
                               w.Date, 
                               w.Duration, 
                               d.Name AS DogName, 
                               wa.Name AS WalkerName
	                         FROM Walks w
                          LEFT JOIN Dog d ON d.id = w.DogId
                          Left Join Walker wa ON wa.id = w.WalkerId
                          LEFT JOIN Owner o ON d.OwnerId = o.Id
                        WHERE o.id = 1
                      