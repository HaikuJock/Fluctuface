using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fluctuface.Models;

namespace Fluctuface.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FluctuantVariablesController : ControllerBase
    {
        private readonly FluctuantContext _context;

        public FluctuantVariablesController(FluctuantContext context)
        {
            _context = context;
        }

        // GET: api/FluctuantVariables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FluctuantVariable>>> GetFluctuantVariables()
        {
            return await _context.FluctuantVariables.ToListAsync();
        }

        // GET: api/FluctuantVariables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FluctuantVariable>> GetFluctuantVariable(string id)
        {
            var fluctuantVariable = await _context.FluctuantVariables.FindAsync(id);

            if (fluctuantVariable == null)
            {
                return NotFound();
            }

            return fluctuantVariable;
        }

        // PUT: api/FluctuantVariables/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFluctuantVariable(string id, FluctuantVariable fluctuantVariable)
        {
            if (id != fluctuantVariable.Id)
            {
                return BadRequest();
            }

            _context.Entry(fluctuantVariable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FluctuantVariableExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FluctuantVariables
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<FluctuantVariable>> PostFluctuantVariable(FluctuantVariable fluctuantVariable)
        {
            _context.FluctuantVariables.Add(fluctuantVariable);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FluctuantVariableExists(fluctuantVariable.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFluctuantVariable", new { id = fluctuantVariable.Id }, fluctuantVariable);
        }

        // DELETE: api/FluctuantVariables/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FluctuantVariable>> DeleteFluctuantVariable(string id)
        {
            var fluctuantVariable = await _context.FluctuantVariables.FindAsync(id);
            if (fluctuantVariable == null)
            {
                return NotFound();
            }

            _context.FluctuantVariables.Remove(fluctuantVariable);
            await _context.SaveChangesAsync();

            return fluctuantVariable;
        }

        private bool FluctuantVariableExists(string id)
        {
            return _context.FluctuantVariables.Any(e => e.Id == id);
        }
    }
}
