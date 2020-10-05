using Fluctuface.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fluctuface.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FluctuantVariablesController : ControllerBase
    {
        internal static FluctuantServer server;
        readonly FluctuantContext _context;

        public FluctuantVariablesController(FluctuantContext context)
        {
            _context = context;

            AddNew();
            RemoveOld();
            _context.SaveChanges();
        }

        // GET: api/FluctuantVariables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FluctuantVariable>>> GetFluctuantVariables()
        {
            return await _context.FluctuantVariables.ToListAsync();
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
                await _context.SaveChangesAsync().ContinueWith(task =>
                {
                    server.SendUpdateToPatron(fluctuantVariable);
                    return task;
                });
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

        void AddNew()
        {
            foreach (var fluct in server.flucts)
            {
                if (!FluctuantVariableExists(fluct.Id))
                {
                    _context.Add(fluct);
                }
            }
        }

        void RemoveOld()
        {
            var newIds = new HashSet<string>(server.flucts.Select(f => f.Id));
            var deletedIds = new HashSet<string>(_context.FluctuantVariables.Select(v => v.Id));

            deletedIds.ExceptWith(newIds);

            foreach (var id in deletedIds)
            {
                var fluct = _context.FluctuantVariables.FirstOrDefault(f => f.Id == id);

                if (fluct != null)
                {
                    _context.Remove(fluct);
                }
            }
        }

        bool FluctuantVariableExists(string id)
        {
            return _context.FluctuantVariables.Any(e => e.Id == id);
        }
    }
}
