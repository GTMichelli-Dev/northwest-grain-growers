using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RWSP.Models;
using RWSP.Dto;

namespace RWSP
{
    public partial class frmLocationServers : Form
    {
        public frmLocationServers()
        {
            InitializeComponent();
            this.DataGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGrid_CellEndEdit);

        }

        private void DataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (DataGrid.Columns[e.ColumnIndex].Name != nameof(ServerLocationDto.Description))
                return;

            var editedRow = DataGrid.Rows[e.RowIndex].DataBoundItem as ServerLocationDto;
            if (editedRow == null || string.IsNullOrWhiteSpace(editedRow.Description))
                return;

            bool isDuplicate = ((BindingList<ServerLocationDto>)DataGrid.DataSource)
                .Any(row => row.Uid != editedRow.Uid &&
                            string.Equals(row.Description.Trim(), editedRow.Description.Trim(), StringComparison.OrdinalIgnoreCase));

            if (isDuplicate)
            {
                editedRow.Description = string.Empty;
                DataGrid.Refresh();

                MessageBox.Show(
                    this,
                    "That location description already exists.",
                    "Duplicate Description",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }


        private async void frmLocationServers_Load(object sender, EventArgs e)
        {
            List<ServerLocationDto> serverLocationList = await LoadServerLocationDtosAsync();

            DataGrid.DataSource = new BindingList<ServerLocationDto>(serverLocationList);
            // Hide specific columns
            DataGrid.Columns[nameof(ServerLocationDto.Uid)].Visible = false;
            DataGrid.Columns[nameof(ServerLocationDto.ServerUid)].Visible = false;

            // Set all columns to read-only except Description
            foreach (DataGridViewColumn column in DataGrid.Columns)
            {
                if (column.Name == nameof(ServerLocationDto.RemotePrint))
                    column.CellTemplate = new DataGridViewCheckBoxCell();

                if (column.Name == nameof(ServerLocationDto.Description))
                {
                    column.ReadOnly = false;
                }
                else
                {
                    column.ReadOnly = true;
                }
            }

            // Optional: autosize columns for a better fit
            DataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }



        private async Task<List<ServerLocationDto>> LoadServerLocationDtosAsync()
        {
            return await Task.Run(() =>
            {
                using (var context = new NW_DataContext())
                {
                    var query = from sl in context.ServerLocations
                                join s in context.Servers on sl.ServerUid equals s.Uid
                                where s.ServerName != "ARCHIVE"
                                   && s.ServerName != "Leo"
                                   && s.ServerName != "MISSION-SRV"
                                   && s.ServerName != "Endicott-NUC"
                                   && s.ServerName != "SHEF-SRV"
                                   && s.ServerName != "ENDPC002"
                                   && s.ServerName != "LYNDB001"
                                   && s.ServerName != "PRES-SRV"
                                   && s.ServerName != "SPOF-SRV"
                                   && s.ServerName != "DAYTON-SRV"
                                   && s.ServerName != "WALDB001"
                                   && s.ServerName != "PK-SRV"
                                   && s.ServerName != "LSPPC003"
                                   && s.ServerName != "DSPDB001"
                                   && s.ServerName != "DELDB001"
                                   && s.ServerName != ""


                                orderby s.ServerName
                                select new ServerLocationDto
                                {
                                    Uid = sl.Uid,
                                    ServerUid = s.Uid,
                                    ServerName = s.ServerName,
                                    Description = sl.Description
                                };

                    return query.ToList();

                }
            });
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            this.pnlSync.Visible = true;
            this.label2.Text = "Saving To Database...";
            if (DataGrid.DataSource is BindingList<ServerLocationDto> updatedList)
            {
                await Task.Run(() =>
                {
                    using (var context = new NW_DataContext())
                    {
                        foreach (var dto in updatedList)
                        {
                            var entity = context.ServerLocations.FirstOrDefault(sl => sl.Uid == dto.Uid);
                            if (entity != null)
                            {
                                if (entity.Description != dto.Description)
                                    entity.Description = dto.Description;

                                if (entity.RemotePrint != dto.RemotePrint)
                                    entity.RemotePrint = dto.RemotePrint;
                            }
                        }

                        context.SaveChanges();
                    }
                });

                this.label2.Text = "Validating Connection To Remote Database...";
                await Task.Run(() =>
                {
                    using (var context = new NW_DataContext())
                    {
                        foreach (var dto in updatedList)
                        {
                            var entity = context.ServerLocations.FirstOrDefault(sl => sl.Uid == dto.Uid);
                            if (entity != null)
                            {
                                if (entity.Description != dto.Description)
                                    entity.Description = dto.Description;

                                if (entity.RemotePrint != dto.RemotePrint)
                                    entity.RemotePrint = dto.RemotePrint;
                            }
                        }

                        context.SaveChanges();
                    }
                });

                this.Close();
            }
        }
    }
}