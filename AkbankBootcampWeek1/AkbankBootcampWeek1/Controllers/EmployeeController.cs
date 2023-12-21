using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using System.Text.RegularExpressions;

namespace AkbankBootcampWeek1.Controllers;

// Employee s�n�f� �zelliklerini i�erir
public class Employee 
{
    public string? Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public double HourlySalary { get; set; }

}

// Employee s�n�f�n�n validation kurallar�n�n tan�mland�g� EmployeeValidater s�n�f�
public class EmployeeValidater : AbstractValidator<Employee>
{
    public EmployeeValidater() 
    {
        // Name �zelli�i i�in bo� olmama, maksimum 250 karakter ve minimum 10 karekter uzunlugunda
        // olma kurallar� tan�mland�.
        RuleFor(u => u.Name).NotEmpty()
            .MaximumLength(250)
            .MinimumLength(10);

        // DateOfBirth �zelli�i i�in bo� olmama kural� tan�mland�.
        RuleFor(u => u.DateOfBirth).NotEmpty();

        // Email �zelli�i i�in bo� olmama ve email format�nda olma kurallar� tan�mland�.
        RuleFor(u => u.Email).NotEmpty()
            .Must(ValidateEmail);

        // Phone �zelli�i i�in bo� olmama ve belirtilen telefon numaras� format�nda olma kurallar� tan�mland�.
        RuleFor(u => u.Phone).NotEmpty()
            .Must(ValidatePhoneNumber);

        // HourlySalary �zelli�i i�in bo� olmama ve maksimum 400 karakter ve minimum 50 degerinde
        // olma kurallar� tan�mland�.
        RuleFor(u => u.HourlySalary).NotEmpty()
            .LessThan(400)
            .GreaterThan(50);
    }

    // Telefon numaras� do�rulamas� i�in kullan�lan metot
    private bool ValidatePhoneNumber(string text)
    {
        var regex = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
        return regex.IsMatch(text);
    }

    // Email do�rulamas� i�in kullan�lan metot
    private bool ValidateEmail(string text)
    {
        var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        return regex.IsMatch(text);
    }

}


[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IValidator<Employee> _validator;
    private static List<Employee> _employees=new List<Employee>();
    public EmployeeController(IValidator<Employee> validator)
    {
        // dogrulama i�leminde kullan�lmak i�in _validator tan�mlan�r
        _validator = validator;

        // �rnek verilerin tan�mlamas� yap�l�r.
        _employees = DataGenerator.InitializeEmployee();
    }

    // T�m employee verilerini getiren metot
    [HttpGet]
    public ActionResult<List<Employee>> Get()
    {
        var _list = _employees.ToList();
        if (_list == null)
        {
            return NotFound();
        }
        return _list;
    }

    // Yeni employee eklemek i�in kullan�lan metot
    [HttpPost]
    public ActionResult<Employee> Post([FromBody] Employee value)
    {
        // employee s�n�f�n�n dogrulamas� yap�l�r 
        var validation = _validator.Validate(value);
        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }
        _employees.Add(value);
        return value;
    }
    
}

