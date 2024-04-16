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

            for (int i = 0; i < symbols.Length; i++)
            {
                if (symbols[i] == "--")
                {
                    MessageBox.Show("Invalid Text","Error Encountered", MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
            }   


            string strScript = "REPLACE(@oldTableName, '" + symbols[0] + "', '')";

          
            for (int i=1; i<symbols.Length; i++)    
            {
                strScript = "REPLACE(" + strScript + ",'" + symbols[i] + "','')";
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
                    DEALLOCATE tableCursor
                     ";

                    string sql = String.Format("SELECT * FROM INFORMATION_SCHEMA.TABLES t where T.TABLE_CATALOG = '{0}' AND (", database);

                    for (int i=0; i<symbols.Length; i++)
                    {
                        sql += String.Format("t.TABLE_NAME like '%{0}%'", symbols[i]);
                        if (i < symbols.Length - 1)
                            sql += " OR ";
                    }

                    sql += ")";

                    string sqlFields = "select * from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_CATALOG = '"+database+"'";


                    List<ChangedTable> tablesToBeChanged = new List<ChangedTable>();
                    

                    using (SqlDataAdapter da = new SqlDataAdapter(sql, connection))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            da.Fill(ds);
                            DataTable dt = ds.Tables[0];
                            string strTablename = String.Empty;
                            //For each table extract columns containing special chars
                            foreach (DataRow dr in dt.Rows)
                            {
                                strTablename = dr["TABLE_NAME"].ToString();
                                
                                //Add table to be changed
                                ChangedTable changedTable = new ChangedTable();
                                changedTable.Name = strTablename;


                                sqlFields += String.Format(" AND c.TABLE_NAME = '{0}' AND (", strTablename);

                                for (int i = 0; i < symbols.Length; i++)
                                {
                                    sqlFields += String.Format("c.COLUMN_NAME like '%{0}%'", symbols[i]);
                                    if (i < symbols.Length - 1)
                                        sqlFields += " OR ";
                                }
                                
                                sqlFields += ")";

                                using (SqlDataAdapter daFields = new SqlDataAdapter(sqlFields, connection))
                                {      
                                    using (DataSet dsFields = new DataSet())
                                    {
                                        daFields.Fill(dsFields);
                                        DataTable dtFields = dsFields.Tables[0];
                                        string strFieldName = "";

                                        foreach (DataRow drField in dtFields.Rows)
                                        {
                                            strFieldName = drField["COLUMN_NAME"].ToString();

                                            changedTable.ChangedFields.Add(new ChangedFields()
                                            { Name = strFieldName});

                                        }

                                    }

                                }
                                tablesToBeChanged.Add(changedTable);
                            }
                        }
                    }

                    using (SqlCommand command = new SqlCommand(script,connection))
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        string strMessage = "The following elements have been altered: \n";

                        
                        foreach (ChangedTable t in tablesToBeChanged)
                        {
                            strMessage += "\nTABLE: " + t.Name;
                            foreach (ChangedFields f in t.ChangedFields)
                            {
                                strMessage += "\n\t FIELD: "+f.Name;   
                            }
                        }

                        MessageBox.Show(strMessage);

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
