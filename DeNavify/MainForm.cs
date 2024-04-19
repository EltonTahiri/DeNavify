namespace DeNavify
{
    using MaterialSkin;
    using MaterialSkin.Controls;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="MainForm" />
    /// </summary>
    public partial class MainForm : MaterialForm
    {
        /// <summary>
        /// Defines the materialSkinManager
        /// </summary>
        private MaterialSkinManager materialSkinManager;

        // Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            // Initialize MaterialSkinManager
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT; // or DARK
            // Define color scheme
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Blue500, Accent.LightBlue200, TextShade.WHITE);
        }

        /// <summary>
        /// The Form1_Load
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        // Method to load databases into ComboBox

        /// <summary>
        /// The LoadDatabases
        /// </summary>
        private void LoadDatabases()
        {
            string server = DbServer.Text.Trim();
            string connectionString = $"Data Source={server};User Id={username.Text}; Password={password.Text};";
            DbComboBox.Items.Clear();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Retrieve database schema
                    DataTable databases = connection.GetSchema("Databases");
                    foreach (DataRow database in databases.Rows)
                    {
                        // Add database names to ComboBox
                        string databaseName = database.Field<string>("database_name");
                        DbComboBox.Items.Add(databaseName);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        // Event handler for DeNavifyButton click

        /// <summary>
        /// The DeNavifyButton_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void DeNavifyButton_Click(object sender, EventArgs e)
        {
            // Split symbols entered in SymbolBox by comma
            string[] symbols = SymbolBox.Text.Split(',', StringSplitOptions.RemoveEmptyEntries);
            // Check if no symbols are entered
            if (symbols.Length == 0)
            {
                return;
            }

            // Check for invalid symbol "--"
            for (int i = 0; i < symbols.Length; i++)
            {
                if (symbols[i] == "--")
                {
                    MessageBox.Show("Invalid Text", "Error Encountered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Build SQL script to remove symbols from table and column names
            string strScript = "REPLACE(@oldTableName, '" + symbols[0] + "', '')";
            for (int i = 1; i < symbols.Length; i++)
            {
                strScript = "REPLACE(" + strScript + ",'" + symbols[i] + "','')";
            }

            // Get database credentials and other information
            string dbUser = username.Text.Trim();
            string dbPass = password.Text.Trim();
            string server = DbServer.Text.Trim();
            string database = DbComboBox.Text.Trim();

            // Check if database is selected
            if (string.IsNullOrEmpty(database))
            {
                MessageBox.Show("Please select a database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Build connection string
            string connectionString = $"Data Source={server};Initial Catalog={database};User ID={dbUser};Password={dbPass}";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Build SQL script to rename tables and columns
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
                        SET @newTableName = " + strScript + @"

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
                        SET @newColumnName = " + strScript.Replace("@oldTableName", "@oldColumnName") + @"

                        IF @oldColumnName <> @newColumnName
                        BEGIN
                            DECLARE @columnSql NVARCHAR(MAX)
                            SET @columnSql = 'EXEC sp_rename ''' + @oldTableName + '.' + @oldColumnName + ''', ''' + @newColumnName + ''', ''COLUMN'''
                            EXEC sp_executesql @columnSql
                            PRINT 'Column ' + @oldColumnName + ' in table ' + @oldTableName + ' has been renamed to ' + @newColumnName
                        END

                        FETCH NEXT FROM columnCursor INTO @oldColumnName
                    END

                    CLOSE columnCursor
                    DEALLOCATE columnCursor

                        FETCH NEXT FROM tableCursor INTO @oldTableName
                    END

                    CLOSE tableCursor
                    DEALLOCATE tableCursor";

                    // SQL query to retrieve tables matching symbols
                    string sql = String.Format("SELECT * FROM INFORMATION_SCHEMA.TABLES t " +
                    "WHERE T.TABLE_CATALOG = '{0}' AND T.TABLE_TYPE='TABLE' AND (", database);

                    for (int i = 0; i < symbols.Length; i++)
                    {
                        switch (symbols[i])
                        {
                            case "_":
                            case "%":
                                sql += String.Format("t.TABLE_NAME LIKE '%\\_%' ESCAPE '\\' OR t.TABLE_NAME LIKE '%\\%%' ESCAPE '\\'", symbols[i]);
                                break;
                            default:
                                sql += String.Format("t.TABLE_NAME like '%{0}%'", symbols[i]);
                                break;
                        }

                        if (i < symbols.Length - 1)
                            sql += " OR ";
                    }
                    sql += ")";

                    // SQL query to retrieve columns matching symbols
                    string sqlFields = "select * from INFORMATION_SCHEMA.COLUMNS c INNER JOIN INFORMATION_SCHEMA.TABLES t on t.TABLE_NAME = c.TABLE_NAME and t.TABLE_CATALOG = c.TABLE_CATALOG where c.TABLE_CATALOG = '" + "'" + database + "' " + "AND t.TABLE_TYPE='TABLE' and";

                    // List to store tables and their changed fields
                    List<ChangedTable> tablesToBeChanged = new List<ChangedTable>();

                    // Retrieve tables matching symbols
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, connection))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            da.Fill(ds);
                            DataTable dt = ds.Tables[0];
                            string strTablename = String.Empty;

                            // For each table extract columns containing special characters
                            foreach (DataRow dr in dt.Rows)
                            {
                                strTablename = dr["TABLE_NAME"].ToString();

                                // Add table to be changed
                                ChangedTable changedTable = new ChangedTable();
                                changedTable.Name = strTablename;

                                // Build SQL query to retrieve columns of the table
                                sqlFields += String.Format(" AND c.TABLE_NAME = '{0}' AND (", strTablename);

                                // Add WHERE clause to search for columns containing symbols in their names                                
                                for (int i = 0; i < symbols.Length; i++)
                                {
                                    switch (symbols[i])
                                    {
                                        case "_":
                                        case "%":
                                            sqlFields += String.Format("c.COLUMN_NAME like '%\\_%' ESCAPE '\\'  OR c.COLUMN_NAME like '%\\%%' ESCAPE '\\'", symbols[i]);
                                            break;
                                        default:
                                            sqlFields += String.Format("c.COLUMN_NAME like '%{0}%'", symbols[i]);
                                            break;
                                    }

                                    if (i < symbols.Length - 1)
                                        sqlFields += " OR ";
                                }
                                sqlFields += ")";

                                // Retrieve columns matching symbols for the table
                                using (SqlDataAdapter daFields = new SqlDataAdapter(sqlFields, connection))
                                {
                                    using (DataSet dsFields = new DataSet())
                                    {
                                        daFields.Fill(dsFields);
                                        DataTable dtFields = dsFields.Tables[0];
                                        string strFieldName = "";

                                        // For each column, add it to the changedTable
                                        foreach (DataRow drField in dtFields.Rows)
                                        {
                                            strFieldName = drField["COLUMN_NAME"].ToString();
                                            changedTable.ChangedFields.Add(new ChangedFields() { Name = strFieldName });
                                        }
                                    }
                                }
                                // Add the changed table to the list
                                tablesToBeChanged.Add(changedTable);
                            }
                        }
                    }

                    // SQL query to retrieve columns matching symbols independent of table names
                    string sqlFieldsInd = "select * from INFORMATION_SCHEMA.COLUMNS c INNER JOIN INFORMATION_SCHEMA.TABLES t on t.TABLE_NAME = c.TABLE_NAME and t.TABLE_CATALOG = c.TABLE_CATALOG where c.TABLE_CATALOG = '" + database + "'";
                    sqlFieldsInd += " AND (";

                    // Build WHERE clause to search for columns containing symbols in their names
                    for (int i = 0; i < symbols.Length; i++)
                    {
                        switch (symbols[i])
                        {
                            case "_":
                            case "%":
                                sqlFieldsInd += String.Format("c.COLUMN_NAME like '%\\_%' ESCAPE '\\'  OR c.COLUMN_NAME like '%\\%%' ESCAPE '\\'", symbols[i]);
                                break;
                            default:
                                sqlFieldsInd += String.Format("c.COLUMN_NAME like '%{0}%'", symbols[i]);
                                break;
                        }

                        if (i < symbols.Length - 1)
                            sqlFieldsInd += " OR ";
                    }
                    sqlFieldsInd += ")";

                    // Retrieve columns matching symbols independent of table names
                    using (SqlDataAdapter daFields = new SqlDataAdapter(sqlFieldsInd, connection))
                    {
                        using (DataSet dsFields = new DataSet())
                        {
                            daFields.Fill(dsFields);
                            DataTable dtFields = dsFields.Tables[0];
                            string strFieldName = "";

                            // For each column, add it to the tablesToBeChanged list with its table name
                            foreach (DataRow drField in dtFields.Rows)
                            {
                                ChangedTable changedFieldTable = new ChangedTable();
                                strFieldName = drField["COLUMN_NAME"].ToString();
                                changedFieldTable.Name = drField["TABLE_NAME"].ToString();
                                changedFieldTable.ChangedFields.Add(new ChangedFields() { Name = strFieldName });
                                tablesToBeChanged.Add(changedFieldTable);
                            }
                        }
                    }

                    // Execute the SQL script to rename tables and columns
                    using (SqlCommand command = new SqlCommand(script, connection))
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        string strMessage = "The following elements have been altered: \n";

                        // Build a message showing the changed tables and fields
                        foreach (ChangedTable t in tablesToBeChanged)
                        {
                            strMessage += "\nTABLE: " + t.Name;
                            foreach (ChangedFields f in t.ChangedFields)
                            {
                                strMessage += "\n\t FIELD: " + f.Name;
                            }
                        }

                        // Show message indicating the changes made
                        if (tablesToBeChanged.Count > 0)
                        {
                            MessageBox.Show(strMessage);
                        }
                        else
                        {
                            MessageBox.Show("No changes have been made");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Show error message if an exception occurs during the process
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// The label3_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void label3_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The DbComboBox_SelectedIndexChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void DbComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The label2_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void label2_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The DbServer_TextChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void DbServer_TextChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The DbPass_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void DbPass_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The SymbolBox_TextChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void SymbolBox_TextChanged(object sender, EventArgs e)
        {
        }

        private void DbComboBox_Enter(object sender, EventArgs e)
        {
            LoadDatabases();
        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }
    }
}
