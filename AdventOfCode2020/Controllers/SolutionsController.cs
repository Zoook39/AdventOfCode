using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AdventOfCode2020.Models;

namespace AdventOfCode2020.Controllers
{
    public class SolutionsController : Controller
    {
        private readonly ILogger<SolutionsController> _logger;

        public SolutionsController(ILogger<SolutionsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Solution(int day){
            SolutionViewModel solutionModel = new SolutionViewModel();
            solutionModel.day = day;
            return View(solutionModel);
        }
      
    }
}