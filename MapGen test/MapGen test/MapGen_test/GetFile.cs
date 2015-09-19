using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapGen_test
{
    class GetFile
    {
        public void OpenFile()
        {
            string hold;
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "XML|.xml";

            if (openFile.ShowDialog() == DialogResult.OK)
                hold = openFile.FileName;

        }


    }
}
