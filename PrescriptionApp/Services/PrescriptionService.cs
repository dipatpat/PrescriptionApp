using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrescriptionApp.DAL;
using PrescriptionApp.DTOs;
using PrescriptionApp.Models;

namespace PrescriptionApp.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly PrescriptionDbContext _context;

    public PrescriptionService(PrescriptionDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> AddPrescriptionAsync(CreatePrescriptionDto dto)
    {
        if (dto.Medicaments.Count > 10)
            return new BadRequestObjectResult("A prescription can include a maximum of 10 medicaments.");

        if (dto.DueDate < dto.Date)
            return new BadRequestObjectResult("Due date must be greater than or equal to the start date.");

        var doctor = await _context.Doctors.FindAsync(dto.IdDoctor);
        if (doctor == null)
            return new NotFoundObjectResult("Doctor not found.");

        var patient = await _context.Patients.FirstOrDefaultAsync(p =>
            p.FirstName == dto.Patient.FirstName &&
            p.LastName == dto.Patient.LastName &&
            p.Birthdate == dto.Patient.Birthdate);

        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = dto.Patient.FirstName,
                LastName = dto.Patient.LastName,
                Birthdate = dto.Patient.Birthdate
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        foreach (var med in dto.Medicaments)
        {
            if (!await _context.Medicaments.AnyAsync(m => m.IdMedicament == med.IdMedicament))
                return new NotFoundObjectResult($"Medicament ID {med.IdMedicament} not found.");
        }

        var prescription = new Prescription
        {
            Date = dto.Date,
            DueDate = dto.DueDate,
            IdDoctor = doctor.IdDoctor,
            IdPatient = patient.IdPatient,
            PrescriptionMedicaments = dto.Medicaments.Select(m => new PrescriptionMedicament
            {
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Details = m.Details
            }).ToList()
        };

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();

        return new CreatedAtActionResult("GetPatient", "Prescriptions", new { id = patient.IdPatient }, null);
    }

    public async Task<GetPatientResponseDto?> GetPatientAsync(int id)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.Doctor)
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.PrescriptionMedicaments)
                    .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == id);

        if (patient == null)
            return null;

        return new GetPatientResponseDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate,
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate)
                .Select(p => new GetPrescriptionDto
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new DoctorDto
                    {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName,
                        LastName = p.Doctor.LastName,
                        Email = p.Doctor.Email
                    },
                    Medicaments = p.PrescriptionMedicaments.Select(pm => new GetMedicamentDto
                    {
                        IdMedicament = pm.Medicament.IdMedicament,
                        Name = pm.Medicament.Name,
                        Description = pm.Medicament.Description,
                        Type = pm.Medicament.Type,
                        Dose = pm.Dose,
                        Details = pm.Details
                    }).ToList()
                }).ToList()
        };
    }
}
