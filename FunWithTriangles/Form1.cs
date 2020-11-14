using System;
using System.IO;
using System.Linq;
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
            var selectedRows = ((DataGridView) sender).SelectedRows;
            if (selectedRows.Count != 1 || selectedRows[0].IsNewRow)
            {
                _triangle.EdgeA = 0;
                _triangle.EdgeB = 0;
                _triangle.EdgeC = 0;
            }
            else
            {
                Double.TryParse((string) selectedRows[0].Cells[0].Value, out double edgeA);
                Double.TryParse((string) selectedRows[0].Cells[1].Value, out double edgeB);
                Double.TryParse((string) selectedRows[0].Cells[2].Value, out double edgeC);
                _triangle.EdgeA = edgeA;
                _triangle.EdgeB = edgeB;
                _triangle.EdgeC = edgeC;
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
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fun Vith Triangles (*.fvt)|*.fvt";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(openFileDialog.FileName))
                {
                    try
                    {
                        var contents = File.ReadAllText(openFileDialog.FileName);
                        var rows = contents.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None);
                        for (var i = 0; i < rows.Length; i++)
                        {
                            if (_dataGridView.Rows.Count < i)
                            {
                                continue;
                            }

                            var cells = rows[i].Split(',');
                            if (cells.Length != 3)
                            {
                                continue;
                            }

                            _dataGridView.Rows[i].Cells[0].Value = cells[0];
                            _dataGridView.Rows[i].Cells[1].Value = cells[1];
                            _dataGridView.Rows[i].Cells[2].Value = cells[2];
                        }
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
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Fun Vith Triangles (*.fvt)|*.fvt";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var contents = _dataGridView.Rows.Cast<DataGridViewRow>().Aggregate("",
                        (current, row) =>
                            current + (row.Cells[0].Value + "," + row.Cells[1].Value + "," + row.Cells[2].Value +
                                       "\n"));
                    File.WriteAllText(saveFileDialog.FileName, contents);
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Error at file writing: " + ex.Message, "Error");
                }
            }
        }
    }
}