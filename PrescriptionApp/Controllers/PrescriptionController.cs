using Microsoft.AspNetCore.Mvc;
using PrescriptionApp.DTOs;
using PrescriptionApp.Services;

namespace PrescriptionApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController : ControllerBase
{
    private readonly IPrescriptionService _service;

    public PrescriptionsController(IPrescriptionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] CreatePrescriptionDto dto)
        => await _service.AddPrescriptionAsync(dto);

    [HttpGet("patients/{id}")]
    public async Task<IActionResult> GetPatient(int id)
    {
        var result = await _service.GetPatientAsync(id);
        return result is null ? NotFound() : Ok(result);
    }
}