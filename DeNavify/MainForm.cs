using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DeNavify
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDatabases();
        }

        private void LoadDatabases()
        {
            string server = DbServer.Text.Trim();
            string connectionString = $"Data Source={server};Integrated Security=True";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    DataTable databases = connection.GetSchema("Databases");
                    foreach (DataRow database in databases.Rows)
                    {
                        string databaseName = database.Field<string>("database_name");
                        DbComboBox.Items.Add(databaseName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching databases: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeNavifyButton_Click(object sender, EventArgs e)
        {
            string[] symbols = SymbolBox.Text.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (symbols.Length == 0 ) 
            {
                return;
            }


            string strScript = "REPLACE(@oldTableName, '" + symbols[0] + "', '')";

            for (int i=1; i<symbols.Length; i++)    
            {
                strScript = "REPLACE(" + strScript + ",'" + symbols[i] +"','')";                
            }

            string dbUser = username.Text.Trim();
            string dbPass = password.Text.Trim();
            string server = DbServer.Text.Trim();
            string database = DbComboBox.Text.Trim();


            if (string.IsNullOrEmpty(database))
            {
                MessageBox.Show("Please select a database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = $"Data Source={server};Initial Catalog={database};User ID={dbUser};Password={dbPass}";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    MessageBox.Show("Connection successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    string script = $@"DECLARE @oldTableName NVARCHAR(255)
DECLARE @newTableName NVARCHAR(255)
DECLARE @oldColumnName NVARCHAR(255)
DECLARE @newColumnName NVARCHAR(255)

DECLARE tableCursor CURSOR FOR
SELECT [name]
FROM sys.tables

OPEN tableCursor

FETCH NEXT FROM tableCursor INTO @oldTableName

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @newTableName = "+strScript+@"

    IF @oldTableName <> @newTableName
    BEGIN
        DECLARE @sql NVARCHAR(MAX)
        SET @sql = 'EXEC sp_rename ''' + @oldTableName + ''', ''' + @newTableName + ''''
        EXEC sp_executesql @sql
    END
    -- Now, iterate through columns of the table
    DECLARE columnCursor CURSOR FOR
SELECT [name]
FROM sys.columns
WHERE object_id = OBJECT_ID(@oldTableName)

OPEN columnCursor
FETCH NEXT FROM columnCursor INTO @oldColumnName

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @newColumnName = "+ strScript.Replace("@oldTableName","@oldColumnName") + @"

    IF @oldColumnName <> @newColumnName
    BEGIN
        DECLARE @columnSql NVARCHAR(MAX)
        SET @columnSql = 'EXEC sp_rename ''' + @oldTableName + '.' + @oldColumnName + ''', ''' + @newColumnName + ''', ''COLUMN'''
        EXEC sp_executesql @columnSql
    END

    FETCH NEXT FROM columnCursor INTO @oldColumnName
END

CLOSE columnCursor
DEALLOCATE columnCursor

    FETCH NEXT FROM tableCursor INTO @oldTableName
END

CLOSE tableCursor
DEALLOCATE tableCursor
 ";

                    using (SqlCommand command = new SqlCommand(script,connection))
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        MessageBox.Show($"Table names have been de-navified successfully! Rows affected: {rowsAffected}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
