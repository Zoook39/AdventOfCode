using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AdventOfCode2020.Models;
using System.Text.RegularExpressions;

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
                    case 2:
                        Day2(solutionModel);
                        break;
                    case 3:
                        Day3(solutionModel);
                        break;
                    case 4:
                        Day4(solutionModel);
                        break;
                    
                }
            }
            return View(solutionModel);
        }
        public void Day1(SolutionViewModel solution){
            string[] lines = solution.GetLines();
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

        public void Day2(SolutionViewModel solution){
            string[] lines = solution.GetLines();
            Regex r = new Regex(@"(?<firstVal>\d+)-(?<secondVal>\d+) (?<reqChar>\D): (?<password>\D+)");
            int part1CorrectPasswords = 0, part2CorrectPasswords = 0;
            foreach(string line in lines){
                Match match = r.Match(line);
                int firstVal = Int32.Parse(match.Groups["firstVal"].Value);
                int secondVal = Int32.Parse(match.Groups["secondVal"].Value);
                string password = match.Groups["password"].Value;
                char reqChar = match.Groups["reqChar"].Value[0];
                //get count for part 1
                int part1Count = 0;
                foreach(char c in password){
                    if (c == reqChar){
                        part1Count++;
                    }
                }
                //check positions for part 2  
                if (password[(firstVal-1)]==reqChar^password[(secondVal-1)]==reqChar){
                    part2CorrectPasswords++;
                }
                //check count for part 1
                if (part1Count>=firstVal&&part1Count<=secondVal){
                    part1CorrectPasswords++;
                }
            }
            solution.outputText1=part1CorrectPasswords.ToString();
            solution.outputText2=part2CorrectPasswords.ToString();
        }
        public void Day3(SolutionViewModel solution){
            string[] lines = solution.GetLines();
            
            solution.outputText1=Day3Traverser(1,3,lines).ToString();
            solution.outputText2=(Day3Traverser(1,1,lines)*Day3Traverser(1,3,lines)*Day3Traverser(1,5,lines)*Day3Traverser(1,7,lines)*Day3Traverser(2,1,lines)).ToString();
        }

        public long Day3Traverser(int slopeX, int slopeY, string[] lines){
            int row = 0;
            int column = 0;
            long treeCount = 0;
            while (row<lines.Length){
                string currentLine = lines[row].Trim();
                char currentPos = currentLine[column%(currentLine.Length)];
                if (currentPos=='#'){
                    treeCount++;
                }
                row+=slopeX;
                column+=slopeY;
            }

            return treeCount;
        }

        class Passport{
            public static string[] keys = new string[]{"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid", "cid"};
            bool[] values = new bool[keys.Length];
            public void SetTrue(string key){
                int index = GetIndexOf(key);
                values[index] = true;
            }
            public bool GetValue(string key){
                int index = GetIndexOf(key);
                return values[index];
            }
            private int GetIndexOf(string key){
                for (int i = 0; i < keys.Length; i++)
                {
                    if (keys[i] == key){
                        return i;
                    }
                }
                return -1;
            }
            public bool IsValidPassport(){
                int index = 0;
                while(values[index]){
                    index++;
                    if (index==keys.Length){
                        return true;
                    }
                }
                return false;
            }
            public bool IsValidNPC(){
                int index = 0;
                while(values[index]){
                    index++;
                    if (index==keys.Length-1){
                        return true;
                    }
                }
                return false;
            }

        }
        public void Day4(SolutionViewModel solution){
            Regex r = new Regex(@"(?<key>[a-z]{3}):(?<value>\S+)");
            string[] lines = solution.GetLines();
            List<Passport> passports = new List<Passport>();
            passports.Add(new Passport());
            int passportIndex = 0;
            foreach(string line in lines){
                if(line.Trim() == ""){
                    passportIndex++;
                    passports.Add(new Passport());
                }
                MatchCollection matches = r.Matches(line);
                foreach(Match m in matches){
                    string key = m.Groups["key"].Value;
                    foreach(string s in Passport.keys){
                        if (key==s){
                            passports[passportIndex].SetTrue(key);
                        }
                    }
                }
            }
            int correctPassports = 0;
            foreach (Passport p in passports){
                if(p.IsValidNPC()){
                    correctPassports++;
                }
            }
            solution.outputText1 = correctPassports.ToString();
        }
    }
}