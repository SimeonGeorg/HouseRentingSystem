﻿using HouseRentingSystem.Attributes;
using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.Agent;
using HouseRentingSystem.Extensions;
using Microsoft.AspNetCore.Mvc;
using static HouseRentingSystem.Core.Constants.MessageConstants;

namespace HouseRentingSystem.Controllers
{

    public class AgentController : BaseController
    {
        private readonly IAgentService agentService;
        public AgentController(IAgentService _agentService)
        {
            agentService = _agentService;
        }
        [HttpGet]
        [NotAnAgent]
        public IActionResult Become()
        {
            var model = new BecomeAgentFormModel();
            return View(model);
        }
        [HttpPost]
        [NotAnAgent]
        public async Task<IActionResult> Become(BecomeAgentFormModel model)
        {
            if(await agentService.UserWithPhoneNumberExistAsync(model.PhoneNumber)) 
            {
                ModelState.AddModelError(nameof(model.PhoneNumber),PhoneExist);
            }
            if(await agentService.UserHasRentsAsync(User.Id()))
            {
                ModelState.AddModelError("Error", HasRents);
            }

            if(ModelState.IsValid == false )
            {
                return View(model);
            }

            await agentService.CreateAsync(User.Id(), model.PhoneNumber);
            return RedirectToAction(nameof(HouseController.All), "Houses");
        }
    }
}
