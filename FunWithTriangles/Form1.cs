using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace FunWithTriangles
{
    public partial class Form1 : Form
    {
        private Triangle triangle;
        private List<Triangle> triangles;
        private DataGridView dataGridView;


        public Form1()
        {
            InitializeComponent();
            Width = 650;
            Height = 600;
            CreateList();
            CreateMenu();
            CreateTriangle();
            CreateDataGridView();
        }

        private void Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (DataGridView) sender;
            var selectedRows = grid.SelectedRows;
            if (selectedRows.Count == 1 && !selectedRows[0].IsNewRow)
            {
                Double.TryParse((string)selectedRows[0].Cells[0].Value, out double edgeA);
                Double.TryParse((string)selectedRows[0].Cells[1].Value, out double edgeB);
                Double.TryParse((string)selectedRows[0].Cells[2].Value, out double edgeC);
                triangle.EdgeA = edgeA;
                triangle.EdgeB = edgeB;
                triangle.EdgeC = edgeC;

                triangle.Text = triangle.EdgeA + " | " + triangle.EdgeB + " | " + triangle.EdgeC;
                triangle.Refresh();
            }
        }

        private void CreateTriangle()
        {
            triangle = new Triangle {Top = 25, Left = 520, Text = "Sigee"};
            Controls.Add(triangle);
        }

        private void CreateList()
        {
            triangles = new List<Triangle>();
            triangles.Add(new Triangle {EdgeA = 10, EdgeB = 10, EdgeC = 10});
            triangles.Add(new Triangle {EdgeA = 15, EdgeB = 15, EdgeC = 15});
        }

        private void CreateDataGridView()
        {
            dataGridView = new DataGridView {Top = 25, Left = 10, Width = 500, Height = 400};
            var edgeA = new DataGridViewTextBoxColumn() {HeaderText = "A oldal", DataPropertyName = "EdgeA"};
            var edgeB = new DataGridViewTextBoxColumn() {HeaderText = "B oldal", DataPropertyName = "EdgeB"};
            var edgeC = new DataGridViewTextBoxColumn() {HeaderText = "C oldal", DataPropertyName = "EdgeC"};
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
//            dataGridView.DataSource = triangles;

            dataGridView.Columns.AddRange(edgeA, edgeB, edgeC);

            dataGridView.CellClick += Grid_CellClick;
            foreach (var column in dataGridView.Columns)
            {
            }

//            dataGridView.Columns["AutoSize"].Visible = false;
//            dataGridView.Columns["AutoSizeMode"].Visible = false;
            Controls.Add(dataGridView);
        }

        private void CreateMenu()
        {
            var menuStrip = new MenuStrip();
            var menuItem = new ToolStripMenuItem {Text = "File", ShortcutKeys = Keys.Alt | Keys.F};
            menuStrip.Items.AddRange(new ToolStripItem[] {menuItem});
            var openMenuItem = new ToolStripMenuItem {Text = "Open", ShortcutKeys = Keys.Alt | Keys.O};
            var saveMenuItem = new ToolStripMenuItem {Text = "Save", ShortcutKeys = Keys.Alt | Keys.S};
            menuItem.DropDownItems.AddRange(new ToolStripItem[] {openMenuItem, saveMenuItem});

            openMenuItem.Click += OpenTriangles;
            Controls.Add(menuStrip);
        }

        private void OpenTriangles(object sender, EventArgs eventArgs)
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "Fun Vith Triangles (*.fvt)|*.fvt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(sfd.FileName))
                {
                    try
                    {
                        /** @TODO: Load data from file */
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("Error at file reading: " + ex.Message, "Error");
                    }
                }
            }
        }
    }
}