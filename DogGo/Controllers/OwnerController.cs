﻿using System.Collections.Generic;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
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
            List<Owner> owners = _ownerRepo.GetAllOwners();

            return View(owners);
        }

        // GET: OwnersController/Details/5
        public ActionResult Details(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);
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

        public ActionResult ViewWalks(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);        
            List<Walk> walks = _walksRepo.GetWalksByOwnerId(id);

            WalkViewModel vm = new WalkViewModel()
            {
                Walks = walks,
                Owner = owner
            };
            return View(vm);
        }

        public ActionResult RequestAWalk(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);
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
        public ActionResult Edit(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);

            if (owner == null)
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
        public ActionResult Delete(int id)
        {
            Owner ownerToDelete = _ownerRepo.GetOwnerById(id);
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
        public ActionResult DeleteWalk(int id, WalkViewModel vm)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);
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
    }
}
