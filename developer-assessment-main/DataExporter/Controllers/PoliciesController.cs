using DataExporter.Dtos;
using DataExporter.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataExporter.Controllers
{
    [ApiController]
    [Route("[controller]")] 
    public class PoliciesController : ControllerBase
    {
        private PolicyService _policyService;

        public PoliciesController(PolicyService policyService) 
        { 
            _policyService = policyService;
        }

        [HttpPost]
        public async Task<IActionResult> PostPolicies([FromBody] CreatePolicyDto createPolicyDto)
        {
            // 1 Validate the incoming DTO 
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 2 Create via service
            var created = await _policyService.CreatePolicyAsync(createPolicyDto);
            if (created is null)
                return BadRequest("Invalid policy data.");

            // 3 Return 201 Created with a Location header to GET /[controller]/{policyId}
            return CreatedAtAction(nameof(GetPolicy), new { policyId = created.Id }, created);
        }


        [HttpGet]
        public async Task<IActionResult> GetPolicies()
        {
            var policies = await _policyService.ReadPoliciesAsync();
            return Ok(policies);
        }

        [HttpGet("{policyId}")]
        public async Task<IActionResult> GetPolicy(int policyId)    //parameter name binds to the method
        {
            var policy = await _policyService.ReadPolicyAsync(policyId);
            if (policy is null) return NotFound("Policy not found");  //returns statusCode = 404
            return Ok(policy);
        }


        [HttpPost("export")]
        public async Task<IActionResult> ExportData([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (endDate < startDate)
                return BadRequest("endDate must be greater than or equal to startDate.");

            var exportItems = await _policyService.ExportAsync(startDate, endDate);
            return Ok(exportItems);
        }

    }
}
