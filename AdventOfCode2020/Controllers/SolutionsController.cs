using System;
using System.Collections;
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
                    case 5:
                        Day5(solutionModel);
                        break;
                    case 6:
                        Day6(solutionModel);
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
            public string[] rawValues = new string[keys.Length];
            public void SetTrue(string key){
                int index = GetIndexOf(key);
                values[index] = true;
            }
            public void CheckInput(string key, string input){
                bool valid = false;
                try{
                    if (key=="byr"){
                        int year = Int32.Parse(input);
                        valid = (year>=1920)&(year<=2002);
                    }
                    else if (key=="iyr"){
                        int year = Int32.Parse(input);
                        valid = (year>=2010)&(year<=2020);
                    }
                    else if (key=="eyr"){
                        int year = Int32.Parse(input);
                        valid = (year>=2020)&(year<=2030);
                    }
                    else if (key=="hgt"){
                        Regex r = new Regex(@"(?<height>\d\d\d)(?<unit>cm)|(?<height>\d\d)(?<unit>in)");
                        Match m = r.Match(input);
                        int height = Int32.Parse(m.Groups["height"].Value);
                        if(m.Groups["unit"].Value=="cm"){
                            valid = (height>=150)&(height<=193);
                        }
                        else if (m.Groups["unit"].Value == "in"){
                            valid = (height>=59)&(height<=76);
                        }
                    }
                    else if (key=="hcl"){
                        Regex r = new Regex(@"^#[a-f,0-9]{6}$");
                        Match m = r.Match(input);
                        valid = m.Length>1;
                    }
                    else if (key=="ecl"){
                        string[] colors = new string[]{"amb","blu","brn","gry","grn","hzl","oth"};
                        foreach(string color in colors){
                            if (input == color){
                                valid = true;
                            }
                        }
                    }
                    else if (key=="pid"){
                        Regex r = new Regex(@"^\d{9}$");
                        Match m = r.Match(input);
                        valid = m.Length>1;
                    }
                    else if (key=="cid"){
                        valid = true;
                    }
                } catch {
                    return;
                }
                if (valid){
                    SetTrue(key);
                    rawValues[GetIndexOf(key)] = input;
                }
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
            public bool IsValid(){
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
            List<Passport> passports1 = new List<Passport>();
            List<Passport> passports2 = new List<Passport>();
            passports1.Add(new Passport());
            passports2.Add(new Passport());
            int passportIndex = 0;
            foreach(string line in lines){
                if(line.Trim() == ""){
                    passportIndex++;
                    passports1.Add(new Passport());
                    passports2.Add(new Passport());
                }
                MatchCollection matches = r.Matches(line);
                foreach(Match m in matches){
                    string key = m.Groups["key"].Value;
                    foreach(string s in Passport.keys){
                        if (key==s){
                            passports1[passportIndex].SetTrue(key);
                        }
                    }
                    passports2[passportIndex].CheckInput(key, m.Groups["value"].Value);
                }
            }
            int correctPassports = 0;
            foreach (Passport p in passports1){
                if(p.IsValid()){
                    correctPassports++;
                }
            }
            solution.outputText1 = correctPassports.ToString();
            correctPassports = 0;
            foreach (Passport p in passports2){
                if(p.IsValid()){
                    for (int i = 0; i < Passport.keys.Length; i++)
                    {
                        Console.WriteLine(Passport.keys[i]+":"+p.rawValues[i]);
                    }
                    Console.WriteLine();
                    correctPassports++;
                }
            }
            solution.outputText2 = correctPassports.ToString();
        }
        
        public int BinaryOp(int min, int max, string direction){
            if (min==max){
                return max;
            }
            if (direction.Length==0){
                Console.WriteLine("Reached end of instructions, can't complete BinaryOp|"+min+","+max);
                return -1;
            }
            if (direction[0]=='f'||direction[0]=='l'){
                return BinaryOp(min,max-((max-min)/2)-1,direction.Substring(1));
            }
            else if (direction[0]=='b'||direction[0]=='r'){
                return BinaryOp(max-((max-min)/2), max, direction.Substring(1));
            }
            return -1;
        }
        public void Day5(SolutionViewModel solution){
            List<int> seatIDs = new List<int>();
            string[] lines = solution.GetLines();

            foreach (string line in lines){
                int row = BinaryOp(0,127,line.Substring(0,7).ToLower());
                int seat = BinaryOp(0,7,line.Substring(7).ToLower());
                seatIDs.Add((row*8)+seat);
            }
            seatIDs.Sort();
            solution.outputText1 = seatIDs.Last<int>().ToString();
            int prev = seatIDs.First<int>();
            seatIDs.RemoveAt(0);
            foreach(int next in seatIDs){
                int guess = prev+1;
                if (guess != next){
                    solution.outputText2 = guess.ToString();
                }
                prev = next;
                
            }
            
        }

        public void Day6(SolutionViewModel solution){
            string[] lines = solution.GetLines();
            Dictionary<char, bool> answerDict = new Dictionary<char, bool>();
            List<Dictionary<char,bool>> groupList = new List<Dictionary<char, bool>>();
            int groupIndex = 0;
            groupList.Add(new Dictionary<char, bool>());
            foreach(string line in lines){
                if (line.Trim() == ""){
                    groupIndex++;
                    groupList.Add(new Dictionary<char, bool>());
                }
                else{
                    
                    foreach (char c in line.Trim()){
                        groupList[groupIndex][c] = true;
                    }
                }
            }
            int sum = 0;
            foreach(Dictionary<char,bool> dict in groupList){
                sum += dict.Count;
            }
            solution.outputText1 = (sum).ToString();
        }
    }
}