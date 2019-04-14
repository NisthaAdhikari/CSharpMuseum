using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisitorForm
{
    public partial class Form1 : Form
    {
        private List<VisitorDetails> listDetails = new List<VisitorDetails>();
        private DataTable dt = new DataTable();
        private Random r = new Random();
        private string time;
       
        public Form1()
        {
            InitializeComponent();
            panelLeft.Height = buttonVisitorsForm.Height;
            panelLeft.Top = buttonVisitorsForm.Top;
            panelDailyReport.Visible = false;
            panelWeeklyReport.Visible = false;
            System.IO.File.WriteAllText(@"details.csv", string.Empty);
            panel7.Visible = true;
        }

        private void buttonVisitorsForm_Click(object sender, EventArgs e)
        {
            panelLeft.Height = buttonVisitorsForm.Height;
            panelLeft.Top = buttonVisitorsForm.Top;

            panelWeeklyReport.Visible = false;
            panelDailyReport.Visible = false;
            panel5.Visible = true;

            //string name = registerForm1.VisitorsName;
        }

        private void buttonWeeklyReport_Click(object sender, EventArgs e)
        {
            panelDailyReport.Visible = true;



            dataGridWeekly.Rows.Clear();


            panelWeeklyReport.BringToFront();
            //panel7.SendToBack();
            panelWeeklyReport.Visible = true;


            List<string> v = visitors();
           
            string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            for (int i = 0; i < days.Length; i++)
            {
                int count = 0;
                double totalMin = 0;
                var visits = from date in v
                             where date.Contains(days[i])
                             select date;

                List<string> cards1 = new List<string>();
                foreach (string vs in visits)
                {


                    string vals = vs.ToString();
                    string[] n = vals.Split(',');
                    cards1.Add(n[0]);
                }

                var uniq1 = cards1.Distinct();
                foreach (var u in uniq1)
                {
                    count = count + 1;

                }
                //minutes
                foreach (string t in v)
                {

                    //Console.WriteLine(i);
                    var values = t.Split(',').ToList();


                    var qTotalTime = from date in values
                                     where date.Contains(days[i])
                                     select values[9];



                    foreach (var name in qTotalTime)
                    {

                        if (name != null && name != "")
                        {
                            totalMin = double.Parse(name) + totalMin;
                        }


                    }
                }
                
                this.dataGridView2.Rows.Add(days[i], count.ToString(), totalMin.ToString());
            }
        }




            

        private void buttonVisitorsList_Click(object sender, EventArgs e)
        {
            panelLeft.Height = buttonVisitorsList.Height;
            panelLeft.Top = buttonVisitorsList.Top;

            //panel5.Visible = false;
            panelDailyReport.BringToFront();
            panelDailyReport.Visible = true;

        }

        private void buttonCheckIn_Click(object sender, EventArgs e)
        {
            //check with the list if the entered cardNo already exists
            VisitorDetails cardExist = listDetails.Where(x => x.CardNo == txtCardNo.Text).FirstOrDefault();
            string cardNo = txtCardNo.Text;
            string name = txtName.Text;
            string email = txtEmail.Text;
            string contactNo = txtContactNo.Text;
            string occupation = txtOccupation.Text;
            string date = DateTime.Now.ToShortDateString();
            
            TimeSpan now = DateTime.Now.TimeOfDay;


            /*
            int cardNo = int.Parse(txtCardNo.Text);
            int newcardNo = cardNo + 1;
            txtCardNo.Text = newcardNo.ToString();
            */

            time = DateTime.Now.ToLongTimeString();
            
            string day = DateTime.Now.DayOfWeek.ToString();

            if (cardExist != null && cardExist.CardNo == txtCardNo.Text)
            {
                MessageBox.Show("Card Number Already taken. Please enter another or generate new number");
            }
            else if (txtCardNo.Text.Equals("") || name == "" || email == "" || contactNo == "" || occupation == "")
            {
                MessageBox.Show("Dont leave empty fields.");
            }

            else if (day == "Monday" || day == "Tuesday" || day == "Wednesday" || day == "Thursday" || day == "Friday")
            {
            
                    dt.Rows.Add(cardNo, date, day, name, email, contactNo, occupation, time, "--", "--");
                    this.dataGridView.DataSource = dt;

                    VisitorDetails v = new VisitorDetails();
                    v.CardNo = cardNo.ToString();
                    v.Name = name;
                    v.Email = email;
                    v.ContactNo = contactNo;
                    v.Occupation = occupation;


                    v.Date = date;
                    v.Day = day;
                    v.InTime = time;
                    v.OutTime = "";
                    v.Duration = "0";

                    String data = v.CardNo + "," + v.Name + "," + v.Email + "," + v.ContactNo + "," + v.Occupation + "," + v.Date + "," + v.Day + "," + v.InTime + "," + v.OutTime + "," + v.Duration;
                    listDetails.Add(v);
                    saveToCSV(data);

                    txtName.Text = "";
                    txtCardNo.Text = "";
                    txtEmail.Text = "";
                    txtContactNo.Text = "";
                    txtOccupation.Text = "";
                    MessageBox.Show("Visitor Checked in.");
                }
                
            else
            {
                MessageBox.Show("Museum not open");
            }
            
            //timer.Start();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dt.Columns.AddRange(new DataColumn[] { new DataColumn("CardNo", typeof(string)),
                new DataColumn("Date", typeof(string)),
                new DataColumn("Day", typeof(string)),
                new DataColumn("Name", typeof(string)),
                new DataColumn("Email", typeof(string)),
                new DataColumn("ContactNo", typeof(string)),
                new DataColumn("Occupation", typeof(string)),
                new DataColumn("InTime", typeof(string)),
                new DataColumn("OutTime", typeof(string)),
                new DataColumn("Duration", typeof(string)),
                new DataColumn("Check out"),
            });
          
        }




        //private void timer_Tick(object sender, EventArgs e)
        //{
        //string time = DateTime.Now.ToLongTimeString();
        //dataGridView[8, a].Value = time;
        //labelCardNo.Text = time;
        //a= a+1;
        //}

        private void buttonCheckOut_Click(object sender, EventArgs e)
        {

        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            txtFilePath.Text = openFileDialog1.FileName;


            if (txtFilePath.Text == "" || txtFilePath.Text == "openFileDialog1")
            {
                MessageBox.Show("Please select a file to import");
            }
            else
            {
                ImportDataCSV(txtFilePath.Text);
                txtFilePath.Text = "";
            }
        }

        private void ImportDataCSV(string filePath)
        {
            string[] lines = System.IO.File.ReadAllLines(filePath);
            if (lines.Length > 0)
            {

                //creating header
                string firstLine = lines[0];
                string[] headerLabels = firstLine.Split(',');


                //displaying data
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] dataWords = lines[i].Split(',');
                    int colIndex = 0;
                    DataRow dr = dt.NewRow();
                    foreach (string headerWord in headerLabels)
                    {
                        dr[headerWord] = dataWords[colIndex++];

                        VisitorDetails v = new VisitorDetails();
                        v.CardNo = dataWords[0];
                        v.Name = dataWords[1];
                        v.Email = dataWords[2];
                        v.ContactNo = dataWords[3];
                        v.Occupation = dataWords[4];


                        v.Date = dataWords[5];
                        v.Day = dataWords[6];
                        v.InTime = dataWords[7];
                        v.OutTime = dataWords[8];
                        v.Duration = dataWords[9];
                        listDetails.Add(v);

                        //MessageBox.Show(dataWords[0]);
                    }

                    dt.Rows.Add(dr);

                }

            }

            if (dt.Rows.Count > 0)
            {
                dataGridView.DataSource = dt;
                MessageBox.Show("Data Imported", "Success!");
            }
        }

        /*
        private void Lists()
        {
            VisitorDetails v = new VisitorDetails();
            v.Name = txtName.Text;
            v.Occupation = txtOccupation.Text;
            String data = v.CardNo + "." + v.Name + ".";
            listDetails.Add(v);
            saveToCSV(data);
        }
        */

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;

            DataGridViewRow d = dataGridView.Rows[row];
            string outTime = DateTime.Now.ToLongTimeString();

            string inTime = d.Cells[7].Value.ToString();

            DateTime ts1 = DateTime.Parse(inTime);
            DateTime ts2 = DateTime.Parse(outTime);
            //TimeSpan span = ts2.Subtract(ts1);
            //double duration = span.TotalMinutes;
            string duration = "" + ts2.Subtract(ts1).TotalMinutes;

            d.Cells[9].Value = duration.ToString();
            d.Cells[8].Value = outTime;

            string card = d.Cells[0].Value.ToString();
            //MessageBox.Show(listDetails.Count.ToString());

            //  for (int i = 0; i < listDetails.Count; i++)
            //{
            var client = listDetails.Where(x => x.CardNo == card).OrderByDescending(x => x.Date).FirstOrDefault();
            if (client != null)
            {

                client.InTime = inTime;
                client.OutTime = outTime;
                client.Duration = Math.Round(double.Parse(duration),2).ToString();

                String newData = client.CardNo + "," + client.Name + "," + client.Email + "," + client.ContactNo + "," + client.Occupation + "," + client.Date + "," + client.Day + "," + client.InTime + "," + client.OutTime + "," + client.Duration;

                MessageBox.Show("Visitor checked out.");
                modifyCSV(newData, client.CardNo);

                saveCheckOutToCSV(newData);

            }

            // }
            
            //MessageBox.Show(listDetails[row].CardNo.ToString());

        }

        /*
        public void modifyCSV( String newData, string cNo)
        {
            //MessageBox.Show(newData, cNo);
            String path = "details.csv";
            var reader = new StreamReader(File.OpenRead(path));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                if (values[1] == cNo)
                {

                    MessageBox.Show(values[1]);
                    //File.WriteAllLines("myFile.txt"), linesList.ToArray());

                    using (var w = new StreamWriter(path, true))
                    {
                        w.WriteLine(newData);
                    }
                }
            }
        }
        */

        public void modifyCSV(String newData, string cNo)
        {
            // MessageBox.Show(newData, cNo);
            List<String> list = new List<string>();
            List<String> chkOutDetails = new List<string>();
            String path = "details.csv";
            using (StreamReader sr = new StreamReader(path))
            {
                var lineCount = File.ReadLines(@"details.csv").Count();
                list.Add(newData);


                for (int i = 1; i <= lineCount; i++)
                {
                    var line = sr.ReadLine();

                    var values = line.Split(',');

                    list.Add(line);

                    if (values[0] == cNo && values[8] == "")
                    {
                        //MessageBox.Show("here");
                        list.Remove(line);
                    }
                }
            }

            using (System.IO.StreamWriter outfile = new System.IO.StreamWriter(path))
            {
                outfile.WriteLine(String.Join(System.Environment.NewLine, list.ToArray()));
            }
            /*
            using (System.IO.StreamWriter outfile = new System.IO.StreamWriter("data.csv", true))
            {
                outfile.WriteLine(String.Join(System.Environment.NewLine, chkOutDetails.ToArray()));
            }

            */

        }

        public List<VisitorDetails> AllVisitor()
        {
            List<VisitorDetails> visitor = new List<VisitorDetails>();

            using (StreamReader sr = new StreamReader("data.csv"))
            {
                string headerLine = sr.ReadLine();
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var values = line.Split(',');

                    VisitorDetails d = new VisitorDetails();
                    d.CardNo = values[0];
                    d.Name = values[1];
                    d.Email = values[2];
                    d.ContactNo = values[3];
                    d.Occupation = values[4];


                    d.Date = values[5];
                    d.Day = values[6];
                    d.InTime = values[7];
                    d.OutTime = values[8];
                    d.Duration = values[9];
                    visitor.Add(d);

                }
                
            }

            return visitor;
        }




        public void saveToCSV(String data)
        {
            String path = "details.csv"; //bin ko debug vitra banxa
            if (!File.Exists(path))
            {

                File.Create(path);

            }

            using (var w = new StreamWriter(path, true))
            {
                w.WriteLine(data);
            }
        }

        public void saveCheckOutToCSV(String newData)
        {
            String path = "data.csv"; //bin ko debug vitra banxa
            if (!File.Exists(path))
            {

                File.Create(path);

            }
            using (var w = new StreamWriter(path, true))
            {
                w.WriteLine(newData);
            }
        }

       

        private void buttonImportDetails_Click(object sender, EventArgs e)
        {
            List<String> details = new List<string>();
            String path = "data.csv";
            string toImport = txtCard.Text;

            using (StreamReader sr = new StreamReader(path))
            {
                var lineCount = File.ReadLines(@"data.csv").Count();

                if (lineCount == 0)
                {
                    MessageBox.Show("No visitor exists");
                }
                for (int i = 1; i <= lineCount; i++)
                {
                    var line = sr.ReadLine();

                    var values = line.Split(',');

                    if (values[0] == toImport)
                    {
                        name.Text = values[1];
                        email.Text = values[2];
                        contactNo.Text = values[3];
                        occupation.Text = values[4];
                    }
                    else if (toImport == "")
                    {
                        MessageBox.Show("Please fill the card number.");
                    }
                    /*
                    else
                    {
                        MessageBox.Show("Visitor doesn't exist. Please register first");
                    }
                    */
                }

            }
        }

        private void buttonReport1_Click_1(object sender, EventArgs e)
        {
            dataGrid.Rows.Clear();
            dataGridViewAll.Rows.Clear();
            dataGridView1.Rows.Clear();

            List<string> visitor = visitors();
            List<string> visitedCard = new List<string>();
            List<VisitorDetails> l = new List<VisitorDetails>();
            int count = 0;
            double totalTime = 0;

            string[] n;
            //string choseDate = dateTimePicker1.Text;

            string theDate = dateTimePicker1.Value.ToString("M/dd/yyyy");


            //String path = "details.csv";

            var onDate = from date in visitor
                         where date.Contains(dateTimePicker1.Value.ToShortDateString())
                         select date;


            foreach (string each in onDate)
            {
               
                n = each.ToString().Split(',');
                dataGridViewAll.Rows.Add(n[0], n[1], n[5], n[6], n[7], n[8], Math.Round(double.Parse(n[9]), 2));
                visitedCard.Add(n[0]);
                VisitorDetails vi = new VisitorDetails();
                vi.CardNo = n[0];
                vi.Name = n[1];
                vi.Email = n[2];
                vi.ContactNo = n[3];
                vi.Occupation = n[4];
                vi.Date = n[5];
                vi.Day = n[6];
                vi.InTime = n[7];
                vi.OutTime = n[8];
                vi.Duration = n[9];
                l.Add(vi);
            }


            var client = from c in l
                         group c by c.CardNo;
            string vcard = "";
            string vname = "";
            double vtotal = 0;
            foreach (var vtr in client)
            {
                vcard = vtr.Key.ToString();

                foreach (var v in vtr)
                {
                    vname = v.Name;
                    vtotal = double.Parse(v.Duration) + vtotal;

                }

                this.dataGridView1.Rows.Add(vcard, vname, Math.Round(vtotal, 2).ToString());
                vtotal = 0;
                vcard = "";
                vname = "";

            }
            
            var single = visitedCard.Distinct();
            foreach (var u in single)
            {
                count = count + 1;
            }

            foreach (string v in visitor)
            {
                var values = v.Split(',').ToList();

                var timeTotal = from date in values
                                where date.Contains(dateTimePicker1.Value.ToShortDateString())
                                select values[9];


                foreach (var item in timeTotal)
                {

                    if (item != null && item != "")
                    {
                        totalTime = double.Parse(item) + totalTime;
                    }
                }
            }
            this.dataGrid.Rows.Add(count.ToString(), Math.Round(totalTime, 2).ToString());
        }

       
        public List<string> visitors()
        {
            List<string> visitor = new List<string>();

            using (StreamReader sr = new StreamReader("data.csv"))
            {
                string headerLine = sr.ReadLine();
                string line;
                while ((line = sr.ReadLine()) != null)
                {

                    visitor.Add(line);

                }
                visitor.RemoveAt(0);
            }

            return visitor;
        }






        private void buttonDailyReport_Click(object sender, EventArgs e)
        {
            panelDailyReport.BringToFront();
            panelDailyReport.Visible = true;
        }

        private void buttonChkIn_Click(object sender, EventArgs e)
        {
            string input = txtCard.Text;
            List<VisitorDetails> list = new List<VisitorDetails>();

            if (input == "")
            {
                MessageBox.Show("Please fill up your card number");
                // input = Microsoft.VisualBasic.Interaction.InputBox("Enter your Card Number below.", "Visitor Details", "", 500, 300);
            }
            else if (!listDetails.Any())
            {
                MessageBox.Show("You are not a registered visitor. Please register first.");
            }

            foreach (var item in listDetails)
            {
                //MessageBox.Show(item.CardNo);
                if (item.CardNo == input && item.OutTime != "")
                {
                    string date = DateTime.Now.ToShortDateString();
                    time = DateTime.Now.ToLongTimeString();
                    string day = DateTime.Now.DayOfWeek.ToString();

                    dt.Rows.Add(item.CardNo, date, day, item.Name, item.Email, item.ContactNo, item.Occupation, time, "--", "--");
                    this.dataGridView.DataSource = dt;

                    VisitorDetails v = new VisitorDetails();
                    v.CardNo = item.CardNo;
                    v.Name = item.Name;
                    v.Email = item.Email;
                    v.ContactNo = item.CardNo;
                    v.Occupation = item.Occupation;
                    v.Date = date;
                    v.Day = day;
                    v.InTime = time;
                    v.OutTime = "";
                    v.Duration = "0";

                    string data = v.CardNo + "," + v.Name + "," + v.Email + "," + v.ContactNo + "," + v.Occupation + "," + v.Date + "," + v.Day + "," + v.InTime + "," + v.OutTime + "," + v.Duration;
                    listDetails.Add(v);

                    txtCard.Text = "";
                    name.Text = "";
                    email.Text = "";
                    contactNo.Text = "";
                    occupation.Text = "";

                    saveToCSV(data);
                    break;
                }
                else if (item.CardNo == input && item.OutTime == "")
                {
                    MessageBox.Show("You have not checked-out yet.");
                    break;
                }
            }
        }

        
            
        

        private void dataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonGenerateCardNo_Click(object sender, EventArgs e)
        {
            int newCardNo = r.Next(1601000, 1699999);
            txtCardNo.Text = newCardNo.ToString();
        }

        private void buttonShowWeeklyReport_Click(object sender, EventArgs e)
        {
            dataGridWeekly.Rows.Clear();
            dataGridWeekly.Refresh();


            //string choseDate = dateTimePicker1.Text;
            string[] n;
            // string theDate = dateTimePicker1.Value.ToString("dd/M/yyyy");

            DateTime choseDate = DateTime.Parse(dateTimePicker1.Text);
            int year = choseDate.Date.Year;  //get year from the date
            DateTime firstDay = new DateTime(year, 1, 1); //set the first day of the year

            //MessageBox.Show(year.ToString());
            DateTime date = DateTime.Now; //get current datetime 
            DayOfWeek day = date.DayOfWeek;//get Day of the week 
            CultureInfo cul = CultureInfo.CurrentCulture;
            //get no of week for the date
            int weekNo = cul.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
            int days = (weekNo - 1) * 7;
            DateTime dt1 = firstDay.AddDays(days);
            DayOfWeek dow = dt1.DayOfWeek;
            DateTime startDateOfWeek = dt1.AddDays(-(int)dow);
            DateTime endDateOfWeek = startDateOfWeek.AddDays(6);
            

            List<VisitorDetails> visitedCard = new List<VisitorDetails>();
            List<string> temp = new List<string>();
            List<VisitorDetails> allVistor = AllVisitor();
            allVistor = allVistor.GroupBy(a => a.CardNo).Select(r => r.First()).ToList();
            string[] weekdays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

            foreach (var visitor in allVistor)
            {
                if (DateTime.Parse(visitor.InTime) >= startDateOfWeek &&
                    DateTime.Parse(visitor.InTime) <= endDateOfWeek)
                {
                    visitedCard.Add(visitor);
                }
            }

            var totalVisitorInAWeek = visitedCard.Count;
            List<WeeklyData> weekData = new List<WeeklyData>();
            int[] totalVisitor = new int[5];
            double[] totalTime = new double[5];

            foreach (var visited in visitedCard)
            {
                if (visited.Day == DayOfWeek.Monday.ToString())
                {
                    totalVisitor[0]++;
                    totalTime[0] += double.Parse(visited.Duration);
                }
                else if (visited.Day == DayOfWeek.Tuesday.ToString())
                {
                    totalVisitor[1]++;
                    totalTime[1] += double.Parse(visited.Duration);
                }
                else if (visited.Day == DayOfWeek.Wednesday.ToString())
                {
                    totalVisitor[2]++;
                    totalTime[2] += double.Parse(visited.Duration);
                }
                else if (visited.Day == DayOfWeek.Thursday.ToString())
                {
                    totalVisitor[3]++;
                    totalTime[3] += double.Parse(visited.Duration);
                }
                else if (visited.Day == DayOfWeek.Friday.ToString())
                {
                    totalVisitor[4]++;
                    totalTime[4] += double.Parse(visited.Duration);
                }
            }

            foreach (var chosenVisitor in visitedCard)
            {
                if (chosenVisitor.Day == DayOfWeek.Monday.ToString())
                {
                    weekData.Add(
                        new WeeklyData(chosenVisitor.Day, "" + totalVisitor[0], "" + totalTime[0]));
                }
                else if (chosenVisitor.Day == DayOfWeek.Tuesday.ToString())
                {
                    weekData.Add(
                        new WeeklyData(chosenVisitor.Day, "" + totalVisitor[1], "" + totalTime[1]));
                }
                else if (chosenVisitor.Day == DayOfWeek.Wednesday.ToString())
                {
                    weekData.Add(
                        new WeeklyData(chosenVisitor.Day, "" + totalVisitor[2], "" + totalTime[2]));
                }
                else if (chosenVisitor.Day == DayOfWeek.Thursday.ToString())
                {
                    weekData.Add(
                        new WeeklyData(chosenVisitor.Day, "" + totalVisitor[3], "" + totalTime[4]));
                }
                else if (chosenVisitor.Day == DayOfWeek.Friday.ToString())
                {
                    weekData.Add(
                        new WeeklyData(chosenVisitor.Day, "" + totalVisitor[4], "" + totalTime[4]));
                }
            }
            weekData = weekData.GroupBy(p => p.DayOfWeek).Select(grp => grp.First()).ToList();

            foreach (var data in weekData)
            {
                this.weeklyDataBindingSource.Add(new WeeklyData(data.DayOfWeek, data.totalVisitors,
                    data.totalDuration));
            }
            foreach (var series in weeklyChart.Series)
            {
                series.Points.Clear();
            }

            foreach (var weekdayInBar in weekData)
            {
                this.weeklyChart.Series["Total Visitor"].Points.AddXY(weekdayInBar.DayOfWeek, weekdayInBar.totalVisitors);
                this.weeklyChart.Series["Total Duration"].Points.AddXY(weekdayInBar.DayOfWeek, weekdayInBar.totalDuration);
            }

        }

        

        private void btnSorting_Click(object sender, EventArgs e)
        {
           
            string[,] datavalue = new string[dataGridView2.Rows.Count - 1, dataGridView2.Columns.Count];
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {

                foreach (DataGridViewColumn col in dataGridView2.Columns)
                {
                    if (dataGridView2.Rows[row.Index].Cells[col.Index].Value != null && dataGridView2.Rows[row.Index].Cells[col.Index].Value.ToString() != "")
                    {
                        datavalue[row.Index, col.Index] = dataGridView2.Rows[row.Index].Cells[col.Index].Value.ToString();
                    }
                }
            }

            string[] temp = new string[3];
           
            for (int i = 0; i < datavalue.GetLength(0) - 1; i++)
            {

                int j;
                
                for (j=0; j < datavalue.GetLength(0) - 1; j++)
                {
                    if (Int32.Parse(datavalue[j, 1]) > Int32.Parse(datavalue[j + 1, 1]))
                    {
                        //put array record j into temp holder
                        temp[0] = datavalue[j, 0];
                        temp[1] = datavalue[j, 1];
                        temp[2] = datavalue[j, 2];

                        //copy j + 1 into j
                        datavalue[j, 0] = datavalue[j + 1, 0];
                        datavalue[j, 1] = datavalue[j + 1, 1];
                        datavalue[j, 2] = datavalue[j + 1, 2];

                        //copy temp into j + 1
                        datavalue[j + 1, 0] = temp[0];
                        datavalue[j + 1, 1] = temp[1];
                        datavalue[j + 1, 2] = temp[2];
                    }

                }
            }

          
            this.dataGridView2.Rows.Clear();
            this.dataGridView2.Refresh();
            for (int i = 0; i < datavalue.GetLength(0); i++)
            {
                Console.WriteLine(datavalue[i, 0] + " , " + datavalue[i, 1] + " , " + datavalue[i, 2]);
                this.dataGridView2.Rows.Add(datavalue[i, 0], datavalue[i, 1], datavalue[i, 2]);
            }
        }
    }
}

   




