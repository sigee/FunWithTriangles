using System;
using System.IO;
using System.Windows.Forms;

namespace FunWithTriangles
{
    public partial class Form1 : Form
    {
        private Triangle _triangle;
        private DataGridView _dataGridView;

        public Form1()
        {
            InitializeComponent();
            CreateMenu();
            CreateTriangle();
            CreateDataGridView();
        }

        private void CellEvents(object sender, DataGridViewCellEventArgs e)
        {
            _triangle.EdgeA = 0;
            _triangle.EdgeB = 0;
            _triangle.EdgeC = 0;
            var selectedRows = ((DataGridView) sender).SelectedRows;
            if (selectedRows.Count == 1 && !selectedRows[0].IsNewRow)
            {
                if (selectedRows[0].Cells[0].Value != null)
                {
                    double.TryParse(selectedRows[0].Cells[0].Value.ToString(), out var edgeA);
                    _triangle.EdgeA = edgeA;
                }

                if (selectedRows[0].Cells[1].Value != null)
                {
                    double.TryParse(selectedRows[0].Cells[1].Value.ToString(), out var edgeB);
                    _triangle.EdgeB = edgeB;
                }

                if (selectedRows[0].Cells[2].Value != null)
                {
                    double.TryParse(selectedRows[0].Cells[2].Value.ToString(), out var edgeC);
                    _triangle.EdgeC = edgeC;
                }
            }

            _triangle.Refresh();
        }

        private void CreateTriangle()
        {
            _triangle = new Triangle {Top = 25, Left = 520, Width = 300, Height = 300};
            Controls.Add(_triangle);
        }

        private void CreateDataGridView()
        {
            _dataGridView = new DataGridView {Top = 25, Left = 10, Width = 500, Height = 400};
            var edgeA = new DataGridViewTextBoxColumn()
                {HeaderText = "A oldal", DataPropertyName = "EdgeA", Width = 80};
            var edgeB = new DataGridViewTextBoxColumn()
                {HeaderText = "B oldal", DataPropertyName = "EdgeB", Width = 80};
            var edgeC = new DataGridViewTextBoxColumn()
                {HeaderText = "C oldal", DataPropertyName = "EdgeC", Width = 80};
            var area = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Terület", DataPropertyName = "Area", ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            var perimeter = new DataGridViewTextBoxColumn()
                {HeaderText = "Kerület", DataPropertyName = "Perimeter", ReadOnly = true, Width = 80};
            _dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _dataGridView.Columns.AddRange(edgeA, edgeB, edgeC, area, perimeter);
            _dataGridView.RowCount = 11;
            _dataGridView.AllowUserToAddRows = false;
            _dataGridView.AllowUserToDeleteRows = false;
            _dataGridView.CellEnter += CellEvents;
            _dataGridView.CellValueChanged += CellEvents;
            Controls.Add(_dataGridView);
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
            saveMenuItem.Click += SaveTriangles;
            Controls.Add(menuStrip);
        }

        private void OpenTriangles(object sender, EventArgs eventArgs)
        {
            var openFileDialog = new OpenFileDialog {Filter = "Fun Vith Triangles (*.fvt)|*.fvt"};
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(openFileDialog.FileName))
                {
                    try
                    {
                        var reader = new BinaryReader(File.Open(openFileDialog.FileName, FileMode.Open));
                        for (var i = 0; i < _dataGridView.RowCount - 1; i++)
                        {
                            for (var j = 0; j < 3; j++)
                            {
                                var value = reader.ReadDouble();
                                if (value != 0.0)
                                {
                                    _dataGridView.Rows[i].Cells[j].Value = value;
                                }
                            }
                        }

                        reader.Close();
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("Error at file reading: " + ex.Message, "Error");
                    }
                }
            }
        }

        private void SaveTriangles(object sender, EventArgs eventArgs)
        {
            var saveFileDialog = new SaveFileDialog {Filter = "Fun Vith Triangles (*.fvt)|*.fvt"};
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var writer = new BinaryWriter(File.Open(saveFileDialog.FileName, FileMode.Create));
                    foreach (DataGridViewRow row in _dataGridView.Rows)
                    {
                        for (var i = 0; i < 3; i++)
                        {
                            double edge;
                            if (row.Cells[i].Value != null)
                            {
                                double.TryParse(row.Cells[i].Value.ToString(), out edge);
                            }
                            else
                            {
                                edge = 0.0;
                            }

                            writer.Write(edge);
                        }
                    }

                    writer.Close();
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Error at file writing: " + ex.Message, "Error");
                }
            }
        }
    }
}