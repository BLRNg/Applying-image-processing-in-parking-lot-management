using CefSharp;
using CefSharp.WinForms;
using MetroFramework.Controls;
using MeTroUIDemo.Helper;
using MeTroUIDemo.Repo;
using OfficeOpenXml;
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
        private CustomerRepository customerRepository;
        private UserRepository userRepository;
        private CardRepository cardRepository;
        private HandlingLostVehicleRepository handlingLostVehicleRepository;
        private HandlingLostCardRepository handlingLostCardRepository;
        private SettingParamRepo settingParamRepo;

        private string dayApplySelected;
        private string typeTicketSelected;
        private int idCustomerSelected;
        private int registerInforIdSelected;
        private string timeVehicleIn;
        private string numberPlateVehicleIn;
        public MainForm()
        {
            InitializeComponent();
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            tabStack = new Stack<MetroTabPage>();
            generalRepository = new GeneralRepository();
            statictisSingleTicketRepository = new StatictisSingleTicketRepository();
            ticketRepository = new TicketRepository();
            customerRepository = new CustomerRepository();
            userRepository = new UserRepository();
            cardRepository = new CardRepository();
            handlingLostVehicleRepository = new HandlingLostVehicleRepository();
            handlingLostCardRepository = new HandlingLostCardRepository();
            settingParamRepo = new SettingParamRepo();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            logOutBttn.Visible = false;
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

            // Subscribe to the ValueChanged event
            dateRegistered.ValueChanged += DateTimePicker_ValueChanged;

            initHistoryTab();
            initChartTab();
            initTicketTab();
            initCustomerTab();
            initStatusTab();
            initHistoryTabLongDay();
            initLostVehicleTab();
            initLostCard();
            List<string> controlNames = new List<string>() { "inOutTile", "statusTile", "caTile", "ticketPriceTimeSlotTile",
            "historyTile","generalStatisticTile","diaryLostVehicleTile","longTimeTile","diaryLostTicketTile","customerTile","userManagementTile"};
           
            //DisableControls(controlNames);

            //this.Shown += LoginForm_Shown;

        }
       
         private void initLostCard()
        {
            Vehicle_Registration_Date.Format = DateTimePickerFormat.Custom;
            Vehicle_Registration_Date.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Adjust format as needed
            BirthdayC.Format = DateTimePickerFormat.Custom;
            BirthdayC.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Adjust format as needed
            Identity_date_issueC.Format = DateTimePickerFormat.Custom;
            Identity_date_issueC.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Adjust format as needed
            LostDateTime.Format = DateTimePickerFormat.Custom;
            LostDateTime.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Adjust format as needed
            HandlingDateTime.Format = DateTimePickerFormat.Custom;
            HandlingDateTime.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Adjust format as needed

        }
        private void initLostVehicleTab()
        {
            IncidentTime.Format = DateTimePickerFormat.Custom;
            IncidentTime.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Adjust format as needed
            ParkingTime.Format = DateTimePickerFormat.Custom;
            ParkingTime.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Adjust format as needed
            Identity_date_issueV.Format = DateTimePickerFormat.Custom;
            Identity_date_issueV.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Adjust format as needed
            birthDay.Format = DateTimePickerFormat.Custom;
            birthDay.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Adjust format as needed

        }
        // Function to enable controls based on their names
        private void EnableControls(List<string> controlNames)
        {
            foreach (string controlName in controlNames)
            {
                // Find the control by its name
                Control control = Controls.Find(controlName, true).FirstOrDefault() as Control;

                // Check if the control is found
                if (control != null)
                {
                    // Enable the control
                    control.Visible = true;
                }
            }
        }
        private void DisableControls(List<string> controlNames)
        {
            foreach (string controlName in controlNames)
            {
                // Find the control by its name
                Control control = Controls.Find(controlName, true).FirstOrDefault() as Control;

                // Check if the control is found
                if (control != null)
                {
                    // Enable the control
                    control.Visible = false;
                }
            }
        }
        // Function to access an item in a CheckedListBox control by its displayed text
        private object GetItemByText(CheckedListBox checkedListBox, string searchText)
        {
            // Find the index of the item by its displayed text
            int index = checkedListBox.FindString(searchText);

            // Check if the item is found
            if (index != -1)
            {
                // Return the item
                return checkedListBox.Items[index];
            }
            else
            {
                // Item not found, return null or handle the case accordingly
                return null;
            }
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
        private void initCustomerTab()
        {
            dateRegistered.Format = DateTimePickerFormat.Custom;
            dateRegistered.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Adjust format as needed
            dateExpire.Format = DateTimePickerFormat.Custom;
            dateExpire.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Adjust format as needed

            dateExpire.Enabled = false;
            totalAmount.Enabled = false;

        }
        private void initStatusTab()
        {
            dateTimePickerStatusFrom.Format = DateTimePickerFormat.Custom;
            dateTimePickerStatusFrom.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Adjust format as needed
            dateTimePickerStatusTo.Format = DateTimePickerFormat.Custom;
            dateTimePickerStatusTo.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Adjust format as needed

        }
        private void initHistoryTabLongDay()
        {
            pictureBoxImagePlateInLongDay.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxImagePlateOutLongDay.SizeMode = PictureBoxSizeMode.Zoom;
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
                string listRole = userRepository.sp_UserByUsername(username);
                if (listRole.Length > 0)
                {
                    List<string> stringListRole = ConvertToStringList(listRole);
                    EnableControls(stringListRole);
                }
                logOutBttn.Visible = true;

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
            buttonDeletehistory.Enabled = false;
        }



        private void statusTile_Click(object sender, EventArgs e)
        {
            navigateToTab(homePage, currentStatusTabPage);

        }
        private bool ValidateStatisticMenu()
        {
            if (
                dateTimePickerFromStatictis.Value > dateTimePickerToStatictis.Value)
            {
                MessageBox.Show("Invalid time range! End time must be after start time.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
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

            if (!ValidateAllFieldsTicketMenu())
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
            if (!ValidateAllFieldsTicketMenu())
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
        private bool ValidateAllFieldsTicketSubMenu()
        {
            bool isValid = true;

            // Validate comboBoxShiftID
            if (comboBoxShiftID.SelectedIndex == -1)
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate textBoxPrice
            if (!decimal.TryParse(textBoxPrice.Text, out decimal price) || price <= 0)
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate textBoxPriceTimeSlot
            if (!decimal.TryParse(textBoxPriceTimeSlot.Text, out decimal priceTimeSlot) || priceTimeSlot <= 0)
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate textBoxMinPrice
            if (!decimal.TryParse(textBoxMinPrice.Text, out decimal minPrice) || minPrice <= 0)
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate textBoxPriceOverTimeSlot
            if (!decimal.TryParse(textBoxPriceOverTimeSlot.Text, out decimal priceOverTimeSlot) || priceOverTimeSlot <= 0)
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            return isValid;
        }


        private bool ValidateAllFieldsTicketMenu()
        {
            bool isValid = true;

            // Validate textBoxTypeId
            if (string.IsNullOrWhiteSpace(textBoxTypeId.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate textBoxTicketName
            if (string.IsNullOrWhiteSpace(textBoxTicketName.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate textBoxMinTime
            if (!int.TryParse(textBoxMinTime.Text, out int minTime) || minTime <= 0)
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate textBoxDayAlert
            if (!int.TryParse(textBoxDayAlert.Text, out int dayAlert) || dayAlert <= 0)
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate ApplyDayBox
            if (ApplyDayBox.Value < DateTime.Today)
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate comboBoxCarType
            if (comboBoxCarType.SelectedIndex == -1)
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate comboBoxMethodCalPrice
            if (comboBoxMethodCalPrice.SelectedIndex == -1)
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            return isValid;
        }
        private void customerTile_Click(object sender, EventArgs e)
        {
            navigateToTab(homePage, customerListTabPage);
            LoadDataTodataGridViewCustomer();

        }
        private void dataGridViewCustomer_SelectionChanged(object sender, EventArgs e)
        {

            // Assuming columnIndex is the index of the column you want to display
            int columnIndexcustomerID = GetColumnIndexByName(dataGridViewCustomer,"Mã khách hàng");
            int columnIndexidentitycard = GetColumnIndexByName(dataGridViewCustomer, "Số CMND/CCCD");
            int columnIndexcustomerName = GetColumnIndexByName(dataGridViewCustomer, "Tên khách hàng");
         

            // Make sure at least one row is selected
            if (dataGridViewCustomer.SelectedRows.Count > 0 &&
                dataGridViewCustomer.SelectedRows[0].Cells[columnIndexcustomerID] != null &&
                dataGridViewCustomer.SelectedRows[0].Cells[columnIndexidentitycard] != null &&
                dataGridViewCustomer.SelectedRows[0].Cells[columnIndexcustomerID].Value != null &&
                dataGridViewCustomer.SelectedRows[0].Cells[columnIndexidentitycard].Value != null)
            {


                // Get the value from the selected row and column
                string selectedValuecustomerID = dataGridViewCustomer.SelectedRows[0].Cells[columnIndexcustomerID].Value.ToString();
                string selectedValueidentitycard = dataGridViewCustomer.SelectedRows[0].Cells[columnIndexidentitycard].Value.ToString();
                string selectedValuecustomerName = dataGridViewCustomer.SelectedRows[0].Cells[columnIndexcustomerName].Value.ToString();


                textBoxIdCustomer.Text = selectedValuecustomerID;
                textBoxNameCustomer.Text = selectedValueidentitycard;
                textBoxIdentityCustomer.Text = selectedValuecustomerName;


                idCustomerSelected = int.Parse(selectedValuecustomerID);
                LoadDataTodataGridViewCustomerSubmenu(int.Parse(selectedValuecustomerID));

                updateCustomer.Enabled = true;
                deleteCustomer.Enabled = true;
                addCustomer.Enabled = false;



            }
            else
            {

                textBoxIdCustomer.Text = "";
                textBoxNameCustomer.Text = "";
                textBoxIdentityCustomer.Text = "";


                addCustomer.Enabled = true;
                updateCustomer.Enabled = false;
                deleteCustomer.Enabled = false;

                // Clear existing columns and data in DataGridView
                dataGridViewInforRegister.Columns.Clear();
                dataGridViewInforRegister.Rows.Clear();

            }
        }
        private bool ValidateRequiredFieldsCustomerId()
        {
            bool isValid = true;

            // Validate textBoxIdCustomer
            if (string.IsNullOrWhiteSpace(textBoxIdCustomer.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }
            else
            {
                // Additional validation if needed
                if (!int.TryParse(textBoxIdCustomer.Text, out _))
                {
                    isValid = false;
                    // You can show an error message or change the appearance of the control to indicate an error.
                }
            }

            // Validate textBoxIdentityCustomer
            if (string.IsNullOrWhiteSpace(textBoxIdentityCustomer.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }
            else
            {
                // Additional validation if needed
                // Example: Check if the identity format is correct
            }

            // Validate textBoxNameCustomer
            if (string.IsNullOrWhiteSpace(textBoxNameCustomer.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }
            else
            {
                // Additional validation if needed
            }

            return isValid;
        }

        private void addCustomer_Click(object sender, EventArgs e)
        {
            if (!ValidateRequiredFieldsCustomerId())
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            customerRepository.sp_insertcustomer(int.Parse(textBoxIdCustomer.Text), textBoxIdentityCustomer.Text, textBoxNameCustomer.Text);

            LoadDataTodataGridViewCustomer();
            updateCustomer.Enabled = false;
            deleteCustomer.Enabled = false;
            addCustomer.Enabled = false;
        }

        private void updateCustomer_Click(object sender, EventArgs e)
        {
            if (!ValidateRequiredFieldsCustomerId())
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            customerRepository.sp_updatecustomer(int.Parse(textBoxIdCustomer.Text), textBoxIdentityCustomer.Text, textBoxNameCustomer.Text);

            LoadDataTodataGridViewCustomer();
            updateCustomer.Enabled = false;
            deleteCustomer.Enabled = false;
            addCustomer.Enabled = false;
        }

        private void deleteCustomer_Click(object sender, EventArgs e)
        {
            customerRepository.sp_deletecustomer(int.Parse(textBoxIdCustomer.Text));

            LoadDataTodataGridViewCustomer();
            updateCustomer.Enabled = false;
            deleteCustomer.Enabled = false;
            addCustomer.Enabled = false;
        }
        private void dataGridViewInforRegister_SelectionChanged(object sender, EventArgs e)
        {
            


            // Assuming columnIndex is the index of the column you want to display
            int columnIndexregisterInforId = GetColumnIndexByName(dataGridViewInforRegister, "Id đăng ký");
            int columnIndexdrivingLicenseNumer = GetColumnIndexByName(dataGridViewInforRegister, "Số giấy phép lái xe");
            int columnIndextimeRegister = GetColumnIndexByName(dataGridViewInforRegister, "Ngày đăng ký"); 
            int columnIndexcustomerID = GetColumnIndexByName(dataGridViewInforRegister, "Id khách hàng");
            int columnIndexlicensePlateNumber = GetColumnIndexByName(dataGridViewInforRegister, "Biển số xe");
            int columnIndexexpirationDate = GetColumnIndexByName(dataGridViewInforRegister, "Ngày hết hạn"); 
            int columnIndextotalAmount = GetColumnIndexByName(dataGridViewInforRegister, "Tổng tiền");
            int columnIndexnumberRegisterMonth = GetColumnIndexByName(dataGridViewInforRegister, "Số tháng đăng ký");
            int columnIndexcardInfroID = GetColumnIndexByName(dataGridViewInforRegister, "Mã thẻ");


            // Make sure at least one row is selected
            if (dataGridViewInforRegister.SelectedRows.Count > 0 &&
                dataGridViewInforRegister.SelectedRows[0].Cells[columnIndexcustomerID] != null &&
                dataGridViewInforRegister.SelectedRows[0].Cells[columnIndexregisterInforId] != null &&
                dataGridViewInforRegister.SelectedRows[0].Cells[columnIndexcustomerID].Value != null &&
                dataGridViewInforRegister.SelectedRows[0].Cells[columnIndexregisterInforId].Value != null)
            {


                // Get the value from the selected row and column
                string selectedValuecustomerID = dataGridViewInforRegister.SelectedRows[0].Cells[columnIndexcustomerID].Value.ToString();
                string selectedValueregisterInforId = dataGridViewInforRegister.SelectedRows[0].Cells[columnIndexregisterInforId].Value.ToString();
                string selectedValuedrivingLicenseNumer = dataGridViewInforRegister.SelectedRows[0].Cells[columnIndexdrivingLicenseNumer].Value.ToString();  
                string selectedValuetimeRegister = ConvertDateFormat(dataGridViewInforRegister.SelectedRows[0].Cells[columnIndextimeRegister].Value.ToString());
                string selectedValuelicensePlateNumber = dataGridViewInforRegister.SelectedRows[0].Cells[columnIndexlicensePlateNumber].Value.ToString();
                string selectedValueexpirationDate = ConvertDateFormat(dataGridViewInforRegister.SelectedRows[0].Cells[columnIndexexpirationDate].Value.ToString());  
                string selectedValuetotalAmount= dataGridViewInforRegister.SelectedRows[0].Cells[columnIndextotalAmount].Value.ToString();
                string selectedValuenumberRegisterMonth = dataGridViewInforRegister.SelectedRows[0].Cells[columnIndexnumberRegisterMonth].Value.ToString();
                string selectedValuecardInfroID = dataGridViewInforRegister.SelectedRows[0].Cells[columnIndexcardInfroID].Value.ToString();


                //textBoxIdCustomer.Text = selectedValuecustomerID;
                //textBoxNameCustomer.Text = selectedValueidentitycard;
                //textBoxIdentityCustomer.Text = selectedValuecustomerName;
                dateRegistered.Text = selectedValuetimeRegister;
                dateExpire.Text = selectedValueexpirationDate;
                cardNoBox.Text = selectedValuecardInfroID;
                licenseNumberBox.Text = selectedValuelicensePlateNumber;
                drivingLicenseBox.Text = selectedValuedrivingLicenseNumer;
                numberOfMonth.Text = selectedValuenumberRegisterMonth;
                totalAmount.Text = selectedValuetotalAmount;
                registerInforIdSelected = int.Parse(selectedValueregisterInforId);



                updateRegisterCar.Enabled = true;
                deleteRegisterCar.Enabled = true;
                addRegisterCar.Enabled = false;



            }
            else
            {

                dateRegistered.Text = "";
                dateExpire.Text = "";
                cardNoBox.Text = "";
                licenseNumberBox.Text = "";
                drivingLicenseBox.Text = "";
                numberOfMonth.Text = "";
                totalAmount.Text = "";


                addRegisterCar.Enabled = true;
                updateRegisterCar.Enabled = false;
                deleteRegisterCar.Enabled = false;



            }
        }
        public void LoadDataTodataGridViewCustomerSubmenu(int customerID)
        {
            // Retrieve data from the database
            DataTable dataTable = customerRepository.sp_ticketRegistInforManagement(customerID);

            // Clear existing columns and data in DataGridView
            dataGridViewInforRegister.Columns.Clear();
            dataGridViewInforRegister.Rows.Clear();
       
            // Define column names for DataGridView
            string[] columnNamesMap = { "registerInforId", "drivingLicenseNumer", "timeRegister", "customerID", "licensePlateNumber", "expirationDate", "totalAmount", "numberRegisterMonth", "cardInfroID", "TicketTypeName" };
            string[] columnNames = { "Id đăng ký", "Số giấy phép lái xe", "Ngày đăng ký", "Id khách hàng", "Biển số xe", "Ngày hết hạn", "Tổng tiền", "Số tháng đăng ký", "Mã thẻ","Loại vé" };


            // Add columns to the DataGridView
            foreach (string columnName in columnNames)
            {
                dataGridViewInforRegister.Columns.Add(columnName, columnName);
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
                dataGridViewInforRegister.Rows.Add(rowData);
            }
            //ticketdataGridView.Columns[7].Visible = false;
            //ticketdataGridView.Columns[8].Visible = false;

            // Auto resize column width to fit text
            foreach (DataGridViewColumn column in dataGridViewInforRegister.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            //dataGridViewInforRegister.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        private bool ValidateRequiredFieldsRegisterCar()
        {
            bool isValid = true;

            // Validate drivingLicenseBox
            if (string.IsNullOrWhiteSpace(drivingLicenseBox.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate dateRegistered
            if (!DateTime.TryParse(dateRegistered.Text, out _))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate idCustomerSelected
            if (idCustomerSelected <= 0)
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate licenseNumberBox
            if (string.IsNullOrWhiteSpace(licenseNumberBox.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate dateExpire
            if (!DateTime.TryParse(dateExpire.Text, out _))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate totalAmount
            if (!float.TryParse(totalAmount.Text, out _) || float.Parse(totalAmount.Text) <= 0)
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate numberOfMonth
            if (!int.TryParse(numberOfMonth.Text, out _) || int.Parse(numberOfMonth.Text) <= 0)
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate cardNoBox
            if (string.IsNullOrWhiteSpace(cardNoBox.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            return isValid;
        }

        private void addRegisterCar_Click(object sender, EventArgs e)
        {
            if (!ValidateRequiredFieldsRegisterCar())
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //   idCustomerSelected;
            //registerInforIdSelected;
            customerRepository.sp_insertticketRegistInfor(drivingLicenseBox.Text, ConvertDateFormat(dateRegistered.Text), idCustomerSelected,
                 licenseNumberBox.Text, ConvertDateFormat(dateExpire.Text),float.Parse(totalAmount.Text),int.Parse(numberOfMonth.Text), cardNoBox.Text);

            LoadDataTodataGridViewCustomerSubmenu(idCustomerSelected);

            addRegisterCar.Enabled = false;
            updateRegisterCar.Enabled = false;
            deleteRegisterCar.Enabled = false;
        }

        private void updateRegisterCar_Click(object sender, EventArgs e)
        {
            if (!ValidateRequiredFieldsRegisterCar())
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            customerRepository.sp_updateticketRegistInfor(registerInforIdSelected,drivingLicenseBox.Text, ConvertDateFormat(dateRegistered.Text), idCustomerSelected,
                licenseNumberBox.Text, ConvertDateFormat(dateExpire.Text), float.Parse(totalAmount.Text), int.Parse(numberOfMonth.Text), cardNoBox.Text);

            LoadDataTodataGridViewCustomerSubmenu(idCustomerSelected);

            addRegisterCar.Enabled = false;
            updateRegisterCar.Enabled = false;
            deleteRegisterCar.Enabled = false;
        }

        private void deleteRegisterCar_Click(object sender, EventArgs e)
        {
            customerRepository.sp_deleteticketRegistInfor(registerInforIdSelected);

            LoadDataTodataGridViewCustomerSubmenu(idCustomerSelected);

            addRegisterCar.Enabled = false;
            updateRegisterCar.Enabled = false;
            deleteRegisterCar.Enabled = false;
        }

        private void cardNoBox_TextChanged(object sender, EventArgs e)
        {
            if (numberOfMonth.Text != null && cardNoBox.Text != null && numberOfMonth.Text.Length > 0)
                totalAmount.Text = customerRepository.sp_totalAmountBycardID(int.Parse(numberOfMonth.Text), cardNoBox.Text);
        }

        private void numberOfMonth_TextChanged(object sender, EventArgs e)
        {
            if (numberOfMonth.Text != null && cardNoBox.Text != null && numberOfMonth.Text.Length > 0)
            totalAmount.Text = customerRepository.sp_totalAmountBycardID(int.Parse(numberOfMonth.Text), cardNoBox.Text);

            if (numberOfMonth.Text.Length > 0)
            {
                dateExpire.Value = dateRegistered.Value.AddMonths(int.Parse(numberOfMonth.Text));
            }
        }
        // Event handler for the ValueChanged event
        private void DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (numberOfMonth.Text.Length > 0)
            {
                dateExpire.Value = dateRegistered.Value.AddMonths(int.Parse(numberOfMonth.Text));
            }
          
        }
        public void LoadDataTodataGridViewCustomer()
        {
            // Retrieve data from the database
            DataTable dataTable = customerRepository.sp_CustomerRegister();

            // Clear existing columns and data in DataGridView
            dataGridViewCustomer.Columns.Clear();
            dataGridViewCustomer.Rows.Clear();
           
            // Define column names for DataGridView
            string[] columnNamesMap = { "customerID", "identitycard", "customerName"};
            string[] columnNames = { "Mã khách hàng","Số CMND/CCCD","Tên khách hàng"};


            // Add columns to the DataGridView
            foreach (string columnName in columnNames)
            {
                dataGridViewCustomer.Columns.Add(columnName, columnName);
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
                dataGridViewCustomer.Rows.Add(rowData);
            }
            //ticketdataGridView.Columns[7].Visible = false;
            //ticketdataGridView.Columns[8].Visible = false;

            // Auto resize column width to fit text
            foreach (DataGridViewColumn column in dataGridViewCustomer.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            dataGridViewCustomer.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void generalStatisticTile_Click(object sender, EventArgs e)
        {
            navigateToTab(homePage, statictisTabPage);
            LoadDataTodataGridViewCardLost();

        }

       

        private void diaryLostVehicleTile_Click(object sender, EventArgs e)
        {

            navigateToTab(homePage, diaryLostVehicleTabPage);
            LoadDataTodataGridViewLostVehicle();

        }

        private void longTimeTile_Click(object sender, EventArgs e)
        {
            navigateToTab(homePage, overdueTabPage);


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
        private bool ValidateTimeRangeHistoryMenu()
        {
            if (dateTimePickerFrom.Value > dateTimePickerTo.Value)
            {
                MessageBox.Show("Invalid time range! End time must be after start time.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
        private void buttonReload_Click(object sender, EventArgs e)
        {
            if (!ValidateTimeRangeHistoryMenu()) { return; }


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
            DataTable dataTable = generalRepository.sp_exportMonthlyTicketRevenueStatistics(formattedDateTimeFrom, formattedDateTimeTo);

            string filePath = @"ResultExport\" + $"BaoCaoChiTietLuotRaVao_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            
           
            ExcelHelper.ExportToExcel(@"TemplateExport\BaoCaoChiTietLuotRaVao.xlsx", filePath, dataTable,
                ExcelHelper.GetDataTimeReport(formattedDateTimeFrom, formattedDateTimeTo),5,1);

        }

        private void excelHistorySinle_Click(object sender, EventArgs e)
        {
            DateTime selectedDateTimeFrom = dateTimePickerFrom.Value;
            string formattedDateTimeFrom = selectedDateTimeFrom.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime selectedDateTimeTo = dateTimePickerTo.Value;
            string formattedDateTimeTo = selectedDateTimeTo.ToString("yyyy-MM-dd HH:mm:ss");

            DataTable dataTable = generalRepository.sp_exportSingleUseTicketRevenueStatistics(formattedDateTimeFrom, formattedDateTimeTo);
            string filePath = @"ResultExport\" + $"BaoCaoChiTietVeLuot_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";


            ExcelHelper.ExportToExcel(@"TemplateExport\BaoCaoChiTietVeLuot.xlsx", filePath, dataTable,
                ExcelHelper.GetDataTimeReport(formattedDateTimeFrom, formattedDateTimeTo), 5, 1);
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
            int columntimeIn = GetColumnIndexByName(dataGridViewHistory, "Ngày vào");
            int columnnumberPlate = GetColumnIndexByName(dataGridViewHistory, "Biển số xe vào");
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
                timeVehicleIn = ConvertDateFormat(dataGridViewHistory.SelectedRows[0].Cells[columntimeIn].Value.ToString());
                numberPlateVehicleIn = dataGridViewHistory.SelectedRows[0].Cells[columnnumberPlate].Value.ToString();

                buttonDeletehistory.Enabled = true;

                if (selectedValueurlIn.Length >0)
                {
                    pictureBoxImagePlateIn.Image = Image.FromFile(Path.GetFullPath(selectedValueurlIn));
                    if(selectedValueurlOut.Length < 1)
                    {
                        pictureBoxImagePlateOut.Image = null;
                    }
                }
                if (selectedValueurlOut.Length > 0)
                {
                    pictureBoxImagePlateOut.Image = Image.FromFile(Path.GetFullPath(selectedValueurlOut));

                    if (selectedValueurlIn.Length < 1)
                    {
                        pictureBoxImagePlateIn.Image = null;
                    }
                }
                if (selectedValueurlOut.Length < 1 && selectedValueurlIn.Length <1)
                {
                    pictureBoxImagePlateOut.Image = null;
                    pictureBoxImagePlateIn.Image = null;
                }
            }
            else
            {
                buttonDeletehistory.Enabled = false;
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
            if (!ValidateStatisticMenu()) { return; }
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
            if (!ValidateAllFieldsTicketSubMenu())
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                // You can choose to focus on the first control that requires attention or handle the error as you see fit.
            }
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
            if (!ValidateAllFieldsTicketSubMenu())
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                // You can choose to focus on the first control that requires attention or handle the error as you see fit.
            }
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
            if (idTimeSlot.Text.Length < 1 || nameTimeSlot.Text.Length < 1)
            {
                MessageBox.Show("Mã khung giờ và tên khung giờ không được bỏ trống", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string message = ticketRepository.sp_validationTimeSlot(int.Parse(idTimeSlot.Text), ConvertDateFormat(startTimeSlot.Text), ConvertDateFormat(endTimeSlot.Text));
            if(message.Length > 0)
            {
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ticketRepository.sp_ShiftManagementSubmenuInsert(int.Parse(idTimeSlot.Text), nameTimeSlot.Text, ConvertDateFormat(startTimeSlot.Text), ConvertDateFormat(endTimeSlot.Text));

            LoadDataToTimeSlotDataGridView();
            updateTimeSlot.Enabled = false;
            deleteTimeSlot.Enabled = false;
            addTimeSlot.Enabled = false;
        }

        private void updateTimeSlot_Click(object sender, EventArgs e)
        {
            if(idTimeSlot.Text.Length < 1 || nameTimeSlot.Text.Length < 1)
            {
                MessageBox.Show("Mã khung giờ và tên khung giờ không được bỏ trống", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
          

            string message = ticketRepository.sp_validationTimeSlot(int.Parse(idTimeSlot.Text), ConvertDateFormat(startTimeSlot.Text), ConvertDateFormat(endTimeSlot.Text));
            if (message.Length > 0)
            {
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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
        // Method to get the display text of all checked items in the CheckedListBox
        private List<string> GetCheckedItemTexts(CheckedListBox checkedListBox)
        {
            List<string> checkedItemTexts = new List<string>();

            // Iterate over each checked item in the CheckedListBox
            foreach (object itemChecked in checkedListBox.CheckedItems)
            {
                // Get the display text of the checked item and add it to the list
                checkedItemTexts.Add(checkedListBox.GetItemText(itemChecked));
            }

            return checkedItemTexts;
        }
        // Function to add a tag for each item in a CheckedListBox

        // Function to create a mapping from List 2 to List 1
        static Dictionary<string, string> CreateMap(List<string> list1, List<string> list2)
        {
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            for (int i = 0; i < list2.Count; i++)
            {
                mapping[list2[i]] = list1[i];
            }
            return mapping;
        }

        // Function to get the corresponding value from List 1 for a value from List 2
        static string GetCorrespondingValue(Dictionary<string, string> mapping, string valueFromList2)
        {
            string correspondingValue;
            if (mapping.TryGetValue(valueFromList2, out correspondingValue))
            {
                return correspondingValue;
            }
            else
            {
                return "Value not found in mapping.";
            }
        }
        private bool ValidateRequiredFieldsLogin()
        {
            bool isValid = true;

            // Validate textBoxUsername
            if (string.IsNullOrWhiteSpace(textBoxUsername.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate textBoxPassWord
            if (string.IsNullOrWhiteSpace(textBoxPassWord.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate valueRoleSelected
            if (valueRoleSelected().Length < 1)
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            return isValid;
        }

        private string valueRoleSelected()
        {
            List<string> checkedItemTexts = GetCheckedItemTexts(checkedListRole);
            string value = "";


            List<string> list1 = new List<string>
        {
            "inOutTile",
            "statusTile",
            "caTile",
            "ticketPriceTimeSlotTile",
            "historyTile",
            "generalStatisticTile",
            "diaryLostVehicleTile",
            "longTimeTile",
            "diaryLostTicketTile",
            "customerTile",
            "userManagementTile"
        };

            List<string> list2 = new List<string>
        {
            "Làn ra vào",
            "Trạng thái bãi xe",
            "Biểu đồ thống kê",
            "Thiết lập giá vé và khung giờ",
            "Lịch sử ra vào",
            "Nhật ký xử lý mất thẻ",
            "Nhật ký xử lý mất xe",
            "Xe gửi nhiều ngày",
            "Quản lý thẻ xe",
            "Quản lý khách hàng",
            "Quản lý đăng nhập"
        };

            Dictionary<string, string> mapping = CreateMap(list1, list2);

            foreach (string item in checkedItemTexts)
            {
                value += GetCorrespondingValue(mapping, item) + ";";
            }
            return value;
        }
        private void addNewUser_Click(object sender, EventArgs e)
        {
            if (!ValidateRequiredFieldsLogin())
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            userRepository.CreateUserWithRoles(textBoxUsername.Text, textBoxPassWord.Text, valueRoleSelected());

            loadDataToUserGridView();
            addNewUser.Enabled = true;
            updateUser.Enabled = false;
            deleteUser.Enabled = false;
        }
        private void updateUser_Click(object sender, EventArgs e)
        {
            if (!ValidateRequiredFieldsLogin())
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            userRepository.UpdateUserPassword(textBoxUsername.Text, textBoxPassWord.Text, valueRoleSelected());

            loadDataToUserGridView();
            addNewUser.Enabled = true;
            updateUser.Enabled = false;
            deleteUser.Enabled = false;
        }

        private void deleteUser_Click(object sender, EventArgs e)
        {
            userRepository.DeleteUser(textBoxUsername.Text);

            loadDataToUserGridView();
            addNewUser.Enabled = true;
            updateUser.Enabled = false;
            deleteUser.Enabled = false;
        }

        private void userManagementTile_Click(object sender, EventArgs e)
        {
            navigateToTab(homePage, userManagementTabPage);
            loadDataToUserGridView();
        }

        private void loadDataToUserGridView()
        {
            
            // Retrieve data from the database
            DataTable dataTable = userRepository.sp_User();

            // Clear existing columns and data in DataGridView
            dataGridViewUser.Columns.Clear();
            dataGridViewUser.Rows.Clear();

            // Define column names for DataGridView
            string[] columnNamesMap = { "user_name", "description"};
            string[] columnNames = { "Tên đăng nhập", "Quyền"};


            // Add columns to the DataGridView
            foreach (string columnName in columnNames)
            {
                dataGridViewUser.Columns.Add(columnName, columnName);
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
                dataGridViewUser.Rows.Add(rowData);
            }
            //ticketdataGridView.Columns[7].Visible = false;
            //ticketdataGridView.Columns[8].Visible = false;

            // Auto resize column width to fit text
            foreach (DataGridViewColumn column in dataGridViewUser.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            dataGridViewUser.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        // Function to convert a comma-separated string into a string list
        private List<string> ConvertToStringList(string inputString)
        {
            List<string> stringList = new List<string>();

            // Check if the input string contains a comma
            if (inputString.Contains(','))
            {
                // Split the input string based on commas and convert it into a string array
                string[] stringArray = inputString.Split(',');

                // Convert the string array into a list
                stringList.AddRange(stringArray);
            }
            else
            {
                // If the input string does not contain a comma, add the whole string as a single item to the list
                stringList.Add(inputString);
            }

            return stringList;
        }
        private void SetItemCheckedByText(CheckedListBox checkedListBox, string searchText, bool isChecked)
        {
            // Find the index of the item by its displayed text
            int index = checkedListBox.FindString(searchText);

            // Check if the item is found
            if (index != -1)
            {
                // Set the checked state of the item
                checkedListBox.SetItemChecked(index, isChecked);
            }
        }
        private void UncheckAllItems(CheckedListBox checkedListBox)
        {
            // Iterate over each item in the CheckedListBox
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                // Uncheck the item
                checkedListBox.SetItemChecked(i, false);
            }
        }
        private void dataGridViewUser_SelectionChanged(object sender, EventArgs e)
        {
            //comboBoxTicketTypeID,comboBoxShiftID,ApplyDayBoxSubMenu,textBoxPrice,textBoxPriceTimeSlot,
            //    textBoxMinPrice,textBoxPriceOverTimeSlot

            // Assuming columnIndex is the index of the column you want to display
            int columnIndexuser_name = GetColumnIndexByName(dataGridViewUser, "Tên đăng nhập");
            int columnIndexdescription = GetColumnIndexByName(dataGridViewUser, "Quyền");
           

            // Make sure at least one row is selected
            if (dataGridViewUser.SelectedRows.Count > 0 &&
                dataGridViewUser.SelectedRows[0].Cells[columnIndexuser_name] != null &&
                dataGridViewUser.SelectedRows[0].Cells[columnIndexuser_name].Value != null 
               )
            {


                // Get the value from the selected row and column
                string selectedValueuser_name = dataGridViewUser.SelectedRows[0].Cells[columnIndexuser_name].Value.ToString();
                string selectedValuedescription = dataGridViewUser.SelectedRows[0].Cells[columnIndexdescription].Value.ToString();

                textBoxUsername.Text = selectedValueuser_name;
                textBoxUsername.Enabled = false;
                List<string> stringList = ConvertToStringList(selectedValuedescription);
                UncheckAllItems(checkedListRole);
                if (stringList.Count > 0 && stringList[0].Length >0)
                {

                    foreach (string menu in stringList)
                    {
                        SetItemCheckedByText(checkedListRole, menu, true);
                    }
                }
                

                updateUser.Enabled = true;
                deleteUser.Enabled = true;
                addNewUser.Enabled = false;



            }
            else
            {
                textBoxUsername.Enabled = true;
                UncheckAllItems(checkedListRole);
                textBoxUsername.Text = "";
                textBoxPassWord.Text = "";

                addNewUser.Enabled = true;
                updateUser.Enabled = false;
                deleteUser.Enabled = false;

             

            }
        }

        private void logOutBttn_Click(object sender, EventArgs e)
        {
            // Close the current form
            this.Close();

            // Restart the application
            Application.Restart();
            Environment.Exit(0); // Exit the current process to ensure a clean restart
        }
        private void reloadComboBoxCardMenu()
        {

            comboTicketTypeId.DataSource = ticketRepository.sp_ticketTypeList();
            comboTicketTypeId.DisplayMember = "Description";
            comboTicketTypeId.ValueMember = "TicketTypeID";


            comboBoxParkingLot.DataSource = cardRepository.sp_packingSlotList();
            comboBoxParkingLot.DisplayMember = "Description";
            comboBoxParkingLot.ValueMember = "packingSlotID";
        }
        private void loadDataTodataGridViewCard()
        {
            reloadComboBoxCardMenu();
            // Retrieve data from the database
            DataTable dataTable = cardRepository.sp_cardInfor();

            // Clear existing columns and data in DataGridView
            dataGridViewCard.Columns.Clear();
            dataGridViewCard.Rows.Clear();


            // Define column names for DataGridView
            string[] columnNamesMap = { "cardInforId", "ticketTypeId", "packingSlotID", "isUsing", "isMonthlyCard", "isLock", "isCardWasCancel" };
            string[] columnNames = { "Mã thẻ", "Loại vé","Khu đậu xe","Đang được dùng","Thẻ tháng","Khóa","Hủy" };


            // Add columns to the DataGridView
            foreach (string columnName in columnNames)
            {
                dataGridViewCard.Columns.Add(columnName, columnName);
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
                dataGridViewCard.Rows.Add(rowData);
            }
            //ticketdataGridView.Columns[7].Visible = false;
            //ticketdataGridView.Columns[8].Visible = false;

            // Auto resize column width to fit text
            foreach (DataGridViewColumn column in dataGridViewCard.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            dataGridViewCard.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void diaryLostTicketTile_Click(object sender, EventArgs e)
        {
            navigateToTab(homePage, diaryLostTicketTabPage);
            loadDataTodataGridViewCard();
        }

        private void dataGridViewCard_SelectionChanged(object sender, EventArgs e)
        {
           

            // Assuming columnIndex is the index of the column you want to display
            int columnIndexcardInforId = GetColumnIndexByName(dataGridViewCard, "Mã thẻ");
            int columnIndexticketTypeId = GetColumnIndexByName(dataGridViewCard, "Loại vé");
            int columnIndexpackingSlotID = GetColumnIndexByName(dataGridViewCard, "Khu đậu xe");
            int columnIndexisUsing = GetColumnIndexByName(dataGridViewCard, "Đang được dùng");
            int columnIndexisMonthlyCard = GetColumnIndexByName(dataGridViewCard, "Thẻ tháng");
            int columnIndexisLock = GetColumnIndexByName(dataGridViewCard, "Khóa");
            int columnIndexisCardWasCancel = GetColumnIndexByName(dataGridViewCard, "Hủy");
           
            // Make sure at least one row is selected
            if (dataGridViewCard.SelectedRows.Count > 0 &&
                dataGridViewCard.SelectedRows[0].Cells[columnIndexcardInforId] != null &&
                dataGridViewCard.SelectedRows[0].Cells[columnIndexticketTypeId] != null &&
                dataGridViewCard.SelectedRows[0].Cells[columnIndexcardInforId].Value != null &&
                dataGridViewCard.SelectedRows[0].Cells[columnIndexticketTypeId].Value != null)
            {


                // Get the value from the selected row and column
                string selectedValuecardInforId = dataGridViewCard.SelectedRows[0].Cells[columnIndexcardInforId].Value.ToString();
                string selectedValueticketTypeId = dataGridViewCard.SelectedRows[0].Cells[columnIndexticketTypeId].Value.ToString();
                string selectedValuepackingSlotID = dataGridViewCard.SelectedRows[0].Cells[columnIndexpackingSlotID].Value.ToString();
                string selectedValueisUsing = dataGridViewCard.SelectedRows[0].Cells[columnIndexisUsing].Value.ToString();
                string selectedValueisMonthlyCard = dataGridViewCard.SelectedRows[0].Cells[columnIndexisMonthlyCard].Value.ToString();
                string selectedValueisLock = dataGridViewCard.SelectedRows[0].Cells[columnIndexisLock].Value.ToString();
                string selectedValueisCardWasCancel = dataGridViewCard.SelectedRows[0].Cells[columnIndexisCardWasCancel].Value.ToString();

                textBoxCardId.Text = selectedValuecardInforId;
                comboTicketTypeId.SelectedValue = selectedValueticketTypeId;
                comboBoxParkingLot.SelectedValue = selectedValuepackingSlotID;

                checkBoxIsusing.Checked = bool.Parse(selectedValueisUsing);
                checkBoxIsMonthly.Checked = bool.Parse(selectedValueisMonthlyCard);
                checkBoxislock.Checked = bool.Parse(selectedValueisLock);
                checkBoxIsCancel.Checked = bool.Parse(selectedValueisCardWasCancel);



                buttonUpdateCard.Enabled = true;
                buttonDeleteCard.Enabled = true;
                buttonAddCard.Enabled = false;

                textBoxCardId.Enabled = false;

            }
            else
            {
                textBoxCardId.Enabled = true;
                textBoxCardId.Text = "";
                ResetCombobox(comboTicketTypeId);
                ResetCombobox(comboBoxParkingLot);
             
                checkBoxIsusing.Checked = false;
                checkBoxIsMonthly.Checked = false;
                checkBoxislock.Checked = false;
                checkBoxIsCancel.Checked = false;



                buttonAddCard.Enabled = true;
                buttonUpdateCard.Enabled = false;
                buttonDeleteCard.Enabled = false;
            }
        }
        private bool ValidateRequiredFieldsCardMenu()
        {
            bool isValid = true;

            // Validate textBoxCardId
            if (string.IsNullOrWhiteSpace(textBoxCardId.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }
            if(comboTicketTypeId.SelectedIndex == -1 || comboBoxParkingLot.SelectedIndex == -1)
            {
                return false;
            }
            // Validate comboTicketTypeId
            // Validate comboTicketTypeId
            if (comboTicketTypeId?.SelectedValue != null && (int)comboTicketTypeId.SelectedValue <= 0)
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate comboBoxParkingLot
            if (comboBoxParkingLot != null && comboBoxParkingLot.SelectedValue != null && (int)comboBoxParkingLot.SelectedValue <= 0)
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }


            // No specific validation required for checkBoxIsusing, checkBoxIsMonthly, checkBoxislock, checkBoxIsCancel

            return isValid;
        }

        private void buttonAddCard_Click(object sender, EventArgs e)
        {

            if (!ValidateRequiredFieldsCardMenu())
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            cardRepository.sp_insertCardInfor(textBoxCardId.Text, (int)comboTicketTypeId.SelectedValue, (int)comboBoxParkingLot.SelectedValue, checkBoxIsusing.Checked, checkBoxIsMonthly.Checked, checkBoxislock.Checked, checkBoxIsCancel.Checked);

            loadDataTodataGridViewCard();

            buttonAddCard.Enabled = false;
            buttonUpdateCard.Enabled = false;
            buttonDeleteCard.Enabled = false;

 
        }

        private void buttonUpdateCard_Click(object sender, EventArgs e)
        {
            if (!ValidateRequiredFieldsCardMenu())
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            cardRepository.sp_updateCardInfor(textBoxCardId.Text, (int)comboTicketTypeId.SelectedValue, (int)comboBoxParkingLot.SelectedValue, checkBoxIsusing.Checked, checkBoxIsMonthly.Checked, checkBoxislock.Checked, checkBoxIsCancel.Checked);

            loadDataTodataGridViewCard();

            buttonAddCard.Enabled = false;
            buttonUpdateCard.Enabled = false;
            buttonDeleteCard.Enabled = false;
        }

        private void buttonDeleteCard_Click(object sender, EventArgs e)
        {
            cardRepository.sp_deleteCardInfor(textBoxCardId.Text);

            loadDataTodataGridViewCard();

            buttonAddCard.Enabled = false;
            buttonUpdateCard.Enabled = false;
            buttonDeleteCard.Enabled = false;
        }

        private void sp_export_statictisGrossSale_Click(object sender, EventArgs e)
        {
            if (!ValidateStatusMenu()) { return; }
            DateTime selectedDateTimeFrom = dateTimePickerStatusFrom.Value;
            string formattedDateTimeFrom = selectedDateTimeFrom.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime selectedDateTimeTo = dateTimePickerStatusTo.Value;
            string formattedDateTimeTo = selectedDateTimeTo.ToString("yyyy-MM-dd HH:mm:ss");

            DataTable dataTable = generalRepository.sp_export_statictisGrossSale(formattedDateTimeFrom, formattedDateTimeTo);
            string filePath = @"ResultExport\" + $"ThongKeTongQuat_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";


            ExcelHelper.ExportToExcel(@"TemplateExport\ThongKeTongQuat.xlsx", filePath, dataTable,
                ExcelHelper.GetDataTimeReport(formattedDateTimeFrom, formattedDateTimeTo), 4, 1);
        }

        private void sp_SelectStatisticsByTimeSlot_Click(object sender, EventArgs e)
        {
            if (!ValidateStatusMenu()) { return; }
            DateTime selectedDateTimeFrom = dateTimePickerStatusFrom.Value;
            string formattedDateTimeFrom = selectedDateTimeFrom.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime selectedDateTimeTo = dateTimePickerStatusTo.Value;
            string formattedDateTimeTo = selectedDateTimeTo.ToString("yyyy-MM-dd HH:mm:ss");

            DataTable dataTable = generalRepository.sp_SelectStatisticsByTimeSlot(formattedDateTimeFrom, formattedDateTimeTo);
            string filePath = @"ResultExport\" + $"BaoCaoTheoKhungGioTrongNgay_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";


            ExcelHelper.ExportToExcel(@"TemplateExport\BaoCaoTheoKhungGioTrongNgay.xlsx", filePath, dataTable,
                ExcelHelper.GetDataTimeReport(formattedDateTimeFrom, formattedDateTimeTo), 4, 1);
        }
        private void loadDataTodataGridViewGeneralReport()
        {
            DateTime selectedDateTimeFrom = dateTimePickerStatusFrom.Value;
            string formattedDateTimeFrom = selectedDateTimeFrom.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime selectedDateTimeTo = dateTimePickerStatusTo.Value;
            string formattedDateTimeTo = selectedDateTimeTo.ToString("yyyy-MM-dd HH:mm:ss");

            DataTable dataTable = generalRepository.sp_export_statictisGrossSale(formattedDateTimeFrom, formattedDateTimeTo);

            // Clear existing columns and data in DataGridView
            dataGridViewGeneralReport.Columns.Clear();
            dataGridViewGeneralReport.Rows.Clear();

          

            // Define column names for DataGridView
            string[] columnNamesMap = { "Loai", "TicketTypeName", "carTypeName", "tondauky", "VaoTrongKy", "RaTrongKy", "TonCuoiKy", "tongTien" };
            string[] columnNames = { "Loại","Loại vé", "Loại xe", "Số lượt xe chưa ra đầu kì", "Số lượt xe vào trong kì", "Số lượt xe ra trong kì", "Số lượt xe còn trong bãi cuối kì", "Tổng tiền" };


            // Add columns to the DataGridView
            foreach (string columnName in columnNames)
            {
                dataGridViewGeneralReport.Columns.Add(columnName, columnName);
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
                dataGridViewGeneralReport.Rows.Add(rowData);
            }
            //ticketdataGridView.Columns[7].Visible = false;
            //ticketdataGridView.Columns[8].Visible = false;

            // Auto resize column width to fit text
            foreach (DataGridViewColumn column in dataGridViewGeneralReport.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            dataGridViewGeneralReport.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void loadDataTodataGridViewTimeSlot()
        {
            DateTime selectedDateTimeFrom = dateTimePickerStatusFrom.Value;
            string formattedDateTimeFrom = selectedDateTimeFrom.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime selectedDateTimeTo = dateTimePickerStatusTo.Value;
            string formattedDateTimeTo = selectedDateTimeTo.ToString("yyyy-MM-dd HH:mm:ss");

            DataTable dataTable = generalRepository.sp_SelectStatisticsByTimeSlot(formattedDateTimeFrom, formattedDateTimeTo);

            // Clear existing columns and data in DataGridView
            dataGridViewTimeSlot.Columns.Clear();
            dataGridViewTimeSlot.Rows.Clear();



            // Define column names for DataGridView
            string[] columnNamesMap = { "timeSlot", "tondayky", "VaoTrongKy", "RaTrongKy", "TonCuoiKy", "tongTien" };
            string[] columnNames = { "Khung giờ", "Lượt xe tồn đầu kì", "Lượt xe vào trong kì", "Lượt xe ra trong kì","Chưa ra cuối kì","Tổng tiền" };


            // Add columns to the DataGridView
            foreach (string columnName in columnNames)
            {
                dataGridViewTimeSlot.Columns.Add(columnName, columnName);
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
                dataGridViewTimeSlot.Rows.Add(rowData);
            }
            //ticketdataGridView.Columns[7].Visible = false;
            //ticketdataGridView.Columns[8].Visible = false;

            // Auto resize column width to fit text
            foreach (DataGridViewColumn column in dataGridViewTimeSlot.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            dataGridViewTimeSlot.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        private bool ValidateStatusMenu()
        {
            if (dateTimePickerStatusFrom.Value > dateTimePickerStatusTo.Value)
            {
                MessageBox.Show("Invalid time range! End time must be after start time.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return  false;
            }
            return true;
        }
        private void ReloadStatus_Click(object sender, EventArgs e)
        {
            if (!ValidateStatusMenu()) { return; }
            loadDataTodataGridViewGeneralReport();
            loadDataTodataGridViewTimeSlot();
        }

        private void ReportStatictis_Click_1(object sender, EventArgs e)
        {
            if (!ValidateStatusMenu()) { return; }
            DateTime selectedDateTimeFrom = dateTimePickerStatusFrom.Value;
            string formattedDateTimeFrom = selectedDateTimeFrom.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime selectedDateTimeTo = dateTimePickerStatusTo.Value;
            string formattedDateTimeTo = selectedDateTimeTo.ToString("yyyy-MM-dd HH:mm:ss");

            DataTable dataTable = generalRepository.sp_reportRevenueByMonthlyTicket(formattedDateTimeFrom, formattedDateTimeTo);
            string filePath = @"ResultExport\" + $"BaoCaoDoanhThuTheoThang_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";


            ExcelHelper.ExportToExcel(@"TemplateExport\BaoCaoDoanhThuTheoThang.xlsx", filePath, dataTable,
                ExcelHelper.GetDataTimeReport(formattedDateTimeFrom, formattedDateTimeTo), 5, 1);
        }
        public void LoadDataTodataGridViewLongDay()
        {
      
            // Retrieve data from the database
            DataTable dataTable = generalRepository.sp_LongTermParking(checkBoxIsMontlyVehicle.Checked, checkBoxIsSingle.Checked, int.Parse(textBoxNumberOfDay.Text.Length >0 ? textBoxNumberOfDay.Text: "1"));

            // Clear existing columns and data in DataGridView
            dataGridViewLongDay.Columns.Clear();
            dataGridViewLongDay.Rows.Clear();

            // Define column names for DataGridView
            string[] columnNames = { "STT", "Số ngày trong bãi xe", "Loại xe", "Biển số xe vào", "Thời gian vào", "Mã thẻ", "Loại vé", "urlIn", "urlOut" };
            string[] columnNamesMap = { "Row#", "numberOfDay", "carTypeName", "licensePlateNumberIn", "time_In", "cardInfroID", "Loai", "PhotoLicensePlateNumberInPath", "PhotoLicensePlateNumberOutPath" };

            // Add columns to the DataGridView
            foreach (string columnName in columnNames)
            {
                dataGridViewLongDay.Columns.Add(columnName, columnName);
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
                dataGridViewLongDay.Rows.Add(rowData);
            }
            dataGridViewLongDay.Columns[7].Visible = false;
            dataGridViewLongDay.Columns[8].Visible = false;

            // Auto resize column width to fit text
            foreach (DataGridViewColumn column in dataGridViewLongDay.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            dataGridViewLongDay.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }

        private void longDayReload_Click(object sender, EventArgs e)
        {
            LoadDataTodataGridViewLongDay();
        }

        private void dataGridViewLongDay_SelectionChanged(object sender, EventArgs e)
        {
            // Assuming columnIndex is the index of the column you want to display
            int columnIndexurlOut = GetColumnIndexByName(dataGridViewLongDay, "urlOut");
            int columnIndexurlIn = GetColumnIndexByName(dataGridViewLongDay, "urlIn");
            // Make sure at least one row is selected
            if (dataGridViewLongDay.SelectedRows.Count > 0 &&
                dataGridViewLongDay.SelectedRows[0].Cells[columnIndexurlOut] != null &&
                dataGridViewLongDay.SelectedRows[0].Cells[columnIndexurlIn] != null &&
                dataGridViewLongDay.SelectedRows[0].Cells[columnIndexurlOut].Value != null &&
                dataGridViewLongDay.SelectedRows[0].Cells[columnIndexurlIn].Value != null)
            {


                // Get the value from the selected row and column
                string selectedValueurlOut = dataGridViewLongDay.SelectedRows[0].Cells[columnIndexurlOut].Value.ToString();
                string selectedValueurlIn = dataGridViewLongDay.SelectedRows[0].Cells[columnIndexurlIn].Value.ToString();
                if (selectedValueurlIn.Length > 0 && selectedValueurlOut.Length > 0)
                {
                    pictureBoxImagePlateInLongDay.Image = Image.FromFile(selectedValueurlIn);
                    pictureBoxImagePlateOutLongDay.Image = Image.FromFile(selectedValueurlOut);
                }

            }

        }

        private void exportReportLostCard_Click(object sender, EventArgs e)
        {
            if (textBoxCardLostId.Text.Length < 1)
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataTable dt = generalRepository.sp_ExportReportOfLostVehicleCard(textBoxCardLostId.Text);
      
           
            string sourceFile = @"TemplateWord\BienBanMatThe.docx"; //this is where you store your template
            string filePath = @"ResultExportWord\" + $"BienBanMatThe_{DateTime.Now:yyyyMMdd_HHmmss}.docx";
            MailMergeProcessor.Mailmerge(sourceFile, filePath, dt.Rows[0], dt.Columns);
            
        }

        public void LoadDataTodataGridViewCardLost()
        {

            // Retrieve data from the database
            DataTable dataTable = generalRepository.sp_HandlingLostCard();

            // Clear existing columns and data in DataGridView
            dataGridViewCardLost.Columns.Clear();
            dataGridViewCardLost.Rows.Clear();

            // Define column names for DataGridView
            string[] columnNames = { "Mã thẻ","Loại thẻ","Tên khách hàng","BIển số xe","Thời gian mất","Thời gian giải quyết","Hướng giải quyết",
                                    "Tiền phạt","Ghi chú","Ngày sinh","Số CCCD/CMND","Ngày cấp","Nơi cấp","Địa chỉ thường chú",
                                    "Nơi ở hiện tại","Số điện thoại","Ngày đăng ký phương tiện","Nơi đăng ký phương tiện","Loại phương tiện","Màu phương tiện","Số máy",
                                    "Số khung","Chủ sở hữu","Địa điểm mất thẻ" };
            string[] columnNamesMap ={ "LostCardID","CardType","GuestName","PlateNumber","LostDateTime","HandlingDateTime","HandlingAction",
                                    "PenaltyAmount","AdditionalNotes","Birthday","Identity_number","Identity_date_issue","Identity_plate_issue","permanent_residence",
                                    "current_residence","phone_number","Vehicle_Registration_Date","Vehicle_Registration_Place","vehicleType","vehicleColor","Engine_number",
                                    "Chassis_number","Vehicle_owner","Location_of_lost_vehicle_card" };

            // Add columns to the DataGridView
            foreach (string columnName in columnNames)
            {
                dataGridViewCardLost.Columns.Add(columnName, columnName);
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
                dataGridViewCardLost.Rows.Add(rowData);
            }
            //dataGridViewCardLost.Columns[7].Visible = false;
            //dataGridViewCardLost.Columns[8].Visible = false;

            // Auto resize column width to fit text
            foreach (DataGridViewColumn column in dataGridViewCardLost.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            //dataGridViewCardLost.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }

        private void buttonReloadLostCard_Click(object sender, EventArgs e)
        {
            LoadDataTodataGridViewCardLost();
        }
        public void LoadDataTodataGridViewLostVehicle()
        {

            // Retrieve data from the database
            DataTable dataTable = generalRepository.sp_HandlingLostVehicle();

            // Clear existing columns and data in DataGridView
            dataGridViewLostVehicle.Columns.Clear();
            dataGridViewLostVehicle.Rows.Clear();

            // Define column names for DataGridView
            string[] columnNames = {"Mã vụ việc","Tên khách gửi xe","Địa chỉ khách","Số điện thoại khách","Biển số","Loại xe","Màu xe",
                                    "Thời gian vào","Địa điểm đỗ xe","Thời gian xảy ra","Địa điểm xảy ra","Kết quả điều tra","Tiền đền bù","Đã trình báo với cơ quan chức năng",
                                    "Phản hồi khách gửi","Đơn vị giải quyết","Ngày sinh khách gửi xe","Nguyên quán","Nơi ở hiện tại","Số CCCD/CMND","Ngày cấp căn cước",
                                    "Nơi cấp căn cước","Nguyên nhân mất xe"};
            string[] columnNamesMap ={"IncidentID","CustomerName","CustomerAddress","CustomerPhoneNumber","PlateNumber","VehicleType","VehicleColor",
                                    "ParkingTime","ParkingLocation","IncidentTime","IncidentLocation","InvestigationResult","CompensationAmount","ReportToAuthorities",
                                    "ResponseToCustomer","Agency_resolving_the_incident","birthDay","placeOfOrigin","currentOccupation","Identity_Number","Identity_date_issue",
                                    "Identity_place_issue","Cause_of_vehicle_loss"};

            // Add columns to the DataGridView
            foreach (string columnName in columnNames)
            {
                dataGridViewLostVehicle.Columns.Add(columnName, columnName);
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
                dataGridViewLostVehicle.Rows.Add(rowData);
            }
            //dataGridViewCardLost.Columns[7].Visible = false;
            //dataGridViewCardLost.Columns[8].Visible = false;

            // Auto resize column width to fit text
            foreach (DataGridViewColumn column in dataGridViewLostVehicle.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            //dataGridViewCardLost.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }

        private void buttonReloadLostVehicle_Click(object sender, EventArgs e)
        {
            LoadDataTodataGridViewLostVehicle();
        }

        private void exportLostVehicle_Click(object sender, EventArgs e)
        {
            if (textBoxIdLostVehicle.Text.Length < 1)
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataTable dt = generalRepository.sp_ExportReportHandlingLostVehicle(textBoxIdLostVehicle.Text);


            string sourceFile = @"TemplateWord\DonTrinhBaoMatTaiSan.docx"; //this is where you store your template
            string filePath = @"ResultExportWord\" + $"DonTrinhBaoMatTaiSan_{DateTime.Now:yyyyMMdd_HHmmss}.docx";
            MailMergeProcessor.Mailmerge(sourceFile, filePath, dt.Rows[0], dt.Columns);
        }

        private void dataGridViewLostVehicle_SelectionChanged(object sender, EventArgs e)
        {
            // Define column names for DataGridView
            string[] columnNames = {"Mã vụ việc","Tên khách gửi xe","Địa chỉ khách","Số điện thoại khách","Biển số","Loại xe","Màu xe",
                                    "Thời gian vào","Địa điểm đỗ xe","Thời gian xảy ra","Địa điểm xảy ra","Kết quả điều tra","Tiền đền bù","Đã trình báo với cơ quan chức năng",
                                    "Phản hồi khách gửi","Đơn vị giải quyết","Ngày sinh khách gửi xe","Nguyên quán","Nơi ở hiện tại","Số CCCD/CMND","Ngày cấp căn cước",
                                    "Nơi cấp căn cước","Nguyên nhân mất xe"};
            // Usage example
            int columnIndexIDIncidence = GetColumnIndexByName(dataGridViewLostVehicle, columnNames[0]);
  
            // Make sure at least one row is selected
            if (dataGridViewLostVehicle.SelectedRows.Count > 0 &&
                dataGridViewLostVehicle.SelectedRows[0].Cells[columnIndexIDIncidence] != null &&
                dataGridViewLostVehicle.SelectedRows[0].Cells[columnIndexIDIncidence].Value != null )
               
            {

                //string selectedValueDayApply = ConvertDateFormat(ticketdataGridView.SelectedRows[0].Cells[columnIndexDayApply].Value.ToString());
                // Usage example to retrieve values from selected row and columns
                string selectedValueIDIncidence = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[0])].Value.ToString();
                string selectedValueCustomerName = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[1])].Value.ToString();
                string selectedValueCustomerAddress = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[2])].Value.ToString();
                string selectedValueCustomerPhone = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[3])].Value.ToString();
                string selectedValuePlateNumber = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[4])].Value.ToString();
                string selectedValueCarType = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[5])].Value.ToString();
                string selectedValueColor = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[6])].Value.ToString();
                string selectedValueEntryTime = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[7])].Value.ToString();
                string selectedValueParkingLocation = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[8])].Value.ToString();
                string selectedValueIncidentTime = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[9])].Value.ToString();
                string selectedValueIncidentLocation = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[10])].Value.ToString();
                string selectedValueInvestigationResult = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[11])].Value.ToString();
                string selectedValueCompensationAmount = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[12])].Value.ToString();
                string selectedValueReportedToAuthorities = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[13])].Value.ToString();
                string selectedValueCustomerFeedback = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[14])].Value.ToString();
                string selectedValueResolvingUnit = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[15])].Value.ToString();
                string selectedValueCustomerDOB = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[16])].Value.ToString();
                string selectedValueCustomerOrigin = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[17])].Value.ToString();
                string selectedValueCurrentResidence = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[18])].Value.ToString();
                string selectedValueIDCardNumber = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[19])].Value.ToString();
                string selectedValueIDIssueDate = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[20])].Value.ToString();
                string selectedValueIDIssuePlace = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[21])].Value.ToString();
                string selectedValueVehicleLossReason = dataGridViewLostVehicle.SelectedRows[0].Cells[GetColumnIndexByName(dataGridViewLostVehicle, columnNames[22])].Value.ToString();


                IncidentID.Text = selectedValueIDIncidence;
                CustomerName.Text = selectedValueCustomerName;
                CustomerAddress.Text = selectedValueCustomerAddress;
                CustomerPhoneNumber.Text = selectedValueCustomerPhone;
                PlateNumberV.Text = selectedValuePlateNumber;
                VehicleType.Text = selectedValueCarType;
                VehicleColor.Text = selectedValueColor;
                ParkingTime.Text = selectedValueEntryTime;
                ParkingLocation.Text = selectedValueParkingLocation;
                IncidentTime.Text = selectedValueIncidentTime;
                IncidentLocation.Text = selectedValueIncidentLocation;
                InvestigationResult.Text = selectedValueInvestigationResult;
                CompensationAmount.Text = selectedValueCompensationAmount;
                ReportToAuthorities.Checked = bool.Parse(selectedValueReportedToAuthorities);
                ResponseToCustomer.Text = selectedValueCustomerFeedback;
                Agency_resolving_the_incident.Text = selectedValueResolvingUnit;
                birthDay.Text = selectedValueCustomerDOB;
                placeOfOrigin.Text = selectedValueCustomerOrigin;
                currentOccupation.Text = selectedValueCurrentResidence;
                Identity_Number.Text = selectedValueIDCardNumber;
                Identity_date_issueV.Text = selectedValueIDIssueDate;
                Identity_place_issue.Text = selectedValueIDIssuePlace;
                Cause_of_vehicle_loss.Text = selectedValueVehicleLossReason;


                saveLostVehicle.Enabled = true;
                deleteLostVehicle.Enabled = true;
                addLostVehicle.Enabled = false;

                

            }
            else
            {

                IncidentID.Text = "";
                CustomerName.Text = "";
                CustomerAddress.Text = "";
                CustomerPhoneNumber.Text = "";
                PlateNumberV.Text = "";
                VehicleType.Text = "";
                VehicleColor.Text = "";
                ParkingTime.Text = "";
                ParkingLocation.Text = "";
                IncidentTime.Text = "";
                IncidentLocation.Text = "";
                InvestigationResult.Text = "";
                CompensationAmount.Text = "";
                ReportToAuthorities.Checked = false; // Uncheck the checkbox
                ResponseToCustomer.Text = "";
                Agency_resolving_the_incident.Text = "";
                birthDay.Text = "";
                placeOfOrigin.Text = "";
                currentOccupation.Text = "";
                Identity_Number.Text = "";
                Identity_date_issueV.Text = "";
                Identity_place_issue.Text = "";
                Cause_of_vehicle_loss.Text = "";


                saveLostVehicle.Enabled = false;
                deleteLostVehicle.Enabled = false;
                addLostVehicle.Enabled = true;

            }
        }
        private bool ValidateRequiredFieldsLostVehicle()
        {
            bool isValid = true;

            // Validate IncidentID
            if (!int.TryParse(IncidentID.Text, out _) || int.Parse(IncidentID.Text) <= 0)
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate CustomerName
            if (string.IsNullOrWhiteSpace(CustomerName.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate CustomerAddress
            if (string.IsNullOrWhiteSpace(CustomerAddress.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate CustomerPhoneNumber
            if (string.IsNullOrWhiteSpace(CustomerPhoneNumber.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate PlateNumberV
            if (string.IsNullOrWhiteSpace(PlateNumberV.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate VehicleType
            if (string.IsNullOrWhiteSpace(VehicleType.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate VehicleColor
            if (string.IsNullOrWhiteSpace(VehicleColor.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate ParkingTime
            if (!DateTime.TryParse(ParkingTime.Text, out _))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate ParkingLocation
            if (string.IsNullOrWhiteSpace(ParkingLocation.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate IncidentTime
            if (!DateTime.TryParse(IncidentTime.Text, out _))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate IncidentLocation
            if (string.IsNullOrWhiteSpace(IncidentLocation.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate InvestigationResult
            if (string.IsNullOrWhiteSpace(InvestigationResult.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate CompensationAmount
            if (!float.TryParse(CompensationAmount.Text, out _) || float.Parse(CompensationAmount.Text) <= 0)
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate ResponseToCustomer
            // No specific validation required for ResponseToCustomer

            // Validate Agency_resolving_the_incident
            if (string.IsNullOrWhiteSpace(Agency_resolving_the_incident.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate birthDay
            if (!DateTime.TryParse(birthDay.Text, out _))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate placeOfOrigin
            if (string.IsNullOrWhiteSpace(placeOfOrigin.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate currentOccupation
            if (string.IsNullOrWhiteSpace(currentOccupation.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate Identity_Number
            if (string.IsNullOrWhiteSpace(Identity_Number.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate Identity_date_issueV
            if (!DateTime.TryParse(Identity_date_issueV.Text, out _))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate Identity_place_issue
            if (string.IsNullOrWhiteSpace(Identity_place_issue.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate Cause_of_vehicle_loss
            if (string.IsNullOrWhiteSpace(Cause_of_vehicle_loss.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            return isValid;
        }

        private void addLostVehicle_Click(object sender, EventArgs e)
        {
            if (!ValidateRequiredFieldsLostVehicle())
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            handlingLostVehicleRepository.sp_InsertHandlingLostVehicle(
                 int.Parse(IncidentID.Text), CustomerName.Text, CustomerAddress.Text, CustomerPhoneNumber.Text, PlateNumberV.Text,
             VehicleType.Text, VehicleColor.Text, ConvertDateFormat(ParkingTime.Text), ParkingLocation.Text, ConvertDateFormat(IncidentTime.Text),
              IncidentLocation.Text,  InvestigationResult.Text,  float.Parse(CompensationAmount.Text),  ReportToAuthorities.Checked,  ResponseToCustomer.Text,
             Agency_resolving_the_incident.Text,  birthDay.Text,  placeOfOrigin.Text,  currentOccupation.Text,  Identity_Number.Text,
             Identity_date_issueV.Text,  Identity_place_issue.Text,  Cause_of_vehicle_loss.Text
                );
           

            LoadDataTodataGridViewLostVehicle();
            saveLostVehicle.Enabled = false;
            deleteLostVehicle.Enabled = false;
            addLostVehicle.Enabled = false;
        }

        private void saveLostVehicle_Click(object sender, EventArgs e)
        {
            if (!ValidateRequiredFieldsLostVehicle())
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                handlingLostVehicleRepository.sp_UpdateHandlingLostVehicle(
                 int.Parse(IncidentID.Text), CustomerName.Text, CustomerAddress.Text, CustomerPhoneNumber.Text, PlateNumberV.Text,
             VehicleType.Text, VehicleColor.Text, ConvertDateFormat(ParkingTime.Text), ParkingLocation.Text, ConvertDateFormat(IncidentTime.Text),
              IncidentLocation.Text, InvestigationResult.Text, float.Parse(CompensationAmount.Text), ReportToAuthorities.Checked, ResponseToCustomer.Text,
             Agency_resolving_the_incident.Text, birthDay.Text, placeOfOrigin.Text, currentOccupation.Text, Identity_Number.Text,
             Identity_date_issueV.Text, Identity_place_issue.Text, Cause_of_vehicle_loss.Text
                );

            LoadDataTodataGridViewLostVehicle();
            saveLostVehicle.Enabled = false;
            deleteLostVehicle.Enabled = false;
            addLostVehicle.Enabled = false;
        }

        private void deleteLostVehicle_Click(object sender, EventArgs e)
        {
            handlingLostVehicleRepository.sp_DeleteHandlingLostVehicle(int.Parse(IncidentID.Text));
            LoadDataTodataGridViewLostVehicle();
            saveLostVehicle.Enabled = false;
            deleteLostVehicle.Enabled = false;
            addLostVehicle.Enabled = false;
        }

        private void dataGridViewCardLost_SelectionChanged(object sender, EventArgs e)
        {
            // Define column names for DataGridView
            string[] columnNames = {"Mã thẻ","Loại thẻ","Tên khách hàng","BIển số xe","Thời gian mất","Thời gian giải quyết","Hướng giải quyết",
                            "Tiền phạt","Ghi chú","Ngày sinh","Số CCCD/CMND","Ngày cấp","Nơi cấp","Địa chỉ thường chú",
                            "Nơi ở hiện tại","Số điện thoại","Ngày đăng ký phương tiện","Nơi đăng ký phương tiện","Loại phương tiện","Màu phương tiện","Số máy",
                            "Số khung","Chủ sở hữu","Địa điểm mất thẻ"};

            // Make sure at least one row is selected
            if (dataGridViewCardLost.SelectedRows.Count > 0)
            {
                // Retrieve the index of each column using GetColumnIndexByName
                int[] columnIndexes = new int[columnNames.Length];
                for (int i = 0; i < columnNames.Length; i++)
                {
                    columnIndexes[i] = GetColumnIndexByName(dataGridViewCardLost, columnNames[i]);
                }

                // Get the selected row
                DataGridViewRow selectedRow = dataGridViewCardLost.SelectedRows[0];

                // Assign values to text boxes based on column names
                LostCardID.Text = selectedRow.Cells[columnIndexes[0]].Value?.ToString() ?? "";
                CardType.Text = selectedRow.Cells[columnIndexes[1]].Value?.ToString() ?? "";
                GuestName.Text = selectedRow.Cells[columnIndexes[2]].Value?.ToString() ?? "";
                PlateNumberC.Text = selectedRow.Cells[columnIndexes[3]].Value?.ToString() ?? "";
                LostDateTime.Text = selectedRow.Cells[columnIndexes[4]].Value?.ToString() ?? "";
                HandlingDateTime.Text = selectedRow.Cells[columnIndexes[5]].Value?.ToString() ?? "";
                HandlingAction.Text = selectedRow.Cells[columnIndexes[6]].Value?.ToString() ?? "";
                PenaltyAmount.Text = selectedRow.Cells[columnIndexes[7]].Value?.ToString() ?? "";
                AdditionalNotes.Text = selectedRow.Cells[columnIndexes[8]].Value?.ToString() ?? "";
                BirthdayC.Text = selectedRow.Cells[columnIndexes[9]].Value?.ToString() ?? "";
                Identity_numberC.Text = selectedRow.Cells[columnIndexes[10]].Value?.ToString() ?? "";
                Identity_date_issueC.Text = selectedRow.Cells[columnIndexes[11]].Value?.ToString() ?? "";
                Identity_plate_issue.Text = selectedRow.Cells[columnIndexes[12]].Value?.ToString() ?? "";
                permanent_residence.Text = selectedRow.Cells[columnIndexes[13]].Value?.ToString() ?? "";
                current_residence.Text = selectedRow.Cells[columnIndexes[14]].Value?.ToString() ?? "";
                phone_number.Text = selectedRow.Cells[columnIndexes[15]].Value?.ToString() ?? "";
                Vehicle_Registration_Date.Text = selectedRow.Cells[columnIndexes[16]].Value?.ToString() ?? "";
                Vehicle_Registration_Place.Text = selectedRow.Cells[columnIndexes[17]].Value?.ToString() ?? "";
                vehicleTypeC.Text = selectedRow.Cells[columnIndexes[18]].Value?.ToString() ?? "";
                vehicleColorC.Text = selectedRow.Cells[columnIndexes[19]].Value?.ToString() ?? "";
                Engine_number.Text = selectedRow.Cells[columnIndexes[20]].Value?.ToString() ?? "";
                Chassis_number.Text = selectedRow.Cells[columnIndexes[21]].Value?.ToString() ?? "";
                Vehicle_owner.Text = selectedRow.Cells[columnIndexes[22]].Value?.ToString() ?? "";
                Location_of_lost_vehicle_card.Text = selectedRow.Cells[columnIndexes[23]].Value?.ToString() ?? "";

                if(LostCardID.Text.Length > 0)
                {
                    // Enable appropriate buttons
                    saveLostCard.Enabled = true;
                    deleteLostCard.Enabled = true;
                    addLostCard.Enabled = false;
                }
                else
                {
                   
                    saveLostCard.Enabled = false;
                    deleteLostCard.Enabled = false;
                    addLostCard.Enabled = true;
                }
                
            }
           
        }
        private bool ValidateRequiredFieldsLostCard()
        {
            bool isValid = true;

            // Validate LostCardID
            if (string.IsNullOrWhiteSpace(LostCardID.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate CardType
            if (string.IsNullOrWhiteSpace(CardType.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate GuestName
            if (string.IsNullOrWhiteSpace(GuestName.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate PlateNumberC
            if (string.IsNullOrWhiteSpace(PlateNumberC.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate LostDateTime
            if (!DateTime.TryParse(LostDateTime.Text, out _))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate HandlingDateTime
            if (!DateTime.TryParse(HandlingDateTime.Text, out _))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate HandlingAction
            if (string.IsNullOrWhiteSpace(HandlingAction.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate PenaltyAmount
            if (!float.TryParse(PenaltyAmount.Text, out _) || float.Parse(PenaltyAmount.Text) <= 0)
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate AdditionalNotes
            // No specific validation required for AdditionalNotes

            // Validate BirthdayC
            if (!DateTime.TryParse(BirthdayC.Text, out _))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate Identity_numberC
            if (string.IsNullOrWhiteSpace(Identity_numberC.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate Identity_date_issueC
            if (!DateTime.TryParse(Identity_date_issueC.Text, out _))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate Identity_plate_issue
            if (string.IsNullOrWhiteSpace(Identity_plate_issue.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate permanent_residence
            if (string.IsNullOrWhiteSpace(permanent_residence.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate current_residence
            if (string.IsNullOrWhiteSpace(current_residence.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate phone_number
            if (string.IsNullOrWhiteSpace(phone_number.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate Vehicle_Registration_Date
            if (!DateTime.TryParse(Vehicle_Registration_Date.Text, out _))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate Vehicle_Registration_Place
            if (string.IsNullOrWhiteSpace(Vehicle_Registration_Place.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate vehicleTypeC
            if (string.IsNullOrWhiteSpace(vehicleTypeC.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate vehicleColorC
            if (string.IsNullOrWhiteSpace(vehicleColorC.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate Engine_number
            if (string.IsNullOrWhiteSpace(Engine_number.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate Chassis_number
            if (string.IsNullOrWhiteSpace(Chassis_number.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate Vehicle_owner
            if (string.IsNullOrWhiteSpace(Vehicle_owner.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            // Validate Location_of_lost_vehicle_card
            if (string.IsNullOrWhiteSpace(Location_of_lost_vehicle_card.Text))
            {
                isValid = false;
                // You can show an error message or change the appearance of the control to indicate an error.
            }

            return isValid;
        }

        private void addLostCard_Click(object sender, EventArgs e)
        {
            if (!ValidateRequiredFieldsLostCard())
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            handlingLostCardRepository.sp_InsertHandlingLostCard(
                   LostCardID.Text,  CardType.Text,  GuestName.Text,  PlateNumberC.Text, ConvertDateFormat(LostDateTime.Text),
            ConvertDateFormat(HandlingDateTime.Text),  HandlingAction.Text, float.Parse(PenaltyAmount.Text),  AdditionalNotes.Text, ConvertDateFormat( BirthdayC.Text),
             Identity_numberC.Text,  Identity_date_issueC.Text,  Identity_plate_issue.Text,  permanent_residence.Text,  current_residence.Text,
             phone_number.Text, ConvertDateFormat( Vehicle_Registration_Date.Text),  Vehicle_Registration_Place.Text,  vehicleTypeC.Text,  vehicleColorC.Text,
             Engine_number.Text,  Chassis_number.Text,  Vehicle_owner.Text,  Location_of_lost_vehicle_card.Text

                );

            LoadDataTodataGridViewCardLost();
            saveLostCard.Enabled = false;
            deleteLostCard.Enabled = false;
            addLostCard.Enabled = false;
        }

        private void saveLostCard_Click(object sender, EventArgs e)
        {
            if (!ValidateRequiredFieldsLostCard())
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            handlingLostCardRepository.sp_UpdateHandlingLostCard(
                 LostCardID.Text, CardType.Text, GuestName.Text, PlateNumberC.Text, ConvertDateFormat(LostDateTime.Text),
          ConvertDateFormat(HandlingDateTime.Text), HandlingAction.Text, float.Parse(PenaltyAmount.Text), AdditionalNotes.Text, ConvertDateFormat(BirthdayC.Text),
           Identity_numberC.Text, Identity_date_issueC.Text, Identity_plate_issue.Text, permanent_residence.Text, current_residence.Text,
           phone_number.Text, ConvertDateFormat(Vehicle_Registration_Date.Text), Vehicle_Registration_Place.Text, vehicleTypeC.Text, vehicleColorC.Text,
           Engine_number.Text, Chassis_number.Text, Vehicle_owner.Text, Location_of_lost_vehicle_card.Text

              );

            LoadDataTodataGridViewCardLost();
            saveLostCard.Enabled = false;
            deleteLostCard.Enabled = false;
            addLostCard.Enabled = false;
        }

        private void deleteLostCard_Click(object sender, EventArgs e)
        {
            handlingLostCardRepository.sp_DeleteHandlingLostCard(
                 LostCardID.Text

              );

            LoadDataTodataGridViewCardLost();
            saveLostCard.Enabled = false;
            deleteLostCard.Enabled = false;
            addLostCard.Enabled = false;
        }

        private void paramSettingMenu_Click(object sender, EventArgs e)
        {
            navigateToTab(homePage, paramSettingTabPage);
            reloadMenuParamSetting();
        }
        private void reloadMenuParamSetting()
        {
            textBoxNameReaderIn.Text = settingParamRepo.sp_SelectSetting("NameReaderIn");
            textBoxNameReaderOut.Text = settingParamRepo.sp_SelectSetting("NameReaderOut");
        }
        private void ButtonCloseParamSettingTab_Click(object sender, EventArgs e)
        {
            closeTab();
        }

        private void buttonSaveParamSett_Click(object sender, EventArgs e)
        {
            settingParamRepo.sp_UpdateSettingParam("NameReaderIn", textBoxNameReaderIn.Text);
            settingParamRepo.sp_UpdateSettingParam("NameReaderOut", textBoxNameReaderOut.Text);
            reloadMenuParamSetting();
            MessageBox.Show("Lưa thành công", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonDeletehistory_Click(object sender, EventArgs e)
        {
            userRepository.sp_DeleteHistoryInOut(timeVehicleIn, numberPlateVehicleIn);
            buttonReload.PerformClick();
        }
    }
}
