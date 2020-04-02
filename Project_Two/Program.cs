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
            const string PATH = @"C:\Users\Jake\Documents\College\SENG Semester 2\Advanced Programming\Projects\Project 2\Project_Two\Super_Bowl_Project.csv";

            FileStream input;
            StreamReader read;
            string line;
            string[] sbData;

            try
            {
                input = new FileStream(PATH, FileMode.Open, FileAccess.Read);
                read = new StreamReader(input);
                line = read.ReadLine();
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

                Console.WriteLine("List of Winning Teams");
                Console.WriteLine("---------------------");

                foreach (SuperBowl sb in sbDataList)
                {
                    Console.WriteLine(sb.printWinner());
                }

                Console.WriteLine("List of Players That Won Mvp More Than 1 Time");
                Console.WriteLine("---------------------------------------------");

                var MVPCount = from sb in sbDataList //defining an MVPCount variable that considers each superbowl(sb) in the superbowl data list
                               group sb by sb.MVP into MVPGroup //creates a group of MVP data to be counted
                               where MVPGroup.Count() > 1 //only adds the MVP to the group if the player was MVP more than once
                               orderby MVPGroup.Key //orders the list from least amount of MVPs to most
                               select MVPGroup; //returns the ordered list of MVPs in a list format

                foreach (var sb in MVPCount)
                {
                    Console.WriteLine($"{sb.Key} won MVP {sb.Count()} times.");
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
                return String.Format($"{winningTeam} won the Superbowl in {Date}. " +
                    $"Their quarterback was {winningQB}. Their winning coach was {winningCoach}." +
                    $"The MVP was {MVP}. The point difference was {winningPoints - losingPoints}.\n");
                
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
 