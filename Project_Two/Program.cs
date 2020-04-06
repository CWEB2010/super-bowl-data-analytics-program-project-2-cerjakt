using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Project_Two
{
    class Program
    {
        static void Main(string[] args)
        {
            //Reading from a file
            //Declaring variables
            FileStream input;
            StreamReader read;
            string primingValue;
            string[] sbData;
            string csvPATH = "";
            string txtPATH = "";

            //Program Introduction
            Console.WriteLine("Welcome to the Superbowl CSV file reader/");
            Console.WriteLine("This Program will read the data from the CSV file and write it to a new file\n");
            Console.WriteLine("Where do you want to read the csv file from?\n");
            Console.WriteLine("for example:\nC:\\Users\\Jake\\Documents\\College\\SENG Semester 2\\Advanced Programming\\Projects\\Project 2\\Project_Two\\Super_Bowl_Project.csv\n");

            //User defines the PATH to the csv
            Console.WriteLine("Input the PATH to the CSV File:");
            csvPATH = @Console.ReadLine();

            Console.WriteLine("Where do you want to read the csv file from?\n");
            Console.WriteLine("for example:\nC:\\Users\\Jake\\Documents\\College\\SENG Semester 2\\Advanced Programming\\Projects\\Project 2\\Project_Two\\Super_Bowl_Project.txt\n");

            //User defines the PATH to the txt
            Console.WriteLine("Input the PATH to the TXT File:");
            txtPATH = @Console.ReadLine();

            try
            {
                input = new FileStream(csvPATH, FileMode.Open, FileAccess.Read);
                read = new StreamReader(input);
                primingValue = read.ReadLine();
                List<SuperBowl> sbDataList = new List<SuperBowl>();

                //Establishing a looping structure to read in all the superbowl data
                while (!read.EndOfStream)
                {
                    sbData = read.ReadLine().Split(',');
                    SuperBowl sb = new SuperBowl(sbData);
                    sbDataList.Add(sb);
                    //Console.WriteLine(sbDataList[sbDataList.Count -1]); //used to insure that the data is populating the list
                }

                read.Dispose();
                input.Dispose();

                //Below outputs the list of the winning teams
                Console.WriteLine("          List of Winning Teams         ");
                Console.WriteLine("---------------------------------------------\n");

                foreach (SuperBowl sb in sbDataList)
                {
                    Console.WriteLine(sb.printWinner());
                }

                //Below outputs the list of the top attended superbowl counts
                Console.WriteLine("          Top 5 Attended Superbowls         ");
                Console.WriteLine("---------------------------------------------\n");
                var attendanceQuery = (from sb in sbDataList orderby sb.Attendance descending select sb).ToList<SuperBowl>().Take(5);
                attendanceQuery.ToList<SuperBowl>().ForEach(x => Console.WriteLine($"1. The date the team won: {x.Date}\n2. The winning team: {x.winningTeam}\n3. The losing team: {x.losingTeam}\n" +
                                                                                   $"4. The city: {x.City}\n5. The state: {x.State}\n6. The stadium: {x.Stadium}\n"));
                Console.WriteLine("\n");

                //Below outputs players that won MVP for than once as well as their team and the team they beat
                Console.WriteLine("List of Players That Won Mvp More Than 1 Time");
                Console.WriteLine("---------------------------------------------\n");

                var MVPCount = from sb in sbDataList //defining an MVPCount variable that considers each superbowl(sb) in the superbowl data list
                               group sb by sb.MVP into MVPGroup //creates a group of MVP data to be counted
                               where MVPGroup.Count() > 1 //only adds the MVP to the group if the player was MVP more than once
                               orderby MVPGroup.Key //orders the list from least amount of MVPs to most
                               select MVPGroup; //returns the ordered list of MVPs in a list format

                foreach (var sb in MVPCount)
                {
                    Console.WriteLine($"{sb.Key} won MVP {sb.Count()} times\n");
                    Console.WriteLine("             Their winning teams           ");
                    Console.WriteLine("---------------------------------------------");
                    foreach (var info in sb)
                    {
                        Console.WriteLine($"Their winning team: {info.winningTeam}"); //using methods to pull data from a superbowl
                        Console.WriteLine($"The team they beat: {info.losingTeam}");
                        Console.WriteLine($"Superbowl took place: {info.Date}\n");
                    }
                    Console.WriteLine("\n");
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }
        class SuperBowl //Making a class for the superbowl data
        {
            public string Date { get; set; }
            public string SB { get; set; }
            public int Attendance { get; set; }
            public string winningQB { get; set; }
            public string winningCoach { get; set; }
            public string winningTeam { get; set; }
            public int winningPoints { get; set; }
            public string losingQB { get; set; }
            public string losingCoach { get; set; }
            public string losingTeam { get; set; }
            public int losingPoints { get; set; }
            public string MVP { get; set; }
            public string Stadium { get; set; }
            public string City { get; set; }
            public string State { get; set; }

            //Making a constructor
            public SuperBowl(string[] sbData)
            {
                this.Date = sbData[0];
                this.SB = sbData[1];
                this.Attendance = Convert.ToInt32(sbData[2]);
                this.winningQB = sbData[3];
                this.winningCoach = sbData[4];
                this.winningTeam = sbData[5];
                this.winningPoints = Convert.ToInt32(sbData[6]);
                this.losingQB = sbData[7];
                this.losingCoach = sbData[8];
                this.losingTeam = sbData[9];
                this.losingPoints = Convert.ToInt32(sbData[10]);
                this.MVP = sbData[11];
                this.Stadium = sbData[12];
                this.City = sbData[13];
                this.State = sbData[14];
            }
            public string printWinner()
            {
                return String.Format($"1. Winning Team: {winningTeam}\n2. Date: {Date}\n" +
                    $"3. The winning quarterback: {winningQB}\n4. Their winning coach was {winningCoach}\n" +
                    $"5. The MVP: {MVP}\n6. The point difference: {winningPoints - losingPoints}\n");
                
            }

            //Defining toString
            public override string ToString()
            {
                return String.Format($"Date: {Date}, Superbowl: {SB}, Attendance: {Attendance}, Winning Quaterback: {winningQB}, Winning Coach: {winningCoach}, Winning Team: {winningTeam}, Winning Points: {winningPoints}," +
                    $" Losing Quarterback: {losingQB}, Losing Coach: {losingCoach}, Losing Team: {losingTeam}, Losing Points: {losingPoints}, MVP: {MVP}, Stadium: {Stadium}, City: {City}, State: {State}");
            }
        }       
    }
}
 