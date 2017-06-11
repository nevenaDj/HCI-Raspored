using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using Raspored.Tabele;
using Raspored.DDrop;

namespace Raspored
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public class JavaScriptControlHelper
    {
        MainWindow prozor;
        Tabele.Tabele prozorTabele;
        IzborSmera prozorIzborSmera;
        SoftveriOtvori prozorSoftveri;
        PravljenjeRasporeda prozorPravljenjeRasporeda;
        PrikaziPredmet prozorZaPrikazPredmeta;

        public JavaScriptControlHelper(MainWindow w)
        {
            prozor = w;
        }

        public JavaScriptControlHelper(Tabele.Tabele w)
        {
            prozorTabele = w;
        }

        public JavaScriptControlHelper(IzborSmera w)
        {
            prozorIzborSmera = w;
        }

        public JavaScriptControlHelper(SoftveriOtvori w)
        {
            prozorSoftveri = w;
        }
        public JavaScriptControlHelper(PravljenjeRasporeda w)
        {
            prozorPravljenjeRasporeda = w;
        }
        public JavaScriptControlHelper(PrikaziPredmet w)
        {
            prozorZaPrikazPredmeta = w;
        }

        public void RunFromJavascript(string param)
        {
            prozor.doThings(param);
        }


    }
}
