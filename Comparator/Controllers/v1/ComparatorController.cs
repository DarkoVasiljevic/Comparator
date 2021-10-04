using Comparator.Dtos;
using Comparator.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comparator.Controllers.v1
{
    [Route("v1/diff/")]
    [ApiController]
    public class ComparatorController : ControllerBase
    {

        private readonly IComparatorService _comparatorService;

        public ComparatorController(IComparatorService comparatorService)
        {
            _comparatorService = comparatorService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetComparatorResultById(int id)
        {
            try
            {
                var result = await _comparatorService.GetComparatorResultByIdAsync(id);
                if (result == null) 
                    return NotFound("404 Not Found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id:int}/left")]
        public async Task<ActionResult> PutLeft(int id, [FromBody] DataRequest data)
        {
            try
            {
                var isVaild = await Task.Run(() => ValidateData(data));
                if (!isVaild) return BadRequest("400 Bad Request");

                var success = await _comparatorService.InsertOrUpdateLeftAsync(id, data);
                if (success)
                    return Created($"~v1/diff/{id}/left", "201 Created");
                
                return BadRequest("400 Bad Request");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}/right")]
        public async Task<ActionResult> PutRight(int id, [FromBody] DataRequest data)
        {
            try
            {
                var isVaild = await Task.Run(() => ValidateData(data));
                if (!isVaild) return BadRequest("400 Bad Request");

                var success = await _comparatorService.InsertOrUpdateRightAsync(id, data);
                if (success)
                    return Created($"~v1/diff/{id}/right", "201 Created");
                
                return BadRequest("400 Bad Request");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool ValidateData(DataRequest data)
        {
            try
            {
                if (data is null || string.IsNullOrWhiteSpace(data.Data)) return false;

                var bytes = Convert.FromBase64String(data.Data);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
