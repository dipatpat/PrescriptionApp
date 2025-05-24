using Microsoft.EntityFrameworkCore;
using PrescriptionApp.Models;

namespace PrescriptionApp.DAL;

public class PrescriptionDbContext : DbContext
{
    
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    public PrescriptionDbContext(DbContextOptions<PrescriptionDbContext> options) : base(options) { }

    protected PrescriptionDbContext() { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PrescriptionMedicament>()
            .HasKey(pm => new { pm.IdPrescription, pm.IdMedicament });

        modelBuilder.Entity<PrescriptionMedicament>()
            .HasOne(pm => pm.Prescription)
            .WithMany(p => p.PrescriptionMedicaments)
            .HasForeignKey(pm => pm.IdPrescription);

        modelBuilder.Entity<PrescriptionMedicament>()
            .HasOne(pm => pm.Medicament)
            .WithMany(m => m.PrescriptionMedicaments)
            .HasForeignKey(pm => pm.IdMedicament);
        
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor { IdDoctor = 1, FirstName = "Alice", LastName = "Smith", Email = "alice.smith@hospital.com" },
            new Doctor { IdDoctor = 2, FirstName = "Bob", LastName = "Brown", Email = "bob.brown@hospital.com" }
        );

        modelBuilder.Entity<Medicament>().HasData(
            new Medicament { IdMedicament = 1, Name = "Aspirin", Description = "Pain reliever", Type = "Tablet" },
            new Medicament { IdMedicament = 2, Name = "Penicillin", Description = "Antibiotic", Type = "Injection" },
            new Medicament { IdMedicament = 3, Name = "Ibuprofen", Description = "Anti-inflammatory", Type = "Capsule" }
        );

        modelBuilder.Entity<Patient>().HasData(
            new Patient { IdPatient = 1, FirstName = "John", LastName = "Doe", Birthdate = new DateTime(1990, 1, 1) },
            new Patient { IdPatient = 2, FirstName = "Jane", LastName = "Doe", Birthdate = new DateTime(1985, 5, 15) }
        );
        
        modelBuilder.Entity<Prescription>().HasData(
            new Prescription
            {
                IdPrescription = 1,
                Date = new DateTime(2024, 5, 1),
                DueDate = new DateTime(2024, 5, 15),
                IdPatient = 1,
                IdDoctor = 1
            }
        );

        modelBuilder.Entity<PrescriptionMedicament>().HasData(
            new PrescriptionMedicament
            {
                IdPrescription = 1,
                IdMedicament = 1,
                Dose = 2,
                Details = "Take after meal"
            },
            new PrescriptionMedicament
            {
                IdPrescription = 1,
                IdMedicament = 2,
                Dose = 1,
                Details = "Once daily"
            }
        );

    }
}

