CREATE DATABASE Scheduler;
GO

Use Scheduler;
GO

Create Table Tasks
(
	Task_ID INT PRIMARY KEY IDENTITY(1,1),
	[Description] VARCHAR(255) NOT NULL,
	DueDate DATETIME NOT NULL,
	[Priority] INT NOT NULL, -- ('Low' = 3, 'Medium' = 2, 'High' = 1)
	CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    [Status] VARCHAR(10) DEFAULT 'Pending' CHECK ([Status] IN ('Pending', 'Completed', 'Overdue')) -- Validate valid statuses
);
GO
-- to test
INSERT INTO Tasks ([Description], DueDate, [Priority], CreatedAt,[Status])
VALUES 
    ('Finish project report', '2024-12-01 17:00:00', 1, GETDATE(),'Completed'),
    ('Prepare presentation', '2024-12-05 10:00:00', 2, GETDATE(),'Pending'),
    ('Submit tax documents', '2024-11-30 12:00:00', 3, GETDATE(),'Overdue'),
    ('Review team feedback', '2024-12-03 15:00:00', 2, GETDATE(),'Pending'),
    ('Plan team meeting', '2024-12-10 09:00:00', 3, GETDATE(),'Overdue');
GO

SELECT * FROM Tasks
drop table Tasks;

-- Actual Add Task
INSERT INTO Tasks ([Description], DueDate, [Priority]) VALUES ('Complete the project', '2024-12-30', 1);

SELECT *
FROM Tasks
WHERE Status = 'Completed' OR Priority OR DueDate
