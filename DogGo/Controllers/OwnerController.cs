using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DogGo.Controllers
{
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepo;
        private readonly DogRepository _dogRepo;
        private readonly WalkerRepository _walkerRepo;
        private readonly NeighborhoodRepository _neighborhoodRepo;
        private readonly WalksRepository _walksRepo;

        // ASP.NET will give us an instance of our Owner Repository. This is called "Dependency Injection"
        public OwnerController(IConfiguration config)
        {
            _ownerRepo = new OwnerRepository(config);
            _dogRepo = new DogRepository(config);
            _walkerRepo = new WalkerRepository(config);
            _neighborhoodRepo = new NeighborhoodRepository(config);
            _walksRepo = new WalksRepository(config);
        }
        // GET: OwnersController

        public ActionResult Index()
        {
            try
            {
                int ownerId = GetCurrentUserId();
                Owner owner = _ownerRepo.GetOwnerById(ownerId);

                List<Dog> dogs = _dogRepo.GetDogsByOwnerId(owner.Id);
                List<Walker> walkers = _walkerRepo.GetWalkersInNeighborhood(owner.NeighborhoodId);


                ProfileViewModel vm = new ProfileViewModel()
                {
                    Owner = owner,
                    Dogs = dogs,
                    Walkers = walkers
                };
                return View(vm);
            }
            catch
            {
                return RedirectToAction("Create", "Owner");
            }

            
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel viewModel)
        {
            Owner owner = _ownerRepo.GetOwnerByEmail(viewModel.Email);

            if (owner == null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, owner.Id.ToString()),
        new Claim(ClaimTypes.Email, owner.Email),
        new Claim(ClaimTypes.Role, "DogOwner"),
    };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Dog");
        }

        // GET: OwnersController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            int ownerId = GetCurrentUserId();
            Owner owner = _ownerRepo.GetOwnerById(id);

            if (ownerId != owner.Id)
            {
                return NotFound();
            }
            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(owner.Id);
            List<Walker> walkers = _walkerRepo.GetWalkersInNeighborhood(owner.NeighborhoodId);


            ProfileViewModel vm = new ProfileViewModel()
            {
                Owner = owner,
                Dogs = dogs,
                Walkers = walkers
            };
            return View(vm);
        }

        [Authorize]
        public ActionResult ViewWalks(int id)
        {

            Owner owner = _ownerRepo.GetOwnerById(id);

            int ownerId = GetCurrentUserId();
            if (ownerId != owner.Id)
            {
                return NotFound();
            }
            List<Walk> walks = _walksRepo.GetWalksByOwnerId(id);

            WalkViewModel vm = new WalkViewModel()
            {
                Walks = walks,
                Owner = owner
            };
            return View(vm);
        }

        [Authorize]
        public ActionResult RequestAWalk(int id)
        {

            Owner owner = _ownerRepo.GetOwnerById(id);

            int ownerId = GetCurrentUserId();
            if (ownerId != owner.Id)
            {
                return NotFound();
            }
            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(owner.Id);
            List<Walker> walkers = _walkerRepo.GetWalkersInNeighborhood(owner.NeighborhoodId);

            WalkFormViewModel vm = new WalkFormViewModel()
            {
                Owner = owner,
                Dogs = dogs,
                WalkersInNeighborhood = walkers
            };

            return View(vm);
        }
        // POST: OwnersController/RequestWalk
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestAWalk(WalkFormViewModel vm)
        {
            try
            {
               _ownerRepo.AddWalk(vm.Walk);
                return RedirectToAction("Index", "Owner");
            }
            catch
            {
                return View(vm);
            }
        }
        // GET: OwnersController/Create
        public ActionResult Create()
        {
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAll();

            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Owner = new Owner(),
                Neighborhoods = neighborhoods
            };

            return View(vm);
        }

        // POST: OwnersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Owner owner)
        {
            try
            {
                _ownerRepo.AddOwner(owner);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(owner);
            }
        }

        // GET: OwnersController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);
            //Verify the edit is only for the current owner
            int ownerId = GetCurrentUserId();
           
            if (owner == null || ownerId != owner.Id)
            {
                return NotFound();
            }
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAll();

            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Owner = owner,
                Neighborhoods = neighborhoods
            };

            return View(vm);
           
        }

        // POST: OwnersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Owner owner)
        {
            

            try
            {
                _ownerRepo.UpdateOwner(owner);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(owner);
            }
        }

        // GET: OwnersController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            Owner ownerToDelete = _ownerRepo.GetOwnerById(id);
            int ownerId = GetCurrentUserId();
            if (ownerId != ownerToDelete.Id)
            {
                return NotFound();
            }
            return View(ownerToDelete);
        }

        // POST: OwnersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Owner owner)
        {
            try
            {
                _ownerRepo.DeleteOwner(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(owner);
            }
        }
        // GET: OwnersController/Delete/5
/*        public ActionResult DeleteWalks(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);
            List<Walk> walks = _walksRepo.GetWalksByOwnerId(id);

            WalkViewModel vm = new WalkViewModel()
            {
                Walks = walks,
                Owner = owner
            };
            return View(vm);
        }*/

        // POST: OwnersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteWalk( WalkViewModel vm)
        {
          
            try
            {
                _walksRepo.DeleteWalks(vm);
                return RedirectToAction("Index", "Owner");
            }
            catch
            {
                return View(vm);
            }
        }
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
