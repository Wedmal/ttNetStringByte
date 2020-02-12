using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace tt1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            string s = System.IO.File.ReadAllText(@"xml_primer.txt", Encoding.Default).Replace("\n", " ");
            XElement xmlTree = XElement.Parse(s);
            List<string> ListString = new List<string>();
            byte[] buf = Encoding.UTF8.GetBytes(s);
            StringBuilder sb = new StringBuilder(buf.Length * 8);
            
            foreach (byte b in buf)
            {
                ListString.Add(Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            List<Byte> byteList = new List<Byte>();
            foreach (var item    in ListString)
            {
                byteList.Add(Convert.ToByte(item,2));
            }

            string binaryStr = sb.ToString();
            string str= Encoding.UTF8.GetString(byteList.ToArray());
            string resultText = BinaryToString(binaryStr);

            //string[] arr = XDocument.Load(@"xml_primer.txt").Descendants("response")
            //        .Select(element => element.Value).ToArray();
        }
        public static string BinaryToString(string data)
        {
            List<Byte> byteList = new List<Byte>();

            for (int i = 0; i < data.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }

            return Encoding.UTF8.GetString(byteList.ToArray());
        }
    }
}
