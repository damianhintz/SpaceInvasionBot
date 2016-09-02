using System;
using System.Text;
using System.Text.RegularExpressions;
//using System.Speech;
//using System.Speech.Synthesis;
using System.Collections.Generic;

namespace SpaceInvasionBot
{
    /// <summary>
    /// Defender
    /// </summary>
	class Defender
	{
		public TimeSpan IdleTime;
		public DateTime LastAcctionTime;
		public bool Active = false;
		//private SpeechSynthesizer _ss = new SpeechSynthesizer();
		
        /// <summary>
        /// Sprawdza w panelu dowodzenia ruchy flot(w szczególnoœci czy ktoœ atakuje).
        /// </summary>
        public void CheckAttack()
        {
	        string html = Navigator.GoHome();
	        if(html != null)
	        {
		        List<FleetMove> moves = FleetMove.ParseEnemy(html);
		        Console.WriteLine("Fleet moves:");
		        foreach(FleetMove move in moves)
            	{
	            	Console.WriteLine("{0} {1} {2}", move.Mission, move.Start, move.End);
            	}
            	if(Regex.IsMatch(html, "Wrogie floty"))
            	{
	            	Console.WriteLine("Wrogie floty.");
	            	//_ss.Speak("Enemy fleet is approaching.");
            	}
		        if(UnderAttack(moves))
		        {
			        Console.WriteLine("You are under attack.");
					//_ss.Speak("You are under attack.");
		        }else
		        if(Spying(moves))
		        {
			        Console.WriteLine("Someone is spying you.");
					//_ss.Speak("Someone is spying you.");
		        }else
		        {
			        Console.WriteLine("There is no enemy moves.");
		        	//_ss.Speak("There is no enemy moves.");
	        	}
	        	int count = GetMessages(html);
	        	if(count > 0)
	        	{
		        	string message = string.Format("You've got {0} message{1}.", count, count > 1 ? "s" : "");
	        		Console.WriteLine(message);
	        		//_ss.Speak(message);
        		}
	        	else
	        		Console.WriteLine("There is no new message(s)");
        	}else
        		Console.WriteLine("Server error, no response.");
        }
        
        private bool UnderAttack(List<FleetMove> moves)
        {
            foreach(FleetMove move in moves)
            {
	            if(move.Mission.Contains("Atak"))
	            	return true;
            }
            return false;
        }
        
        public bool Spying(List<FleetMove> moves)
        {
	        foreach(FleetMove move in moves)
            {
	            if(move.Mission.Contains("Szpiegowanie"))
	            	return true;
            }
            return false;
        }
        
        public static int GetMessages(string html)
        {
	        //Masz <font color="#FF0000">2</font> nieprzeczytane wiadomoœci
	        string pattern = @"Masz\s+[^\>]+>(?<count>[0-9]+)</font> nieprzeczytane wiadomo.ci";
            Regex regex = new Regex(pattern);
            if(regex.IsMatch(html))
	            return int.Parse(regex.Match(html).Groups["count"].ToString());
	        else
            	return 0;
        }
        
        private void Alert()
        {
	        int f = 800;
            int m = 50;
            int s = 200;
            for (int j = 0; j < 10; j++)
            {
                if (j % 2 == 0)
                    Console.Beep(f + s, m);
                else
                    Console.Beep(f - s, m);
            }
        }
	}
}