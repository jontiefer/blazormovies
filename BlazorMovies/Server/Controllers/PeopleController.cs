using AutoMapper;
using BlazorMovies.Server.Helpers;
using BlazorMovies.Shared.DTOs;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovies.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;

        public PeopleController(ApplicationDbContext context, 
            IFileStorageService fileStorageService,
            IMapper mapper)
        {
            _context = context;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = _context.People.AsQueryable();
            await HttpContext.InsertPaginationParametersInResponse(queryable, paginationDTO.RecordsPerPage);

            return await _context.People.Paginate(paginationDTO).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> Get(int id)
        {
            var person = await _context.People.FirstOrDefaultAsync(x => x.Id == id);
            if (person == null) return NotFound();
            return person;
        }

        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<List<Person>>> FilterByName(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) { return new List<Person>();  }
            return await _context.People.Where(x => x.Name.Contains(searchText))
                .Take(5)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Person person)
        {
            if (!string.IsNullOrWhiteSpace(person.Picture))
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                person.Picture = await _fileStorageService.SaveFile(personPicture, person.Name.Replace(" ", "") + ".jpg", "movies/actors");
            }

            _context.Add(person);
            await _context.SaveChangesAsync();
            return person.Id;
        }

        [HttpPut]
        public async Task<ActionResult<int>> Put(Person person)
        {
            var personDB = await _context.People.FirstOrDefaultAsync(x => x.Id == person.Id);

            if (personDB == null) { return NotFound(); }

            var prevPictLink = personDB.Picture;
            personDB = _mapper.Map(person, personDB);            

            if (!string.IsNullOrWhiteSpace(person.Picture))
            {
                if (person.Picture.Substring(0, 4).ToLower() != "http")
                {
                    var personPicture = Convert.FromBase64String(person.Picture);
                    personDB.Picture = await _fileStorageService.EditFile(personPicture, person.Name.Replace(" ", "") + ".jpg", "movies/actors", prevPictLink);
                }
            }
            else if(!string.IsNullOrWhiteSpace(prevPictLink))
            {
                await _fileStorageService.DeleteFileByLink(prevPictLink);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var person = await _context.People.FirstOrDefaultAsync(x => x.Id == id);
            if (person == null) return NotFound();

            _context.Remove(person);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
