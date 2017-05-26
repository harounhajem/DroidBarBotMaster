using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroidBarBotMaster.Droid.Class.Helper
{
    public class FractionalConverter
    {
        public Double ResultOz
        {
            get { return this.result; }
            private set { this.result = value; }
        }
        private Double result;
        private double multiplier = 2.95735296;

        public double ResultCl { get; set; }

        public FractionalConverter(String input)
        {
            this.ResultOz = this.Parse(input);
            ResultCl = this.ResultOz * multiplier;
        }



        private string DetectMeasurementType(String input)
        {

            // oz
            if (input.ToLower().Contains("oz"))
            {
                multiplier = 2.95735296;
                return SliceChars(input, 'o');
            }

            // can
            if (input.ToLower().Contains("can"))
            {
                multiplier = 2.95735296 * 0.2;
                return SliceChars(input, 'c');
            }

            // fifth
            if (input.ToLower().Contains("fifth"))
            {
                multiplier = 2.95735296 * 0.2;
                return SliceChars(input, 'f');
            }

            // qt
            if (input.ToLower().Contains("qt"))
            {
                multiplier = 2.95735296 *0.5;
                return SliceChars(input, 'q');
            }

            // jiggers
            if (input.ToLower().Contains("jig"))
            {
                multiplier = 2.95735296 * 0.2;
                return SliceChars(input, 'j');
            }

            // cups
            if (input.ToLower().Contains("cup"))
            {
                multiplier = 2.95735296 * 0.4;
                return SliceChars(input, 'c');
            }

            // Part
            if (input.ToLower().Contains("part"))
            {
                multiplier = 2.95735296 * 1.2;
                return SliceChars(input, 'p');
            }

            // tblsp
            if (input.ToLower().Contains("tblsp"))
            {
                multiplier = 2.95735296 * 0.4;
                return SliceChars(input, 't');
            }

            // Bottle
            if (input.ToLower().Contains("bottle"))
            {
                multiplier = 2.95735296 * 10;
                return SliceChars(input, 'b');
            }

            if (input.ToLower().Contains("handful"))
            {
                multiplier = 2.95735296 * 0.5;
                return SliceChars(input, 'h');
            }

            // Shot
            if (input.ToLower().Contains("shot"))
            {
                multiplier = 2.95735296 * 8;
                return SliceChars(input, 's');
            }

            // TSP
            if (input.ToLower().Contains("tsp"))
            {
                multiplier = 2.95735296 * 0.3;
                return SliceChars(input, 't');
            }

            // cl
            if (input.ToLower().Contains("cl"))
            {
                multiplier = 1;
                return SliceChars(input, 'c');
            }


            // Pint
            if (input.ToLower().Contains("pint"))
            {
                multiplier = 2.95735296 * 2;
                return SliceChars(input, 'p');
            }

            // cl
            if (input.ToLower().Contains("ml"))
            {
                multiplier = 0.1;
                return SliceChars(input, 'm');
            }

            // cl
            if (input.ToLower().Contains("dashes"))
            {
                multiplier = 2.95735296 * 0.2;
                return SliceChars(input, 'd');
            }

            return input;
        }


        private string SliceChars(String input, char beginChar) {

            int CharOPosition = input.ToLower().IndexOf(beginChar);

            if (CharOPosition < 0) return "none";

            input = input.Remove(CharOPosition, input.Length - CharOPosition);

            input = (input ?? String.Empty).Trim();

            if (String.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(beginChar.ToString());
            }

            return input;
        }










        private Double Parse(String input)
        {
            input = DetectMeasurementType(input);

            // standard decimal number (e.g. 1.125)
            if (input.IndexOf('.') != -1 || (input.IndexOf(' ') == -1 && input.IndexOf('/') == -1 && input.IndexOf('\\') == -1))
            {
                Double result;
                if (Double.TryParse(input.Replace('.', ','), out result))
                {
                    return result;
                }
            }

            String[] parts = input.Split(new[] { ' ', '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);

            // stand-off fractional (e.g. 7/8)
            if (input.IndexOf(' ') == -1 && parts.Length == 2)
            {
                Double num, den;
                if (Double.TryParse(parts[0], out num) && Double.TryParse(parts[1], out den))
                {
                    return num / den;
                }
            }

            // Number and fraction (e.g. 2 1/2)
            if (parts.Length == 3)
            {
                Double whole, num, den;
                if (Double.TryParse(parts[0], out whole) && Double.TryParse(parts[1], out num) && Double.TryParse(parts[2], out den))
                {
                    return whole + (num / den);
                }
            }

            // Bogus / unable to parse
            return 0;
        }

        public override string ToString()
        {
            return this.ResultOz.ToString();
        }

        public static implicit operator Double(FractionalConverter number)
        {
            return number.ResultOz;
        }
    }
}
