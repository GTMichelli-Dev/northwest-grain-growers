using RWSP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RWSP
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
           
        }



        public static async Task EnsureAllServersInServerLocationAsync()
        {
            await Task.Run(() =>
            {
                using (var context = new NW_DataContext())
                {
                    var allServerUids = context.Servers.Select(s => s.Uid).ToList();
                        var existingServerUids = context.ServerLocations.Select(sl => sl.ServerUid).ToList();

                    var missingServerUids = allServerUids.Except(existingServerUids).ToList();

                    foreach (var uid in missingServerUids)
                    {
                        var serverLocation = new ServerLocation
                        {
                            Uid = Guid.NewGuid(),
                            ServerUid = uid,
                            Description = string.Empty,
                            RemotePrint = false,
                        };

                        context.ServerLocations.Add(serverLocation);
                    }

                    context.SaveChanges();
                }
            });
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await EnsureAllServersInServerLocationAsync();
            menuStrip1.Enabled = true;
            pnlSync.Visible = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void setLocationServersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new frmLocationServers())
            {
                frm.ShowDialog();
            }
        }
    }

}
