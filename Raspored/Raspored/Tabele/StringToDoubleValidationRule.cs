using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Raspored.Tabele
{
    public class StringToDoubleValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                double r;
                if (double.TryParse(s, out r))
                {
                    return new ValidationResult(true, null);
                }
                return new ValidationResult(false, "Unesite pozitivan broj.");
            }
            catch
            {
                return new ValidationResult(false, "Greska prilikom unosa.");
            }
        }
    }
    public class StringToIntValidationRule:ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                double r;
                if (double.TryParse(s, out r))
                {
                    return new ValidationResult(true, null);
                }
                return new ValidationResult(false, "Unesite pozitivan ceo broj.");
            }
            catch
            {
                return new ValidationResult(false, "Greska prilikom unosa.");
            }
        }
    }

    public class MinMaxValidationRule : ValidationRule
    {
        public int Min
        {
            get;
            set;
        }

        public int Max
        {
            get;
            set;
        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value is int)
            {
                int d = (int)value;
                if (d < Min) return new ValidationResult(false, "Molimo Vas unesite pozitivan ceo broj.");
                if (d > Max) return new ValidationResult(false, "Broj je preveliki.");
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Greska prilikom unosa.");
            }
        }
    }

    public class GodinaValidationRule : ValidationRule
    {
        public int Min
        {
            get;
            set;
        }

        public int Max
        {
            get;
            set;
        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value is int)
            {
                int d = (int)value;
                if (d < Min) return new ValidationResult(false, "Godina nije ispravna.");
                if (d > Max) return new ValidationResult(false, "Godina nije ispravna.");
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Greska prilikom unosa.");
            }
        }
    }


}
