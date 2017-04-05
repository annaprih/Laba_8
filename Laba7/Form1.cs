using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Laba7
{
    public partial class Form1 : Form
    {
        private List<Owner> listOwners;
        private List<Operation> listOperations;
        private List<Account> listAccounts;

        private readonly XmlSerializer serializerOwners = new XmlSerializer(typeof (List<Owner>));
        private readonly XmlSerializer serializerOperations = new XmlSerializer(typeof (List<Operation>));
        private readonly XmlSerializer serializerAccounts = new XmlSerializer(typeof (List<Account>));

        private readonly string Owners = @"Owners.xml";
        private readonly string Operations = @"Operations.xml";
        private readonly string Accounts = @"Accounts.xml";
        private readonly string Search = @"Search.xml";
        private readonly string Sort = @"Sort.xml";

        public Timer timer;

        public Form1()
        {
            InitializeComponent();
            trackBar1.Scroll += scroll_Click;
            label1.Text = "0";
            listOwners = new List<Owner>();
            listOperations = new List<Operation>();
            listAccounts = new List<Account>();

            try
            {
                using (FileStream stream = new FileStream(Owners, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    listOwners = serializerOwners.Deserialize(stream) as List<Owner>;

                }
                using (FileStream stream = new FileStream(Operations, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    listOperations = serializerOperations.Deserialize(stream) as List<Operation>;

                }
                using (FileStream stream = new FileStream(Accounts, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    listAccounts = serializerAccounts.Deserialize(stream) as List<Account>;

                }
                comboBox1.DataSource = listOwners;
                comboBox2.DataSource = listOperations;
            }
            catch (Exception)
            {


            }
            timer = new Timer() {Interval = 1000};
            timer.Tick += toolStripStatusLabel1_Timer;
            timer.Start();
        }

        private void scroll_Click(object sender, EventArgs e)
        {
            label1.Text = trackBar1.Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Owner tempOwner = new Owner();
                var result = new List<ValidationResult>();
                if (textBox1.Text == "")
                {
                    throw new Exception("Enter FLS");
                }
                else
                {
                    tempOwner.FLS = textBox1.Text;
                }
                if (monthCalendar3.SelectionStart >= DateTime.Now)
                {
                    throw new Exception("Enter Birthday");
                }
                else
                {
                    tempOwner.DataOfBirthday = monthCalendar3.SelectionStart.Date.ToString();
                }
                if (textBox3.Text == "")
                {
                    throw new Exception("Enter Pasport");
                }
                else
                {
                    tempOwner.PasportInf = textBox3.Text;
                }
                var context = new ValidationContext(tempOwner);


                if (!(Validator.TryValidateObject(tempOwner, context, result, true)))
                {
                    foreach (var error in result)
                    {
                        throw new Exception(error.ErrorMessage);
                    }
                }

                listOwners.Add(tempOwner);
                comboBox1.DataSource = listOwners.ToArray();
                MessageBox.Show("Owner added");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Operation tempOperation = new Operation();
                var result = new List<ValidationResult>();

                if (!radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked)
                {
                    throw new Exception("Choose TypeOfOperation");
                }
                else
                {
                    foreach (var i in groupBox1.Controls)
                    {
                        if (((RadioButton) i).Checked)
                        {
                            tempOperation.TypeOfOperation = ((RadioButton) i).Text;
                        }
                    }

                }
                if (monthCalendar1.SelectionStart < DateTime.Now)
                {
                    throw new Exception("Enter DateOfOperation");
                }
                else
                {
                    tempOperation.DateOfOperation = monthCalendar1.SelectionStart.Date.ToString();
                }
                if (trackBar1.Value == 0)
                {
                    throw new Exception("Enter Sum");
                }
                else
                {
                    tempOperation.Sum = trackBar1.Value;
                }
                var context = new ValidationContext(tempOperation);


                if (!(Validator.TryValidateObject(tempOperation, context, result, true)))
                {
                    foreach (var error in result)
                    {
                        throw new Exception(error.ErrorMessage);
                    }
                }

                listOperations.Add(tempOperation);
                comboBox2.DataSource = listOperations.ToArray();
                MessageBox.Show("Operation added");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void button3_Click(object sender, EventArgs e)
        {
            try
            {

                Account account = new Account();
                var result = new List<ValidationResult>();

                if (textBox4.Text == "")
                {
                    throw new Exception("Enter Balance");
                }
                else
                {
                    account.Balance = Convert.ToInt32(textBox4.Text);
                }
                if (monthCalendar2.SelectionStart >= DateTime.Now)
                {
                    throw new Exception("Enter Date Of Open");
                }
                else
                {
                    account.DataOfOpen = monthCalendar2.SelectionStart.Date.ToString();
                }
                if (textBox5.Text == "")
                {
                    throw new Exception("Enter Number of account");
                }
                else
                {
                    account.Number = Convert.ToInt32(textBox5.Text);
                }
                if (!radioButton8.Checked && !radioButton9.Checked && !radioButton10.Checked)
                {
                    throw new Exception("Choose TypeOfAccount");
                }
                else
                {
                    foreach (var i in groupBox4.Controls)
                    {
                        if (((RadioButton) i).Checked)
                        {
                            account.TypeofAmount = ((RadioButton) i).Text;
                        }
                    }

                }
                if (!radioButton5.Checked && !radioButton6.Checked)
                {
                    throw new Exception("Choose Sms Information");
                }
                else
                {
                    if (radioButton5.Checked)
                    {
                        account.Sms = false;
                    }
                    else account.Sms = true;

                }
                if (!radioButton4.Checked && !radioButton7.Checked)
                {
                    throw new Exception("Choose Sms Information");
                }
                else
                {
                    if (radioButton4.Checked)
                    {
                        account.InternetBanking = false;
                    }
                    else account.InternetBanking = true;

                }
                if (comboBox1.Items.Count == 0)
                {
                    throw new Exception("Owners are not added");
                }
                else
                {
                    account.OwnerTemp = comboBox1.SelectedItem as Owner;
                }
                if (comboBox2.Items.Count == 0)
                {
                    throw new Exception("Operation are not added");
                }
                else
                {
                    account.OperationTemp = comboBox2.SelectedItem as Operation;
                }
                var context = new ValidationContext(account);


                if (!(Validator.TryValidateObject(account, context, result, true)))
                {
                    foreach (var error in result)
                    {
                        throw new Exception(error.ErrorMessage);
                    }
                }
                listAccounts.Add(account);
                MessageBox.Show("Account added");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                using (FileStream stream = new FileStream(Owners, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    serializerOwners.Serialize(stream, listOwners);

                }
                using (FileStream stream = new FileStream(Operations, FileMode.Create, FileAccess.Write, FileShare.Read)
                    )
                {
                    serializerOperations.Serialize(stream, listOperations);

                }
                using (FileStream stream = new FileStream(Accounts, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    serializerAccounts.Serialize(stream, listAccounts);

                }
            }
            catch (Exception)
            {


            }

        }

        private void button4_Click(object sender, EventArgs e)
            
        {
           
            dataGridView1.DataSource = listAccounts.ToArray();
        }

        private void панельЭлементовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (panel1.Visible)
            {
                panel1.Visible = false;
            }
            else panel1.Visible = true;
        }

        private void toolStripStatusLabel1_Timer(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Кол-во созданных объектов " + listAccounts.Count;
            toolStripStatusLabel3.Text = DateTime.Now.ToString();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Прихач Анна Александровна Version 1.8", "О программе", MessageBoxButtons.OK);
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                using (FileStream stream = new FileStream(Search, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    serializerAccounts.Serialize(stream, dataGridView2.DataSource as List<Account>);

                }

                using (FileStream stream = new FileStream(Sort, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    serializerAccounts.Serialize(stream, dataGridView3.DataSource as List<Account>);

                }
            }
            catch (Exception)
            {


            }
            toolStripStatusLabel2.Text = "Произошло сохранение";
        }

        private void типВкладаToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            XDocument xDocument = XDocument.Load(Accounts);
            List<Account> templist = new List<Account>();
            foreach (XElement i in xDocument.Element("ArrayOfAccount").Elements("Account"))
            {
                Account tempaccounts = new Account();
                tempaccounts.OwnerTemp = new Owner();
                tempaccounts.OperationTemp = new Operation();
                tempaccounts.Number = Convert.ToInt32(i.Element("Number").Value);
                tempaccounts.TypeofAmount = i.Element("TypeofAmount").Value;
                tempaccounts.Balance = Convert.ToInt32(i.Element("Balance").Value);
                tempaccounts.DataOfOpen = i.Element("DataOfOpen").Value;
                tempaccounts.Sms = Convert.ToBoolean(i.Element("Sms").Value);
                tempaccounts.InternetBanking = Convert.ToBoolean(i.Element("InternetBanking").Value);
                tempaccounts.OwnerTemp.FLS = i.Element("OwnerTemp").Element("FLS").Value;
                tempaccounts.OwnerTemp.DataOfBirthday = i.Element("OwnerTemp").Element("DataOfBirthday").Value;
                tempaccounts.OwnerTemp.PasportInf = i.Element("OwnerTemp").Element("PasportInf").Value;
                tempaccounts.OperationTemp.TypeOfOperation = i.Element("OperationTemp").Element("TypeOfOperation").Value;
                tempaccounts.OperationTemp.Sum = Convert.ToInt32(i.Element("OperationTemp").Element("Sum").Value);
                tempaccounts.OperationTemp.DateOfOperation = i.Element("OperationTemp").Element("DateOfOperation").Value;
                
                templist.Add(tempaccounts);



            }
            templist.Sort((a, b) =>
            {
               return a.TypeofAmount.CompareTo(b.TypeofAmount);
            });
            dataGridView3.DataSource = templist;
            toolStripStatusLabel2.Text = "Произошла сортировка по типу вклада";

        }

        private void датаОткрытияСчетаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            XDocument xDocument = XDocument.Load(Accounts);
            List<Account> templist = new List<Account>();
            foreach (XElement i in xDocument.Element("ArrayOfAccount").Elements("Account"))
            {
                Account tempaccounts = new Account();
                tempaccounts.OwnerTemp = new Owner();
                tempaccounts.OperationTemp = new Operation();
                tempaccounts.Number = Convert.ToInt32(i.Element("Number").Value);
                tempaccounts.TypeofAmount = i.Element("TypeofAmount").Value;
                tempaccounts.Balance = Convert.ToInt32(i.Element("Balance").Value);
                tempaccounts.DataOfOpen = i.Element("DataOfOpen").Value;
                tempaccounts.Sms = Convert.ToBoolean(i.Element("Sms").Value);
                tempaccounts.InternetBanking = Convert.ToBoolean(i.Element("InternetBanking").Value);
                tempaccounts.OwnerTemp.FLS = i.Element("OwnerTemp").Element("FLS").Value;
                tempaccounts.OwnerTemp.DataOfBirthday = i.Element("OwnerTemp").Element("DataOfBirthday").Value;
                tempaccounts.OwnerTemp.PasportInf = i.Element("OwnerTemp").Element("PasportInf").Value;
                tempaccounts.OperationTemp.TypeOfOperation = i.Element("OperationTemp").Element("TypeOfOperation").Value;
                tempaccounts.OperationTemp.Sum = Convert.ToInt32(i.Element("OperationTemp").Element("Sum").Value);
                tempaccounts.OperationTemp.DateOfOperation = i.Element("OperationTemp").Element("DateOfOperation").Value;

                templist.Add(tempaccounts);


            }
            templist.Sort((a, b) =>
            {
                DateTime dateTime = Convert.ToDateTime(a.DataOfOpen);
                DateTime dateTime2 = Convert.ToDateTime(b.DataOfOpen);

                return dateTime.CompareTo(dateTime2);
            });
            dataGridView3.DataSource = templist;
            toolStripStatusLabel2.Text = "Произошла сортировка по дате открытия счета";


        }

        private void номерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Regex regex = new Regex(textBox2.Text);

            XDocument xDocument = XDocument.Load(Accounts);
            List<Account> templist = new List<Account>();
            foreach (XElement i in xDocument.Element("ArrayOfAccount").Elements("Account"))
            {
                if (textBox2.Text.Equals(i.Element("Number").Value))
                {

                    Account tempaccounts = new Account();
                    tempaccounts.OwnerTemp = new Owner();
                    tempaccounts.OperationTemp = new Operation();
                    tempaccounts.Number = Convert.ToInt32(i.Element("Number").Value);
                    tempaccounts.TypeofAmount = i.Element("TypeofAmount").Value;
                    tempaccounts.Balance = Convert.ToInt32(i.Element("Balance").Value);
                    tempaccounts.DataOfOpen = i.Element("DataOfOpen").Value;
                    tempaccounts.Sms = Convert.ToBoolean(i.Element("Sms").Value);
                    tempaccounts.InternetBanking = Convert.ToBoolean(i.Element("InternetBanking").Value);
                    tempaccounts.OwnerTemp.FLS = i.Element("OwnerTemp").Element("FLS").Value;
                    tempaccounts.OwnerTemp.DataOfBirthday = i.Element("OwnerTemp").Element("DataOfBirthday").Value;
                    tempaccounts.OwnerTemp.PasportInf = i.Element("OwnerTemp").Element("PasportInf").Value;
                    tempaccounts.OperationTemp.TypeOfOperation = i.Element("OperationTemp").Element("TypeOfOperation").Value;
                    tempaccounts.OperationTemp.Sum = Convert.ToInt32(i.Element("OperationTemp").Element("Sum").Value);
                    tempaccounts.OperationTemp.DateOfOperation = i.Element("OperationTemp").Element("DateOfOperation").Value;

                    templist.Add(tempaccounts);
                }


            }
          
            dataGridView2.DataSource = templist;
            toolStripStatusLabel2.Text = "Поиск по номеру счета";



        }

        private void фИОToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex(textBox6.Text);

            XDocument xDocument = XDocument.Load(Accounts);
            List<Account> templist = new List<Account>();
            foreach (XElement i in xDocument.Element("ArrayOfAccount").Elements("Account"))
            {
                if (regex.IsMatch(i.Element("OwnerTemp").Element("FLS").Value))
                {

                    Account tempaccounts = new Account();
                    tempaccounts.OwnerTemp = new Owner();
                    tempaccounts.OperationTemp = new Operation();
                    tempaccounts.Number = Convert.ToInt32(i.Element("Number").Value);
                    tempaccounts.TypeofAmount = i.Element("TypeofAmount").Value;
                    tempaccounts.Balance = Convert.ToInt32(i.Element("Balance").Value);
                    tempaccounts.DataOfOpen = i.Element("DataOfOpen").Value;
                    tempaccounts.Sms = Convert.ToBoolean(i.Element("Sms").Value);
                    tempaccounts.InternetBanking = Convert.ToBoolean(i.Element("InternetBanking").Value);
                    tempaccounts.OwnerTemp.FLS = i.Element("OwnerTemp").Element("FLS").Value;
                    tempaccounts.OwnerTemp.DataOfBirthday = i.Element("OwnerTemp").Element("DataOfBirthday").Value;
                    tempaccounts.OwnerTemp.PasportInf = i.Element("OwnerTemp").Element("PasportInf").Value;
                    tempaccounts.OperationTemp.TypeOfOperation = i.Element("OperationTemp").Element("TypeOfOperation").Value;
                    tempaccounts.OperationTemp.Sum = Convert.ToInt32(i.Element("OperationTemp").Element("Sum").Value);
                    tempaccounts.OperationTemp.DateOfOperation = i.Element("OperationTemp").Element("DateOfOperation").Value;

                    templist.Add(tempaccounts);
                }


            }

            dataGridView2.DataSource = templist;
            toolStripStatusLabel2.Text = "Поиск по ФИО";
        }

        private void балансToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex("^[" + textBox7 + "]");

            XDocument xDocument = XDocument.Load(Accounts);
            List<Account> templist = new List<Account>();
            foreach (XElement i in xDocument.Element("ArrayOfAccount").Elements("Account"))
            {
                if (regex.IsMatch(i.Element("Balance").Value))
                {

                    Account tempaccounts = new Account();
                    tempaccounts.OwnerTemp = new Owner();
                    tempaccounts.OperationTemp = new Operation();
                    tempaccounts.Number = Convert.ToInt32(i.Element("Number").Value);
                    tempaccounts.TypeofAmount = i.Element("TypeofAmount").Value;
                    tempaccounts.Balance = Convert.ToInt32(i.Element("Balance").Value);
                    tempaccounts.DataOfOpen = i.Element("DataOfOpen").Value;
                    tempaccounts.Sms = Convert.ToBoolean(i.Element("Sms").Value);
                    tempaccounts.InternetBanking = Convert.ToBoolean(i.Element("InternetBanking").Value);
                    tempaccounts.OwnerTemp.FLS = i.Element("OwnerTemp").Element("FLS").Value;
                    tempaccounts.OwnerTemp.DataOfBirthday = i.Element("OwnerTemp").Element("DataOfBirthday").Value;
                    tempaccounts.OwnerTemp.PasportInf = i.Element("OwnerTemp").Element("PasportInf").Value;
                    tempaccounts.OperationTemp.TypeOfOperation = i.Element("OperationTemp").Element("TypeOfOperation").Value;
                    tempaccounts.OperationTemp.Sum = Convert.ToInt32(i.Element("OperationTemp").Element("Sum").Value);
                    tempaccounts.OperationTemp.DateOfOperation = i.Element("OperationTemp").Element("DateOfOperation").Value;

                    templist.Add(tempaccounts);
                }


            }

            dataGridView2.DataSource = templist;
            toolStripStatusLabel2.Text = "Поиск по балансу";

        }

        private void типВкладаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex(textBox8.Text);

            XDocument xDocument = XDocument.Load(Accounts);
            List<Account> templist = new List<Account>();
            foreach (XElement i in xDocument.Element("ArrayOfAccount").Elements("Account"))
            {
                if (regex.IsMatch(i.Element("TypeofAmount").Value))
                {

                    Account tempaccounts = new Account();
                    tempaccounts.OwnerTemp = new Owner();
                    tempaccounts.OperationTemp = new Operation();
                    tempaccounts.Number = Convert.ToInt32(i.Element("Number").Value);
                    tempaccounts.TypeofAmount = i.Element("TypeofAmount").Value;
                    tempaccounts.Balance = Convert.ToInt32(i.Element("Balance").Value);
                    tempaccounts.DataOfOpen = i.Element("DataOfOpen").Value;
                    tempaccounts.Sms = Convert.ToBoolean(i.Element("Sms").Value);
                    tempaccounts.InternetBanking = Convert.ToBoolean(i.Element("InternetBanking").Value);
                    tempaccounts.OwnerTemp.FLS = i.Element("OwnerTemp").Element("FLS").Value;
                    tempaccounts.OwnerTemp.DataOfBirthday = i.Element("OwnerTemp").Element("DataOfBirthday").Value;
                    tempaccounts.OwnerTemp.PasportInf = i.Element("OwnerTemp").Element("PasportInf").Value;
                    tempaccounts.OperationTemp.TypeOfOperation = i.Element("OperationTemp").Element("TypeOfOperation").Value;
                    tempaccounts.OperationTemp.Sum = Convert.ToInt32(i.Element("OperationTemp").Element("Sum").Value);
                    tempaccounts.OperationTemp.DateOfOperation = i.Element("OperationTemp").Element("DateOfOperation").Value;

                    templist.Add(tempaccounts);
                }


            }

            dataGridView2.DataSource = templist;
            toolStripStatusLabel2.Text = "Поиск по типу вклада";

        }

        private void AtrFunc(object sender, CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;
        }
        private void button6_Click(object sender, EventArgs e)
        {
           
            XDocument xDocument = XDocument.Load(Accounts);
            List<Account> templist = new List<Account>();
            foreach (XElement i in xDocument.Element("ArrayOfAccount").Elements("Account"))
            {
                if ((i.Element("Number").Value.Equals(textBox2.Text)) &&
                    (i.Element("OwnerTemp").Element("FLS").Value.Equals(textBox6.Text)) &&
                    (i.Element("Balance").Value.Equals(textBox7.Text)) &&
                    (i.Element("TypeofAmount").Value.Equals(textBox8.Text)))
                {

                    Account tempaccounts = new Account();
                    tempaccounts.OwnerTemp = new Owner();
                    tempaccounts.OperationTemp = new Operation();
                    tempaccounts.Number = Convert.ToInt32(i.Element("Number").Value);
                    tempaccounts.TypeofAmount = i.Element("TypeofAmount").Value;
                    tempaccounts.Balance = Convert.ToInt32(i.Element("Balance").Value);
                    tempaccounts.DataOfOpen = i.Element("DataOfOpen").Value;
                    tempaccounts.Sms = Convert.ToBoolean(i.Element("Sms").Value);
                    tempaccounts.InternetBanking = Convert.ToBoolean(i.Element("InternetBanking").Value);
                    tempaccounts.OwnerTemp.FLS = i.Element("OwnerTemp").Element("FLS").Value;
                    tempaccounts.OwnerTemp.DataOfBirthday = i.Element("OwnerTemp").Element("DataOfBirthday").Value;
                    tempaccounts.OwnerTemp.PasportInf = i.Element("OwnerTemp").Element("PasportInf").Value;
                    tempaccounts.OperationTemp.TypeOfOperation = i.Element("OperationTemp").Element("TypeOfOperation").Value;
                    tempaccounts.OperationTemp.Sum = Convert.ToInt32(i.Element("OperationTemp").Element("Sum").Value);
                    tempaccounts.OperationTemp.DateOfOperation = i.Element("OperationTemp").Element("DateOfOperation").Value;

                    templist.Add(tempaccounts);
                }



            }
            if ((textBox2.Text == "") || (textBox6.Text == "") || (textBox7.Text == "") || (textBox8.Text == ""))
            {
                MessageBox.Show("Задайте все поля", "Error", MessageBoxButtons.OK);
            }

            dataGridView2.DataSource = templist;
            toolStripStatusLabel2.Text = "Поиск";

        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.listAccounts.Clear();
            this.listOperations.Clear();
            this.listOwners.Clear();
            toolStripStatusLabel2.Text = "Удаление";

        }

        
    }
}
