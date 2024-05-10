using AutoMapper;
using BLL.Interfaces;
using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Movies.ViewModels;
using NToastNotify;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace Movies.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;
        private new List<string> _allowedExtensions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;

        public MoviesController(
            IMovieRepository movieRepository,
            IGenreRepository genreRepository,
            IMapper mapper,
            IToastNotification toastNotification
            
            )
        {
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }
        public async Task<IActionResult> Index()
        {
            var Movies = await _movieRepository.GetAll();

            return View(Movies);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new MovieFormViewModel
            {
                Genres = await _genreRepository.GetAll()
            };

            return View("MovieForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = await _genreRepository.GetAll();
                return View("MovieForm", model);
            }

            var file = Request.Form.Files;
            if (!file.Any())
            {
                model.Genres = await _genreRepository.GetAll();
                ModelState.AddModelError("Poster", "Please Select Movie Poster");
                return View("MovieForm", model);

            }

            var poster = file.FirstOrDefault();

            var posterValidationResult = await HandlePosterValidation(model, poster);
            if (posterValidationResult != null)
            {
                return posterValidationResult;
            }


            using var dataStream = new MemoryStream();
            await poster.CopyToAsync(dataStream);

            var movies = new Movie
            {
                Title = model.Title,
                GenreId = model.GenreId,
                year = model.year,
                Rate = model.Rate,
                StoryLine = model.StoryLine,
                Poster = dataStream.ToArray()
            };

            _movieRepository.Add(movies);

            _toastNotification.AddSuccessToastMessage("Movie Created Successfuly");
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
                return BadRequest();

            var movie = await _movieRepository.GetEntityById(id);
            if (movie == null)
                return NotFound();

            var viewModel = new MovieFormViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                GenreId = movie.GenreId,
                Rate = movie.Rate,
                StoryLine = movie.StoryLine,
                Poster = movie.Poster,
                year = movie.year,
                Genres = await _genreRepository.GetAll()
            };
            return View("MovieForm", viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = await _genreRepository.GetAll();
                return View("MovieForm", model);
            }
            var movie = await _movieRepository.GetEntityById(model.Id);
            if (movie == null)
                return NotFound();

            var files = Request.Form.Files;

            if (files.Any())
            {
                var poster = files.FirstOrDefault();
                
                using var datastream= new MemoryStream();

                await poster.CopyToAsync(datastream);

                model.Poster = datastream.ToArray();

                var posterValidationResult = await HandlePosterValidation(model, poster);
                if (posterValidationResult != null)
                {
                    return posterValidationResult;
                }
                movie.Poster = model.Poster;
                _movieRepository.Update(movie);

            }

            movie.Title = model.Title;
            movie.GenreId = model.GenreId;
            movie.year = model.year;
            movie.Rate = model.Rate;
            movie.StoryLine = model.StoryLine;
            

            _movieRepository.Update(movie);

            _toastNotification.AddSuccessToastMessage("Movie Updated Successfuly");

            return RedirectToAction("Index");


        }
        public async Task<IActionResult> Details(int? id ,MovieFormViewModel model)
        {
            if (id == null)
                return BadRequest();

            model.Genres = await _genreRepository.GetAll();
            var movie = await _movieRepository.GetEntityById(id);
            if(movie== null)
                return NotFound();


            return View(movie);
        }

        public async Task<IActionResult> Delete(int?id)
        {
            var movie= await _movieRepository.GetEntityById(id);
            _movieRepository.Delete(movie);
            return RedirectToAction("Index");

        }

        private async Task<IActionResult> HandlePosterValidation(MovieFormViewModel model, IFormFile poster)
        {
            model.Genres = await _genreRepository.GetAll();

            if (!_allowedExtensions.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {
                ModelState.AddModelError("Poster", "Only .png and .jpg are allowed!");
                return View("MovieForm", model);
            }

            if (poster.Length > _maxAllowedPosterSize)
            {
                ModelState.AddModelError("Poster", "Poster size cannot be more than 1 MB!");
                return View("MovieForm", model);
            }

            return null; 
        }
    }

  
}