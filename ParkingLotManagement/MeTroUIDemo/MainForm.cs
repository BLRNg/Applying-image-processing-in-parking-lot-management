using CefSharp;
using CefSharp.WinForms;
using MetroFramework.Controls;
using MeTroUIDemo.Helper;
using MeTroUIDemo.Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MeTroUIDemo
{
    public partial class MainForm : MetroFramework.Forms.MetroForm
    {
        private Stack<MetroTabPage> tabStack;
        private LaneInOut formInOut;
        private GeneralRepository generalRepository;
        private StatictisSingleTicketRepository statictisSingleTicketRepository;
        private TicketRepository ticketRepository;

        private string dayApplySelected;
        private string typeTicketSelected;
        public MainForm()
        {
            InitializeComponent();
            tabStack = new Stack<MetroTabPage>();
            generalRepository = new GeneralRepository();
            statictisSingleTicketRepository = new StatictisSingleTicketRepository();
            ticketRepository = new TicketRepository();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            //this.Shown += LoginForm_Shown;
            formInOut = new LaneInOut();
            // To hide a tab with a specific name
            foreach (MetroTabPage tabPage in homePage.TabPages)
            {

                if (!tabPage.Name.Equals(homeTabPage.Name))
                {
                    homePage.TabPages.Remove(tabPage);
                    // break; // Optional: If you want to stop looping after finding and removing the tab
                }
            }

            initHistoryTab();
            initChartTab();
            initTicketTab();
            //Load_Chart("sp_statisticLineChartByDay");
            //LoadDataToTicketDataGridView();
            //ticketdataGridView.DataSource = ticketRepository.sp_TicketPriceManagement();



        }
        static string[][] ConvertToMatrix(string input)
        {
            // Remove unwanted characters and split by comma
            string[] rows = input.Split(new[] { "], " }, StringSplitOptions.None);

            string[][] matrix = new string[rows.Length][];

            for (int i = 0; i < rows.Length; i++)
            {
                // Remove brackets and split by comma
                string[] elements = rows[i].Trim('[', ']').Split(',');

                matrix[i] = new string[elements.Length];

                for (int j = 0; j < elements.Length; j++)
                {
                    // Remove leading and trailing spaces
                    matrix[i][j] = elements[j].Trim();
                }
            }

            return matrix;
        }
        static string[] SplitStringToArray(string input)
        {
            // Remove unwanted characters
            input = input.Replace("[", "").Replace("]", "").Replace("'", "").Trim();

            // Split the string by comma
            string[] array = input.Split(',');

            // Trim each element in the array to remove leading and trailing spaces
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = array[i].Trim();
            }

            return array;
        }
        private void initTicketTab()
        {
            ApplyDayBox.Format = DateTimePickerFormat.Custom;
            ApplyDayBox.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Adjust format as needed
            ApplyDayBoxSubMenu.Format = DateTimePickerFormat.Custom;
            ApplyDayBoxSubMenu.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Adjust format as needed


            startTimeSlot.Format = DateTimePickerFormat.Time;
            startTimeSlot.ShowUpDown = true; // This will display up and down arrows for easier hour selection
            startTimeSlot.CustomFormat = "HH"; // Display hours only (24-hour format)

            endTimeSlot.Format = DateTimePickerFormat.Time;
            endTimeSlot.ShowUpDown = true; // This will display up and down arrows for easier hour selection
            endTimeSlot.CustomFormat = "HH"; // Display hours only (24-hour format)


        }
        private void initChartTab()
        {
            dateTimePickerFromStatictis.Format = DateTimePickerFormat.Custom;
            dateTimePickerFromStatictis.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Adjust format as needed
            dateTimePickerToStatictis.Format = DateTimePickerFormat.Custom;
            dateTimePickerToStatictis.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Adjust format as needed
        }

        private void initHistoryTab()
        {
            dateTimePickerFrom.Format = DateTimePickerFormat.Custom;
            dateTimePickerFrom.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Adjust format as needed
            dateTimePickerTo.Format = DateTimePickerFormat.Custom;
            dateTimePickerTo.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Adjust format as needed


            dataGridViewHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12); // Set font size for column headers
            dataGridViewHistory.RowsDefaultCellStyle.Font = new Font("Arial", 12); // Set font size for rows
            dataGridViewHistory.RowHeadersDefaultCellStyle.Font = new Font("Arial", 12); // Set font size for row headers
            dataGridViewHistory.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            pictureBoxImagePlateIn.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxImagePlateOut.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void fillChart(string[] arraySeries, string[][] matrix, string typeChart)
        {
            SeriesChartType typeC;
            if (typeChart.Equals("Pie"))
            {
                typeC = SeriesChartType.Pie;
            }
            else
            {
                typeC = SeriesChartType.Column;
            }
           
            for (int i = 1; i < arraySeries.Length; i++)
            {
                Series s = new Series();
                // Append a unique identifier to the series name
                s.Name = arraySeries[i]; // For example: "Xe gắn máy_1", "Xe mô tô_2", etc.
                s.ChartType = typeC;
                chart1.Series.Add(s);
            }
            // Iterate over each row
            for (int i = 0; i < matrix.Length; i++)
            {
                // Iterate over each column in the current row
                for (int j = 1; j < matrix[i].Length; j++)
                {
                    float tep = float.Parse(matrix[i][j]);
                    chart1.Series[j - 1].Points.AddXY(matrix[i][0], tep);
                }
            }

       
        }
        private void Load_Chart(string procedure, string formattedDateTimeFrom, string formattedDateTimeTo, string typeChart)
        {
            var dt = statictisSingleTicketRepository.sp_statisticLineChartByDay(procedure, formattedDateTimeFrom, formattedDateTimeTo);
            DataRow row = dt.Rows[0];
            string Series = (string)row["Series"];
            string Data = (string)row["Data"];

            string[] arraySeries = SplitStringToArray(Series);
            string[][] matrix = ConvertToMatrix(Data);
            fillChart(arraySeries, matrix,typeChart);
        }
        private void LoginForm_Shown(object sender, EventArgs e)
        {
            Login loginForm = new Login();

            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // Access the username property from the login form
                string username = loginForm.User.user_name;


            }
        }
        public bool ContainsTabWithName(MetroTabControl tabControl, string tabName)
        {
            foreach (MetroTabPage tabPage in tabControl.TabPages)
            {
                if (tabPage.Name == tabName)
                {
                    return true;
                }
            }

            return false;
        }
        private void historyTile_Click(object sender, EventArgs e)
        {
            navigateToTab(homePage, historyInOutTabPage);
            //DataGridViewHistory.AutoGenerateColumns = true;
            //DataGridViewHistory.DataSource = generalRepository.ExecDataTableHistoryInOut();

            DateTime selectedDateTimeFrom = dateTimePickerFrom.Value;
            string formattedDateTimeFrom = selectedDateTimeFrom.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime selectedDateTimeTo = dateTimePickerTo.Value;
            string formattedDateTimeTo = selectedDateTimeTo.ToString("yyyy-MM-dd HH:mm:ss");


            string isOut = checkBoxIsOut.Checked ? "1" : "0";
            string isMonthLy = checkBoxMonthly.Checked ? "1" : "0";
            string isIn = checkBoxIsIn.Checked ? "1" : "0";
            string isSingleTiceket = checkBoxSingleTicket.Checked ? "1" : "0";

            LoadDataToDataGridViewHistory(formattedDateTimeFrom, formattedDateTimeTo, isOut, isMonthLy, isIn, isSingleTiceket);

        }
        private void ticketdataGridView_SelectionChanged(object sender, EventArgs e)
        {
            // Assuming columnIndex is the index of the column you want to display
            int columnIndexTicketTypeID = GetColumnIndexByName(ticketdataGridView, "Mã loại vé");
            int columnIndexDayApply = GetColumnIndexByName(ticketdataGridView, "Ngày áp dụng"); 
            int columnIndexTicketTypeName = GetColumnIndexByName(ticketdataGridView, "Tên loại vé");
            int columnIndexcarTypeID = GetColumnIndexByName(ticketdataGridView, "Loại xe");     
            int columnIndexmonthlyFree = GetColumnIndexByName(ticketdataGridView, "Giá tháng");
            int columnIndexhourMin = GetColumnIndexByName(ticketdataGridView, "Khung giờ tối thiểu"); 
            int columnIndexmethod = GetColumnIndexByName(ticketdataGridView, "Cách tính tiền");
            int columnIndexReminderCardExpiration = GetColumnIndexByName(ticketdataGridView, "Số ngày trước ngày hết hạn để hiện thông báo");
            // Make sure at least one row is selected
            if (ticketdataGridView.SelectedRows.Count > 0 &&
                ticketdataGridView.SelectedRows[0].Cells[columnIndexTicketTypeID] != null &&
                ticketdataGridView.SelectedRows[0].Cells[columnIndexDayApply] != null &&
                ticketdataGridView.SelectedRows[0].Cells[columnIndexTicketTypeID].Value != null &&
                ticketdataGridView.SelectedRows[0].Cells[columnIndexDayApply].Value != null)
            {

              
                // Get the value from the selected row and column
                string selectedValueTicketTypeID = ticketdataGridView.SelectedRows[0].Cells[columnIndexTicketTypeID].Value.ToString();
                string selectedValueDayApply = ConvertDateFormat(ticketdataGridView.SelectedRows[0].Cells[columnIndexDayApply].Value.ToString());
                string selectedValueTicketTypeName = ticketdataGridView.SelectedRows[0].Cells[columnIndexTicketTypeName].Value.ToString();
                string selectedValuecarTypeID = ticketdataGridView.SelectedRows[0].Cells[columnIndexcarTypeID].Value.ToString();
                string selectedValuemonthlyFree = ticketdataGridView.SelectedRows[0].Cells[columnIndexmonthlyFree].Value.ToString();
                string selectedValuehourMin = ticketdataGridView.SelectedRows[0].Cells[columnIndexhourMin].Value.ToString();
                string selectedValuemethod = ticketdataGridView.SelectedRows[0].Cells[columnIndexmethod].Value.ToString();
                string selectedValueReminderCardExpiration = ticketdataGridView.SelectedRows[0].Cells[columnIndexReminderCardExpiration].Value.ToString();

                textBoxTypeId.Text = selectedValueTicketTypeID;
                textBoxTicketName.Text = selectedValueTicketTypeName;
                textBoxMinTime.Text = selectedValuehourMin;
                textBoxDayAlert.Text = selectedValueReminderCardExpiration;
                textBoxMonthLyPrice.Text = selectedValuemonthlyFree;
                ApplyDayBox.Text = selectedValueDayApply;
                comboBoxCarType.SelectedValue = selectedValuecarTypeID;
                comboBoxMethodCalPrice.SelectedValue = selectedValuemethod;


                ButtonUpdate.Enabled = true;
                ButtonDelete.Enabled = true;
                ButtonSaveTicketType.Enabled = false;

                sp_TicketPriceManagementSubmenu(int.Parse(selectedValueTicketTypeID), selectedValueDayApply);

                dayApplySelected = selectedValueDayApply;
                typeTicketSelected = selectedValueTicketTypeID;

                comboBoxTicketTypeID.SelectedValue = typeTicketSelected;
                ApplyDayBoxSubMenu.Text = dayApplySelected;

                ApplyDayBox.Enabled = false;
                textBoxTypeId.Enabled = false;


                comboBoxTicketTypeID.Enabled = false;
                ApplyDayBoxSubMenu.Enabled = false;

            }
            else
            {
               
                textBoxTypeId.Text = "";
                textBoxTicketName.Text = "";
                textBoxMinTime.Text = "";
                textBoxDayAlert.Text = "";
                textBoxMonthLyPrice.Text = "";
                ApplyDayBox.Text = "";
                ResetCombobox(comboBoxCarType);
                ResetCombobox(comboBoxMethodCalPrice);

                // Clear existing columns and data in DataGridView
                dataGridTicketPriceByTimeSlot.Columns.Clear();
                dataGridTicketPriceByTimeSlot.Rows.Clear();

                ApplyDayBox.Enabled = true;
                textBoxTypeId.Enabled = true;

                ButtonSaveTicketTypeSm.Enabled = false;

                ButtonSaveTicketType.Enabled = true;
                ButtonUpdate.Enabled = false;
                ButtonDelete.Enabled = false;
            }
        }
        public void sp_TicketPriceManagementSubmenu(int TicketTypeID, string DayApply)
        {
            // Retrieve data from the database
            DataTable dataTable = ticketRepository.sp_TicketPriceManagementSubmenu(TicketTypeID, DayApply);


            // Clear existing columns and data in DataGridView
            dataGridTicketPriceByTimeSlot.Columns.Clear();
            dataGridTicketPriceByTimeSlot.Rows.Clear();
          
            // Define column names for DataGridView
            string[] columnNamesMap = { "TicketTypeID", "ShiftID", "DayApply", "Price", "minPrice", "generalPriceForTimeSlot", "overtimePay" };
            string[] columnNames = { "Mã loại vé", "Mã khung giờ", "Ngày áp dụng", "Giá tiền", "Giá tối thiểu", "Giá chung cho cả khung", "Tiền vượt giờ"};


            // Add columns to the DataGridView
            foreach (string columnName in columnNames)
            {
                dataGridTicketPriceByTimeSlot.Columns.Add(columnName, columnName);
            }

            // Bind data from DataTable to DataGridView
            foreach (DataRow row in dataTable.Rows)
            {
                // Create an array to hold data for each row
                object[] rowData = new object[columnNames.Length];

                // Copy data from DataTable row to the array for selected columns
                for (int i = 0; i < columnNamesMap.Length; i++)
                {
                    rowData[i] = row[columnNamesMap[i]];
                }

                // Add row to the DataGridView
                dataGridTicketPriceByTimeSlot.Rows.Add(rowData);
            }
            //ticketdataGridView.Columns[7].Visible = false;
            //ticketdataGridView.Columns[8].Visible = false;

            // Auto resize column width to fit text
            foreach (DataGridViewColumn column in dataGridTicketPriceByTimeSlot.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            dataGridTicketPriceByTimeSlot.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        public void LoadDataToTicketDataGridView()
        {
            // Retrieve data from the database
            DataTable dataTable = ticketRepository.sp_TicketPriceManagement();

            // Clear existing columns and data in DataGridView
            ticketdataGridView.Columns.Clear();
            ticketdataGridView.Rows.Clear();

            // Define column names for DataGridView
            string[] columnNamesMap = { "TicketTypeName", "DayApply", "carTypeID", "monthlyFree", "hourMin", "method", "ReminderCardExpiration", "TicketTypeID" };
            string[] columnNames = { "Tên loại vé", "Ngày áp dụng", "Loại xe", "Giá tháng", "Khung giờ tối thiểu", "Cách tính tiền", "Số ngày trước ngày hết hạn để hiện thông báo","Mã loại vé"};


            // Add columns to the DataGridView
            foreach (string columnName in columnNames)
            {
                ticketdataGridView.Columns.Add(columnName, columnName);
            }

            // Bind data from DataTable to DataGridView
            foreach (DataRow row in dataTable.Rows)
            {
                // Create an array to hold data for each row
                object[] rowData = new object[columnNames.Length];

                // Copy data from DataTable row to the array for selected columns
                for (int i = 0; i < columnNamesMap.Length; i++)
                {
                    rowData[i] = row[columnNamesMap[i]];
                }

                // Add row to the DataGridView
                ticketdataGridView.Rows.Add(rowData);
            }
            //ticketdataGridView.Columns[7].Visible = false;
            //ticketdataGridView.Columns[8].Visible = false;

            // Auto resize column width to fit text
            foreach (DataGridViewColumn column in ticketdataGridView.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            ticketdataGridView.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        public void LoadDataToTimeSlotDataGridView()
        {
            // Retrieve data from the database
            DataTable dataTable = ticketRepository.sp_ShiftManagementSubmenu();

            // Clear existing columns and data in DataGridView
            timeSlotDataGridView.Columns.Clear();
            timeSlotDataGridView.Rows.Clear();
       
            // Define column names for DataGridView
            string[] columnNamesMap = { "ShiftID", "ShiftName", "startPackingShifts", "endPackingShifts" };
            string[] columnNames = { "Mã khung giờ", "Tên khung giờ", "Giờ bắt đầu", "Giờ kết thúc"};


            // Add columns to the DataGridView
            foreach (string columnName in columnNames)
            {
                timeSlotDataGridView.Columns.Add(columnName, columnName);
            }

            // Bind data from DataTable to DataGridView
            foreach (DataRow row in dataTable.Rows)
            {
                // Create an array to hold data for each row
                object[] rowData = new object[columnNames.Length];

                // Copy data from DataTable row to the array for selected columns
                for (int i = 0; i < columnNamesMap.Length; i++)
                {
                    rowData[i] = row[columnNamesMap[i]];
                }

                // Add row to the DataGridView
                timeSlotDataGridView.Rows.Add(rowData);
            }
            //ticketdataGridView.Columns[7].Visible = false;
            //ticketdataGridView.Columns[8].Visible = false;

            // Auto resize column width to fit text
            foreach (DataGridViewColumn column in timeSlotDataGridView.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            timeSlotDataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        static string ConvertDateFormat(string inputDate)
        {
            CultureInfo culture = new CultureInfo("en-US");

            return Convert.ToDateTime(inputDate, culture).ToString("yyyy-MM-dd HH:mm:ss");
        }
        public void LoadDataToDataGridViewHistory(string formattedDateTimeFrom, string formattedDateTimeTo, string isOut, string isMonthLy, string isIn, string isSingleTiceket)
        {
            // Retrieve data from the database
            DataTable dataTable = generalRepository.ExecDataTableHistoryInOut(formattedDateTimeFrom, formattedDateTimeTo, isOut, isMonthLy, isIn, isSingleTiceket);

            // Clear existing columns and data in DataGridView
            dataGridViewHistory.Columns.Clear();
            dataGridViewHistory.Rows.Clear();

            // Define column names for DataGridView
            string[] columnNames = { "Ngày vào", "Ngày ra", "Tổng tiền", "Biển số xe vào", "Khu đậu", "Loại vé", "Mã thẻ","urlIn","urlOut" };
            string[] columnNamesMap = { "time_In", "time_Out", "totalAmount", "licensePlateNumberIn", "packingSlotName", "TicketTypeName", "cardInfroID", "PhotoLicensePlateNumberInPath", "PhotoLicensePlateNumberOutPath" };

            // Add columns to the DataGridView
            foreach (string columnName in columnNames)
            {
                dataGridViewHistory.Columns.Add(columnName, columnName);
            }

            // Bind data from DataTable to DataGridView
            foreach (DataRow row in dataTable.Rows)
            {
                // Create an array to hold data for each row
                object[] rowData = new object[columnNames.Length];

                // Copy data from DataTable row to the array for selected columns
                for (int i = 0; i < columnNamesMap.Length; i++)
                {
                    rowData[i] = row[columnNamesMap[i]];
                }

                // Add row to the DataGridView
                dataGridViewHistory.Rows.Add(rowData);
            }
            dataGridViewHistory.Columns[7].Visible = false;
            dataGridViewHistory.Columns[8].Visible = false;

            // Auto resize column width to fit text
            foreach (DataGridViewColumn column in dataGridViewHistory.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
          
            dataGridViewHistory.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }



        private void statusTile_Click(object sender, EventArgs e)
        {
            navigateToTab(homePage, currentStatusTabPage);

        }

        private void caTile_Click(object sender, EventArgs e)
        {
            navigateToTab(homePage, chartTabPage);

        }
        private void reloadComboBoxTicketMenu()
        {
            comboBoxCarType.DataSource = ticketRepository.sp_carTypeList_DataSetting();
            comboBoxCarType.DisplayMember = "Description";
            comboBoxCarType.ValueMember = "carTypeId";

            comboBoxMethodCalPrice.DataSource = ticketRepository.sp_methodCalPrice_DataSetting();
            comboBoxMethodCalPrice.DisplayMember = "Description";
            comboBoxMethodCalPrice.ValueMember = "method";

            comboBoxShiftID.DataSource = ticketRepository.sp_shiftList();
            comboBoxShiftID.DisplayMember = "ShiftName";
            comboBoxShiftID.ValueMember = "ShiftID";

            comboBoxTicketTypeID.DataSource = ticketRepository.sp_ticketTypeList();
            comboBoxTicketTypeID.DisplayMember = "Description";
            comboBoxTicketTypeID.ValueMember = "TicketTypeID";

        }
        private void ticketPriceTimeSlotTile_Click(object sender, EventArgs e)
        {
            navigateToTab(homePage, ticketPriceTimeSlotTabPage);
            LoadDataToTicketDataGridView();
            LoadDataToTimeSlotDataGridView();

            // Bind the DataTable to the ComboBox
            reloadComboBoxTicketMenu();

            //comboBoxTicketTypeID,comboBoxShiftID,ApplyDayBoxSubMenu

            //textBoxTypeId, textBoxTicketName,textBoxMinTime,textBoxDayAlert,textBoxMonthLyPrice
            ButtonUpdate.Enabled = false;
            ButtonDelete.Enabled = false;
            ButtonSaveTicketType.Enabled = false;

        }
        private void ButtonUpdate_Click(object sender, EventArgs e)
        {
            DateTime selectedDateTimeFrom = ApplyDayBox.Value;
            string formattedDateTimeFrom = selectedDateTimeFrom.ToString("yyyy-MM-dd");
            ticketRepository.sp_updateTicketType(textBoxTicketName.Text,
               formattedDateTimeFrom, (int)comboBoxCarType.SelectedValue, float.Parse(textBoxMonthLyPrice.Text),
                 int.Parse(textBoxMinTime.Text), (int)comboBoxMethodCalPrice.SelectedValue, int.Parse(textBoxDayAlert.Text), int.Parse(textBoxTypeId.Text));
            LoadDataToTicketDataGridView();
            ButtonUpdate.Enabled = false;
            ButtonDelete.Enabled = false;
            ButtonSaveTicketType.Enabled = false;

            reloadComboBoxTicketMenu();
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            DateTime selectedDateTimeFrom = ApplyDayBox.Value;
            string formattedDateTimeFrom = selectedDateTimeFrom.ToString("yyyy-MM-dd");
            ticketRepository.sp_deleteTicketType(
               formattedDateTimeFrom,  int.Parse(textBoxTypeId.Text));
            LoadDataToTicketDataGridView();

            ButtonUpdate.Enabled = false;
            ButtonDelete.Enabled = false;
            ButtonSaveTicketType.Enabled = false;

            reloadComboBoxTicketMenu();
        }
        private void ButtonSaveTicketType_Click(object sender, EventArgs e)
        {
            DateTime selectedDateTimeFrom = ApplyDayBox.Value;
            string formattedDateTimeFrom = selectedDateTimeFrom.ToString("yyyy-MM-dd");
            ticketRepository.sp_insertTicketType(textBoxTicketName.Text,
               formattedDateTimeFrom, (int)comboBoxCarType.SelectedValue, float.Parse(textBoxMonthLyPrice.Text),
                 int.Parse(textBoxMinTime.Text), (int)comboBoxMethodCalPrice.SelectedValue, int.Parse(textBoxDayAlert.Text), int.Parse(textBoxTypeId.Text));
            LoadDataToTicketDataGridView();
            ButtonUpdate.Enabled = false;
            ButtonDelete.Enabled = false;
            ButtonSaveTicketType.Enabled = false;

            reloadComboBoxTicketMenu();
        }

        private void customerTile_Click(object sender, EventArgs e)
        {
            navigateToTab(homePage, customerListTabPage);


        }

        private void generalStatisticTile_Click(object sender, EventArgs e)
        {
            navigateToTab(homePage, statictisTabPage);

        }

        private void diaryLostTicketTile_Click(object sender, EventArgs e)
        {
            navigateToTab(homePage, diaryLostTicketTabPage);

        }

        private void diaryLostVehicleTile_Click(object sender, EventArgs e)
        {

            navigateToTab(homePage, diaryLostVehicleTabPage);

        }

        private void longTimeTile_Click(object sender, EventArgs e)
        {
            navigateToTab(homePage, overdueTabPage);


        }

        private void userManagementTile_Click(object sender, EventArgs e)
        {
            navigateToTab(homePage, userManagementTabPage);
        }

        private void navigateToTab(MetroTabControl homePage, MetroTabPage tabPageToAdd)
        {
            if (ContainsTabWithName(homePage, tabPageToAdd.Name))
            {
                homePage.SelectedTab = tabPageToAdd;
            }
            else
            {
                homePage.TabPages.Add(tabPageToAdd);
                homePage.SelectedTab = tabPageToAdd;
            }
        }

        private void closeTabGeneralStatisticButton_Click(object sender, EventArgs e)
        {
            closeTab();
        }

        private void closeLoginManagementButton_Click(object sender, EventArgs e)
        {

            closeTab();
        }

        private void closeTabHisButton_Click(object sender, EventArgs e)
        {
            closeTab();
        }

        private void closeStatusButton_Click(object sender, EventArgs e)
        {
            closeTab();
        }

        private void closeChartStatisticButton_Click(object sender, EventArgs e)
        {
            closeTab();
        }

        private void closeSettingTicketTabButton_Click(object sender, EventArgs e)
        {

            closeTab();
        }

        private void closeCustomerTabButton_Click(object sender, EventArgs e)
        {
            closeTab();
        }

        private void closeOverdueTabButton_Click(object sender, EventArgs e)
        {
            closeTab();
        }

        private void closeDiaryLostTicketTabButton_Click(object sender, EventArgs e)
        {
            closeTab();
        }

        private void closeDiaryLostVehicleTabButton_Click(object sender, EventArgs e)
        {
            closeTab();
        }

        private void homePage_SelectedIndexChanged(object sender, EventArgs e)
        {


            // Check if the top item in the stack is not the same as the currently selected tab
            if (homePage.SelectedTab != null || tabStack.Peek() != homePage.SelectedTab)
            {
                // Push the currently selected tab onto the stack
                tabStack.Push((MetroTabPage)homePage.SelectedTab);
            }
        }

        private void closeTab()
        {



            if (tabStack.Count > 0)
            {
                if (tabStack.Count == 1 && tabStack.Peek().Name.Equals(homeTabPage.Name))
                {
                    homePage.SelectedTab = tabStack.Pop();
                    return;
                }

                MetroTabPage previousTabPage1 = tabStack.Pop();
                homePage.TabPages.Remove(previousTabPage1);


                MetroTabPage previousTabPage = tabStack.Pop();


                // Create a list to store the items to remove
                List<MetroTabPage> itemsToRemove = new List<MetroTabPage>();

                // Iterate over the stack and add items to remove to the list
                foreach (var item in tabStack)
                {
                    if (!ContainsTabWithName(homePage, item.Name))
                    {
                        itemsToRemove.Add(item);
                    }
                }

                // Remove the items from the stack
                foreach (var itemToRemove in itemsToRemove)
                {
                    RemoveItemFromStack(tabStack, itemToRemove);
                }


                if (tabStack.Count == 1 && tabStack.Peek().Name.Equals(homeTabPage.Name))
                {
                    homePage.SelectedTab = tabStack.Pop();
                    return;
                }

                if (tabStack.Count > 1)
                {

                    previousTabPage = tabStack.Pop();
                    previousTabPage = tabStack.Pop();

                }


                homePage.SelectedTab = previousTabPage;

            }
        }

        private void RemoveItemFromStack(Stack<MetroTabPage> stack, MetroTabPage itemToRemove)
        {
            // Convert the stack to an array
            MetroTabPage[] stackArray = stack.ToArray();

            // Remove the item from the array
            stackArray = Array.FindAll(stackArray, x => x != itemToRemove);

            // Reconstruct the stack from the modified array
            stack.Clear();
            foreach (var item in stackArray)
            {
                stack.Push(item);
            }
        }

        private void inOutTile_Click(object sender, EventArgs e)
        {
            if (formInOut.IsDisposed)
            {
                formInOut = new LaneInOut();
            }
            formInOut.WindowState = FormWindowState.Maximized;
            formInOut.Show();
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {



            DateTime selectedDateTimeFrom = dateTimePickerFrom.Value;
            string formattedDateTimeFrom = selectedDateTimeFrom.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime selectedDateTimeTo = dateTimePickerTo.Value;
            string formattedDateTimeTo = selectedDateTimeTo.ToString("yyyy-MM-dd HH:mm:ss");


            string isOut = checkBoxIsOut.Checked ? "1" : "0";
            string isMonthLy = checkBoxMonthly.Checked ? "1" : "0";
            string isIn = checkBoxIsIn.Checked ? "1" : "0";
            string isSingleTiceket = checkBoxSingleTicket.Checked ? "1" : "0";


            LoadDataToDataGridViewHistory(formattedDateTimeFrom, formattedDateTimeTo, isOut, isMonthLy, isIn, isSingleTiceket);
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            // Get the search keyword from the textbox
            string keyword = textBoxSearchHistory.Text.Trim();

            // Perform the search
            if (!string.IsNullOrEmpty(keyword))
            {
                // Loop through each row in the DataGridView
                foreach (DataGridViewRow row in dataGridViewHistory.Rows)
                {
                    // Loop through each cell in the row
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        // Check if the cell's value contains the search keyword
                        if (cell.Value != null && cell.Value.ToString().Contains(keyword))
                        {
                            // Select the row and scroll it into view
                            row.Selected = true;
                            dataGridViewHistory.FirstDisplayedScrollingRowIndex = row.Index;
                            return; // Exit the method after finding the first match
                        }
                    }
                }

                // If no matching records found, display a message
                MessageBox.Show("No matching records found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void excelHistoryMonthLy_Click(object sender, EventArgs e)
        {
            DateTime selectedDateTimeFrom = dateTimePickerFrom.Value;
            string formattedDateTimeFrom = selectedDateTimeFrom.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime selectedDateTimeTo = dateTimePickerTo.Value;
            string formattedDateTimeTo = selectedDateTimeTo.ToString("yyyy-MM-dd HH:mm:ss");


            string isOut = checkBoxIsOut.Checked ? "1" : "0";
            string isMonthLy = checkBoxMonthly.Checked ? "1" : "0";
            string isIn = checkBoxIsIn.Checked ? "1" : "0";
            string isSingleTiceket = checkBoxSingleTicket.Checked ? "1" : "0";
            DataTable dataTable = generalRepository.ExecDataTableHistoryInOut(formattedDateTimeFrom, formattedDateTimeTo, isOut, isMonthLy, isIn, isSingleTiceket);
            string filePath = @"C:\Users\trant\Documents\File.xlsx"; // Specify your file path here
            ExcelHelper.ExportDataTableToExcel(dataTable, filePath);

        }

        private void excelHistorySinle_Click(object sender, EventArgs e)
        {
            DateTime selectedDateTimeFrom = dateTimePickerFrom.Value;
            string formattedDateTimeFrom = selectedDateTimeFrom.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime selectedDateTimeTo = dateTimePickerTo.Value;
            string formattedDateTimeTo = selectedDateTimeTo.ToString("yyyy-MM-dd HH:mm:ss");

            DataTable dataTable = generalRepository.sp_exportSingleUseTicketRevenueStatistics(formattedDateTimeFrom, formattedDateTimeTo);
            string filePath = @"C:\Users\trant\Documents\File.xlsx"; // Specify your file path here
            ExcelHelper.ExportDataTableToExcel(dataTable, filePath);
        }

        private void historyInOutTabPage_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewHistory_SelectionChanged(object sender, EventArgs e)
        {

            // Assuming columnIndex is the index of the column you want to display
            int columnIndexurlOut = GetColumnIndexByName(dataGridViewHistory, "urlOut");
            int columnIndexurlIn = GetColumnIndexByName(dataGridViewHistory, "urlIn");
            // Make sure at least one row is selected
            if (dataGridViewHistory.SelectedRows.Count > 0 &&
                dataGridViewHistory.SelectedRows[0].Cells[columnIndexurlOut] != null &&
                dataGridViewHistory.SelectedRows[0].Cells[columnIndexurlIn] != null &&
                dataGridViewHistory.SelectedRows[0].Cells[columnIndexurlOut].Value != null &&
                dataGridViewHistory.SelectedRows[0].Cells[columnIndexurlIn].Value != null)
            {
              
                
                // Get the value from the selected row and column
                string selectedValueurlOut = dataGridViewHistory.SelectedRows[0].Cells[columnIndexurlOut].Value.ToString();
                string selectedValueurlIn = dataGridViewHistory.SelectedRows[0].Cells[columnIndexurlIn].Value.ToString();

                pictureBoxImagePlateIn.Image = Image.FromFile(selectedValueurlIn);
                pictureBoxImagePlateOut.Image = Image.FromFile(selectedValueurlOut);
            }

        }

        private int GetColumnIndexByName(DataGridView dataGridView, string columnName)
        {
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return column.Index;
                }
            }
            // If column not found, return -1 or throw an exception
            return -1;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void RealoadStatictis_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();

            if (isMonthlyTicket.Checked)
            {
                DateTime selectedDateTimeFrom = dateTimePickerFromStatictis.Value;
                string formattedDateTimeFrom = selectedDateTimeFrom.ToString("yyyy-MM-dd");

                DateTime selectedDateTimeTo = dateTimePickerToStatictis.Value;
                string formattedDateTimeTo = selectedDateTimeTo.ToString("yyyy-MM-dd");
                Load_Chart("sp_statisticColumnChartByMonthlyTicket", formattedDateTimeFrom, formattedDateTimeTo, "Column");
            }
            else if (isYear.Checked)
            {
                DateTime selectedDateTimeFrom = dateTimePickerFromStatictis.Value;
                string formattedDateTimeFrom = selectedDateTimeFrom.ToString("yyyy") +"-01-01";

                DateTime selectedDateTimeTo = dateTimePickerToStatictis.Value;
                string formattedDateTimeTo = selectedDateTimeTo.ToString("yyyy-MM-dd");
                Load_Chart("sp_3D_Pie_Chart_ByMonthInYear", formattedDateTimeFrom, formattedDateTimeTo,"Pie");
            } else if (isDay.Checked)
            {
                DateTime selectedDateTimeFrom = dateTimePickerFromStatictis.Value;
                string formattedDateTimeFrom = selectedDateTimeFrom.ToString("yyyy-MM-dd");

                DateTime selectedDateTimeTo = dateTimePickerToStatictis.Value;
                string formattedDateTimeTo = selectedDateTimeTo.ToString("yyyy-MM-dd");
                Load_Chart("sp_statisticLineChartByDay", formattedDateTimeFrom, formattedDateTimeTo, "Column");

                

            } else if (isMonth.Checked)
            {
                DateTime selectedDateTimeFrom = dateTimePickerFromStatictis.Value;
                string formattedDateTimeFrom = selectedDateTimeFrom.ToString("yyyy-MM-dd");

                DateTime selectedDateTimeTo = dateTimePickerToStatictis.Value;
                string formattedDateTimeTo = selectedDateTimeTo.ToString("yyyy-MM-dd");
                Load_Chart("sp_statisticColumnChartByMonth", formattedDateTimeFrom, formattedDateTimeTo, "Column");
               
            } else if (isTimeSlot.Checked)
            {
                DateTime selectedDateTimeFrom = dateTimePickerFromStatictis.Value;
                string formattedDateTimeFrom = selectedDateTimeFrom.ToString("yyyy-MM-dd HH:mm:ss");

                DateTime selectedDateTimeTo = dateTimePickerToStatictis.Value;
                string formattedDateTimeTo = selectedDateTimeTo.ToString("yyyy-MM-dd HH:mm:ss");

                Load_Chart("sp_StatisticsChartByTimeSlot", formattedDateTimeFrom, formattedDateTimeTo, "Column");
            }
        }

        private void isTimeSlot_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePickerFromStatictis.Format = DateTimePickerFormat.Custom;
            dateTimePickerFromStatictis.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Adjust format as needed
            dateTimePickerToStatictis.Format = DateTimePickerFormat.Custom;
            dateTimePickerToStatictis.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Adjust format as needed

            label1.Text = "Từ ngày";
            label4.Text = "Đến ngày";
            label4.Visible = true;
            dateTimePickerToStatictis.Visible = true;
        }

        private void isDay_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePickerFromStatictis.Format = DateTimePickerFormat.Custom;
            dateTimePickerFromStatictis.CustomFormat = "yyyy-MM-dd"; // Adjust format as needed
            dateTimePickerToStatictis.Format = DateTimePickerFormat.Custom;
            dateTimePickerToStatictis.CustomFormat = "yyyy-MM-dd"; // Adjust format as needed

            label1.Text = "Từ ngày";
            label4.Text = "Đến ngày";
            label4.Visible = true;
            dateTimePickerToStatictis.Visible = true;
        }

        private void isMonth_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePickerFromStatictis.Format = DateTimePickerFormat.Custom;
            dateTimePickerFromStatictis.CustomFormat = "yyyy-MM"; // Adjust format as needed
            dateTimePickerToStatictis.Format = DateTimePickerFormat.Custom;
            dateTimePickerToStatictis.CustomFormat = "yyyy-MM"; // Adjust format as needed

            label1.Text = "Từ tháng";
            label4.Text = "Đến tháng";
            label4.Visible = true;
            dateTimePickerToStatictis.Visible = true;
        }

        private void isYear_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePickerFromStatictis.Format = DateTimePickerFormat.Custom;
            dateTimePickerFromStatictis.CustomFormat = "yyyy"; // Adjust format as needed

            dateTimePickerToStatictis.Hide();
            label4.Hide();
            label1.Text = "Năm";
          
            //dateTimePickerToStatictis.Format = DateTimePickerFormat.Custom;
            //dateTimePickerToStatictis.CustomFormat = "yyyy"; // Adjust format as needed
        }

        private void isMonthlyTicket_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePickerFromStatictis.Format = DateTimePickerFormat.Custom;
            dateTimePickerFromStatictis.CustomFormat = "yyyy-MM"; // Adjust format as needed
            dateTimePickerToStatictis.Format = DateTimePickerFormat.Custom;
            dateTimePickerToStatictis.CustomFormat = "yyyy-MM"; // Adjust format as needed

            label1.Text = "Từ tháng";
            label4.Text = "Đến tháng";
            label4.Visible = true;
            dateTimePickerToStatictis.Visible = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridTicketPriceByTimeSlot_SelectionChanged(object sender, EventArgs e)
        {
            //comboBoxTicketTypeID,comboBoxShiftID,ApplyDayBoxSubMenu,textBoxPrice,textBoxPriceTimeSlot,
            //    textBoxMinPrice,textBoxPriceOverTimeSlot

            // Assuming columnIndex is the index of the column you want to display
            int columnIndexTicketTypeID = GetColumnIndexByName(dataGridTicketPriceByTimeSlot, "Mã loại vé");
            int columnIndexShiftID = GetColumnIndexByName(dataGridTicketPriceByTimeSlot, "Mã khung giờ");
            int columnIndexDayApply = GetColumnIndexByName(dataGridTicketPriceByTimeSlot, "Ngày áp dụng");
            int columnIndexPrice = GetColumnIndexByName(dataGridTicketPriceByTimeSlot, "Giá tiền");
            int columnIndexminPrice = GetColumnIndexByName(dataGridTicketPriceByTimeSlot, "Giá tối thiểu");
            int columnIndexgeneralPriceForTimeSlot = GetColumnIndexByName(dataGridTicketPriceByTimeSlot, "Giá chung cho cả khung");
            int columnIndexovertimePay = GetColumnIndexByName(dataGridTicketPriceByTimeSlot, "Tiền vượt giờ");
           
            // Make sure at least one row is selected
            if (dataGridTicketPriceByTimeSlot.SelectedRows.Count > 0 &&
                dataGridTicketPriceByTimeSlot.SelectedRows[0].Cells[columnIndexTicketTypeID] != null &&
                dataGridTicketPriceByTimeSlot.SelectedRows[0].Cells[columnIndexDayApply] != null &&
                dataGridTicketPriceByTimeSlot.SelectedRows[0].Cells[columnIndexTicketTypeID].Value != null &&
                dataGridTicketPriceByTimeSlot.SelectedRows[0].Cells[columnIndexDayApply].Value != null)
            {


                // Get the value from the selected row and column
                string selectedValueTicketTypeID = dataGridTicketPriceByTimeSlot.SelectedRows[0].Cells[columnIndexTicketTypeID].Value.ToString();
                string selectedValueDayApply = ConvertDateFormat(dataGridTicketPriceByTimeSlot.SelectedRows[0].Cells[columnIndexDayApply].Value.ToString());
                string selectedValueShiftID = dataGridTicketPriceByTimeSlot.SelectedRows[0].Cells[columnIndexShiftID].Value.ToString();
                string selectedValuePrice = dataGridTicketPriceByTimeSlot.SelectedRows[0].Cells[columnIndexPrice].Value.ToString();
                string selectedValueminPrice = dataGridTicketPriceByTimeSlot.SelectedRows[0].Cells[columnIndexminPrice].Value.ToString();
                string selectedValuegeneralPriceForTimeSlot = dataGridTicketPriceByTimeSlot.SelectedRows[0].Cells[columnIndexgeneralPriceForTimeSlot].Value.ToString();
                string selectedValueovertimePay = dataGridTicketPriceByTimeSlot.SelectedRows[0].Cells[columnIndexovertimePay].Value.ToString();


                comboBoxTicketTypeID.SelectedValue = int.Parse(selectedValueTicketTypeID);
                comboBoxShiftID.SelectedValue = int.Parse(selectedValueShiftID);
                ApplyDayBoxSubMenu.Text = selectedValueDayApply;
                textBoxPrice.Text = selectedValuePrice;
                textBoxPriceTimeSlot.Text = selectedValuegeneralPriceForTimeSlot;
                textBoxMinPrice.Text = selectedValueminPrice;
                textBoxPriceOverTimeSlot.Text = selectedValueovertimePay;

              

                ButtonUpdateSm.Enabled = true;
                ButtonDeleteSm.Enabled = true;
                ButtonSaveTicketTypeSm.Enabled = false;

             

            }
            else
            {
           
                //ResetCombobox(comboBoxTicketTypeID);
                ResetCombobox(comboBoxShiftID);
                //ApplyDayBoxSubMenu.Text = "";
                textBoxPrice.Text = "";
                textBoxPriceTimeSlot.Text = "";
                textBoxMinPrice.Text = "";
                textBoxPriceOverTimeSlot.Text = "";
               

                ButtonSaveTicketTypeSm.Enabled = true;
                ButtonUpdateSm.Enabled = false;
                ButtonDeleteSm.Enabled = false;

                if (typeTicketSelected != null)
                {
                    comboBoxTicketTypeID.SelectedValue = typeTicketSelected;
                    ApplyDayBoxSubMenu.Text = dayApplySelected;
                }
          
            }
        }

        public void ResetCombobox(ComboBox cb)
        {
            cb.SelectedIndex = -1;
            cb.Text = "";
        }

        private void ButtonSaveTicketTypeSm_Click(object sender, EventArgs e)
        {

            //comboBoxTicketTypeID,comboBoxShiftID,ApplyDayBoxSubMenu,textBoxPrice,textBoxPriceTimeSlot,
            //    textBoxMinPrice,textBoxPriceOverTimeSlot

            ticketRepository.sp_insertTicketPricing((int)comboBoxTicketTypeID.SelectedValue, (int)comboBoxShiftID.SelectedValue,float.Parse(textBoxPrice.Text),float.Parse(textBoxMinPrice.Text),
                float.Parse(textBoxPriceTimeSlot.Text),float.Parse(textBoxPriceOverTimeSlot.Text),ConvertDateFormat(ApplyDayBoxSubMenu.Text));
            LoadDataToTicketDataGridView();
            ButtonUpdateSm.Enabled = false;
            ButtonDeleteSm.Enabled = false;
            ButtonSaveTicketTypeSm.Enabled = false;
        }

        private void ButtonUpdateSm_Click(object sender, EventArgs e)
        {
     
            ticketRepository.sp_updateTicketPricing((int)comboBoxTicketTypeID.SelectedValue, (int)comboBoxShiftID.SelectedValue, float.Parse(textBoxPrice.Text), float.Parse(textBoxMinPrice.Text),
                float.Parse(textBoxPriceTimeSlot.Text), float.Parse(textBoxPriceOverTimeSlot.Text), ConvertDateFormat(ApplyDayBoxSubMenu.Text));
            LoadDataToTicketDataGridView();
            ButtonUpdateSm.Enabled = false;
            ButtonDeleteSm.Enabled = false;
            ButtonSaveTicketTypeSm.Enabled = false;
        }

        private void ButtonDeleteSm_Click(object sender, EventArgs e)
        {
            ticketRepository.sp_deleteTicketPricing( (int)comboBoxShiftID.SelectedValue,ConvertDateFormat(ApplyDayBoxSubMenu.Text), (int)comboBoxTicketTypeID.SelectedValue);
            LoadDataToTicketDataGridView();
            ButtonUpdateSm.Enabled = false;
            ButtonDeleteSm.Enabled = false;
            ButtonSaveTicketTypeSm.Enabled = false;
        }

        private void timeSlotDataGridView_SelectionChanged(object sender, EventArgs e)
        {
           
            // Assuming columnIndex is the index of the column you want to display
            int columnIndexShiftID = GetColumnIndexByName(timeSlotDataGridView, "Mã khung giờ");
            int columnIndexShiftName = GetColumnIndexByName(timeSlotDataGridView, "Tên khung giờ");
            int columnIndexstartPackingShifts = GetColumnIndexByName(timeSlotDataGridView, "Giờ bắt đầu");
            int columnIndexendPackingShifts = GetColumnIndexByName(timeSlotDataGridView, "Giờ kết thúc");
           

            // Make sure at least one row is selected
            if (timeSlotDataGridView.SelectedRows.Count > 0 &&
                timeSlotDataGridView.SelectedRows[0].Cells[columnIndexShiftID] != null &&
                timeSlotDataGridView.SelectedRows[0].Cells[columnIndexShiftName] != null &&
                timeSlotDataGridView.SelectedRows[0].Cells[columnIndexShiftID].Value != null &&
                timeSlotDataGridView.SelectedRows[0].Cells[columnIndexShiftName].Value != null)
            {


                // Get the value from the selected row and column
                string selectedValueShiftID = timeSlotDataGridView.SelectedRows[0].Cells[columnIndexShiftID].Value.ToString();
                string selectedValueShiftName = timeSlotDataGridView.SelectedRows[0].Cells[columnIndexShiftName].Value.ToString();
                string selectedValuestartPackingShifts = ConvertDateFormat(timeSlotDataGridView.SelectedRows[0].Cells[columnIndexstartPackingShifts].Value.ToString());
                string selectedValueendPackingShifts = ConvertDateFormat(timeSlotDataGridView.SelectedRows[0].Cells[columnIndexendPackingShifts].Value.ToString());

                idTimeSlot.Text = selectedValueShiftID;
                nameTimeSlot.Text = selectedValueShiftName;
                startTimeSlot.Text = selectedValuestartPackingShifts;
                endTimeSlot.Text = selectedValueendPackingShifts;

              



                updateTimeSlot.Enabled = true;
                deleteTimeSlot.Enabled = true;
                addTimeSlot.Enabled = false;



            }
            else
            {

                idTimeSlot.Text = "";
                nameTimeSlot.Text = "";
                startTimeSlot.Text = "";
                endTimeSlot.Text = "";


                updateTimeSlot.Enabled = false;
                deleteTimeSlot.Enabled = false;
                addTimeSlot.Enabled = true;


             
              

            }
        }

        private void addTimeSlot_Click(object sender, EventArgs e)
        {
            ticketRepository.sp_ShiftManagementSubmenuInsert(int.Parse(idTimeSlot.Text), nameTimeSlot.Text, ConvertDateFormat(startTimeSlot.Text), ConvertDateFormat(endTimeSlot.Text));

            LoadDataToTimeSlotDataGridView();
            updateTimeSlot.Enabled = false;
            deleteTimeSlot.Enabled = false;
            addTimeSlot.Enabled = false;
        }

        private void updateTimeSlot_Click(object sender, EventArgs e)
        {
            ticketRepository.sp_ShiftManagementSubmenuUpdate(int.Parse(idTimeSlot.Text), nameTimeSlot.Text, ConvertDateFormat(startTimeSlot.Text), ConvertDateFormat(endTimeSlot.Text));

            LoadDataToTimeSlotDataGridView();

            updateTimeSlot.Enabled = false;
            deleteTimeSlot.Enabled = false;
            addTimeSlot.Enabled = false;
        }

        private void deleteTimeSlot_Click(object sender, EventArgs e)
        {
            ticketRepository.sp_ShiftManagementSubmenuDelete(int.Parse(idTimeSlot.Text));

            LoadDataToTimeSlotDataGridView();

            updateTimeSlot.Enabled = false;
            deleteTimeSlot.Enabled = false;
            addTimeSlot.Enabled = false;
        }
    }
}
