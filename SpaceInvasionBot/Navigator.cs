using System;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace SpaceInvasionBot
{
	/// <summary>
    /// Navigator unit(tylko jeden, obiekt globalny, wszystkie jednostki powinny mieæ wspólnego nawigatora).
    /// Nawigator powinien byæ "thread safe".
    /// catch WebException
    /// Nawigator powinien pozwalaæ tylko na jedno nawigowanie w ci¹gu jednej sekundy.
    /// </summary>
	static class Navigator
	{
		public static string Sid;
		
		private static string LoginUrl    = @"http://inwazja.wp.pl/";
		private static string HomeUrl     = @"http://inwazja.wp.pl/indexInternal.es?action=internalHome";
	    private static string OrderUrl 	  = @"http://inwazja.wp.pl/indexInternal.es?action=internalFleet";
	    private static string BuildingUrl = @"http://inwazja.wp.pl/indexInternal.es?action=internalBuildings";
	    private static string ResearchUrl = @"http://inwazja.wp.pl/indexInternal.es?action=internalResearch";
	    private static string DefenceUrl  = @"http://inwazja.wp.pl/indexInternal.es?action=internalDefense";
	    private static string WeaponUrl   = @"http://inwazja.wp.pl/indexInternal.es?action=internalWeaponFactory";
	    private static string GalaxyUrl   = @"http://inwazja.wp.pl/indexInternal.es?action=internalGalaxy";
	    private static string SpionageUrl = @"http://inwazja.wp.pl/indexInternal.es?action=internalSpionageReport";
	    private static string SendUrl 	  = @"http://inwazja.wp.pl/indexInternal.es?action=internalSendFleet";
	    private static string FinishUrl	  = @"http://inwazja.wp.pl/indexInternal.es?action=internalSendFleetFinish";
	    
	    private static HttpWebRequest _request;
	    private static HttpWebResponse _response;
	    private static CookieCollection _cookies = null;
	    
	    private static object _lock = new object();
	    private static DateTime _lastGo = DateTime.Now;
	    private static int _wait = 1000;
	    
	    public static string GetAction(string url)
	    {
		    string pattern = @"=(?<action>[a-zA-Z]+)";
		    Regex regex = new Regex(pattern);
		    if(regex.IsMatch(url))
		    {
			    return regex.Match(url).Groups["action"].ToString();
		    }
		    else
		    	return "null";
	    }
	    
	    public static string FormatMessage(string url)
	    {
		    string action = GetAction(url);
		    return action;
	    }
	    
	    public static string GetLogin()
	    {
		    return LoginUrl;
	    }
	    
	    public static string GetOrder()
	    {
		    return OrderUrl + "&sid=" + Sid;
	    }
	    
	    public static string GoOrder()
	    {
		    return Go(GetOrder(), null);
	    }
	    
	    public static string GetHome()
	    {
		    return HomeUrl + "&sid=" + Sid;
	    }
	    
	    public static string GoHome()
	    {
		    return Go(GetHome(), null);
	    }
	    
	    public static string GetGalaxy()
	    {
		    return GalaxyUrl + "&sid=" + Sid;
	    }
	    
	    public static string GoGalaxy()
	    {
		    return Go(GetGalaxy(), null);
	    }
	    
	    public static string GetSpionage()
	    {
		    return SpionageUrl + "&sid=" + Sid;
	    }
	    
	    public static string GetBuilding()
	    {
		    return BuildingUrl + "&sid=" + Sid;
	    }
	    
	    public static string GoBuilding()
	    {
		    return Go(GetBuilding(), null);
	    }
	    
	    public static string GetResearch()
	    {
		    return ResearchUrl + "&sid=" + Sid;
	    }
	    
	    public static string GoResearch()
	    {
		    return Go(GetResearch(), null);
	    }
	    
	    public static string GetWeapon()
	    {
		    return WeaponUrl + "&sid=" + Sid;
	    }
	    
	    public static string GoWeapon()
	    {
		    return Go(GetWeapon(), null);
	    }
	    
	    public static string GetDefence()
	    {
		    return DefenceUrl + "&sid=" + Sid;
	    }
	    
	    public static string GoDefence()
	    {
		    return Go(GetDefence(), null);
	    }
	    
	    public static string GetSend()
	    {
		    return SendUrl + "&sid=" + Sid;
	    }
	    
	    public static string GoSend(string data)
	    {
		    return Go(GetSend(), data);
	    }
	    
	    public static string GetFinish()
	    {
		    return FinishUrl + "&sid=" + Sid;
	    }
	    
	    public static string GoFinish(string data)
	    {
		    return Go(GetFinish(), data);
	    }
	    
	    /// <summary>
	    /// Go to url or submit data.
	    /// </summary>
	    /// <param name="url"></param>
	    /// <param name="data">Je¿eli data jest rózne od null, to wysy³amy formularz na dany url</param>
	    /// <returns>Strona html lub null(b³¹d)</returns>
	    public static string Go(string url, string data)
	    {
		    string html = null;
		    try
		    {
			    lock(_lock)
			    {
			        //Request
			        _request = (HttpWebRequest)WebRequest.Create(url);
			        //_request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip"; //gzip doesn't work
					_request.CookieContainer = new CookieContainer();
					_request.AllowAutoRedirect = false;
					
					//Bez ciasteczek nie uda nam siê zalogowaæ.
					if(_cookies != null)
						foreach(Cookie cookie in _cookies)
						 	_request.CookieContainer.SetCookies(new Uri(url), cookie.ToString());
						 	//_request.CookieContainer.Add(new Uri(url), _cookies); //Add doesn't work
					
					if(data != null)
					{
						_request.Method = "POST";
						byte[] bytes = Encoding.UTF8.GetBytes(data);
			            _request.ContentType = "application/x-www-form-urlencoded";
			            _request.ContentLength = bytes.Length;
			            
			            Stream dataStream = null;
			            dataStream = _request.GetRequestStream();
			            dataStream.Write(bytes, 0, bytes.Length);
			            dataStream.Close();
			    	}
			    	//Console.WriteLine("Requesting {0} ...", url);
			    	Console.WriteLine("Requesting {0} ...", FormatMessage(url));
			    	
			    	//Mechanizm spowalniaj¹cy zapytania, tak aby by³o wysy³ane tylko jedno na sekundê.
			    	DateTime now = DateTime.Now;
			    	TimeSpan span = now - _lastGo;
			    	int wait = _wait - (int)span.TotalMilliseconds;
			    	if(wait > 0)
			    	{
				    	Console.WriteLine("Waiting {0}ms ...", wait);
				    	Thread.Sleep(wait);
			    	}
			    	
			    	_lastGo = DateTime.Now;
			    	
			    	//Response
			    	_response = _request.GetResponse() as HttpWebResponse;
			    	if(_cookies == null)
			    		_cookies = _response.Cookies;
			    	Console.WriteLine("Response: {0}({1}) - {2}", (int)_response.StatusCode, _response.StatusCode, _response.StatusDescription);
					
			        StreamReader reader = new StreamReader(_response.GetResponseStream());
			        html = reader.ReadToEnd();
			        
					reader.Close();
			        _response.Close();
			        
			        //Sprawdzamy czy wyst¹pi³y przekierowania i pod¹¿amy za nimi.
			        if(_response.StatusCode == HttpStatusCode.Redirect || _response.StatusCode == HttpStatusCode.RedirectMethod)
			        {
			            url = "http://" + _request.Headers["Host"] + "/" + _response.Headers["Location"];
			            data = null;
			            return Go(url, data);
			        }
		        }
	        }
	        catch(WebException we)
	        {
		        Console.WriteLine("weberror: " + we.ToString());
		        html = null;
	        }
	        catch(Exception)
	        {
		        html = null;
	        }
	        
	        return html;
	    }
	    
	    /// <summary>
	    /// Login at url with username and password
	    /// </summary>
	    /// <param name="instance">Indentyfikator uniwersum.</param>
	    public static string Login(string url, string username, string password, int univers)
	    {
		    string html = Go(url, null);
		    if(html == null)
		    	return null;
            string sid = GetValue(html, "sid");
            string reloadToken = GetValue(html, "reloadToken");
            string globalLogin = GetValue(html, "globalLogin");
            string login = GetValue(html, "login");
            
            //Login 1
            url = string.Format("http://inwazja.wp.pl/indexExternal.es?action=externalLogin&sid={0}", sid);
            string data = string.Format("sid={0}&reloadToken={1}&username={2}&password={3}&globalLogin={4}&login={5}",
            	sid, reloadToken, username, password, globalLogin, login);
            
            Console.WriteLine("\nLogging user {0}:", username);
            
            html = Go(url, data);
            if(html == null)
            	return null;
            if(html.Contains(username))
            {
				Regex regex = new Regex(string.Format(@"<option value=""(?<instance>[0-9]+)"">Uniwersum {0}</option>", univers));
				string instance = null;
				if(regex.IsMatch(html))
				{
					instance = regex.Match(html).Groups["instance"].ToString();
				}else
				{
					Console.WriteLine("Couldn't login. Wrong number of univers.");
					return null;
				}
	            url = string.Format("http://inwazja.wp.pl/indexExternal.es?action=externalLogin&sid={0}", sid);
	            data = string.Format("instance={0}", instance);
	            
	            Console.WriteLine("Logging at {0} univers.", univers);
	            html = Go(url, data);
	            
	            if(html != null)
	            {
		            return sid;
	            }else
	            	return null;
        	}else
        	{
        		Console.WriteLine("Logging failed.");
        		return sid;
    		}
	    }
	    
	    public static string CheckLocation(string html)
	    {
		    return null;
	    }
	    
		/// <summary>
        /// <input type="hidden" name="sid" value="3027f08b5dc77cde3b976d92a7843943" />
        /// </summary>
        public static string GetValue(string html, string name)
        {
            string pattern = string.Format(@"name=""{0}"" value=""(?<value>[^""]*)""", name);
            Regex regex = new Regex(pattern);
            string value = null;
			if(regex.IsMatch(html))
			{
				Match match = regex.Match(html);
				value = match.Groups["value"].ToString();
			}
	        return value;
        }
    }
}