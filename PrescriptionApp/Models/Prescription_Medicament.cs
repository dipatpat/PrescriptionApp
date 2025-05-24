using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrescriptionApp.Models;

public class PrescriptionMedicament
{
    public int IdPrescription { get; set; }
    public int IdMedicament { get; set; }
    public int Dose { get; set; }
    [MaxLength(100)] public string Details { get; set; }

    [ForeignKey(nameof(IdPrescription))]
    public Prescription Prescription { get; set; }

    [ForeignKey(nameof(IdMedicament))]
    public Medicament Medicament { get; set; }
}