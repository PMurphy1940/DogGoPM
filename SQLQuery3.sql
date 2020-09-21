SELECT w.Date, w.Duration, d.Name AS DogName, o.Name AS OwnerName
	FROM Walks w
LEFT JOIN Dog d ON d.id = w.DogId
LEFT JOIN Owner o ON d.OwnerId = o.Id
WHERE w.WalkerId = 1


