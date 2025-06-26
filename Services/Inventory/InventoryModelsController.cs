using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Data;

namespace Inventory
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryModelsController : ControllerBase
    {
        private readonly InventoryContext _context;

        public InventoryModelsController(InventoryContext context)
        {
            _context = context;
        }

        // GET: api/InventoryModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryModel>>> GetInventoryModel()
        {
            return await _context.InventoryModel.ToListAsync();
        }

        // GET: api/InventoryModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryModel>> GetInventoryModel(int id)
        {
            var inventoryModel = await _context.InventoryModel.FindAsync(id);

            if (inventoryModel == null)
            {
                return NotFound();
            }

            return inventoryModel;
        }

        // PUT: api/InventoryModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventoryModel(int id, InventoryModel inventoryModel)
        {
            if (id != inventoryModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(inventoryModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryModelExists(id))
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

        // POST: api/InventoryModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InventoryModel>> PostInventoryModel(InventoryModel inventoryModel)
        {
            _context.InventoryModel.Add(inventoryModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInventoryModel", new { id = inventoryModel.Id }, inventoryModel);
        }

        // DELETE: api/InventoryModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryModel(int id)
        {
            var inventoryModel = await _context.InventoryModel.FindAsync(id);
            if (inventoryModel == null)
            {
                return NotFound();
            }

            _context.InventoryModel.Remove(inventoryModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InventoryModelExists(int id)
        {
            return _context.InventoryModel.Any(e => e.Id == id);
        }
    }
}
