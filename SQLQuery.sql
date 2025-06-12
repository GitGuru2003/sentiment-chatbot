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