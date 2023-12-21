using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AkbankBootcampWeek1.Controllers;

// Staff s�n�f� �zelliklerini i�erir
public class Staff
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public decimal HourlySalary { get; set; }
}

// Staff s�n�f�n�n validation kurallar�n�n tan�mland�g� StaffValidator s�n�f�
public class StaffValidater : AbstractValidator<Staff>
{
    public StaffValidater() 
    {
        // Name �zelli�i i�in bo� olmama, maksimum 250 karakter ve minimum 10 karekter uzunlugunda
        // olma kurallar� tan�mland�.
        RuleFor(u => u.Name)
            .NotEmpty()
            .MaximumLength(250)
            .MinimumLength(10);

        // Email �zelli�i i�in bo� olmama ve email format�nda olma kurallar� tan�mland�.
        RuleFor(u => u.Email).NotEmpty()
            .Must(ValidateEmail);

        // Phone �zelli�i i�in bo� olmama ve belirtilen telefon numaras� format�nda olma kurallar� tan�mland�.
        RuleFor(u => u.Phone).NotEmpty()
            .Must(ValidatePhoneNumber);

        // HourlySalary �zelli�i i�in bo� olmama ve maksimum 400 karakter ve minimum 30 degerinde
        // olma kurallar� tan�mland�.
        RuleFor(u => u.HourlySalary).NotEmpty()
            .LessThan(400)
            .GreaterThan(30);
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
public class StaffController : ControllerBase
{
    private readonly IValidator<Staff> _validator;
    private static List<Staff> _staffList = new List<Staff>();
    public StaffController(IValidator<Staff> validator)
    {
        // dogrulama i�leminde kullan�lmak i�in _validator tan�mlan�r
        _validator = validator;

        // �rnek verilerin tan�mlamas� yap�l�r.
        _staffList = DataGenerator.InitializeStaff();
    }

    // T�m staff verilerini getiren metot
    [HttpGet]
    public ActionResult<List<Staff>> Get()
    {
        var _list = _staffList.ToList();
        if (_list == null)
        {
            return NotFound();
        }
        return _list;
    }

    // Yeni staff eklemek i�in kullan�lan metot
    [HttpPost]
    public ActionResult<Staff> Post([FromBody] Staff value)
    {
        // staff s�n�f�n�n dogrulamas� yap�l�r 
        var validation = _validator.Validate(value);
        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }
        _staffList.Add(value);
        return value;
    }
    
}