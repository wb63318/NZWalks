using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionController : Controller
    {
        private readonly IRegionRepositories regionRepositories;
        private readonly IMapper mapper;

        public RegionController(IRegionRepositories regionRepositories, IMapper mapper)
        {
            this.regionRepositories = regionRepositories;
            this.mapper = mapper;
            
        }

        

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await regionRepositories.GetAllAsync();

            ////return DTO regions
            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name, 
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population,
            //    };

            //    regionsDTO.Add(regionDTO);
            //}
            //);

            var regionsDTO =  mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);
        }
        [ActionName("GetRegionAsync")]
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepositories.GetAsync(id);

            if(region == null) { return NotFound(); }
            var regionDTO =  mapper.Map<Models.DTO.Region>(region); 
            
            return Ok(regionDTO);
        }

        [HttpPost]
        
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //Request to domain model
            var region = new Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population
            };
            //Pass details to repository
            region = await regionRepositories.AddAsync(region);

            //Convert back to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };
            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //Get Region From Database
            var region = await regionRepositories.DeleteAsync(id);
            //If null NotFound
            if (region == null) { return NotFound(); }
            //Convert response to DTO
            // var regionDTO = mapper.Map<Models.DTO.Region>(region);
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population

            };
            //Return Ok Response
            return Ok(regionDTO);
        }
        [HttpPut]
        [Route("{id:guid}")]
        
        public async Task<IActionResult>UpdateRegionAsync([FromRoute] Guid id, [FromBody]Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            //Convert DTO to Domain Model
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population,
            };
            //Update Region using repository

            region = await regionRepositories.UpdateAsync(id,region);
            
            //If Null then Not Found 
            if(region== null)
            {
                return NotFound();
            }
            //Convert Domain back to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };
            //Return Ok response
            return Ok(regionDTO);
        }
    }
}
