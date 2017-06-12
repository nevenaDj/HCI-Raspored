using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Raspored.Model
{
    public class CitanjeIPisanje
    {
        public CitanjeIPisanje(String fileName)
        {
            SmeroviP = new ObservableCollection<Smer>();
            List<Smer> s = otvoriSmer(fileName);
            Smerovi = new ObservableCollection<Smer>(s);

            SoftveriP = new ObservableCollection<Softver>();
            List<Softver> sf = otvoriSoftver(fileName);
            Softveri = new ObservableCollection<Softver>(sf);

            PredmetiP = new ObservableCollection<Predmet>();
            List<Predmet> p = otvoriPredmet(fileName);
            Predmeti = new ObservableCollection<Predmet>(p);

            UcioniceP = new ObservableCollection<Ucionica>();
            List<Ucionica> u = otvoriUcionicu(fileName);
            Ucionice = new ObservableCollection<Ucionica>(u);
        }
        public ObservableCollection<Predmet> Predmeti
        {
            get;
            set;
        }

        public ObservableCollection<Smer> Smerovi
        {
            get;
            set;
        }

        public ObservableCollection<Ucionica> Ucionice
        {
            get;
            set;
        }


        public ObservableCollection<Softver> Softveri
        {
            get;
            set;
        }
        public ObservableCollection<Predmet> PredmetiP
        {
            get;
            set;
        }

        public ObservableCollection<Smer> SmeroviP
        {
            get;
            set;
        }

        public ObservableCollection<Ucionica> UcioniceP
        {
            get;
            set;
        }


        public ObservableCollection<Softver> SoftveriP
        {
            get;
            set;
        }
        public Raspored Raspored
        {
            get;
            set;
        }

        public void sacuvajUcionicu()
        {
            FileStream f1 = new FileStream("../../Save/ucionica.txt", FileMode.Create);
            f1.Close();

            StreamWriter f = new StreamWriter("../../Save/ucionica.txt");
            // MessageBox.Show("123");
            foreach (Ucionica u in Ucionice)
            {
                f.Write(u.BrojRadnihMesta + "|" + u.ImaPametnaTabla + "|" + u.ImaProjektor + "|" + u.ImaTabla + "|");
                f.Write(u.Sistem);
                f.Write("|" + u.Opis + "|" + u.Oznaka + "|");
                if (u.Softveri != null)
                    if (u.Softveri.Count > 0)
                    {
                        foreach (Softver s in u.Softveri)
                        {
                            if (s != null)
                            {
                                f.Write(s.Oznaka + ",");
                            }
                        }
                    }
                f.WriteLine();
            }
            f.Close();
        }

        public void sacuvajUcionicu(ObservableCollection<Ucionica> Ucionice, Raspored _raspored)
        {
            FileStream f1 = new FileStream("../../Save/ucionica.txt", FileMode.Create);
            f1.Close();

            StreamWriter f = new StreamWriter("../../Save/ucionica.txt");
            // MessageBox.Show("123");
            foreach (Ucionica u in Ucionice)
            {
                f.Write(u.BrojRadnihMesta + "|" + u.ImaPametnaTabla + "|" + u.ImaProjektor + "|" + u.ImaTabla + "|");
                f.Write(u.Sistem);
                f.Write("|" + u.Opis + "|" + u.Oznaka + "|");
                f.Write(_raspored.File + "|");
                if (u.Softveri != null)
                    if (u.Softveri.Count > 0)
                    {
                        foreach (Softver s in u.Softveri)
                        {
                            if (s != null)
                            {
                                f.Write(s.Oznaka + ",");
                            }
                        }
                    }

                f.WriteLine();
            }
            foreach (Ucionica u in UcioniceP)
            {
                f.Write(u.BrojRadnihMesta + "|" + u.ImaPametnaTabla + "|" + u.ImaProjektor + "|" + u.ImaTabla + "|");
                f.Write(u.Sistem);
                f.Write("|" + u.Opis + "|" + u.Oznaka + "|");
                f.Write(u.File + "|");
                if (u.Softveri != null)
                    if (u.Softveri.Count > 0)
                    {
                        foreach (Softver s in u.Softveri)
                        {
                            if (s != null)
                            {
                                f.Write(s.Oznaka + ",");
                            }
                        }
                    }

                f.WriteLine();
            }
            f.Close();
        }

        public List<Ucionica> otvoriUcionicu(String FileName)
        {
            List<Ucionica> ucionice = new List<Ucionica>();
            FileStream f = new FileStream("../../Save/ucionica.txt", FileMode.OpenOrCreate);
            f.Close();

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[\r\n]{3,}", options);
            string recentText = File.ReadAllText("../../Save/ucionica.txt");

            string[] tekst = recentText.Split('\n');
            foreach (string ucionica in tekst)
            {
                Ucionica u = new Ucionica();
                if (ucionica == "" || ucionica == "\r")
                    return ucionice;
                string[] uc = ucionica.Split('|');
                u.BrojRadnihMesta = Convert.ToInt32(uc[0]);
                u.ImaTabla = Convert.ToBoolean(uc[3]);
                u.ImaPametnaTabla = Convert.ToBoolean(uc[1]);
                u.ImaProjektor = Convert.ToBoolean(uc[2]);
                u.Sistem = uc[4];
                u.Opis = uc[5];
                u.Oznaka = uc[6];
                List<Softver> softveri = new List<Softver>();
                u.File = uc[7];
                foreach (string sof in uc[8].Split(','))
                {
                    Softver s = nadjiSoftver(sof);
                    if (s != null)
                        softveri.Add(s);

                }
                // 
                u.Softveri = softveri;
                // u.Softveri = new ObservableCollection<Softver>( softveri);
                // MessageBox.Show("" + u.Softveri.Count);
                if (FileName == u.File)
                    ucionice.Add(u);
                else
                    UcioniceP.Add(u);
            }

            return ucionice;
       
        }

        public void sacuvajSmer()
        {
            StreamWriter f = new StreamWriter("../../Save/smer.txt");
            foreach (Smer s in Smerovi)
            {
                f.WriteLine(s.Oznaka + "|" + s.Skracenica + "|" + s.Opis + "|" + s.Naziv + "|" + s.DatumUvodjenja);
            }
            f.Close();
        }

        public void sacuvajSmer(ObservableCollection<Smer> Smerovi, Raspored _raspored)
        {
            StreamWriter f = new StreamWriter("../../Save/smer.txt");
            foreach (Smer s in Smerovi)
            {
                f.WriteLine(s.Oznaka + "|" + s.Skracenica + "|" + s.Opis + "|" + s.Naziv + "|" + s.DatumUvodjenja + "|" + _raspored.File);
            }
            foreach (Smer s in SmeroviP)
            {
                f.WriteLine(s.Oznaka + "|" + s.Skracenica + "|" + s.Opis + "|" + s.Naziv + "|" + s.DatumUvodjenja + "|" + s.File);
            }
            f.Close();
        }

        public List<Smer> otvoriSmer(String fileName)
        {

            List<Smer> smerovi = new List<Smer>();
            FileStream f = new FileStream("../../Save/smer.txt", FileMode.OpenOrCreate);
            f.Close();

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[\r\n]{3,}", options);
            string recentText = File.ReadAllText("../../Save/smer.txt");

            string[] tekst = recentText.Split('\n');
            foreach (string smer in tekst)
            {
                Smer s = new Smer();
                if (smer == "")
                {
                    return smerovi;
                }

                string[] sm = smer.Split('|');

                s.Oznaka = sm[0];
                s.Skracenica = sm[1];
                s.Opis = sm[2];
                s.Naziv = sm[3];
                //sm[4].Replace("\r", "");
                string[] sss = sm[4].Split(' ');
                s.DatumUvodjenja = Convert.ToDateTime(sss[0]);
                s.File = sm[5].Replace("\r","");
                if (fileName == s.File)
                    smerovi.Add(s);
                else
                    SmeroviP.Add(s);
            }

            return smerovi;
        }

        public void sacuvajSoftver()
        {
            StreamWriter f = new StreamWriter("../../Save/softver.txt");
            foreach (Softver s in Softveri)
            {
                f.Write(s.Oznaka + "|" + s.Naziv + "|" + s.Cena + "|" + s.GodinaIzdavanja + "|");
                f.Write(s.Sistem);
                f.Write("|" + s.Opis + "|" + s.Proizvodjac + "|" + s.Sajt);
                f.Write("\r\n");
            }
            f.Close();
        }

        public void sacuvajSoftver(ObservableCollection<Softver> Softveri, Raspored _raspored)
        {
            StreamWriter f = new StreamWriter("../../Save/softver.txt");
            foreach (Softver s in Softveri)
            {
                f.Write(s.Oznaka + "|" + s.Naziv + "|" + s.Cena + "|" + s.GodinaIzdavanja + "|");
                f.Write(s.Sistem);
                f.Write("|" + s.Opis + "|" + s.Proizvodjac + "|" + _raspored.File + "|" + s.Sajt);
                f.Write("\r\n");
            }
            foreach (Softver s in SoftveriP)
            {
                f.Write(s.Oznaka + "|" + s.Naziv + "|" + s.Cena + "|" + s.GodinaIzdavanja + "|");
                f.Write(s.Sistem);
                f.Write("|" + s.Opis + "|" + s.Proizvodjac + "|" + s.File + "|" + s.Sajt);
                f.Write("\r\n");
            }
            f.Close();
        }

        public List<Softver> otvoriSoftver(String fileName)
        {
            List<Softver> softveri = new List<Softver>();
            FileStream f = new FileStream("../../Save/softver.txt", FileMode.OpenOrCreate);
            f.Close();

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[\r\n]{3,}", options);
            string recentText = File.ReadAllText("../../Save/softver.txt");

            string[] tekst = recentText.Split('\n');
            foreach (string softver in tekst)
            {
                Softver s = new Softver();
                if (softver == "")
                    return softveri;
                string[] sf = softver.Split('|');

                s.Oznaka = sf[0];
                s.Naziv = sf[1];
                s.Cena = Convert.ToDouble(sf[2]);
                s.GodinaIzdavanja = Convert.ToInt32(sf[3]);
                s.Sistem = sf[4];

                s.Opis = sf[5];
                s.Proizvodjac = sf[6];
                s.File = sf[7];
                s.Sajt = sf[8].Trim();

                if (s.File == fileName)
                    softveri.Add(s);
                else
                    SoftveriP.Add(s);
            }

            return softveri;
        }

        public void sacuvajPredmet()
        {
            StreamWriter f = new StreamWriter("../../Save/predmet.txt");
            foreach (Predmet p in Predmeti)
            {
                f.Write(p.Naziv + "|" + p.BrojTermina + "|" + p.DuzinaTermina + "|");
                f.Write(p.Sistem);

                f.Write("|" + p.Opis + "|" + p.Oznaka + "|" + p.Skracenica + "|");
                if (p.SmerPredmeta != null)
                    f.Write(p.SmerPredmeta.Oznaka);
                f.Write("|" + p.TrebaPametnaTabla + "|" + p.TrebaProjektor + "|" + p.TrebaTabla + "|" + p.VelicinaGrupe + "|");
                //if (p.Softveri == null || p.Softveri.Count==0)
                foreach (Softver s in p.Softveri)
                {
                    f.Write(s.Oznaka + ",");
                }
                f.WriteLine();
            }
            f.Close();
        }

        public void sacuvajPredmet(ObservableCollection<Predmet> Predmeti, Raspored _raspored)
        {
            StreamWriter f = new StreamWriter("../../Save/predmet.txt");
            foreach (Predmet p in Predmeti)
            {
                f.Write(p.Naziv + "|" + p.BrojTermina + "|" + p.DuzinaTermina + "|");
                f.Write(p.Sistem);

                f.Write("|" + p.Opis + "|" + p.Oznaka + "|" + p.Skracenica + "|");
                if (p.SmerPredmeta != null)
                    f.Write(p.SmerPredmeta.Oznaka);
                f.Write("|" + p.TrebaPametnaTabla + "|" + p.TrebaProjektor + "|" + p.TrebaTabla + "|" + p.VelicinaGrupe + "|" + _raspored.File + "|");
                //if (p.Softveri == null || p.Softveri.Count==0)
                foreach (Softver s in p.Softveri)
                {
                    f.Write(s.Oznaka + ",");
                }
                f.WriteLine();
            }
            foreach (Predmet p in PredmetiP)
            {
                f.Write(p.Naziv + "|" + p.BrojTermina + "|" + p.DuzinaTermina + "|");
                f.Write(p.Sistem);

                f.Write("|" + p.Opis + "|" + p.Oznaka + "|" + p.Skracenica + "|");
                if (p.SmerPredmeta != null)
                    f.Write(p.SmerPredmeta.Oznaka);
                f.Write("|" + p.TrebaPametnaTabla + "|" + p.TrebaProjektor + "|" + p.TrebaTabla + "|" + p.VelicinaGrupe + "|" + p.File + "|");
                //if (p.Softveri == null || p.Softveri.Count==0)
                foreach (Softver s in p.Softveri)
                {
                    f.Write(s.Oznaka + ",");
                }
                f.WriteLine();
            }
            f.Close();
            f.Close();
        }




        public List<Predmet> otvoriPredmet(String fileName)
        {
            List<Predmet> predmeti = new List<Predmet>();
            FileStream f = new FileStream("../../Save/predmet.txt", FileMode.OpenOrCreate);
            f.Close();

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[\r\n]{3,}", options);
            string recentText = File.ReadAllText("../../Save/predmet.txt");

            string[] tekst = recentText.Split('\n');
            foreach (string predmet in tekst)
            {
                Predmet p = new Predmet();
                if (predmet == "")
                    return predmeti;
                string[] pr = predmet.Split('|');

                p.Naziv = pr[0];

                p.BrojTermina = Convert.ToInt32(pr[1]);
                p.DuzinaTermina = Convert.ToInt32(pr[2]);
                p.Sistem = pr[3];

                p.Opis = pr[4];
                p.Oznaka = pr[5];
                p.Skracenica = pr[6];
                p.SmerPredmeta = nadjiSmer(pr[7]);
                //MessageBox.Show("TrebaPametnaTabla: " + pr[8]);
                p.TrebaPametnaTabla = Convert.ToBoolean(pr[8]);
                //MessageBox.Show("TrebaProjektor: " + pr[9]);
                p.TrebaProjektor = Convert.ToBoolean(pr[9]);
                //MessageBox.Show("TrebaTabla: " + pr[10]);
                p.TrebaTabla = Convert.ToBoolean(pr[10]);
                p.VelicinaGrupe = Convert.ToInt32(pr[11]);
                p.File = pr[12];
                List<Softver> softveri = new List<Softver>();
                foreach (string sof in pr[13].Split(','))
                {
                    Softver s = nadjiSoftver(sof);
                    if (s != null)
                        softveri.Add(s);
                }
                p.Softveri = softveri;
                // MessageBox.Show(""+p.Softveri.Count);
                if (p.File == fileName)
                    predmeti.Add(p);
                else
                    PredmetiP.Add(p);

            }

            return predmeti;
        }

        public Smer nadjiSmer(string oznaka)
        {
            //MessageBox.Show(""+Smerovi.Count);
            foreach (Smer s in Smerovi)
            {
                if (s.Oznaka == oznaka)
                    return s;
            }
            return null;
        }

        public Softver nadjiSoftver(string oznaka)
        {
            //MessageBox.Show("" + Softveri.Count);
            foreach (Softver s in Softveri)
            {
                if (s.Oznaka == oznaka)
                    return s;
            }
            return null;
        }

        public Ucionica nadjiUcionicu(string oznaka)
        {
            //MessageBox.Show("" + Softveri.Count);
            foreach (Ucionica u in Ucionice)
            {
                if (u.Oznaka == oznaka)
                    return u;
            }
            return null;
        }

        public Predmet nadjiPredmet(string oznaka)
        {
            //MessageBox.Show("" + Softveri.Count);
            foreach (Predmet p in Predmeti)
            {
                if (p.Oznaka == oznaka)
                    return p;
            }
            return null;
        }

        public void sacuvajRaspored()
        {
            StreamWriter f = new StreamWriter(Raspored.File);
            f.WriteLine(Raspored.Naziv);
            foreach (Predmet p in Raspored.OstaliTermini)
            {
                f.Write(p.Oznaka + "," + p.BrojTermina + "|");
            }
            f.WriteLine();
            foreach (UcionicaRaspored r in Raspored.Rasporedi)
            {
                f.Write(r.Ucionica.Oznaka + ":");
                for (int i = 0; i < 61; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        f.Write(r.Rasporedi[i][j].Oznaka + ",");
                    }
                    f.Write("|");
                }
                f.WriteLine();
            }

            f.Close();
        }

        public void sacuvajRaspored(Raspored Raspored)
        {
            StreamWriter f = new StreamWriter(Raspored.File);
            f.WriteLine(Raspored.Naziv);
            foreach (Predmet p in Raspored.OstaliTermini)
            {
                f.Write(p.Oznaka + "," + p.BrojTermina + "|");
            }
            f.WriteLine();
            foreach (UcionicaRaspored r in Raspored.Rasporedi)
            {
                f.Write(r.Ucionica.Oznaka + ":");
                for (int i = 0; i < 61; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        f.Write(r.Rasporedi[i][j].Oznaka + ",");
                    }
                    f.Write("|");
                }
                f.WriteLine();
            }

            f.Close();
        }

        public Model.Raspored otvoriRaspored(String fileName)
        {
            Model.Raspored rasp = new Model.Raspored();
            rasp.File = fileName;

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[\r\n]{3,}", options);
            string open_text = File.ReadAllText(fileName);
            if (open_text == "")
                return rasp;

            string[] tekst = open_text.Split('\n');
            rasp.Naziv = tekst[0];

            foreach (string pr in tekst[1].Split('|'))
            {
                string[] pr_termin = pr.Split(',');
                // pr_termin.ToList().Count
                if (pr_termin.ToList().Count == 2)
                {
                    if (pr_termin[0] != "" && pr_termin[1] != "")
                    {
                        Predmet p = nadjiPredmet(pr_termin[0]);
                        if (p != null)
                        {
                            p.BrojTermina = Convert.ToInt32(pr_termin[1]);
                            rasp.OstaliTermini.Add(p);
                        }

                    }
                }
            }
            int broj = 2;
            while (true)
            {
                if (tekst.ToList().Count == broj || tekst[broj] == "" || tekst[broj] == "\r")
                    return rasp;
                string[] uc_term = tekst[broj].Split(':');
                Ucionica u = nadjiUcionicu(uc_term[0]);
                UcionicaRaspored ur = new UcionicaRaspored(u);
                string[] pr = uc_term[1].Split('|');
                for (int i = 0; i < 61; i++)
                {
                    if (pr[i] != "" || pr[i] != "\r")
                    {
                        string[] predmeti = pr[i].Split(',');
                        for (int j = 0; j < 7; j++)
                        {
                            Predmet p = nadjiPredmet(predmeti[j]);
                            if (p != null)
                                ur.Rasporedi[i][j] = p;
                            if (predmeti[j] == "Pauza")
                            {
                                Predmet pauza = new Predmet();
                                pauza.Oznaka = "Pauza";
                                pauza.Naziv = "Pauza";
                                pauza.DuzinaTermina = 0;
                                ur.Rasporedi[i][j] = pauza;
                            }
                        }

                    }
                }
                rasp.Rasporedi.Add(ur);
                broj++;

            }
            //return rasp;
        }

    }
}
