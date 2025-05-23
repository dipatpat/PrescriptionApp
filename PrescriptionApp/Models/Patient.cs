using System.ComponentModel.DataAnnotations;

namespace PrescriptionApp.Models;

public class Patient
{
    [Key] public int IdPatient { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    public ICollection<Prescription> Prescriptions { get; set; }
}