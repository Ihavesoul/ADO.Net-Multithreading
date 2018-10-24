using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fileDownloader
{
    public partial class frmMain : Form
    {
        private readonly List<DownloadHelper.downloadFile> _ldf = new List<DownloadHelper.downloadFile>();

        public frmMain()
        {
            InitializeComponent();
        }

        private void btn_download_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    int indx = list_Items.Items.Count;
                    list_Items.Items.Add(sfd.FileName);
                    for (int i = 1; i < 6; i++)
                    {
                        list_Items.Items[indx].SubItems.Add("");
                    }

                    DownloadHelper.downloadFile d = new DownloadHelper.downloadFile(txt_url.Text, sfd.FileName);
                    _ldf.Add(d);
                    
                    

                    Action<int,int, object> act1 = (delegate (int idx, int sidx, object obj) 
                                       { list_Items.Invoke(new Action(() => list_Items.Items[idx].SubItems[sidx].Text = obj.ToString())); });


                    d.eSize += (s1, size) => act1.Invoke(indx, 1, size);
                    d.eDownloadedSize += (s1, size) => act1.Invoke(indx, 2, size);
                    d.eSpeed += (s1, size) => act1.Invoke(indx, 3, size);
                    d.eDownloadState += (s1, size) => act1.Invoke(indx, 4, size);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void list_Items_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (list_Items.SelectedItems.Count > 0)
            {
                var indx = list_Items.SelectedItems[0].Index;
                if (list_Items.Items[indx].SubItems[4].Text == "Downloading")
                {
                    btn_pause.Enabled = true;
                    btn_pause.Text = "Pause";
                }
                else if (list_Items.Items[indx].SubItems[4].Text == "Paused")
                {
                    btn_pause.Enabled = true;
                    btn_pause.Text = "Resume";
                }
                else
                {
                    btn_pause.Enabled = false;
                    btn_pause.Text = "Pause/Resume";
                }
            }
            else
            {
                btn_pause.Enabled = false;
                btn_pause.Text = "Pause/Resume";
            }
        }

        private void btn_pause_Click(object sender, EventArgs e)
        {
            var indx = list_Items.SelectedItems[0].Index;
            if (btn_pause.Text == "Pause")
            {
                _ldf[indx].CancelDownload();
                btn_pause.Text = "Resume";
            }
            else if (btn_pause.Text == "Resume")
            {
                _ldf[indx].ResumeDownload();
                btn_pause.Text = "Pause";
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
        }
    }
}
