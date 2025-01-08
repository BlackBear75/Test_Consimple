namespace Test_Consimple.Models.ClientModels;

public class ClientResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public DateTime RegistrationDate { get; set; }
}