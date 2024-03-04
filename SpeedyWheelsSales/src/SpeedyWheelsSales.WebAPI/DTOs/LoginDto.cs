namespace SpeedyWheelsSales.WebAPI.DTOs;

public class LoginDto
{
    public string EmailOrUserName { get; set; } // TODO need to change it by "PhoneOrUserName"
    public string Password { get; set; }
}