using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AkbankBootcampWeek1.Controllers;

public class Staff
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public decimal? HourlySalary { get; set; }
}

public class StaffValidater : AbstractValidator<Staff>
{
    public StaffValidater()
    {
        RuleFor(u => u.Name).NotEmpty()
            .MaximumLength(250)
            .MinimumLength(10);

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
        var regex = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
        return regex.IsMatch(text);
    }

    private bool ValidateEmail(string text)
    {
        var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        return regex.IsMatch(text);
    }

}

[Route("api/[controller]")]
[ApiController]
public class StaffController : ControllerBase
{
    private readonly IValidator<Staff> _validator;

    public StaffController(IValidator<Staff> validator)
    {
        _validator = validator;
    }

    [HttpPost]
    public IActionResult Post([FromBody] Staff value)
    {
        var validation = _validator.Validate(value);
        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }
        return Ok();
    }
}