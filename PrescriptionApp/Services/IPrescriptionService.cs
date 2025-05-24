using Microsoft.AspNetCore.Mvc;
using PrescriptionApp.DTOs;

namespace PrescriptionApp.Services;

public interface IPrescriptionService
{
    Task<IActionResult> AddPrescriptionAsync(CreatePrescriptionDto dto);
    Task<GetPatientResponseDto?> GetPatientAsync(int id);
}