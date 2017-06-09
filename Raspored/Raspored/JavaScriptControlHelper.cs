using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Runtime.InteropServices;
namespace Raspored
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public class JavaScriptControlHelper
    {
        MainWindow prozor;
        Raspored.Tabele.Tabele prozorTabele;
        public JavaScriptControlHelper(MainWindow w)
        {
            prozor = w;
        }

        public JavaScriptControlHelper(Raspored.Tabele.Tabele w)
        {
            prozorTabele = w;
        }

        public void RunFromJavascript(string param)
        {
            prozor.doThings(param);
        }
    }
}
