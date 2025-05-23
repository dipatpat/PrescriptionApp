using System.ComponentModel.DataAnnotations;

namespace PrescriptionApp.Models;

public class Doctor
{
    [Key] public int IdDoctor { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    public ICollection<Prescription> Prescriptions { get; set; }
}