using AkbankBootcampWeek1.Controllers;

namespace AkbankBootcampWeek1;

// Örnek verilerin oluşturulması için kullanılan DataGenerator sınıfı
public class DataGenerator
{
    // InitializeEmployee metodu Employee listesi oluşturur
    public static List<Employee> InitializeEmployee()
    {
        return new List<Employee> {
            new Employee
            {
                Name = "Ahmet ------",
                DateOfBirth = DateTime.Today.AddYears(-20),
                Email = "ahmet---@gmail.com", 
                Phone = "5555555555",
                HourlySalary = 230
            },
            new Employee
            {
                Name = "zenep ------",
                DateOfBirth = DateTime.Today.AddYears(-23),
                Email = "zeynep---@gmail.com",  
                Phone = "5555555533",
                HourlySalary = 270
            },
            new Employee
            {
                Name = "hasan ------",
                DateOfBirth = DateTime.Today.AddYears(-21),
                Email = "hasan---@gmail.com", 
                Phone = "5555555522",
                HourlySalary = 280
            },
            new Employee
            {
                Name = "nisa ------",
                DateOfBirth = DateTime.Today.AddYears(-22),
                Email = "nisa---@gmail.com", 
                Phone = "5555555511",
                HourlySalary = 210
            },
        };

    }

    // InitializeStaff metodu Staff listesi oluşturur
    public static List<Staff> InitializeStaff()
    {
        return new List<Staff> {
            new Staff
            {
                Name = "Ahmet ------",
                Email = "ahmet---@gmail.com", 
                Phone = "5555555555",
                HourlySalary = 230
            },
            new Staff
            {
                Name = "zenep ------",
                Email = "zeynep---@gmail.com", 
                Phone = "5555555533",
                HourlySalary = 270
            },
            new Staff
            {
                Name = "hasan ------",
                Email = "hasan---@gmail.com",
                Phone = "5555555522",
                HourlySalary = 280
            },
            new Staff
            {
                Name = "nisa ------",
                Email = "nisa---@gmail.com",
                Phone = "5555555511",
                HourlySalary = 210
            },
        };

    }
}



