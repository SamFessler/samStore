INSERT INTO Category(Name, Description) VALUES
('Tabletop', 'games played on top of a normal table'),
('Family', 'Appropriate for most ages and skill levels')

INSERT INTO Product(Name, Price, Description) VALUES
('Clue', 19.99, 'Fun for the whole family'),
('Monopoly', 29.99, 'The game of real-estate empires'),
('Payday', 15.99, 'Pay your bills on time and enjoy retro game artwork')

INSERT INTO ProductImage(Path, ProductID, AltText) VALUES
('/content/clue.jpg', (SELECT TOP 1 ID FROM Product WHERE Name = 'Clue'), 'Clue'),
('/content/monopoly.jpg', (SELECT TOP 1 ID FROM Product WHERE Name = 'Monopoly'), 'Monopoly'),
('/content/payday.jpg', (SELECT TOP 1 ID FROM Product WHERE Name = 'Payday'), 'Payday')

INSERT INTO CategoryProduct(CategoryID, ProductID) VALUES
((SELECT TOP 1 ID FROM Category WHERE Name = 'Tabletop'), (SELECT TOP 1 ID FROM Product WHERE Name = 'Clue')),
((SELECT TOP 1 ID FROM Category WHERE Name = 'Tabletop'), (SELECT TOP 1 ID FROM Product WHERE Name = 'Monopoly')),
((SELECT TOP 1 ID FROM Category WHERE Name = 'Tabletop'), (SELECT TOP 1 ID FROM Product WHERE Name = 'Payday')),
((SELECT TOP 1 ID FROM Category WHERE Name = 'Family'), (SELECT TOP 1 ID FROM Product WHERE Name = 'Clue')),
((SELECT TOP 1 ID FROM Category WHERE Name = 'Family'), (SELECT TOP 1 ID FROM Product WHERE Name = 'Monopoly')),
((SELECT TOP 1 ID FROM Category WHERE Name = 'Family'), (SELECT TOP 1 ID FROM Product WHERE Name = 'Payday'))