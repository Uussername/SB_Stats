using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SB_Stats
{
    class Program
    {
        static void Main()
        {
            //DECLARATIONS
            List<Statistics> Rows = new List<Statistics>();
            string[] values;
            Statistics SuperBowl;
            string PATH = @"C:\Users\thoantj\OneDrive - dunwoody.edu\Advanced Programing\Projects\SB_Stats\Super_Bowl_Project.csv";
            string WRITE_PATH = @"C:\Users\thoantj\OneDrive - dunwoody.edu\Advanced Programing\Projects\SB_Stats\stats.txt";
            Console.WriteLine("Please type in the File Path");
            //open file stream
            FileStream file = new FileStream(PATH, FileMode.Open, FileAccess.Read);
            //open stream reader
            StreamReader read = new StreamReader(file);
            read.ReadLine(); // remove the worthless heaer
            while (!read.EndOfStream)
            {
                values = read.ReadLine().Split(',');
                SuperBowl = new Statistics(values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7],
                    values[8], values[9], values[10], values[11], values[12], values[13], values[14]);
                Rows.Add(SuperBowl);
            }
            read.Close(); //close read stream
            using (StreamWriter sw = new StreamWriter(WRITE_PATH))
            {

                SB_Winners(ref Rows, sw);
                sw.WriteLine();
                Attending(ref Rows, sw);
                sw.WriteLine();
                Hosts(ref Rows, sw);
                sw.WriteLine();
                Mveepe(ref Rows, sw);
                sw.WriteLine();
                Misc_Stats(ref Rows, sw);
                
            }
            file.Close(); //close file stream
            Console.ReadKey();
            return;
        }
        //provides list of all superbowl winners, the year, winning qb and coach, mvp, and score difference
        public static void SB_Winners(ref List<Statistics> Rows, StreamWriter sw)
        {
            sw.WriteLine("All SuperBowl Winners");
            sw.Write("{0, -25}", "Winning Team");
            sw.Write("{0, -6}", "Year");
            sw.Write("{0, -30}", "Winning QB");
            sw.Write("{0, -25}", "Winning Coach");
            sw.Write("{0, -30}", "MVP");
            sw.Write("{0, -10}", "Score Diff");
            sw.WriteLine("\n");
            for (var x = 0; x < Rows.Count; x++)
            {
                Statistics.Winning(Rows[x], sw);
            }
            return;
        }
        // displays results of the Find_Most_x methods
        public static void Misc_Stats(ref List<Statistics> Rows, StreamWriter sw)
        {
            sw.WriteLine($"{Find_Most_WC(Rows)} is the coach that has won the most SuperBowls");
            sw.WriteLine($"{Find_Most_LC(Rows)} is the coach that has lost the most SuperBowls");
            sw.WriteLine($"{Find_Most_WT(Rows)} is the team that has won the most SuperBowls");
            sw.WriteLine($"{Find_Most_LT(Rows)} is the team that has lost the most SuperBowls");
            return;
        }
        // finds 2 or more time mvps and displays their name, the winning and losing team of that superbowl
        public static void Mveepe(ref List<Statistics> Rows, StreamWriter sw)
        {
            var top_mvps = (from row in Rows
                       group row by row.MVP into grp
                       where grp.Count() >= 2
                       select grp.First()).ToList();
            sw.WriteLine("Two time SuperBowl MVPs");
            sw.Write("{0, -25}", "The MVP");
            sw.Write("{0, -25}", "Winning Team");
            sw.Write("{0, -25}", "Losing Team");
            sw.WriteLine("\n");

            for (var x = 0; x < top_mvps.Count(); x++)
            {
                sw.Write("{0,-25}", top_mvps[x].MVP);
                sw.Write("{0,-25}", top_mvps[x].Winner);
                sw.Write("{0,-25}", top_mvps[x].Loser);
                sw.WriteLine();
            }
            return;
        }
        // finds the top 5 attended superbowls and displays facts about them
        public static void Attending(ref List<Statistics> Rows, StreamWriter sw)
        {
            var top_attend = (from row in Rows
                            orderby row.Attendance descending
                            select row).Take(5).ToList();
            sw.WriteLine("Top 5 Attended SuperBowls");
            sw.Write("{0, -6}", "Year");
            sw.Write("{0, -25}", "Winning Team");
            sw.Write("{0, -25}", "Losing Team");
            sw.Write("{0, -25}", "City");
            sw.Write("{0, -25}", "State");
            sw.Write("{0, -25}", "Stadium");
            sw.WriteLine("\n");
            for (var x = 0; x < top_attend.Count(); x++)
            {
                sw.Write("{0,-6}", top_attend[x].Dates);
                sw.Write("{0,-25}", top_attend[x].Winner);
                sw.Write("{0,-25}", top_attend[x].Loser);
                sw.Write("{0,-25}", top_attend[x].City);
                sw.Write("{0,-25}", top_attend[x].State);
                sw.Write("{0,-25}", top_attend[x].Stadium);
                sw.WriteLine();
            }
            return;
        }
        // find the state that hosted the most superbowls and shows city state and stadium
        public static void Hosts(ref List<Statistics> Rows, StreamWriter sw)
        {
            var most = (from row in Rows
                        group row by row.State into grp
                        orderby grp.Count() descending
                        select grp.First()).ToList();
            sw.WriteLine($"{most[0].City}, {most[0].State} has hosted the most SuperBowls, in the {most[0].Stadium} Stadium");
        }
        // finds the coach that won the most
        public static string Find_Most_WC(List<Statistics> data)
        {
            var most = (from row in data
                        group row by row.WinningCoach into grp
                        orderby grp.Count() descending
                        select grp.First()).ToList();
            return most[0].WinningCoach;
        }
        // finds the coach that lost the most
        public static string Find_Most_LC(List<Statistics> data)
        {
                var most = (from row in data
                            group row by row.LosingCoach into grp
                            orderby grp.Count() descending
                            select grp.First()).ToList();
                return most[0].LosingCoach;
        }
        // finds the team that won the most
        public static string Find_Most_WT(List<Statistics> data)
        {
                var most = (from row in data
                            group row by row.Winner into grp
                            orderby grp.Count() descending
                            select grp.First()).ToList();
                return most[0].Winner;
            }
        // finds the team that lost the most
        public static string Find_Most_LT(List<Statistics> data)
        {
                var most = (from row in data
                            group row by row.Loser into grp
                            orderby grp.Count() descending
                            select grp).First().ToList();
                return most[0].Loser;
        }
    }      
}
    class Statistics
    {
        public string Dates { get; set; }
        public string SBNum { get; set; }
        public int Attendance { get; set; }
        public string WinningQB { get; set; }
        public string WinningCoach { get; set; }
        public string LosingCoach { get; set; }
        public string Loser { get; set; }
        public string Winner { get; set; }
        public int WinningPts { get; set; }
        public string LosingQB { get; set; }
        public int LosingPts { get; set; }
        public string MVP { get; set; }
        public string Stadium { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public Statistics(string Dates, string SBNum, string Attendance, string WinningQB, string WinningCoach, string Winner, string WinningPts, string LosingQB,
            string LosingCoach, string Loser, string LosingPts, string MVP, string Stadium, string City, string State)
        {
            this.Dates = Dates.Substring(Dates.Length - 2);
            this.SBNum = SBNum;
            this.Attendance = int.Parse(Attendance);
            this.WinningQB = WinningQB;
            this.WinningCoach = WinningCoach;
            this.Winner = Winner;
            this.WinningPts = int.Parse(WinningPts);
            this.LosingQB = LosingQB;
            this.LosingCoach = LosingCoach;
            this.Loser = Loser;
            this.LosingPts = int.Parse(LosingPts);
            this.MVP = MVP;
            this.Stadium = Stadium;
            this.City = City;
            this.State = State;
        }
        //lists off superbowl winners, the year, winning qb and coach, mvp, and score difference within a class
        public static void Winning(Statistics row, StreamWriter sw)
        {
            sw.Write("{0, -25}", row.Winner);
            sw.Write("{0, -6}", row.Dates);
            sw.Write("{0, -30}", row.WinningQB);
            sw.Write("{0, -25}", row.WinningCoach);
            sw.Write("{0, -30}", row.MVP);
            sw.Write("{0, -10}", (row.WinningPts - row.LosingPts));
            sw.WriteLine();
            return;
        }
    }

