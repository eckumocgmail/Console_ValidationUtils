using System.Collections.Generic;

namespace ApplicationUnit.Encoder
{
    public class CharacterStats 
    {
        public string text;
        public int value;

        public CharacterStats left;
        public CharacterStats right;


        public bool Contains( char ch)
        {
            return this.text.IndexOf(ch) != -1;
        }


        /// <summary>
        /// Метод получения узлов не имющих потомков
        /// </summary>
        /// <returns></returns>
        public List<CharacterStats> GetLists()
        {
            List<CharacterStats> lists = new List<CharacterStats>();
            if ( this.left == null && this.right == null)
            {
                lists.Add(this);
            }
            else
            {
                if (this.left != null)
                {
                    lists.AddRange(this.left.GetLists());
                }
                if (this.right != null)
                {
                    lists.AddRange(this.right.GetLists());
                }
            }
            return lists;
        }





        public string ToString(int level=1)
        {
            string intendt = "";
            for (int i = 0; i < level; i++)
            {
                intendt += "    ";
            }
            string result = intendt + $"{this.text}[{this.value}]\n";
            if (this.left != null)
                result += this.left.ToString(level + 1);
            if (this.right != null)
                result += this.right.ToString(level + 1);
            return result;
        }
    }
}
