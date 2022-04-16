using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperHeroes.Entities;
using SuperHeroes.Models;

namespace SuperHeroes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        // Constructor Dependency Injection
        private readonly SuperHeroesContext _context;
        public SuperHeroController(SuperHeroesContext context)
        {
            _context = context;
            // Add sample datas to the In Memory Database
            if (_context.SuperHeroes.Count() == 0)
            {
                _context.SuperHeroes.Add(new SuperHero
                {
                    Id = 1,
                    SuperHeroName = "Spiderman",
                    FirstName = "Peter",
                    LastName = "Parker",
                    Place = "New York City"
                });
                _context.SuperHeroes.Add(new SuperHero
                {
                    Id = 2,
                    SuperHeroName = "Ironman",
                    FirstName = "Tony",
                    LastName = "Stark",
                    Place = "Konya"
                });
                _context.SuperHeroes.Add(new SuperHero
                {
                    Id = 3,
                    SuperHeroName = "Thor",
                    FirstName = "Lord of",
                    LastName = "Thunder",
                    Place = "Paris"
                });
                _context.SaveChangesAsync();
            }
        }


        // GET: SuperHeroes/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SuperHero>>> Heroes()
        {
            return await _context.SuperHeroes.ToListAsync();
        }


        // GET: SuperHeroes/id
        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHero>> Hero(int? id)
        {
            var superHero = await _context.SuperHeroes.FindAsync(id);

            if (superHero == null)
            {
                return NotFound("Hero not found!");
                // or return BadRequest("Hero not found!");
            }

            return superHero;
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<SuperHero>>> Search([FromQuery] SearcQuery searchQuery)
        {
            IQueryable<SuperHero> query = _context.SuperHeroes;

            if(!string.IsNullOrEmpty(searchQuery.SuperHeroName))
            {
                query = query.Where(sh => sh.SuperHeroName == searchQuery.SuperHeroName);
            }

            if (!string.IsNullOrEmpty(searchQuery.FirstName))
            {
                query = query.Where(sh => sh.FirstName == searchQuery.FirstName);
            }

            if (query.Count() == 0)
            {
                return NotFound("Hero not found");
            }

            return await query.ToListAsync();
        }

        // POST: SuperHeroes/Create
        [HttpPost]
        public async Task<ActionResult<SuperHero>> Create([Bind("Id,SuperHeroName,FirstName,LastName,Place,Age")] SuperHero superHero)
        {
            // ModelState.IsValid => indicates if it was possible to bind the incoming values from the request to the model correctly
            // and whether any explicitly specified validation rules were broken during the model binding process.
            if (ModelState.IsValid)
            {
                _context.Add(superHero);
                await _context.SaveChangesAsync();
                // if model is valid it calls the 'Heroes' action in the controller.
                return CreatedAtAction("Heroes", new { id = superHero.Id}, superHero);
            }
            return BadRequest(superHero);
        }


        // POST: SuperHeroes/Edit/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, [Bind("Id,SuperHeroName,FirstName,LastName,Place,Age")] SuperHero superHero)
        {
            if (id != superHero.Id)
            {
                return NotFound("Hero not found!");
            }

            if (!SuperHeroExists(superHero.Id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(superHero);
                    await _context.SaveChangesAsync();
                }
                // in case of concurrent update operation for the same hero
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest("Concurrent Update!");
                    //if (!SuperHeroExists(superHero.Id))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
            }
            return NoContent();
        }


        // POST: SuperHeroes/id
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var superHero = await _context.SuperHeroes.FindAsync(id);
            if(superHero == null)
            {
                return NotFound("Hero not found!");
                // or return BadRequest("Hero not found!");
            }

            _context.SuperHeroes.Remove(superHero);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool SuperHeroExists(int id)
        {
            return _context.SuperHeroes.Any(e => e.Id == id);
        }
    }
}
