using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Taskas3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculationController : ControllerBase
    {
        public static bool goalReachable = false;
        public static TextWriter tsw = new StreamWriter(@"C:\Users\Boo\source\repos\Task3\storage.txt", true);
        [HttpGet]
        public bool Get([FromBody]int[] array)
        {
            ValidateRules(array);
            return goalReachable;
            
        }

        private void ValidateRules(int[] array)
        {
            bool start = true;
            while (start)
            {
                Console.WriteLine("Input the array (1)");
                Console.WriteLine("Previous arrays list (2)");
                Console.WriteLine("Exit (3)");
                string menu = Console.ReadLine();
                switch (menu)
                {
                    case "1":
                        int[] arrayInt = Input();
                        Rules(arrayInt);
                        tsw.WriteLine(goalReachable.ToString());
                        tsw.Close();
                        Console.WriteLine(goalReachable.ToString());
                        /*if (goalReachable)
                        {
                            MostEfficientPath(arrayInt);
                        }*/
                        start = false;
                        break;
                    case "2":
                        string storage = System.IO.File.ReadAllText(@"C:\Users\Boo\source\repos\Task3\storage.txt");
                        System.Console.WriteLine(storage);
                        start = false;
                        break;
                    case "3":
                    default:
                        start = false;
                        break;
                }
            }
        }

        static void Rules(int[] array)
        {
            int currentArrayPos = 0;
            int futureArrayPos;
            int currentDigit;
            int futureDigit;
            int lastArrayPos = array.Length - 1;
            while (currentArrayPos <= lastArrayPos)
            {
                currentDigit = array[currentArrayPos];
                futureArrayPos = currentArrayPos + currentDigit;
                futureArrayPos = exceedLimit(futureArrayPos, lastArrayPos);
                futureDigit = array[futureArrayPos];
                //reducing steps if the chosen amount of steps sets position on 0
                while (futureDigit == 0 & currentDigit != 0 & !isEqual(futureArrayPos, lastArrayPos))
                {
                    currentDigit--;
                    futureArrayPos = currentArrayPos + currentDigit;
                    futureArrayPos = exceedLimit(futureArrayPos, lastArrayPos);
                    futureDigit = array[futureArrayPos];
                }
                //reducing steps if the chosen amount of steps sets position to opposite numbers (3 and -3)
                while (isOppositeNumbers(currentDigit, futureDigit) & currentDigit != 0 & !isEqual(futureArrayPos, lastArrayPos))
                {
                    currentDigit++;
                    futureArrayPos = currentArrayPos + currentDigit;
                    futureArrayPos = exceedLimit(futureArrayPos, lastArrayPos);
                    futureDigit = array[futureArrayPos];
                }
                //checking if endless cycle is created after detecting opposite numbers
                if (isEqual(currentArrayPos, futureArrayPos))
                {
                    goalReachable = false;
                    break;
                }
                //checking if we are standing on zero at the moment which means game over
                if (currentDigit == 0)
                {
                    goalReachable = false;
                    break;
                }
                //checking if we are standing on the last array element which means game won
                if (isEqual(futureArrayPos, lastArrayPos))
                {
                    goalReachable = true;
                    break;
                }
                currentArrayPos = futureArrayPos;
            }
        }

        static int[] Input()
        {
            String input;
            Console.WriteLine("Input the array: ");
            input = Console.ReadLine();
            string[] arrayString = input.Split(' ');
            int[] arrayInt = Array.ConvertAll(arrayString, int.Parse);
            tsw.Write(input + " ");
            return arrayInt;
        }
        //checks for equal numbers
        static bool isEqual(int a, int b)
        {
            return a == b;
        }
        //checks if the calculated index of array is bigger than last index and sets it to last index 
        static int exceedLimit(int a, int b)
        {
            if (a > b)
                a = b;
            return a;
        }
        //checks for two opposite numbers (3 and -3)
        static bool isOppositeNumbers(int a, int b)
        {
            return (a + b) == 0;
        }
    }
}