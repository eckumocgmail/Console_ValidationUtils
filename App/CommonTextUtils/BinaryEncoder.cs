using System;

namespace ApplicationUnit.Encoder
{
    public sealed class BinaryEncoder
    {

        /// <summary>
        /// Преобразование строки в битовою последовательность
        /// </summary>
        /// <param name="ch"> символ </param>
        /// <returns>битовая последовательность</returns>
        public string ToBinary(string text)
        {
            string binary = "";
            foreach(char ch in text)
            {
                binary += this.ToBinary(ch);
            }
            return binary;
        }


        /// <summary>
        /// Преобразование символа в битовою строку
        /// </summary>
        /// <param name="ch"> символ </param>
        /// <returns>битовая строка</returns>
        private string ToBinary(char ch)
        {
            string binary = "";
            int x = ch;
            while (x > 0)
            {
                int dev = x % 2;
                x = (x - dev) / 2;
                binary = ((dev == 1) ? "1" : "0") + binary;
            }
            while (binary.Length < 8)
            {
                binary = "0" + binary;
            }
            return binary;
        }


        /// <summary>
        /// Преобразование битовой строки в символ
        /// </summary>
        /// <param name="binary"> битовая строка </param>
        /// <returns>символ</returns>
        private char ReadChar(string binary)
        {
            int code = 0;
            for (int i = 7; i >= 0; i--)
            {
                if (binary[i] == '1')
                {
                    code += (int)Math.Pow(2, 7 - i);
                }
            }
            System.Diagnostics.Debug.WriteLine(binary + "  " + code + "  " + System.Convert.ToChar(code));
            return System.Convert.ToChar(code);
        }


        /// <summary>
        /// Преобразование битовой строки в символ
        /// </summary>
        /// <param name="binary"> битовая строка </param>
        /// <returns>символ</returns>
        public string FromBinary(string binary)
        {
            string result = "";
            while( binary.Length > 0)
            {
                string charBinaries = binary.Substring(0, Math.Min(8,binary.Length));
                result += this.ReadChar(charBinaries);
                binary = binary.Substring(8);
            }
            return result;
        }
    }
}
