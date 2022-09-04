using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ApplicationUnit.Encoder
{
    public class CharacterEncoder: IComparer<CharacterStats>  
    {
        private CharacterStats searchRoot;
        private BinaryEncoder binaryEncoder;


        public CharacterEncoder()
        {
            this.binaryEncoder = new BinaryEncoder();
        }


        public CharacterEncoder(string text)
        {
            this.binaryEncoder = new BinaryEncoder();
            AnalizeStatistics(text);
        }


        /// <summary>
        /// Сжатие текстового сообщения на основе кода хаффмана.
        /// </summary>
        /// <param name="text"> текст сообщения </param>
        /// <returns> сжатое сообщение </returns>
        public string Encode(string text)
        {
            AnalizeStatistics(text);
            string binary = "";
            foreach (char ch in text)
            {
                string bits = this.Encode(ch);
                Console.WriteLine($"{ch}={bits}");
                binary += bits;
            }
            Console.WriteLine($"binary={binary}");
            return FlushEncode(binary);
        }


        /// <summary>
        /// Метод вывода сжатых данных в текстовое сообщение
        /// </summary>
        /// <param name="binary"> бинарный код </param>
        /// <returns> текстовое сообщение </returns>
        private string FlushEncode(string binary)
        {
            binary += "1";
            while (binary.Length % 8 != 0)
            {
                binary += "0";
            }            
            return Seriallize() + 
                this.binaryEncoder.FromBinary(binary);
        }


        /// <summary>
        /// Метод обратного декодирования сообщения
        /// </summary>
        /// <param name="encode"> закодированное сообщение</param>
        /// <returns> раскодированное сообщение </returns>
        public string Decode(string encode)
        {  
            int begin = this.Deseriallize(encode);   
            
            // получение текстового сообщения в бинарном формате
            string encodedText = encode.Substring(begin);            
            string binaryCode = this.binaryEncoder.ToBinary(encodedText);
            binaryCode = binaryCode.Substring(0, binaryCode.LastIndexOf("1"));

            Console.WriteLine($"binaryCode: {binaryCode}");
            return this.FlushDecode(binaryCode);

        }

        
        /// <summary>
        /// Обход иерархии хаффмана вывод раскодированных символов
        /// </summary>
        /// <param name="binaryCode"></param>
        /// <returns></returns>
        private string FlushDecode(string binaryCode)
        {
            string text = "";
            CharacterStats pnode = this.searchRoot;            
            foreach ( char ch in binaryCode)
            {
                if(ch == '0')
                {
                    pnode = pnode.left;
                }
                else
                {
                    pnode = pnode.right;
                }
                if (pnode.text.Length == 1)
                {
                    text += pnode.text;
                    pnode = this.searchRoot;
                }
            }
            return text;
        }


        /// <summary>
        /// Получение бинарного кода символа 
        /// </summary>
        /// <param name="ch"> символ </param>
        /// <returns> бинарный код </returns>
        private string Encode( char ch )
        {
            string binary = "";
            CharacterStats pnode = this.searchRoot;
            while(pnode.text != (ch + ""))
            {
                if( pnode.left != null && pnode.left.Contains(ch))
                {
                    binary += "0";
                    pnode = pnode.left;
                }
                else
                {
                    binary += "1";
                    pnode = pnode.right;
                }
            }
            return binary;
        }


        /// <summary>
        /// Метод инициаллизации кода Хаффмана по исходному тексту сообщения
        /// </summary>
        /// <param name="text"> текст сообщения </param>
        public void AnalizeStatistics(string text)
        {
            Dictionary<string, CharacterStats> charset = new Dictionary<string, CharacterStats>();
            foreach (char ch in text)
            {
                string code = ch + "";
                if (charset.ContainsKey(code))
                {
                    charset[code].value++;
                }
                else
                {
                    charset[code] = new CharacterStats()
                    {
                        text = code,
                        value = 1
                    };
                }
            }
            AnalizeStatistics(new List<CharacterStats>(charset.Values));
        }


        /// <summary>
        /// Анализ статистики и перестроение структуры символов 
        /// </summary>
        /// <param name="stats"> татистика использования </param>
        private void AnalizeStatistics(List<CharacterStats> stats)
        {     
            stats.Sort(this);
            while (stats.Count > 1)
            {
                CharacterStats[] arr = stats.ToArray();
                CharacterStats parent = new CharacterStats()
                {
                    text = arr[1].text + arr[0].text,
                    value = arr[0].value + arr[1].value
                };
                stats.Remove(parent.left = arr[1]);
                stats.Remove(parent.right = arr[0]);
                stats.Add(parent);
                stats.Sort(this);
            }
            this.searchRoot = stats.ToArray()[0];
        }


        /// <summary>
        /// Сериализация в текстовое сообщение
        /// </summary>        
        /// <returns> текстовое сообщение </returns>
        public string Seriallize()
        {
            List<CharacterStats> stats = this.searchRoot.GetLists();
            string text = stats.Count + "";
            foreach (CharacterStats stat in stats)
            {
                text += "|" + stat.text + stat.value;
            }
            return text + "|";
        }


        /// <summary>
        /// Десериализация состояния из закодированного сообщения
        /// </summary>        
        /// <returns> кол-во считанных символов  </returns>
        public int Deseriallize(string text)
        {
            this.searchRoot = null;
            List<CharacterStats> charset = new List<CharacterStats>();

            int before = text.Length;
            
            // считывание кол-ва уникальных символов
            int indexOfSeparator = text.IndexOf("|");
            int n = int.Parse(text.Substring(0, indexOfSeparator));
            text = text.Substring(indexOfSeparator + 1);
            for (int i = 0; i < n; i++)
            {
                indexOfSeparator = text.IndexOf("|");
                CharacterStats chartStat = new CharacterStats() { 
                    text = text.Substring(0,1),
                    value = int.Parse(text.Substring(1, indexOfSeparator - 1))
                };
                charset.Add(chartStat);
                text = text.Substring(indexOfSeparator + 1);
            }            
            this.AnalizeStatistics(charset);
            return before - text.Length;
        }


        /// <summary>
        /// Сравнение статистики вхождения символов
        /// </summary>
        /// <param name="x"> символ 1 </param>
        /// <param name="y"> символ 2 </param>
        /// <returns></returns>
        public int Compare([AllowNull] CharacterStats x, [AllowNull] CharacterStats y)
        {
            return x.value - y.value;
        }


        /// <summary>
        /// Преобразование в строку
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.searchRoot.ToString();
        }
    }
}
