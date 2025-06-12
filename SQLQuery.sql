SELECT GETDATE(); -- To check the connection


CREATE DATABASE Chatbot
GO

USE Chatbot;
GO

CREATE SCHEMA Chatbot;
GO

-- Users table
CREATE TABLE Chatbot.Users(
    UserId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Gender NVARCHAR(10) NOT NULL,
    PasswordHash VARBINARY(MAX) NOT NULL,
    PasswordSalt VARBINARY(MAX) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE()
);



CREATE TABLE Chatbot.ChatMessages(
    MessageId INT PRIMARY KEY IDENTITY(1, 1),
    UserId INT NOT NULL,
    MessageText NVARCHAR(2000) NOT NULL,
    Sentiment NVARCHAR(15) NOT NULL,
    SentimentScore FLOAT NULL, -- it is null because some sentiment analysis apis might not return a confidence score.
    BotResponse NVARCHAR(2000) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (UserId) REFERENCES Chatbot.Users(UserId) ON DELETE CASCADE
);

-- DROP TABLE Chatbot.ChatMessage; -- To drop a table in sql

SELECT * FROM Chatbot.Users;
SELECT * FROM Chatbot.ChatMessages;

INSERT INTO Chatbot.Users (FirstName, LastName, Email, Gender, PasswordHash, PasswordSalt)
VALUES 
('Alice', 'Johnson', 'alice@example.com', 'Female', 0x01, 0xA1),
('Bob', 'Smith', 'bob@example.com', 'Male', 0x02, 0xA2),
('Charlie', 'Lee', 'charlie@example.com', 'Male', 0x03, 0xA3),
('Diana', 'Brown', 'diana@example.com', 'Female', 0x04, 0xA4),
('Ethan', 'Clark', 'ethan@example.com', 'Male', 0x05, 0xA5),
('Fiona', 'Davis', 'fiona@example.com', 'Female', 0x06, 0xA6),
('George', 'Evans', 'george@example.com', 'Male', 0x07, 0xA7),
('Hannah', 'Fox', 'hannah@example.com', 'Female', 0x08, 0xA8),
('Ian', 'Garcia', 'ian@example.com', 'Male', 0x09, 0xA9),
('Jane', 'Hall', 'jane@example.com', 'Female', 0x0A, 0xAA),
('Kyle', 'Irwin', 'kyle@example.com', 'Male', 0x0B, 0xAB),
('Laura', 'Jackson', 'laura@example.com', 'Female', 0x0C, 0xAC),
('Mike', 'King', 'mike@example.com', 'Male', 0x0D, 0xAD),
('Nina', 'Lopez', 'nina@example.com', 'Female', 0x0E, 0xAE),
('Oscar', 'Martin', 'oscar@example.com', 'Male', 0x0F, 0xAF);


INSERT INTO Chatbot.ChatMessages (UserId, MessageText, Sentiment, SentimentScore, BotResponse)
VALUES 
(1, 'I am feeling amazing today!', 'Positive', 0.95, 'That’s great to hear! 😊'),
(2, 'Feeling really stressed lately.', 'Negative', 0.88, 'I’m here for you. Want to talk?'),
(3, 'I love working on new projects.', 'Positive', 0.91, 'That sounds exciting!'),
(4, 'Not sure how to feel.', 'Neutral', NULL, 'It’s okay to feel uncertain.'),
(5, 'This app is awesome!', 'Positive', 0.98, 'Glad you’re enjoying it!'),
(6, 'I can’t sleep at night.', 'Negative', 0.84, 'That sounds rough. Would relaxing help?'),
(7, 'Just a normal day.', 'Neutral', NULL, 'Thanks for sharing!'),
(8, 'I’m so happy today!', 'Positive', 0.93, 'Amazing! Keep that energy going.'),
(9, 'Nothing special happening.', 'Neutral', NULL, 'Got it. Let me know if you need anything.'),
(10, 'Lost my job today.', 'Negative', 0.96, 'I’m really sorry to hear that.'),
(11, 'Getting better every day.', 'Positive', 0.89, 'That’s inspiring! Keep going.'),
(12, 'Too many things on my mind.', 'Negative', 0.83, 'Let’s unpack it together.'),
(13, 'Nice weather outside.', 'Positive', 0.77, 'Enjoy the sunshine!'),
(14, 'I feel ignored.', 'Negative', 0.86, 'That sounds painful. I’m listening.'),
(15, 'Had an okay day.', 'Neutral', NULL, 'Thanks for checking in.');
