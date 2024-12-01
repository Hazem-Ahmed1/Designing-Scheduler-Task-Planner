# Task Scheduler in F#

## **Overview**

This project is a task scheduler and planner built in F# to help users organize their tasks, set deadlines, and manage priorities effectively. The application supports task creation, updates, filtering, notifications, and persistence.

---

## **Features**

### **1. Task Representation**
Tasks are represented as structured data with the following fields:
- **Task ID**: A unique identifier for each task.
- **Description**: A textual description of the task.
- **Due Date**: The deadline for task completion.
- **Priority**: A numerical value (1–5) indicating task importance.
- **Status**: The state of the task (e.g., Pending, Completed, Overdue).

### **2. Task Management**
- **Add Tasks**: Create new tasks with user-defined properties (description, due date, and priority).
- **Update Tasks**: 
  - Mark tasks as completed.
  - Update task priorities.
- **Delete Tasks**: Remove tasks permanently.

### **3. Filtering and Sorting**
- **Filter Tasks**:
  - By status (Pending, Completed, Overdue).
  - By priority.
  - By due date.
- **Sort Tasks**:
  - By due date.
  - By priority.
  - By creation time.

### **4. Notifications**
- **Deadline Alerts**: Highlight tasks nearing their deadlines.
- **Overdue Alerts**: Notify users of tasks that are overdue.

### **5. Persistence**
- Save tasks to and load tasks from a database for data persistence using Dapper.

---
## **Requirements**

### **Development Environment**
- **F# Compiler**: Ensure you have an F# runtime and compiler installed.
- **Database**: Microsoft SQL Server is used for task persistence.

### **Dependencies**
- [Dapper](https://dapper-tutorial.net/): Lightweight ORM for SQL database interaction.
- Microsoft.Data.SqlClient: A .NET library for SQL Server communication.

---

## **Usage**

### **Run the Program**
1. Clone the repository.
2. Compile and run the F# code using your preferred IDE or CLI tools.
3. The application will present you with a menu to choose from.