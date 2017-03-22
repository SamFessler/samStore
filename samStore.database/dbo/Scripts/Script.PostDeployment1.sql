﻿INSERT INTO Category(CategoryName, CategoryDescription) VALUES
('Tools', 'Wire, Scisorrs and much more for all your tree growing needs'),
('Trees', 'Our wide selection of bonsai trees')

INSERT INTO Product(ProductName, ProductPrice , TreeSpecies, TreeSkill, ProductDescription) VALUES
('Japanese Black Pine', 19.99, 'Pinus thunbergii', 'Beginner', 'Pinus thunbergii, also called black pine, Japanese black pine, and Japanese pine, is a pine native to coastal areas of Japan and South Korea. It is called gomsol in Korean, hēisōng in Chinese, and kuromatsu in Japanese.'),
('Monterey Cypress', 29.99,'Cupressus macrocarpa','Advanced', 'Cupressus macrocarpa, commonly known as Monterey cypress, is a species of cypress native to the Central Coast of California.'),
('Chinese Juniper', 39.99,'Juniperus chinensis','Beginner', 'Juniperus chinensis is a juniper that grows as a shrub or tree with a very variable shape, reaching 1–20 m tall. This native of northeast Asia grows in China, Mongolia, Japan, Korea and the southeast of Russia.')

INSERT INTO ProductImage(ImagePath, ProductID) VALUES
('/content/Images/japaneseBlackPine.jpg', (SELECT TOP 1 ID FROM Product WHERE ProductName = 'Japanese Black Pine')),
('/content/Images/montreyCypress.jpg', (SELECT TOP 1 ID FROM Product WHERE ProductName = 'Monterey Cypress')),
('/content/Images/chinesejuniper.jpg', (SELECT TOP 1 ID FROM Product WHERE ProductName = 'Chinese Juniper'))

INSERT INTO CategoryProduct(CategoryID, ProductID) VALUES
((SELECT TOP 1 ID FROM Category WHERE CategoryName = 'Trees'), (SELECT TOP 1 ID FROM Product WHERE ProductName = 'Japanese Black Pine')),
((SELECT TOP 1 ID FROM Category WHERE CategoryName= 'Trees'), (SELECT TOP 1 ID FROM Product WHERE ProductName = 'Monterey Cypress')),
((SELECT TOP 1 ID FROM Category WHERE CategoryName = 'Trees'), (SELECT TOP 1 ID FROM Product WHERE ProductName = 'Chinese juniper'))
