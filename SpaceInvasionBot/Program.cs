using System;
using System.Threading;
using SpaceInvasionBot.DAL;

namespace SpaceInvasionBot
{
    /* Cycle
    1. Scan
    2. Spy
    3. Harvest/Attack
    4. Defend
    5. Build/Research
    */
    class Program
    {
        private static Commander _commander;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            int uni = 2;
            if (args.Length > 0)
                uni = int.Parse(args[0]);

            string sid = null;
            string host = @"http://www.inwazja.wp.pl/", user = "hdmn749", pass = "2008";
            if ((sid = Navigator.Login(host, user, pass, uni)) != null)
            {
                Navigator.Sid = sid;

                _commander = new Commander();
                _commander.Start();

                Thread.Sleep(Timeout.Infinite);
                Spy.SearchPlanets();
            }
            else
                Console.WriteLine("login err");
            Console.Read();
        }

        private static void BuildDB()
        {
            UserRecord u = new UserRecord();
            u.Drop(); u.Create();
            AllianceRecord a = new AllianceRecord();
            a.Drop(); a.Create();
            PlanetRecord p = new PlanetRecord();
            p.Drop(); p.Create();
            ReportRecord r = new ReportRecord();
            r.Drop(); r.Create();
            ResourceRecord res = new ResourceRecord();
            res.Drop(); res.Create();
            BuildingRecord b = new BuildingRecord();
            b.Drop(); b.Create();
            ResearchRecord re = new ResearchRecord();
            re.Drop(); re.Create();
            FleetRecord f = new FleetRecord();
            f.Drop(); f.Create();
            DefenceRecord d = new DefenceRecord();
            d.Drop(); d.Create();
        }
    }
}
