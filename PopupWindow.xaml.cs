using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.Arm;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace WPFWidget
{
    public partial class PopupWindow : Window
    {
        private DataTable _fullDataTable;
        public PopupWindow()
        {
            InitializeComponent();
            ConfigComboBox.SelectionChanged += ConfigComboBox_SelectionChanged;
        }

        private string GetSelectedRefConfig()
        {
            if (ConfigComboBox.SelectedItem is string selected)
                return selected;
            return string.Empty;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fileName = SearchBox.Text.Trim();

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    MessageBox.Show("Please enter a filename.", "Missing Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string connectionString = "Server=157.93.180.72;" +
                                          "Database=MDV1;" +
                                          "User Id=Mobius_Admin;" +
                                          "Password=App@mobius$123;";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Main Query - Pull everything, including RefConfigs
                    string query = @"
                    WITH RawData AS(
                        SELECT
                            bsr.RowDocumentID,
                            bs.RefConfigs,
                            bsv.RowNo + 1 AS RowNo,
                            bsc.ColumnName,
                            bsv.ValueText
                        FROM BomSheets bs
                        INNER JOIN dbo.Documents d ON d.DocumentID = bs.SourceDocumentID
                        INNER JOIN dbo.BomSheetRow bsr ON bsr.BomDocumentID = bs.BomDocumentID
                        INNER JOIN dbo.BomSheetColumn bsc ON bsc.BomDocumentID = bs.BomDocumentID
                        INNER JOIN dbo.BomSheetValue bsv ON bsv.BomDocumentID = bs.BomDocumentID
                        INNER JOIN dbo.DocumentsInProjects dp ON dp.DocumentID = bs.SourceDocumentID
                        INNER JOIN dbo.Projects p ON p.ProjectID = dp.ProjectID
                        WHERE d.Filename LIKE @Filename
                          AND d.Deleted = 0
                          AND bsr.RowNo = bsv.RowNo
                          AND bsc.ColumnNo = bsv.ColumnNo
                          AND bs.SourceDocumentVersion = (
                              SELECT MAX(bs2.SourceDocumentVersion)
                              FROM BomSheets bs2
                              WHERE bs2.SourceDocumentID = bs.SourceDocumentID
                          )
                          AND p.Path LIKE '\Projects\%'
                    ),
                    Pivoted AS(
                        SELECT
                            RowDocumentID,
                            RowNo,
                            [ITEM NO.],
                            [PART NUMBER],
                            [DESCRIPTION]
                        FROM (
                            SELECT 
                                RowDocumentID,
                                RowNo,
                                ColumnName,
                                ValueText
                            FROM RawData
                        ) AS SourceForPivot
                        PIVOT(
                            MAX(ValueText)
                            FOR ColumnName IN([ITEM NO.], [PART NUMBER], [DESCRIPTION])
                        ) AS PivotTable
                    )
                    SELECT
                        p.RowDocumentID,
                        r.RefConfigs,
                        p.[ITEM NO.] AS ITEM_NO,
                        p.[PART NUMBER],
                        p.[DESCRIPTION]
                    FROM Pivoted p
                    JOIN RawData r ON p.RowDocumentID = r.RowDocumentID AND p.RowNo = r.RowNo
                    GROUP BY
                        p.RowDocumentID, r.RefConfigs, p.[ITEM NO.], p.[PART NUMBER], p.[DESCRIPTION]
                    ORDER BY ITEM_NO; ";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Filename", $"%{fileName}%");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable resultTable = new DataTable();
                    adapter.Fill(resultTable);

                    if (resultTable.Rows.Count == 0)
                    {
                        MessageBox.Show("No BOM data found.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        DataTableGrid.ItemsSource = null;
                        return;
                    }

                    // Add COMMENTS Column
                    if (!resultTable.Columns.Contains("COMMENTS"))
                        resultTable.Columns.Add("COMMENTS", typeof(string));

                    // Store the full table for later filtering
                    // Store the full table for later filtering
                    _fullDataTable = resultTable;

                    // Hide these columns in the DataGrid view
                    if (_fullDataTable.Columns.Contains("RowDocumentID"))
                        _fullDataTable.Columns["RowDocumentID"].ColumnMapping = MappingType.Hidden;

                    if (_fullDataTable.Columns.Contains("RefConfigs"))
                        _fullDataTable.Columns["RefConfigs"].ColumnMapping = MappingType.Hidden;

                    // Initially show everything
                    DataTableGrid.ItemsSource = _fullDataTable.DefaultView;

                    // Explicitly hide these columns from the grid view
                    if (DataTableGrid.Columns.Count > 0)
                    {
                        var rowDocumentColumn = DataTableGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "RowDocumentID");
                        if (rowDocumentColumn != null)
                            rowDocumentColumn.Visibility = Visibility.Collapsed;

                        var refConfigsColumn = DataTableGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "RefConfigs");
                        if (refConfigsColumn != null)
                            refConfigsColumn.Visibility = Visibility.Collapsed;
                    }


                    // Now Fetch Comments for each row
                    foreach (DataRow row in _fullDataTable.Rows)
                    {
                        string rowDocId = row["RowDocumentID"].ToString();
                        string comment = "";

                        using (SqlCommand commentCmd = new SqlCommand(@"
                            SELECT TOP 1 vv.ValueText
                            FROM MDV1.dbo.VariableValue vv
                            INNER JOIN MDV1.dbo.Variable v ON v.VariableID = vv.VariableID
                            INNER JOIN dbo.Documents d ON d.DocumentID = vv.DocumentID
                            WHERE v.VariableName = 'COMMENTS'
                              AND d.DocumentID = @RowDocID
                            ORDER BY vv.RevisionNo DESC;", conn))
                        {
                            commentCmd.Parameters.AddWithValue("@RowDocID", rowDocId);
                            object result = commentCmd.ExecuteScalar();
                            if (result != null)
                                comment = result.ToString();
                        }

                        row["COMMENTS"] = comment;
                    }

                    // Populate the dropdown
                    var distinctConfigs = resultTable
                        .AsEnumerable()
                        .Select(row => row.Field<string>("RefConfigs"))
                        .Distinct()
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .ToList();

                    ConfigComboBox.Items.Clear();
                    ConfigComboBox.Items.Add("All");
                    foreach (var config in distinctConfigs)
                    {
                        ConfigComboBox.Items.Add(config);
                    }
                    ConfigComboBox.SelectedIndex = 0;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving BOM data:\n" + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine("Error: " + ex.Message);
            }
        }

        private void ConfigComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_fullDataTable == null || ConfigComboBox.SelectedItem == null)
                return;

            string selectedConfig = ConfigComboBox.SelectedItem.ToString();

            if (selectedConfig == "All")
            {
                DataTableGrid.ItemsSource = _fullDataTable.DefaultView;
            }
            else
            {
                DataView filteredView = new DataView(_fullDataTable)
                {
                    RowFilter = $"RefConfigs = '{selectedConfig}'"
                };
                DataTableGrid.ItemsSource = filteredView;
            }

            HideUnwantedColumns();
        }

        private void HideUnwantedColumns()
        {
            // Wait for the DataGrid to render the columns
            Dispatcher.Invoke(() =>
            {
                if (DataTableGrid.Columns.Count > 0)
                {
                    var rowDocumentColumn = DataTableGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "RowDocumentID");
                    if (rowDocumentColumn != null)
                        rowDocumentColumn.Visibility = Visibility.Collapsed;

                    var refConfigsColumn = DataTableGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "RefConfigs");
                    if (refConfigsColumn != null)
                        refConfigsColumn.Visibility = Visibility.Collapsed;
                }
            }, System.Windows.Threading.DispatcherPriority.Render);
        }



        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Thumb thumb = sender as Thumb;
            if (thumb == null) return;

            double minWidth = this.MinWidth;
            double minHeight = this.MinHeight;

            if (thumb == TopThumb || thumb == TopLeftThumb || thumb == TopRightThumb)
            {
                double delta = e.VerticalChange;
                if (this.Height - delta >= minHeight)
                {
                    this.Top += delta;
                    this.Height -= delta;
                }
            }

            if (thumb == BottomThumb || thumb == BottomLeftThumb || thumb == BottomRightThumb)
            {
                double delta = e.VerticalChange;
                if (this.Height + delta >= minHeight)
                    this.Height += delta;
            }

            if (thumb == LeftThumb || thumb == TopLeftThumb || thumb == BottomLeftThumb)
            {
                double delta = e.HorizontalChange;
                if (this.Width - delta >= minWidth)
                {
                    this.Left += delta;
                    this.Width -= delta;
                }
            }

            if (thumb == RightThumb || thumb == TopRightThumb || thumb == BottomRightThumb)
            {
                double delta = e.HorizontalChange;
                if (this.Width + delta >= minWidth)
                    this.Width += delta;
            }
        }
    }
}
