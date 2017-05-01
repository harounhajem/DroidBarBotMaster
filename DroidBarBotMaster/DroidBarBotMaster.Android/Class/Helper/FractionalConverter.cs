using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailHttpRequest
{
    public class FractionalConverter
    {
        public Double ResultOz
        {
            get { return this.result; }
            private set { this.result = value; }
        }
        private Double result;

        public double ResultCl { get; set; }

        public FractionalConverter(String input)
        {
            this.ResultOz = this.Parse(input);
            ResultCl = this.ResultOz * 2.95735296;
        }

        private Double Parse(String input)
        {
            int CharOPosition = input.IndexOf('o');

            input = input.Remove(CharOPosition, input.Length - CharOPosition);

            input = (input ?? String.Empty).Trim();

            if (String.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException("input");
            }

            // standard decimal number (e.g. 1.125)
            if (input.IndexOf('.') != -1 || (input.IndexOf(' ') == -1 && input.IndexOf('/') == -1 && input.IndexOf('\\') == -1))
            {
                Double result;
                if (Double.TryParse(input, out result))
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
            return Double.NaN;
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
