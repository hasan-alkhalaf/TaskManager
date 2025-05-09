using System;
using Microsoft.Data.SqlClient;

class Program
{
    static string connString = "Server=localhost,1433;Database=TaskDB;User Id=sa;Password=YourStrong!Passw0rd;";

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n1. Add Task\n2. View Tasks\n3. Complete Task\n4. Delete Task\n5. Exit");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1": AddTask(); break;
                case "2": ViewTasks(); break;
                case "3": CompleteTask(); break;
                case "4": DeleteTask(); break;
                case "5": return;
                default: Console.WriteLine("Invalid choice"); break;
            }
        }
    }

    static void AddTask()
    {
        Console.Write("Title: ");
        var title = Console.ReadLine();
        Console.Write("Description: ");
        var desc = Console.ReadLine();

        using var conn = new SqlConnection(connString);
        conn.Open();
        var cmd = new SqlCommand("INSERT INTO Tasks (Title, Description) VALUES (@title, @desc)", conn);
        cmd.Parameters.AddWithValue("@title", title);
        cmd.Parameters.AddWithValue("@desc", desc);
        cmd.ExecuteNonQuery();

        Console.WriteLine("Task added!");
    }

    static void ViewTasks()
    {
        using var conn = new SqlConnection(connString);
        conn.Open();
        var cmd = new SqlCommand("SELECT * FROM Tasks", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            bool isCompleted = (bool)reader["IsCompleted"];
            Console.WriteLine($"{reader["TaskId"]}: {reader["Title"]} - {(isCompleted ? "Done" : "Pending")}");
        }
    }

    static void CompleteTask()
    {
        Console.Write("Task ID to complete: ");
        var id = Console.ReadLine();

        using var conn = new SqlConnection(connString);
        conn.Open();
        var cmd = new SqlCommand("UPDATE Tasks SET IsCompleted = 1 WHERE TaskId = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        int rowsAffected = cmd.ExecuteNonQuery();

        Console.WriteLine(rowsAffected > 0 ? "Task completed!" : "Task not found.");
    }

    static void DeleteTask()
    {
        Console.Write("Task ID to delete: ");
        var id = Console.ReadLine();

        using var conn = new SqlConnection(connString);
        conn.Open();
        var cmd = new SqlCommand("DELETE FROM Tasks WHERE TaskId = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        int rowsAffected = cmd.ExecuteNonQuery();

        Console.WriteLine(rowsAffected > 0 ? "Task deleted!" : "Task not found.");
    }
}
