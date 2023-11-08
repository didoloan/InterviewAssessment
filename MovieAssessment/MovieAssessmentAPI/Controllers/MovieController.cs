using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAssessmentAPI.Models;
using MovieAssessmentAPI.Services;

namespace MovieAssessmentAPI.Controllers;

[ApiController]
[Route("movies")]
public class MovieController : ControllerBase
{
    private readonly ILogger<MovieController> _logger;
    private readonly IMovieService movieSvc;
    public MovieController(ILogger<MovieController> logger, IMovieService movieService)
    {
        _logger = logger;
        movieSvc = movieService;
    }

    [HttpGet("")]
    public async Task<IEnumerable<Movie>> GetMovies([FromQuery] MoviesReq request)
    {
        return await movieSvc.GetAsync(request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovieById(string id)
    {
        try
        {
            var movie =  await movieSvc.GetAsync(id);
            return Ok(movie);
        }
        catch (Exception ex)
        {            
            return Problem(ex.Message);
        }

    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchMovie([FromQuery] MovieSearch search)
    {
        if(!ModelState.IsValid){
            return BadRequest();
        }
        try
        {
            var movies = await movieSvc.SearchMovie(search);
            return Ok(movies);
        }
        catch (Exception ex)
        {            
            return Problem(ex.Message);
        }
    }

    [HttpPost("updatePrice/{id}")]
    public async Task<IActionResult> UpdateTicketPrice(string id, [FromBody] PriceUpdate priceUpdate)
    {
        if(!ModelState.IsValid){
            return BadRequest();
        }
        try
        {
            await movieSvc.UpdateTicketPriceAsync(id, priceUpdate.NewPrice);
            return Ok(new GenericResponse{
                Message = "Ticket Price updated successfully!"
            });
        }
        catch (Exception ex)
        {            
            return Problem(ex.Message);
        }
    }

    [HttpPost("")]
    public async Task<IActionResult> AddMovie([FromBody] Movie request) {
        if(!ModelState.IsValid){
            return BadRequest();
        }
        try
        {    
            await movieSvc.CreateAsync(request);
            return Created(Request.Path, new GenericResponse{
                Message = "Movie created successfully!"
            });
        }
        catch (Exception ex)
        {            
            return Problem(ex.Message);
        }
    }
}
