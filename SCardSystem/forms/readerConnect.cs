using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SCardSystem.libs;

namespace SCardSystem.forms
{
    public partial class readerConnect : Form
    {
        private bool is_read_card;
        public readerConnect()
        {
            InitializeComponent();
        }
    }
}
