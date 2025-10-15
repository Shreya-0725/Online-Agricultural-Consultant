using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Online_Agricultural_Consultant.Models
{
    public class GeneratePasswords
    {
        public static string generate()
        {
            Random rand = new Random();
            StringBuilder str_build = new StringBuilder();
            char letter;
            for (int i = 0; i < 6; i++)
            {
                double flt = rand.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }

            return str_build.ToString();

        }
    }
}