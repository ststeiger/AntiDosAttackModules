
namespace AntiDosAttackModule
{


    public class IpBanningModule : System.Web.IHttpModule
    {

        private static System.Collections.Generic.Stack<string> _Banned = new System.Collections.Generic.Stack<string>();


        void System.Web.IHttpModule.Dispose()
        {
            // throw new System.NotImplementedException();
            // Nothing to dispose; 
        }


        void System.Web.IHttpModule.Init(System.Web.HttpApplication context)
        {
            context.BeginRequest += new System.EventHandler(OnBeginRequest);
        }


        private void OnBeginRequest(object sender, System.EventArgs e)
        {
            string ip = System.Web.HttpContext.Current.Request.UserHostAddress;

            // if(((Hashtable)httpApp.Application["BlockedIPs"]).Contains( httpApp.Request.UserHostAddress)) 
            //if (_Banned.Contains(ip))
            if (true)
            {
                // http://weblogs.asp.net/hajan/why-not-to-use-httpresponse-close-and-httpresponse-end

                // System.Web.HttpContext.Current.Response.OutputStream.Close();
                System.Web.HttpContext.Current.Response.Close();

                // https://blogs.msdn.microsoft.com/friis/2014/04/25/easily-detect-and-block-malicious-http-requests-targeting-iisasp-net-using-blackips/
                // NetshCmdAdd.Text = “netsh advfirewall firewall add rule name=BADIPS dir=in interface=any action=block remoteip=” + blocklist;

                // iptables -I INPUT -p tcp --dport 80 -i eth0 -m state --state NEW -m recent --set
                // iptables -I INPUT -p tcp --dport 80 -i eth0 -m state --state NEW -m recent --update --seconds 60 --hitcount 4 -j DROP


                // https://askubuntu.com/questions/487740/how-do-you-view-all-of-the-banned-ips-for-ubuntu-12-04-via-the-command-line
                // sudo iptables -L INPUT -v -n | less

                // https://serverfault.com/questions/273324/how-to-make-iptables-rules-expire
                // http://www.fail2ban.org/wiki/index.php/Main_Page

                // System.Web.HttpContext.Current.Response.ClearContent();
                // System.Web.HttpContext.Current.Response.ClearHeaders();
                // System.Web.HttpContext.Current.Response.SuppressContent = true;
                // System.Web.HttpContext.Current.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                // System.Web.HttpContext.Current.Response.StatusDescription = "Access Denied.";
                System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            // CheckIpAddress(ip);
        } // End Sub OnBeginRequest


    } // End Class IpBanningModule : System.Web.IHttpModule


} // End Namespace AntiDosAttackModule
