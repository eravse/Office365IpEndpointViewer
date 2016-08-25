using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using RestSharp;

namespace Eravse.Tool.O365IPAddresses
{
    public partial class Form1 : Form
    {
        public DataSet IpDataset { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            this.IpDataset = new DataSet();

      

            var client = new RestClient(textBox1.Text);
            var request = new RestRequest(Method.GET);
               IRestResponse response = client.Execute(request);


            var xml = response.Content;
            
         
            StringReader xmlSR = new StringReader(xml);


            this.IpDataset.ReadXml(xmlSR);

            DataTable dt = IpDataset.Tables[1];
            comboBox1.DataSource =dt;
            comboBox1.ValueMember = "product_Id";
            comboBox1.DisplayMember = "name";



        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox) sender;
            var value = cmb.SelectedValue;



            int n;
            bool isNumeric = int.TryParse(value.ToString(), out n);

            if (isNumeric)
            {

                
                var source = this.IpDataset.Tables[2].Select("product_Id = "+ value.ToString());
                foreach (var dataRow in source)
                {
                    if (dataRow["type"].ToString() == "URL")
                    {
                
                            
                            var ds = this.IpDataset.Tables[3].Select("addressList_Id = " + dataRow["addressList_Id"].ToString()).CopyToDataTable();
                        listBox1.DisplayMember = "address_Text";
                        listBox1.DataSource = ds;
               
                    
                    }
                    if (dataRow["type"].ToString() == "IPv4")
                    {
                        listBox2.DataSource = this.IpDataset.Tables[3].Select("addressList_Id = " + dataRow["addressList_Id"].ToString()).CopyToDataTable();
                        listBox2.DisplayMember = "address_Text";
                
                    }
                    if (dataRow["type"].ToString() == "IPv6")
                    {
                        listBox3.DataSource = this.IpDataset.Tables[3].Select("addressList_Id = " + dataRow["addressList_Id"].ToString())
                            .CopyToDataTable();
                        listBox3.DisplayMember = "address_Text";
                  
                    }

                }
            }


        }
    }
}