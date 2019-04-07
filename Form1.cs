using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data;
using Microsoft.VisualBasic.FileIO;

namespace Synergy_Automotive_Ratebooks
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string GetDriveLocation()
        {
            string drive = @"C:\Users\JohnHughes\Downloads\";
            return drive;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int rates = 0;
            Form1 f1 = new Form1();
            string drive = f1.GetDriveLocation();
            //string[] filebox = new string[] { @"C:\Users\JohnHughes\Downloads\ALD\ALD 8k nm.csv", @"C:\Users\JohnHughes\Downloads\ALD\ALD 8k wm.csv", @"C:\Users\JohnHughes\Downloads\ALD\ALD 5k nm.csv", @"C:\Users\JohnHughes\Downloads\ALD\ALD 5k wm.csv", @"C:\Users\JohnHughes\Downloads\ALD\ALD 24 nm.csv", @"C:\Users\JohnHughes\Downloads\ALD\ALD 24 wm.csv", @"C:\Users\JohnHughes\Downloads\ALD\ALD 36 nm.csv", @"C:\Users\JohnHughes\Downloads\ALD\ALD 36 wm.csv", @"C:\Users\JohnHughes\Downloads\ALD\ALD 48 nm.csv", @"C:\Users\JohnHughes\Downloads\ALD\ALD 48 wm.csv", @"C:\Users\JohnHughes\Downloads\ALD\ALD LCV wm.csv" };
            string[] filebox = new string[] { drive + @"ALD\ALD 5k nm.csv" };
            foreach (string fil in filebox)
            {
                var dt = GetALDTable(fil);
                if (dt.Rows.Count > 0)
                {
                    var MyCsv = ToCsv(dt);
                    System.IO.File.WriteAllText(@"C:\Users\JohnHughes\Downloads\ald_temp.csv", MyCsv);
                    MyMySQLConnector(@"C:\Users\JohnHughes\Downloads\ald_temp.csv","test");
                    rates += dt.Rows.Count;
                }
            }
            
            //UploadToDatabase("INSERT INTO leases (`lease_cap_id`, `lease_term`, `lease_mileage_annual`, `lease_monthly_cost`, `lease_maintenance`, `lease_ppm`, `lease_ppmm`, `lease_funder_id`, `lease_vat`, `lease_datetime`) SELECT t.`test_cap_id`, t.`test_term`, t.`test_mileage_annual`, t.`test_monthly_cost`, case when t.`test_maintenance` > 0 then t.`test_maintenance`-t.`test_monthly_cost` else 0 end as test_maintenance, case when t.`test_maintenance` > 0 then 0 else 0.01 * t.`test_ppm` end as test_ppm, case when t.`test_maintenance` > 0 then 0.01 * t.`test_ppm` else 0 end as test_ppmm, t.`test_funder`, 0, now() FROM test t WHERE t.`test_cap_id` > 0 and t.test_funder = 4;");
            System.Windows.Forms.MessageBox.Show("The Ratebooks were uploaded successfully with " + rates + " lines.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] filebox = new string[] { @"C:\Users\JohnHughes\Downloads\Arval\3-36-10K.csv" };
            foreach (string fil in filebox)
            {
                var dt = GetArvalTable(fil);
                if (dt.Rows.Count > 0)
                {
                    var MyCsv = ToCsv(dt);
                    System.IO.File.WriteAllText(@"C:\Users\JohnHughes\Downloads\arval_temp.csv", MyCsv);
                    MyMySQLConnector(@"C:\Users\JohnHughes\Downloads\arval_temp.csv","test");
                    System.Windows.Forms.MessageBox.Show("The Ratebook " + fil + " was uploaded successfully with " + dt.Rows.Count + " lines.");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] filebox = new string[] { @"C:\Users\JohnHughes\Downloads\Lex\CH 24.csv", @"C:\Users\JohnHughes\Downloads\Lex\CH 36.csv", @"C:\Users\JohnHughes\Downloads\Lex\CH 48.csv", @"C:\Users\JohnHughes\Downloads\Lex\CH 60.csv", @"C:\Users\JohnHughes\Downloads\Lex\CHNM 24.csv", @"C:\Users\JohnHughes\Downloads\Lex\CHNM 36.csv", @"C:\Users\JohnHughes\Downloads\Lex\CHNM 48.csv", @"C:\Users\JohnHughes\Downloads\Lex\CHNM 60.csv" };
            int rates = 0;
            foreach (string fil in filebox)
            {
                
                var dt = GetLexTable(fil);
                if (dt.Rows.Count > 0)
                {
                    var MyCsv = ToCsv(dt);
                    System.IO.File.WriteAllText(@"C:\Users\JohnHughes\Downloads\lex_temp.csv", MyCsv);
                    MyMySQLConnector(@"C:\Users\JohnHughes\Downloads\lex_temp.csv","test_lex");
                    rates += dt.Rows.Count;
                }
                
            }
            System.Windows.Forms.MessageBox.Show("The Ratebooks were uploaded successfully with " + rates + " lines.");
            this.Hide();
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //, @"C:\Users\JohnHughes\Downloads\Leaseplan\Contract Hire_8k.csv" 
            string[] filebox = new string[] {@"C:\Users\JohnHughes\Downloads\Leaseplan\Contract Hire.csv", @"C:\Users\JohnHughes\Downloads\Leaseplan\Contract Hire_8k.csv"};
            foreach (string fil in filebox)
            {
                var dt = GetLeaseplanTable(fil);
                if (dt.Rows.Count > 0)
                {
                    var MyCsv = ToCsv(dt);
                    System.IO.File.WriteAllText(@"C:\Users\JohnHughes\Downloads\leaseplan_temp.csv", MyCsv);
                    MyMySQLConnector(@"C:\Users\JohnHughes\Downloads\leaseplan_temp.csv", "test");
                    System.Windows.Forms.MessageBox.Show("The Ratebook " + fil + " was uploaded successfully with " + dt.Rows.Count + " lines.");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string[] single_nm = new string[] { };
            string[] double_nm = new string[] { @"C:\Users\JohnHughes\Downloads\Hitachi\Hit-24-1015k-nm.csv",  @"C:\Users\JohnHughes\Downloads\Hitachi\Hit-24-1015k-nmv.csv", @"C:\Users\JohnHughes\Downloads\Hitachi\Hit-24-2025k-nm.csv", @"C:\Users\JohnHughes\Downloads\Hitachi\Hit-24-2025k-nmv.csv" };
            string[] single_wm = new string[] { };
            string[] double_wm = new string[] { @"C:\Users\JohnHughes\Downloads\Hitachi\Hit-24-1015k-wm.csv", @"C:\Users\JohnHughes\Downloads\Hitachi\Hit-24-1015k-wmv.csv", @"C:\Users\JohnHughes\Downloads\Hitachi\Hit-24-2025k-wm.csv", @"C:\Users\JohnHughes\Downloads\Hitachi\Hit-24-2025k-wmv.csv" };
            foreach (string fil in double_nm)
            {
                var dt = GetHitachiTable_2(fil);
                if (dt.Rows.Count > 0)
                {
                    var MyCsv = ToCsv(dt);
                    System.IO.File.WriteAllText(@"C:\Users\JohnHughes\Downloads\hitachi_temp.csv", MyCsv);
                    MyMySQLConnector(@"C:\Users\JohnHughes\Downloads\hitachi_temp.csv", "test");
                    System.Windows.Forms.MessageBox.Show("The Ratebook " + fil + " was uploaded successfully with " + dt.Rows.Count + " lines.");
                }
            }

            foreach (string fil in double_wm)
            {
                var dt = GetHitachiMaintainedTable_2(fil);
                if (dt.Rows.Count > 0)
                {
                    var MyCsv = ToCsv(dt);
                    System.IO.File.WriteAllText(@"C:\Users\JohnHughes\Downloads\hitachi_temp.csv", MyCsv);
                    MyMySQLConnector(@"C:\Users\JohnHughes\Downloads\hitachi_temp.csv", "test");
                    System.Windows.Forms.MessageBox.Show("The Ratebook " + fil + " was uploaded successfully with " + dt.Rows.Count + " lines.");
                }
            }
        }

        private static DataTable GetALDTable(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    //    foreach (string column in colFields)
                    //    {
                    //        DataColumn datecolumn = new DataColumn(column);
                    //        datecolumn.AllowDBNull = true;
                    //        csvData.Columns.Add(datecolumn);
                    //    }
                    DataColumn cap = new DataColumn(colFields[24]);
                    cap.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(cap);
                    DataColumn term = new DataColumn(colFields[0]);
                    term.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(term);
                    DataColumn miles = new DataColumn(colFields[1]);
                    miles.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(miles);
                    DataColumn finance = new DataColumn(colFields[12]);
                    finance.DataType = System.Type.GetType("System.Decimal");
                    csvData.Columns.Add(finance);
                    DataColumn maintenance = new DataColumn(colFields[11]);
                    maintenance.DataType = System.Type.GetType("System.Decimal");
                    csvData.Columns.Add(maintenance);
                    DataColumn ppm = new DataColumn(colFields[25]);
                    ppm.DataType = System.Type.GetType("System.Decimal");
                    csvData.Columns.Add(ppm);
                    csvData.Columns.Add("Funder");
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            //Console.WriteLine(csvData.Columns[0].ColumnName);
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData[24], fieldData[0], fieldData[1], fieldData[12], fieldData[11], fieldData[25], 4);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return csvData;
        }

        private static DataTable GetArvalTable(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    //    foreach (string column in colFields)
                    //    {
                    //        DataColumn datecolumn = new DataColumn(column);
                    //        datecolumn.AllowDBNull = true;
                    //        csvData.Columns.Add(datecolumn);
                    //    }
                    DataColumn cap = new DataColumn(colFields[10]);
                    cap.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(cap);
                    DataColumn term = new DataColumn(colFields[23]);
                    term.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(term);
                    DataColumn miles = new DataColumn(colFields[19]);
                    miles.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(miles);
                    DataColumn finance = new DataColumn(colFields[5]);
                    finance.DataType = System.Type.GetType("System.Decimal");
                    csvData.Columns.Add(finance);
                    DataColumn maintenance = new DataColumn(colFields[6]);
                    maintenance.DataType = System.Type.GetType("System.Decimal");
                    csvData.Columns.Add(maintenance);
                    csvData.Columns.Add("PPM");
                    csvData.Columns.Add("Funder");
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            //Console.WriteLine(csvData.Columns[0].ColumnName);
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData[10], fieldData[23], fieldData[19], fieldData[5], fieldData[6], 0, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return csvData;
        }

        private static DataTable GetHitachiTable(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    //    foreach (string column in colFields)
                    //    {
                    //        DataColumn datecolumn = new DataColumn(column);
                    //        datecolumn.AllowDBNull = true;
                    //        csvData.Columns.Add(datecolumn);
                    //    }
                    DataColumn cap = new DataColumn(colFields[0]);
                    cap.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(cap);
                    DataColumn term = new DataColumn(colFields[21]);
                    term.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(term);
                    DataColumn miles = new DataColumn(colFields[22]);
                    miles.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(miles);
                    DataColumn finance = new DataColumn(colFields[23]);
                    finance.DataType = System.Type.GetType("System.Decimal");
                    csvData.Columns.Add(finance);
                    DataColumn maintenance = new DataColumn();
                    maintenance.DataType = System.Type.GetType("System.Decimal");
                    csvData.Columns.Add(maintenance);
                    DataColumn ppm = new DataColumn(colFields[28]);
                    ppm.DataType = System.Type.GetType("System.Decimal");
                    csvData.Columns.Add(ppm);
                    csvData.Columns.Add("Funder");
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            //Console.WriteLine(csvData.Columns[0].ColumnName);
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData[0], fieldData[21], fieldData[22], fieldData[23], 0, 0, 5);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return csvData;
        }

        private static DataTable GetHitachiTable_2(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    //    foreach (string column in colFields)
                    //    {
                    //        DataColumn datecolumn = new DataColumn(column);
                    //        datecolumn.AllowDBNull = true;
                    //        csvData.Columns.Add(datecolumn);
                    //    }
                    DataColumn cap = new DataColumn(colFields[0]);
                    cap.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(cap);
                    DataColumn term = new DataColumn(colFields[21]);
                    term.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(term);
                    DataColumn miles = new DataColumn(colFields[22]);
                    miles.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(miles);
                    DataColumn finance = new DataColumn(colFields[23]);
                    finance.DataType = System.Type.GetType("System.Decimal");
                    csvData.Columns.Add(finance);
                    DataColumn maintenance = new DataColumn();
                    maintenance.DataType = System.Type.GetType("System.Decimal");
                    csvData.Columns.Add(maintenance);
                    DataColumn ppm = new DataColumn(colFields[26]);
                    ppm.DataType = System.Type.GetType("System.Decimal");
                    csvData.Columns.Add(ppm);
                    csvData.Columns.Add("Funder");
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            //Console.WriteLine(csvData.Columns[0].ColumnName);
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData[0], fieldData[21], fieldData[22], fieldData[23], 0, fieldData[26], 5);
                        csvData.Rows.Add(fieldData[0], fieldData[27], fieldData[28], fieldData[29], 0, fieldData[32], 5);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return csvData;
        }

        private static DataTable GetHitachiMaintainedTable(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    //    foreach (string column in colFields)
                    //    {
                    //        DataColumn datecolumn = new DataColumn(column);
                    //        datecolumn.AllowDBNull = true;
                    //        csvData.Columns.Add(datecolumn);
                    //    }
                    DataColumn cap = new DataColumn(colFields[0]);
                    cap.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(cap);
                    DataColumn term = new DataColumn(colFields[21]);
                    term.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(term);
                    DataColumn miles = new DataColumn(colFields[22]);
                    miles.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(miles);
                    DataColumn finance = new DataColumn(colFields[23]);
                    finance.DataType = System.Type.GetType("System.Decimal");
                    csvData.Columns.Add(finance);
                    DataColumn maintenance = new DataColumn(colFields[24]);
                    maintenance.DataType = System.Type.GetType("System.Decimal");
                    csvData.Columns.Add(maintenance);
                    DataColumn ppm = new DataColumn(colFields[28]);
                    ppm.DataType = System.Type.GetType("System.Decimal");
                    csvData.Columns.Add(ppm);
                    csvData.Columns.Add("Funder");
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            //Console.WriteLine(csvData.Columns[0].ColumnName);
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData[0], fieldData[21], fieldData[22], fieldData[23], fieldData[24], fieldData[28], 5);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return csvData;
        }

        private static DataTable GetHitachiMaintainedTable_2(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    //    foreach (string column in colFields)
                    //    {
                    //        DataColumn datecolumn = new DataColumn(column);
                    //        datecolumn.AllowDBNull = true;
                    //        csvData.Columns.Add(datecolumn);
                    //    }
                    DataColumn cap = new DataColumn(colFields[0]);
                    cap.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(cap);
                    DataColumn term = new DataColumn(colFields[21]);
                    term.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(term);
                    DataColumn miles = new DataColumn(colFields[22]);
                    miles.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(miles);
                    DataColumn finance = new DataColumn(colFields[23]);
                    finance.DataType = System.Type.GetType("System.Decimal");
                    csvData.Columns.Add(finance);
                    DataColumn maintenance = new DataColumn(colFields[24]);
                    maintenance.DataType = System.Type.GetType("System.Decimal");
                    csvData.Columns.Add(maintenance);
                    DataColumn ppm = new DataColumn(colFields[28]);
                    ppm.DataType = System.Type.GetType("System.Decimal");
                    csvData.Columns.Add(ppm);
                    csvData.Columns.Add("Funder");
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            //Console.WriteLine(csvData.Columns[0].ColumnName);
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData[0], fieldData[21], fieldData[22], fieldData[23], fieldData[24], fieldData[28], 5);
                        csvData.Rows.Add(fieldData[0], fieldData[29], fieldData[30], fieldData[31], fieldData[32], fieldData[36], 5);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return csvData;
        }

        private static DataTable GetLeaseplanTable(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    //    foreach (string column in colFields)
                    //    {
                    //        DataColumn datecolumn = new DataColumn(column);
                    //        datecolumn.AllowDBNull = true;
                    //        csvData.Columns.Add(datecolumn);
                    //    }
                    DataColumn cap = new DataColumn(colFields[0]);
                    cap.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(cap);
                    DataColumn term = new DataColumn(colFields[12]);
                    term.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(term);
                    DataColumn miles = new DataColumn(colFields[13]);
                    miles.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(miles);
                    DataColumn finance = new DataColumn(colFields[17]);
                    finance.DataType = System.Type.GetType("System.Decimal");
                    csvData.Columns.Add(finance);
                    DataColumn maintenance = new DataColumn(colFields[18]);
                    maintenance.DataType = System.Type.GetType("System.Decimal");
                    csvData.Columns.Add(maintenance);
                    csvData.Columns.Add("PPM");
                    csvData.Columns.Add("Funder");
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            //Console.WriteLine(csvData.Columns[0].ColumnName);
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData[0], fieldData[12], fieldData[13], fieldData[17], fieldData[18], 0, 3);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return csvData;
        }

        private static DataTable GetLexTable(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    csvReader.TrimWhiteSpace = true;
                    string[] colFields = csvReader.ReadFields();
                    //    foreach (string column in colFields)
                    //    {
                    //        DataColumn datecolumn = new DataColumn(column);
                    //        datecolumn.AllowDBNull = true;
                    //        csvData.Columns.Add(datecolumn);
                    //    }
                    DataColumn cap = new DataColumn(colFields[14]);
                    csvData.Columns.Add(cap);
                    cap.DataType = System.Type.GetType("System.String");
                    DataColumn term = new DataColumn(colFields[7]);
                    term.DataType = System.Type.GetType("System.Int32");
                    csvData.Columns.Add(term);
                    DataColumn miles = new DataColumn(colFields[8]);
                    csvData.Columns.Add(miles);
                    DataColumn finance = new DataColumn(colFields[22]);
                    csvData.Columns.Add(finance);
                    DataColumn maintenance = new DataColumn(colFields[23]);
                    csvData.Columns.Add(maintenance);
                    csvData.Columns.Add("PPM");
                    csvData.Columns.Add("Funder");
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            //Console.WriteLine(csvData.Columns[0].ColumnName);
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData[14], fieldData[7], fieldData[8], fieldData[22], fieldData[23], 0, 9);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return csvData;
        }

        static void MyMySQLConnector(string filepath, string table)
        {
            string connStr = "datasource=160.153.129.221;port=3306;UID=farmison_john;password=Boro2902;database=synergy_auto;";
            // MySql Connection Object
            MySqlConnection conn = new MySqlConnection(connStr);

            //  csv file path
            string file = filepath;

            // MySQL BulkLoader
            MySqlBulkLoader bl = new MySqlBulkLoader(conn);
            bl.TableName = table;
            bl.FieldTerminator = ",";
            bl.LineTerminator = "\n";
            bl.NumberOfLinesToSkip = 1;
            bl.FileName = file;

            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Upload data from file
                int count = bl.Load();
                Console.WriteLine(count + " lines uploaded.");

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("Done.");
            Console.ReadLine();

        }

        public static string ToCsv(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            return sb.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Deleting leases");
            UploadToDatabase("DELETE FROM leases WHERE lease_funder_id = 9;");
            System.Windows.Forms.MessageBox.Show("Inserting leases");
            UploadToDatabase("INSERT INTO leases (`lease_cap_id`, `lease_term`, `lease_mileage_annual`, `lease_monthly_cost`, `lease_maintenance`, `lease_ppm`, `lease_ppmm`, `lease_funder_id`, `lease_vat`, `lease_datetime`) select convert(mid(tl.`test_cap_id`,locate('/',tl.`test_cap_id`)-5,5), signed integer) as test_cap_id,tl.`test_term`, tl.`test_mileage_annual`, round(tl.`test_monthly_cost`*(tl.`test_term`+5)/(tl.`test_term`+2),2), case when tl.`test_maintenance` > 0 then tl.`test_maintenance` else 0 end as test_maintenance, case when tl.`test_maintenance` > 0 then 0 else 0 end as test_ppm, case when tl.`test_maintenance` > 0 then 0 else 0 end as test_ppmm, tl.`test_funder`, 0, now() from test_lex tl WHERE tl.test_funder = 9;");
            System.Windows.Forms.MessageBox.Show("Creating 8k");
            UploadToDatabase("DROP TABLE IF EXISTS lex5k_10k;");
            UploadToDatabase("CREATE TABLE lex5k_10k AS SELECT l.`lease_cap_id`,l.`lease_term`, max(case when l.`lease_mileage_annual` = 5000 then l.`lease_monthly_cost` end) as 5k_price, max(case when l.`lease_mileage_annual` = 10000 then l.`lease_monthly_cost` end) as 10k_price, case when l.`lease_maintenance` > 0 then 1 else 0 end as maintenance_binary, max(case when l.`lease_mileage_annual` = 5000 then l.`lease_maintenance` end) as 5k_main, max(case when l.`lease_mileage_annual` = 10000 then l.`lease_maintenance` end) as 10k_main, l.`lease_ppm`, l.`lease_ppmm` FROM leases l WHERE l.`lease_funder_id` = 9 and (l.`lease_mileage_annual` = 5000 OR l.`lease_mileage_annual` = 10000) GROUP BY l.`lease_cap_id`, l.`lease_term`, `maintenance_binary`;");
            System.Windows.Forms.MessageBox.Show("Adding 8k");
            UploadToDatabase("INSERT INTO leases (`lease_funder_id`,`lease_cap_id`,`lease_term`, `lease_mileage_annual`, `lease_initial_rental`, `lease_monthly_cost`, `lease_maintenance`, `lease_ppm`, `lease_ppmm`, `lease_datetime`) select 9 as lease_funder_id, l5.`lease_cap_id`, l5.`lease_term`, 8000 as lease_annual_mileage, 3 as lease_initial_rental, round((l5.`10k_price`-l5.`5k_price`)/5000*3000 + l5.`5k_price`,2) as lease_monthly_cost, round((l5.`10k_main`-l5.`5k_main`)/5000*3000 + l5.`5k_main`,2) as lease_maintenance, l5.`lease_ppm`, l5.`lease_ppmm`, NOW() from `lex5k_10k` l5 WHERE l5.`5k_price` IS NOT NULL and l5.`10k_price` IS NOT NULL and l5.`5k_main` IS NOT NULL and l5.`10k_main` IS NOT NULL;");
            UploadToDatabase("UPDATE leases SET `lease_funder_id` = 29 WHERE `lease_funder_id` = 9;");
            UploadToDatabase("INSERT INTO leases (`lease_funder_id`,`lease_cap_id`,`lease_term`, `lease_mileage_annual`, `lease_initial_rental`, `lease_monthly_cost`, `lease_maintenance`, `lease_ppm`, `lease_ppmm`, `lease_datetime`) select 9, l.`lease_cap_id`, l.`lease_term`, l.`lease_mileage_annual`, l.`lease_initial_rental`, max(l.`lease_monthly_cost`), max(l.`lease_maintenance`), max(l.`lease_ppm`), max(l.`lease_ppmm`), l.`lease_datetime` from leases l where l.`lease_funder_id` = 29 GROUP BY l.`lease_cap_id`, l.`lease_term`, l.`lease_mileage_annual`;");
            UploadToDatabase("DELETE FROM leases WHERE `lease_funder_id` = 29;");
            System.Windows.Forms.MessageBox.Show("All finished!");
        }

        private static void UploadToDatabase(string query)
        {
            try
            {
                //This is my connection string i have assigned the database file address path  
                string MyConnection2 = "datasource = 160.153.129.221; port = 3306; UID = farmison_john; password = Boro2902; database = synergy_auto;";
                //This is my insert query in which i am taking input from the user through windows forms  
                string Query = query;
                //This is  MySqlConnection here i have created the object and pass my connection string.  
                MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
                //This is command class which will handle the query and connection object.  
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                MySqlDataReader MyReader2;
                MyCommand2.CommandTimeout = 120;
                MyConn2.Open();
                MyReader2 = MyCommand2.ExecuteReader();     // Here our query will be executed and data saved into the database.  
                MessageBox.Show("Save Data");
                while (MyReader2.Read())
                {
                }
                MyConn2.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
