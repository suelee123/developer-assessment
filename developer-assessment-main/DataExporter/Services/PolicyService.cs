using DataExporter.Dtos;
using DataExporter.Model;
using Microsoft.EntityFrameworkCore;


namespace DataExporter.Services
{
    public class PolicyService
    {
        private ExporterDbContext _dbContext;

        public PolicyService(ExporterDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.Database.EnsureCreated();
        }

        /// <summary>
        /// Creates a new policy from the DTO.
        /// </summary>
        /// <param name="policy"></param>
        /// <returns>Returns a ReadPolicyDto representing the new policy, if succeded. Returns null, otherwise.</returns>
        public async Task<ReadPolicyDto?> CreatePolicyAsync(CreatePolicyDto createPolicyDto)
        {
            var entity = new Policy
            {
                PolicyNumber = createPolicyDto.PolicyNumber,
                Premium = createPolicyDto.Premium,
                StartDate = createPolicyDto.StartDate
            };

            _dbContext.Policies.Add(entity);
            await _dbContext.SaveChangesAsync();

            return new ReadPolicyDto
            {
                Id = entity.Id,
                PolicyNumber = entity.PolicyNumber,
                Premium = entity.Premium,
                StartDate = entity.StartDate
            };

        }

        /// <summary>
        /// Retrives all policies.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a list of ReadPoliciesDto.</returns>
        public async Task<IList<ReadPolicyDto>> ReadPoliciesAsync()
        {

            return await _dbContext.Policies.Select(p => new ReadPolicyDto
            {
                Id = p.Id,
                PolicyNumber = p.PolicyNumber,
                Premium = p.Premium,
                StartDate = p.StartDate
            })
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a policy by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a ReadPolicyDto.</returns>
        public async Task<ReadPolicyDto?> ReadPolicyAsync(int id)
        {
            var policy = await _dbContext.Policies.FirstOrDefaultAsync(x => x.Id == id);    //Id is a primary key, return first row

            if (policy == null)
            {
                return null;
            }

            var policyDto = new ReadPolicyDto()
            {
                Id = policy.Id,
                PolicyNumber = policy.PolicyNumber,
                Premium = policy.Premium,
                StartDate = policy.StartDate
            };

            return policyDto;
        }


        public async Task<IList<ExportDto>> ExportAsync(DateTime startDate, DateTime endDate)
        {
            // Export policies include notes
            return await _dbContext.Policies
                .Where(p => p.StartDate >= startDate && p.StartDate <= endDate)
                .Select(p => new ExportDto
                {
                    PolicyNumber = p.PolicyNumber,
                    Premium = p.Premium,
                    StartDate = p.StartDate,
                    Notes = p.Notes
                        .OrderBy(n => n.Id)
                        .Select(n => n.Text)
                        .ToList()
                })
                .ToListAsync();
        }

    }
}
