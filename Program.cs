using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json; // Para la serialización/deserialización de objetos JSON


namespace TaskManagerConsole
{
    class Program
    {
        static List<Task> tasks = new List<Task>();

        static void Main(string[] args)
        {
            LoadTasks(); // Cargar tareas al iniciar la aplicación
            bool exit = false;
            while (!exit)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("=== Gestor de Tareas ===".ToUpper());
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("1. Agregar Tarea");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("2. Marcar Tarea como Completada");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("3. Eliminar Tarea");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("4. Mostrar Tareas Pendientes");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("5. Guardar Tareas Antes de Salir");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("6. Salir");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Seleccione una opción: ");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        AddTask();
                        break;
                    case "2":
                        MarkTaskAsCompleted();
                        break;
                    case "3":
                        RemoveTask();
                        break;
                    case "4":
                        ShowPendingTasks();
                        break;
                    case "5":
                        SaveTasks(); // Guardar tareas antes de salir
                        return;
                    case "6":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Opción inválida. Por favor, seleccione una opción válida.");
                        break;
                }
            }
        }


        static void AddTask()
        {
            Console.Write("Ingrese la nueva tarea: ");
            string description = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(description))
            {
                Task newTask = new Task
                {
                    Description = description,
                    IsCompleted = false
                };

                tasks.Add(newTask);

                Console.WriteLine("Tarea agregada con éxito!");
            }
            else
            {
                Console.WriteLine("Por favor, ingresa una descripción válida para la tarea.");
            }
        }

        static void MarkTaskAsCompleted()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Seleccione la tarea que desea marcar como completada:");

            // Mostrar la lista de tareas
            ShowPendingTasks();

            Console.Write("Ingrese el número de la tarea: ");
            int taskIndex;
            if (int.TryParse(Console.ReadLine(), out taskIndex) && taskIndex > 0 && taskIndex <= tasks.Count)
            {
                // Marcar la tarea seleccionada como completada
                Task task = tasks[taskIndex - 1];
                task.IsCompleted = true;

                Console.WriteLine($"La tarea '{task.Description}' ha sido marcada como completada.");
            }
            else
            {
                Console.WriteLine("Opción inválida. Por favor, ingrese un número válido de tarea.");
            }
            // Implementa la lógica para marcar una tarea como completada
        }

        static void RemoveTask()
        {
            Console.WriteLine("Seleccione la tarea que desea eliminar:");

            // Mostrar la lista de tareas
            ShowPendingTasks();

            Console.Write("Ingrese el número de la tarea: ");
            int taskIndex;
            if (int.TryParse(Console.ReadLine(), out taskIndex) && taskIndex > 0 && taskIndex <= tasks.Count)
            {
                // Obtener la tarea seleccionada (se resta 1 porque los índices comienzan desde 0)
                Task removedTask = tasks[taskIndex - 1];
                string description = removedTask.Description;

                // Eliminar la tarea seleccionada
                tasks.RemoveAt(taskIndex - 1);

                Console.WriteLine($"La tarea '{description}' ha sido eliminada.");
            }
            else
            {
                Console.WriteLine("Opción inválida. Por favor, ingrese un número válido de tarea.");
            }
            // Implementa la lógica para eliminar una tarea
        }

        static void ShowPendingTasks()
        {

            int count = 0;
            foreach (Task task in tasks)
            {
                if (!task.IsCompleted)
                {
                    count++;
                    Console.WriteLine($"{count}. {task.Description}");
                }
            }

            if (count == 0)
            {
                Console.WriteLine("No hay tareas pendientes.");
            }
            // Implementa la lógica para mostrar tareas pendientes
        }
        static void SaveTasks()
        {
            string json = JsonSerializer.Serialize(tasks); // Serializar la lista de tareas a JSON
            File.WriteAllText("tasks.json", json); // Escribir el JSON en un archivo
        }

        static void LoadTasks()
        {
            if (File.Exists("tasks.json"))
            {
                string json = File.ReadAllText("tasks.json"); // Leer el JSON desde el archivo
                tasks = JsonSerializer.Deserialize<List<Task>>(json); // Deserializar el JSON a una lista de tareas
            }
        }
    }
}
