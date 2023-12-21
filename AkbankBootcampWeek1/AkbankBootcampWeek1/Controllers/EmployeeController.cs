using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using System.Text.RegularExpressions;

namespace AkbankBootcampWeek1.Controllers;

// Employee sýnýfý özelliklerini içerir
public class Employee 
{
    public string? Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public double HourlySalary { get; set; }

}

// Employee sýnýfýnýn validation kurallarýnýn tanýmlandýgý EmployeeValidater sýnýfý
public class EmployeeValidater : AbstractValidator<Employee>
{
    public EmployeeValidater() 
    {
        // Name özelliði için boþ olmama, maksimum 250 karakter ve minimum 10 karekter uzunlugunda
        // olma kurallarý tanýmlandý.
        RuleFor(u => u.Name).NotEmpty()
            .MaximumLength(250)
            .MinimumLength(10);

        // DateOfBirth özelliði için boþ olmama kuralý tanýmlandý.
        RuleFor(u => u.DateOfBirth).NotEmpty();

        // Email özelliði için boþ olmama ve email formatýnda olma kurallarý tanýmlandý.
        RuleFor(u => u.Email).NotEmpty()
            .Must(ValidateEmail);

        // Phone özelliði için boþ olmama ve belirtilen telefon numarasý formatýnda olma kurallarý tanýmlandý.
        RuleFor(u => u.Phone).NotEmpty()
            .Must(ValidatePhoneNumber);

        // HourlySalary özelliði için boþ olmama ve maksimum 400 karakter ve minimum 50 degerinde
        // olma kurallarý tanýmlandý.
        RuleFor(u => u.HourlySalary).NotEmpty()
            .LessThan(400)
            .GreaterThan(50);
    }

    // Telefon numarasý doðrulamasý için kullanýlan metot
    private bool ValidatePhoneNumber(string text)
    {
        var regex = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
        return regex.IsMatch(text);
    }

    // Email doðrulamasý için kullanýlan metot
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
        // dogrulama iþleminde kullanýlmak için _validator tanýmlanýr
        _validator = validator;

        // örnek verilerin tanýmlamasý yapýlýr.
        _employees = DataGenerator.InitializeEmployee();
    }

    // Tüm employee verilerini getiren metot
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

    // Yeni employee eklemek için kullanýlan metot
    [HttpPost]
    public ActionResult<Employee> Post([FromBody] Employee value)
    {
        // employee sýnýfýnýn dogrulamasý yapýlýr 
        var validation = _validator.Validate(value);
        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }
        _employees.Add(value);
        return value;
    }
    
}

