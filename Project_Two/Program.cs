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
            Console.WriteLine("Welcome to the Superbowl CSV file reader.");
            Console.WriteLine("This Program will read the data from the CSV file and write it to a TXT file.\n");

            //User defines the PATH to the csv
            Console.WriteLine("Where do you want to read the csv file from?\n");
            Console.WriteLine("for example:\nC:\\Users\\Jake\\Documents\\College\\SENG Semester 2\\Advanced Programming\\Projects\\Project 2\\Project_Two\\Super_Bowl_Project.csv\n");
            Console.WriteLine("Input the PATH to the CSV File:");
            csvPATH = @Console.ReadLine();
            Console.Clear();

            //User defines the PATH to the txt
            Console.WriteLine("Where do you want to read the csv file from?\n");
            Console.WriteLine("for example:\nC:\\Users\\Jake\\Documents\\College\\SENG Semester 2\\Advanced Programming\\Projects\\Project 2\\Project_Two\\Super_Bowl_Project.txt\n");
            Console.WriteLine("Input the PATH to the TXT File:");
            txtPATH = @Console.ReadLine();
            Console.Clear();

            try
            {
                //declaring a file stream to write to the text file with the superbowl data
                FileStream newfile = new FileStream(txtPATH, FileMode.Create, FileAccess.Write);
                StreamWriter outfile = new StreamWriter(newfile);

                input = new FileStream(csvPATH, FileMode.Open, FileAccess.Read);
                read = new StreamReader(input);
                primingValue = read.ReadLine();
                List<SuperBowl> sbDataList = new List<SuperBowl>(); //declaring superbowl data list

                //Establishing a looping structure to read in all the superbowl data
                while (!read.EndOfStream)
                {
                    sbData = read.ReadLine().Split(','); //reads in each element between each comma and splits the comma off
                    SuperBowl sb = new SuperBowl(sbData); //each element of the list will be called sb
                    sbDataList.Add(sb); //adds a superbowl to the list
                    //Console.WriteLine(sbDataList[sbDataList.Count - 1]); //used to insure that the data is populating the list
                }

                read.Dispose();
                input.Dispose();

                //Below outputs the list of the winning teams
                Console.WriteLine("          List of Winning Teams         ");
                outfile.WriteLine("          List of Winning Teams         ");
                Console.WriteLine("---------------------------------------------\n");
                outfile.WriteLine("---------------------------------------------\n");

                //invoking the print winner function in a foreach
                foreach (SuperBowl sb in sbDataList)
                {
                    Console.WriteLine(sb.printWinner());
                }

                foreach (SuperBowl sb in sbDataList)
                {
                    outfile.WriteLine(sb.printWinner()); 
                }

                //Below outputs the list of the top attended superbowl counts
                Console.WriteLine("          Top 5 Attended Superbowls         ");
                outfile.WriteLine("          Top 5 Attended Superbowls         ");
                Console.WriteLine("---------------------------------------------\n");
                outfile.WriteLine("---------------------------------------------\n");

                var attendanceQuery = (from sb in sbDataList orderby sb.Attendance descending select sb).ToList<SuperBowl>().Take(5); //selects 5 superbowls by top attendance from the superbowl data list and pust them into an attendance query list

                //For each element in the attendance query write the date, winning team, losing team, city, state, and stadium
                //This code was influenced off of LeAnn Simonson and Sarah Fox
                attendanceQuery.ToList<SuperBowl>().ForEach(x => Console.WriteLine($"1. The date the team won: {x.Date}\n2. The winning team: {x.winningTeam}\n3. The losing team: {x.losingTeam}\n" +
                                                                                   $"4. The city: {x.City}\n5. The state: {x.State}\n6. The stadium: {x.Stadium}\n7. The attendance: {x.Attendance}\n"));

                attendanceQuery.ToList<SuperBowl>().ForEach(x => outfile.WriteLine($"1. The date the team won: {x.Date}\n2. The winning team: {x.winningTeam}\n3. The losing team: {x.losingTeam}\n" +
                                                                                   $"4. The city: {x.City}\n5. The state: {x.State}\n6. The stadium: {x.Stadium}\n7. The attedance: {x.Attendance}\n"));

                //Below outputs the state that hosted the most superbowls
                //This section and the MVP section contain adapted code originally from LeAnn Simonson
                Console.WriteLine("    State That Hosted The Most Superbowls     ");
                outfile.WriteLine("    State That Hosted The Most Superbowls     ");
                Console.WriteLine("---------------------------------------------\n");
                outfile.WriteLine("---------------------------------------------\n");

                var hostQueryCount = (from sb in sbDataList //defining a query count variable that is used to make a descending list
                                  group sb by sb.State into nestedQuery //creates a group of state data and puts it into a nested query
                                  orderby nestedQuery.Count() descending //orders the list 
                                  select nestedQuery).First().Count(); //takes the count of the first element in nestedQuery (the state that hosted the most superbowls)

                var hostCount = (from sb in sbDataList //defining a host count variable that considers each superbowl(sb) in the superbowl data list
                                group sb by sb.State into hostGroup //creates a group of state data and puts it into host group
                                where hostGroup.Count() == hostQueryCount //adds a condition to the list where the only element it contains can be equal to the most hosted count
                                select hostGroup).Take(1); //takes the one element in the list

                foreach (var sb in hostCount) //iterates through the list (even though it's just one element)
                {
                Console.WriteLine($"{sb.Key} hosted the most superbowls. They hosted {sb.Count()} superbowls.\n"); //writes the state and count to the terminal
                outfile.WriteLine($"{sb.Key} hosted the most superbowls. They hosted {sb.Count()} superbowls.\n"); //writes the state and count to the file
                    foreach (var info in sb) //iterates through the info attached to the sb element and outputs the city, state, stadium, and date
                    {
                        Console.WriteLine($"1. City: {info.City}"); //using methods to pull data from a superbowl
                        outfile.WriteLine($"1. City: {info.City}");
                        Console.WriteLine($"2. State: {info.State}");
                        outfile.WriteLine($"2. State: {info.State}");
                        Console.WriteLine($"3. Stadium: {info.Stadium}");
                        outfile.WriteLine($"3. Stadium: {info.Stadium}");
                        Console.WriteLine($"4. Date: {info.Date}\n");
                        outfile.WriteLine($"4. Date: {info.Date}\n");
                    }
                }

                //Below outputs players that won MVP for than once as well as their team and the team they beat
                Console.WriteLine("List of Players That Won Mvp More Than 1 Time");
                outfile.WriteLine("List of Players That Won Mvp More Than 1 Time");
                Console.WriteLine("---------------------------------------------\n");
                outfile.WriteLine("---------------------------------------------\n");

                var MVPCount = from sb in sbDataList //defining an MVPCount variable that considers each superbowl(sb) in the superbowl data list
                               group sb by sb.MVP into MVPGroup //creates a group of MVP data to be counted
                               where MVPGroup.Count() > 1 //only adds the MVP to the group if the player was MVP more than once
                               orderby MVPGroup.Key //orders the list 
                               select MVPGroup; //returns the ordered list of MVPs in a list format

                foreach (var sb in MVPCount)
                {
                    Console.WriteLine($"{sb.Key} won MVP {sb.Count()} times\n"); //writes the MVP and amount of times they were MVP
                    outfile.WriteLine($"{sb.Key} won MVP {sb.Count()} times\n");
                    foreach (var info in sb) //iterates through the info attached to the sb element(MVP) and outputs the winning team, team they beat, and the date the superbowl took place.
                    {
                        Console.WriteLine($"1. Their winning team: {info.winningTeam}"); //using methods to pull data from a superbowl
                        outfile.WriteLine($"1. Their winning team: {info.winningTeam}");
                        Console.WriteLine($"2. The team they beat: {info.losingTeam}");
                        outfile.WriteLine($"2. The team they beat: {info.losingTeam}");
                        Console.WriteLine($"3. Superbowl took place: {info.Date}\n");
                        outfile.WriteLine($"3. Superbowl took place: {info.Date}\n");
                    }
                    Console.WriteLine("\n");
                    outfile.WriteLine("\n");
                }

                //Below outputs the coach that lost the most superbowls
                Console.WriteLine("     Coach That Lost The Most Superbowls  ");
                outfile.WriteLine("     Coach That Lost The Most Superbowls  ");
                Console.WriteLine("---------------------------------------------\n");
                outfile.WriteLine("---------------------------------------------\n");

                //Defining a query count variable that is used to make a descending list to determine highest loss count.
                //It creates a group of losing coach data and puts it into a nested query, orders the list, and takes the 
                //count of the first element in nestedQuery (the coach that lost the most superbowls).
                var highestLossCount = (from sb in sbDataList group sb by sb.losingCoach into nestedQuery orderby nestedQuery.Count() descending select nestedQuery).First().Count();

                //defining a losing coach query that is designed to take the coach that lost the most by using a condition that
                //was found in the nested query. From there, the coaches that lost the most are selected and added to an array
                var losingCoachQuery = (from sb in sbDataList group sb by sb.losingCoach into losingCoachGroup where losingCoachGroup.Count() == highestLossCount select losingCoachGroup.Key).ToArray();

                for (var x = 0; x < losingCoachQuery.Length; x++) 
                {
                    Console.WriteLine($"- {losingCoachQuery[x]} lost the most superbowls\n");
                    outfile.WriteLine($"- {losingCoachQuery[x]} lost the most superbowls\n");
                }
                Console.WriteLine("\n");
                outfile.WriteLine("\n");

                //Below outputs the coach that won the most superbowls
                //////
                Console.WriteLine("      Coach That Won The Most Superbowls  ");
                outfile.WriteLine("      Coach That Won The Most Superbowls  ");
                Console.WriteLine("---------------------------------------------\n");
                outfile.WriteLine("---------------------------------------------\n");

                //The same process as the most losses coach section is used for this query manipulation.
                var highestWinCount = (from sb in sbDataList group sb by sb.winningCoach into nestedQuery orderby nestedQuery.Count() descending select nestedQuery).First().Count();

                var winningCoachQuery = (from sb in sbDataList group sb by sb.winningCoach into winningCoachGroup where winningCoachGroup.Count() == highestWinCount select winningCoachGroup.Key).ToArray();

                for (var x = 0; x < winningCoachQuery.Length; x++) 
                {
                    Console.WriteLine($"- {winningCoachQuery[x]} won the most superbowls\n");
                    outfile.WriteLine($"- {winningCoachQuery[x]} won the most superbowls\n");
                }
                Console.WriteLine("\n");
                outfile.WriteLine("\n");

                //Below outputs the team that won the most superbowls
                //////
                Console.WriteLine("     Team That Won The Most Superbowls  ");
                outfile.WriteLine("     Team That Won The Most Superbowls  ");
                Console.WriteLine("---------------------------------------------\n");
                outfile.WriteLine("---------------------------------------------\n");

                //The same process as the most losses coach section is used for this query manipulation.
                var teamHighestWinCount = (from sb in sbDataList group sb by sb.winningTeam into nestedQuery orderby nestedQuery.Count() descending select nestedQuery).First().Count();

                var winningTeamQuery = (from sb in sbDataList group sb by sb.winningTeam into winningTeamGroup where winningTeamGroup.Count() == teamHighestWinCount select winningTeamGroup.Key).ToArray();

                for (var x = 0; x < winningTeamQuery.Length; x++)
                {
                    Console.WriteLine($"- {winningTeamQuery[x]} won the most superbowls\n");
                    outfile.WriteLine($"- {winningTeamQuery[x]} won the most superbowls\n");
                }
                Console.WriteLine("\n");
                outfile.WriteLine("\n");

                //Below outputs the team that lost the most superbowls
                //////
                Console.WriteLine("     Team That Lost The Most Superbowls  ");
                outfile.WriteLine("     Team That Lost The Most Superbowls  ");
                Console.WriteLine("---------------------------------------------\n");
                outfile.WriteLine("---------------------------------------------\n");

                //The same process as the most losses coach section is used for this query manipulation.
                var teamHighestLossCount = (from sb in sbDataList group sb by sb.losingTeam into nestedQuery orderby nestedQuery.Count() descending select nestedQuery).First().Count();

                var losingTeamQuery = (from sb in sbDataList group sb by sb.losingTeam into losingTeamGroup where losingTeamGroup.Count() == teamHighestLossCount select losingTeamGroup.Key).ToArray();

                for (var x = 0; x < losingTeamQuery.Length; x++)
                {
                    Console.WriteLine($"- {losingTeamQuery[x]} lost the most superbowls\n");
                    outfile.WriteLine($"- {losingTeamQuery[x]} lost the most superbowls\n");
                }
                Console.WriteLine("\n");
                outfile.WriteLine("\n");

                //End message and file closing
                outfile.Close(); //closes the outfile (this fixes data stream issues as well)
                Console.WriteLine("The data displayed above has been written to the TXT file.");
            } //end of try

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
            public int pointDifference() //simple point difference function
            {
                return (winningPoints - losingPoints);
            }
            public string printWinner() //modularation of the print winners section
            {
                return String.Format($"1. Winning Team: {winningTeam}\n2. Date: {Date}\n" +
                    $"3. Winning Quarterback: {winningQB}\n4. Winning Coach: {winningCoach}\n" +
                    $"5. MVP: {MVP}\n6. Point Difference: {winningPoints - losingPoints}\n");
                
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
 