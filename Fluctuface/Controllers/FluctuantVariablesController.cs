﻿using System;
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
            var server = new Server();
            var flucts = server.Start();

            foreach (var fluct in flucts)
            {
                _context.Add(fluct);
            }
            _context.SaveChanges();
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

        private bool FluctuantVariableExists(string id)
        {
            return _context.FluctuantVariables.Any(e => e.Id == id);
        }
    }
}