namespace PrescriptionApp.DTOs;

public class CreatePrescriptionDto
{
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public PatientDto Patient { get; set; }
    public int IdDoctor { get; set; }
    public List<MedicamentDto> Medicaments { get; set; }
}