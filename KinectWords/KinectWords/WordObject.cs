using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectWords
{
    class WordObject
    {
        string word;
        string text1;
        string text2;
        string text3;

        public WordObject(string word, string text1, string text2, string text3)
        {
            this.word = word;
            this.text1 = text1;
            this.text2 = text2;
            this.text3 = text3;

        }

        public string getText1()
        {
            return text1;
        }
        public string getText2()
        {
            return text2;
        }
        public string getText3()
        {
            return text3;
        }

        public string getWord()
        {
            return word;
        }
    }
}
