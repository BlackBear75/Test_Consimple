namespace Test_Consimple.Models.ClientModels;

public class CreateClientRequest
{
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
}