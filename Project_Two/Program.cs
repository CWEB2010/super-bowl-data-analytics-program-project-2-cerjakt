using System;
using System.IO;
using System.Collections.Generic;

namespace Project_Two
{
    class Program
    {
        static void Main(string[] args)
        {
            //Reading from a file
            //Declaring variables
            const string PATH = @"C:\Users\Jake\Documents\College\SENG Semester 2\Advanced Programming\Projects\Project_Two\Super_Bowl_Project.csv";

            FileStream input;
            StreamReader read;
            string line;
            string[] sbData;

            try
            {
                input = new FileStream(PATH, FileMode.Open, FileAccess.Read);
                read = new StreamReader(input);
                line = read.ReadLine();
                List<superBowl> sbDataList = new List<superBowl>();

                //Establishing a looping structure to read in all the superbowl data
                while (!read.EndOfStream)
                {
                    sbData = read.ReadLine().Split(',');
                    sbDataList.Add(new superBowl(sbData[0], sbData[1], Convert.ToInt32(sbData[2]), sbData[3], sbData[4], sbData[5], Convert.ToInt32(sbData[6]),
                                   sbData[7], sbData[8], sbData[9], Convert.ToInt32(sbData[10]), sbData[11], sbData[12], sbData[13], sbData[14]));
                    Console.WriteLine(sbDataList[sbDataList.Count -1]);
                }

                read.Dispose();
                input.Dispose();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        class superBowl //Making a class for the superbowl data
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
            public superBowl(string Date, string SB, int Attendance, string winningQB, string winningCoach, string winningTeam, int winningPoints,
                             string losingQB, string losingCoach, string losingTeam, int losingPoints, string MVP, string Stadium, string City, string State)
            {
                this.Date = Date;
                this.SB = SB;
                this.Attendance = Attendance;
                this.winningQB = winningQB;
                this.winningCoach = winningCoach;
                this.winningTeam = winningTeam;
                this.winningPoints = winningPoints;
                this.losingQB = losingQB;
                this.losingCoach = losingCoach;
                this.losingTeam = losingTeam;
                this.losingPoints = losingPoints;
                this.MVP = MVP;
                this.Stadium = Stadium;
                this.City = City;
                this.State = State;
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
 