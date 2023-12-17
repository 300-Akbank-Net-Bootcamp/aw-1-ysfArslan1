using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using System.Text.RegularExpressions;

namespace AkbankBootcampWeek1.Controllers;

public class Employee 
{
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public double HourlySalary { get; set; }

}

public class EmployeeValidater : AbstractValidator<Employee>
{
    public EmployeeValidater() 
    {
        RuleFor(u => u.Name).NotEmpty()
            .MaximumLength(250)
            .MinimumLength(10);

        RuleFor(u => u.DateOfBirth).NotEmpty();

        RuleFor(u => u.Email).NotEmpty()
            .Must(ValidateEmail);

        RuleFor(u => u.Phone).NotEmpty()
            .Must(ValidatePhoneNumber);

        RuleFor(u => u.HourlySalary).NotEmpty()
            .LessThan(400)
            .GreaterThan(10);
    }

    private bool ValidatePhoneNumber(string text)
    {
        //var regex = new Regex(@"^\d{10}$");
        var regex = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
        return regex.IsMatch(text);
    }

    private bool ValidateEmail(string text)
    {
        //var regex = new Regex(@"^\d{10}$");
        var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        return regex.IsMatch(text);
    }

}


[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IValidator<Employee> _validator;
    public EmployeeController(IValidator<Employee> validator)
    {
        _validator = validator;
    }

    [HttpPost]
    public IActionResult Post([FromBody] Employee value)
    {
        var validation = _validator.Validate(value);
        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }
        return Ok();
    }
}

