using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Synergy_Automotive_Ratebooks
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UploadToDatabase("DELETE FROM leases WHERE lease_funder_id = 9;");
            UploadToDatabase("INSERT INTO leases (`lease_cap_id`, `lease_term`, `lease_mileage_annual`, `lease_monthly_cost`, `lease_maintenance`, `lease_ppm`, `lease_ppmm`, `lease_funder_id`, `lease_vat`, `lease_datetime`) select convert(mid(tl.`test_cap_id`,locate('/',tl.`test_cap_id`)-5,5), signed integer) as test_cap_id,tl.`test_term`, tl.`test_mileage_annual`, round(tl.`test_monthly_cost`*(tl.`test_term`+5)/(tl.`test_term`+2),2), case when tl.`test_maintenance` > 0 then tl.`test_maintenance` else 0 end as test_maintenance, case when tl.`test_maintenance` > 0 then 0 else 0 end as test_ppm, case when tl.`test_maintenance` > 0 then 0 else 0 end as test_ppmm, tl.`test_funder`, 0, now() from test_lex tl WHERE tl.test_funder = 9;");
            UploadToDatabase("CREATE TABLE lex5k_10k AS SELECT l.`lease_cap_id`,l.`lease_term`, max(case when l.`lease_mileage_annual` = 5000 then l.`lease_monthly_cost` end) as 5k_price, max(case when l.`lease_mileage_annual` = 10000 then l.`lease_monthly_cost` end) as 10k_price, case when l.`lease_maintenance` > 0 then 1 else 0 end as maintenance_binary, max(case when l.`lease_mileage_annual` = 5000 then l.`lease_maintenance` end) as 5k_main, max(case when l.`lease_mileage_annual` = 10000 then l.`lease_maintenance` end) as 10k_main, l.`lease_ppm`, l.`lease_ppmm` FROM leases l WHERE l.`lease_funder_id` = 9 and (l.`lease_mileage_annual` = 5000 OR l.`lease_mileage_annual` = 10000) GROUP BY l.`lease_cap_id`, l.`lease_term`, `maintenance_binary`;");
            UploadToDatabase("INSERT INTO leases (`lease_funder_id`,`lease_cap_id`,`lease_term`, `lease_mileage_annual`, `lease_initial_rental`, `lease_monthly_cost`, `lease_maintenance`, `lease_ppm`, `lease_ppmm`, `lease_datetime`) select 9 as lease_funder_id, l5.`lease_cap_id`, l5.`lease_term`, 8000 as lease_annual_mileage, 3 as lease_initial_rental, round((l5.`10k_price`-l5.`5k_price`)/5000*3000 + l5.`5k_price`,2) as lease_monthly_cost, round((l5.`10k_main`-l5.`5k_main`)/5000*3000 + l5.`5k_main`,2) as lease_maintenance, l5.`lease_ppm`, l5.`lease_ppmm`, NOW() from `lex5k_10k` l5 WHERE l5.`5k_price` IS NOT NULL and l5.`10k_price` IS NOT NULL and l5.`5k_main` IS NOT NULL and l5.`10k_main` IS NOT NULL;");
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
                MessageBox.Show(ex.Message);
            }
        }
    }
}
