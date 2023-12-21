using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AkbankBootcampWeek1.Controllers;

// Staff sýnýfý özelliklerini içerir
public class Staff
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public decimal HourlySalary { get; set; }
}

// Staff sýnýfýnýn validation kurallarýnýn tanýmlandýgý StaffValidator sýnýfý
public class StaffValidater : AbstractValidator<Staff>
{
    public StaffValidater() 
    {
        // Name özelliði için boþ olmama, maksimum 250 karakter ve minimum 10 karekter uzunlugunda
        // olma kurallarý tanýmlandý.
        RuleFor(u => u.Name)
            .NotEmpty()
            .MaximumLength(250)
            .MinimumLength(10);

        // Email özelliði için boþ olmama ve email formatýnda olma kurallarý tanýmlandý.
        RuleFor(u => u.Email).NotEmpty()
            .Must(ValidateEmail);

        // Phone özelliði için boþ olmama ve belirtilen telefon numarasý formatýnda olma kurallarý tanýmlandý.
        RuleFor(u => u.Phone).NotEmpty()
            .Must(ValidatePhoneNumber);

        // HourlySalary özelliði için boþ olmama ve maksimum 400 karakter ve minimum 30 degerinde
        // olma kurallarý tanýmlandý.
        RuleFor(u => u.HourlySalary).NotEmpty()
            .LessThan(400)
            .GreaterThan(30);
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
public class StaffController : ControllerBase
{
    private readonly IValidator<Staff> _validator;
    private static List<Staff> _staffList = new List<Staff>();
    public StaffController(IValidator<Staff> validator)
    {
        // dogrulama iþleminde kullanýlmak için _validator tanýmlanýr
        _validator = validator;

        // örnek verilerin tanýmlamasý yapýlýr.
        _staffList = DataGenerator.InitializeStaff();
    }

    // Tüm staff verilerini getiren metot
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

    // Yeni staff eklemek için kullanýlan metot
    [HttpPost]
    public ActionResult<Staff> Post([FromBody] Staff value)
    {
        // staff sýnýfýnýn dogrulamasý yapýlýr 
        var validation = _validator.Validate(value);
        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }
        _staffList.Add(value);
        return value;
    }
    
}