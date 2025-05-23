using System.ComponentModel.DataAnnotations;

namespace PrescriptionApp.Models;

public class Medicament
{
    [Key] public int IdMedicament { get; set; }
    [Required] public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
}