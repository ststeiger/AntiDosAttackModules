
// Crappy DOS-Attack module courtesy of Mad Kristensen
// http://madskristensen.net/post/Block-DoS-attacks-easily-in-ASPNET
namespace MadChristensensCrappyAntiDosAttackModule
{

    /// <summary>
    /// Block the response to attacking IP addresses.
    /// </summary>
    public class DosAttackModule : System.Web.IHttpModule
    {

        void System.Web.IHttpModule.Dispose()
        {
            // Nothing to dispose; 
        }

        void System.Web.IHttpModule.Init(System.Web.HttpApplication context)
        {
            context.BeginRequest += new System.EventHandler(context_BeginRequest);
        }



        private static System.Collections.Generic.Dictionary<string, short> _IpAdresses = new System.Collections.Generic.Dictionary<string, short>();
        private static System.Collections.Generic.Stack<string> _Banned = new System.Collections.Generic.Stack<string>();
        private static System.Timers.Timer _Timer = CreateTimer();
        private static System.Timers.Timer _BannedTimer = CreateBanningTimer();



        private const int BANNED_REQUESTS = 10;
        private const int REDUCTION_INTERVAL = 1000; // 1 second
        private const int RELEASE_INTERVAL = 5 * 60 * 1000; // 5 minutes


        private void context_BeginRequest(object sender, System.EventArgs e)
        {
            string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            if (_Banned.Contains(ip))
            {
                System.Web.HttpContext.Current.Response.StatusCode = 403;
                // Foo - very bad throws exceptions and aborts here
                System.Web.HttpContext.Current.Response.End();
            }

            CheckIpAddress(ip);
        }

        /// <summary>
        /// Checks the requesting IP address in the collection
        /// and bannes the IP if required.
        /// </summary>
        private static void CheckIpAddress(string ip)
        {
            if (!_IpAdresses.ContainsKey(ip))
            {
                _IpAdresses[ip] = 1;
            }
            else if (_IpAdresses[ip] == BANNED_REQUESTS)
            {
                _Banned.Push(ip);
                _IpAdresses.Remove(ip);
            }
            else
            {
                _IpAdresses[ip]++;
            }
        }





        /// <summary>
        /// Creates the timer that substract a request
        /// from the _IpAddress dictionary.
        /// </summary>
        private static System.Timers.Timer CreateTimer()
        {
            System.Timers.Timer timer = GetTimer(REDUCTION_INTERVAL);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(TimerElapsed);
            return timer;
        }

        /// <summary>
        /// Creates the timer that removes 1 banned IP address
        /// everytime the timer is elapsed.
        /// </summary>
        /// <returns></returns>
        private static System.Timers.Timer CreateBanningTimer()
        {
            System.Timers.Timer timer = GetTimer(RELEASE_INTERVAL);
            timer.Elapsed += delegate { _Banned.Pop(); };
            return timer;
        }

        /// <summary>
        /// Creates a simple timer instance and starts it.
        /// </summary>
        /// <param name="interval">The interval in milliseconds.</param>
        private static System.Timers.Timer GetTimer(int interval)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = interval;
            timer.Start();

            return timer;
        }

        /// <summary>
        /// Substracts a request from each IP address in the collection.
        /// </summary>
        private static void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (string key in _IpAdresses.Keys)
            {
                _IpAdresses[key]--;
                if (_IpAdresses[key] == 0)
                    _IpAdresses.Remove(key);
            }
        }


    }


}
