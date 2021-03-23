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
        public IActionResult Solution(SolutionViewModel solutionModel){
            if (solutionModel.inputText!=null){
                switch (solutionModel.day)
                {
                    case 1:
                        Day1(solutionModel);
                        break;
                    
                }
            }
            return View(solutionModel);
        }
        public void Day1(SolutionViewModel solution){
            string[] lines = solution.inputText.Trim().Split("\n");
            int[] numbers = new int[lines.Length];
            try{
                for (int i = 0;i<numbers.Length;i++){
                    numbers[i] = Int32.Parse(lines[i]);
                }
            } catch {
                Console.WriteLine("Found a non number in day 1 input");
            }
            foreach(int first in numbers){
                foreach(int second in numbers){
                    if (first+second==2020){
                        solution.outputText1 = (first*second).ToString();
                    }
                    foreach(int third in numbers){
                        if(first+second+third==2020){
                            solution.outputText2 = (first*second*third).ToString();
                        }
                    }
                }
            }
        }
    }
}