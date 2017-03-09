using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;


namespace PFMSModule
{
    public class DataModule
    {
        static string compQuery = "  Select [CompID] ,[CompName],[CompDesc],[ActivePeriod] ,[IsActive],Convert (varchar(10),[OpeningDate],126) OpeningDate from Company";
        static string deptQuery = "Select * from Department";
        static string empQuery = "Select * from Employee";
        static string loanQuery = "Select * from Loan";
        static string loanSequence = "Select LoanSequence from Loan";
        static string QueryClosingSaveList = "select * from vw_closing_savelist";
        static string userQuery = "Select userid, Role, Company.CompID,Company.CompName from [User] left join Company on [User].CompID=Company.CompID";
        public static DataTable UserDT, CompDT;
        public static string UserRole;
        static SqlConnection conn;
        static readonly string dbcon = ConfigurationManager.ConnectionStrings["PFMS"].ConnectionString;
        //Set User and Company Globally
        public static void SetUserDT(DataTable dt)
        {
            UserDT = new DataTable();
            UserDT = dt;
        }
        public static void SetCompDT(DataTable dt)
        {
            CompDT = new DataTable();
            CompDT = dt;
        }



        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["PFMS"].ConnectionString;
        public static SqlConnection GetConnection()
        {

            //string dbcon = ConfigurationManager.ConnectionStrings["PFMS"].ConnectionString;
            conn = new SqlConnection(dbcon);
            try
            {
                SqlConnection con = new SqlConnection(dbcon);
                //con.Close();
                //con.Open();
                return con;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public static DataTable GetTable(string query)      
        {
            //string dbcon = ConfigurationManager.ConnectionStrings["Sebis"].ConnectionString;
            //SqlConnection con = new SqlConnection(dbcon);
            conn = new SqlConnection(dbcon);
            conn.Open();
            try
            {
                DataTable dt = new DataTable();

                //con.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand(query, conn);
                    adapter.Fill(dt);
                    // con.Close();
                    return dt;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                SqlConnection.ClearAllPools();
            }
        }
        public static void ExecuteQuery(string query)
        {
            //string dbcon = ConfigurationManager.ConnectionStrings["Sebis"].ConnectionString;
            //SqlConnection con = new SqlConnection(dbcon);
            //con.Open();
            conn = new SqlConnection(dbcon);
            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandTimeout = 50000000;
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                SqlConnection.ClearAllPools();
            }

        }

        //Insert methods
        public static string CompanySetup(string compName, string compDesc, string ActivePeriod, string OpeningDate)
        {
            string msg;
            try
            {
                conn = GetConnection();
                conn.Open();
                SqlCommand sqlComm = new SqlCommand("dbo.sp_Company", conn);
                sqlComm.CommandType = CommandType.StoredProcedure;

                sqlComm.Parameters.AddWithValue("@CompName", compName);
                sqlComm.Parameters.AddWithValue("@Description", compDesc);
                sqlComm.Parameters.AddWithValue("@ActivePeriod", ActivePeriod);
                sqlComm.Parameters.AddWithValue("@OpeningDate", OpeningDate);


                sqlComm.ExecuteNonQuery();
                sqlComm.Connection.Close();
                msg = "Company has been added Successfully";

            }
            catch (Exception x)
            {
                msg = x.Message.ToString();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                SqlConnection.ClearAllPools();
            }

            return msg;
        }
        public static string DepartmentSetup(string DeptName, string DeptDesc)
        {
            string msg;
            try
            {
                conn = GetConnection();
                conn.Open();
                SqlCommand sqlComm = new SqlCommand("dbo.sp_Department", conn);
                sqlComm.CommandType = CommandType.StoredProcedure;

                sqlComm.Parameters.AddWithValue("@DeptName", DeptName);
                sqlComm.Parameters.AddWithValue("@DeptDesc", DeptDesc);

                sqlComm.ExecuteNonQuery();
                sqlComm.Connection.Close();
                msg = "Department has been added successfully";
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                SqlConnection.ClearAllPools();
            }

            return msg;
        }
        public static string EmployeeSetup(string EmpComCode, string CompID, string EmpName, string CNIC, string BirthDate, string JoiningDate, string ConfirmDate, string ExitDate, string Desig, string DeptID, string EMPStatus, decimal EmployeeCont, string IsActive, decimal EmployerCont, string EntryType)
        {
            string msg;
            try
            {
                conn = GetConnection();
                conn.Open();
                SqlCommand sqlcmd = new SqlCommand("dbo.sp_Employee_add", conn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@EmployeeComCode", EmpComCode);
                sqlcmd.Parameters.AddWithValue("@CompID", CompID);
                sqlcmd.Parameters.AddWithValue("@EmployeeName", EmpName);
                sqlcmd.Parameters.AddWithValue("@CNIC", CNIC);
                sqlcmd.Parameters.AddWithValue("@BirthDate", BirthDate);
                sqlcmd.Parameters.AddWithValue("@JoiningDate", JoiningDate);
                sqlcmd.Parameters.AddWithValue("@Confirmation", ConfirmDate);
                sqlcmd.Parameters.AddWithValue("@ExitDate", ExitDate);
                sqlcmd.Parameters.AddWithValue("@Designation", Desig);
                sqlcmd.Parameters.AddWithValue("@DepartmentID", DeptID);
                sqlcmd.Parameters.AddWithValue("@EmployementStatus", EMPStatus);
                sqlcmd.Parameters.AddWithValue("@EmployeePercentage", EmployeeCont);
                sqlcmd.Parameters.AddWithValue("@IsActive", IsActive);
                sqlcmd.Parameters.AddWithValue("@EmployerPercentage", EmployerCont);
                sqlcmd.Parameters.AddWithValue("@EntryType", EntryType);

                sqlcmd.ExecuteNonQuery();
                sqlcmd.Connection.Close();
                msg = "Employee added successfully";
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                SqlConnection.ClearAllPools();
            }

            return msg;

        }
        public static string LoanSetup(string compID, string empID, int loanAmount, string IssueDate, string IsPayBack, string duration, decimal profitRate, string LoanSeq, int slab, decimal MonthlyRepay)
        {
            string msg;
            try
            {
                conn = GetConnection();
                conn.Open();
                SqlCommand sqlComm = new SqlCommand("dbo.sp_Loan", conn);
                sqlComm.CommandType = CommandType.StoredProcedure;

                sqlComm.Parameters.AddWithValue("@CompID", compID);
                sqlComm.Parameters.AddWithValue("@EmpID", empID);
                sqlComm.Parameters.AddWithValue("@LoanAmount", loanAmount);
                sqlComm.Parameters.AddWithValue("@IssueDate", IssueDate);
                sqlComm.Parameters.AddWithValue("@IsPayBack", IsPayBack);
                sqlComm.Parameters.AddWithValue("@LoanDuration", duration);
                sqlComm.Parameters.AddWithValue("@ProfitRate", profitRate);
                sqlComm.Parameters.AddWithValue("@LoanSequence", LoanSeq);
                sqlComm.Parameters.AddWithValue("@slab", slab);
                sqlComm.Parameters.AddWithValue("@MonthlyRepay", MonthlyRepay);


                sqlComm.ExecuteNonQuery();
                sqlComm.Connection.Close();
                msg = "Loan has been assgined Successfully";

            }
            catch (Exception x)
            {

                msg = x.Message.ToString();

            }
            finally
            {
                conn.Close();
                conn.Dispose();
                SqlConnection.ClearAllPools();
            }

            return msg;
        }
        public static string WithDrawl(string compID, string empID, int loanAmount, string TranDate, string IsPayBack)
        {
            string msg;
            try
            {

                conn = GetConnection();
                conn.Open();
                SqlCommand sqlComm = new SqlCommand("dbo.sp_withdrawl", conn);
                sqlComm.CommandType = CommandType.StoredProcedure;

                sqlComm.Parameters.AddWithValue("@EmpID", empID);
                sqlComm.Parameters.AddWithValue("@CompID", compID);
                sqlComm.Parameters.AddWithValue("@Amount", loanAmount);
                sqlComm.Parameters.AddWithValue("@TranDate", TranDate);
                sqlComm.Parameters.AddWithValue("IsPayBack", IsPayBack);

                sqlComm.ExecuteNonQuery();
                sqlComm.Connection.Close();
                msg = "Withdrawl Successful";

            }
            catch (Exception x)
            {

                msg = x.Message.ToString();

            }
            finally
            {
                conn.Close();
                conn.Dispose();
                SqlConnection.ClearAllPools();
            }

            return msg;
        }
        public static string LoanSchedule(string compID, string empID, string LoanSq, decimal PaidAmt)
        {
            string msg;
            try
            {
                conn = GetConnection();
                conn.Open();
                SqlCommand sqlComm = new SqlCommand("dbo.sp_loanSchedule", conn);
                sqlComm.CommandType = CommandType.StoredProcedure;

                sqlComm.Parameters.AddWithValue("@EmpID", empID);
                sqlComm.Parameters.AddWithValue("@CompID", compID);
                sqlComm.Parameters.AddWithValue("@PaidAmount", PaidAmt);
                sqlComm.Parameters.AddWithValue("@LoanSeqeunce", LoanSq);
              //  sqlComm.Parameters.AddWithValue("@Date_for_nextSchedule", date_for_nextSchedule);
               // sqlComm.Parameters.AddWithValue("@LoanOutStandingAmount", LoanOutStanding);
               // sqlComm.Parameters.AddWithValue("@LoanStartingDate", LoanStartDate);
              //  sqlComm.Parameters.AddWithValue("@duration_month", Months);

                sqlComm.ExecuteNonQuery();
                sqlComm.Connection.Close();
                msg = "Loan scheduled successful";

            }
            catch (Exception x)
            {

                msg = x.Message.ToString();

            }
            finally
            {
                conn.Close();
                conn.Dispose();
                SqlConnection.ClearAllPools();
            }

            return msg;
        }
        public static string AddLoanSchedule(string compID, string empID, decimal PayableAmt, string AppMonth, string AppYear, string LoanSq)
        {
            string msg;
            try
            {
                conn = GetConnection();
                conn.Open();
                SqlCommand sqlComm = new SqlCommand("dbo.sp_Add_LoanSchedule", conn);
                sqlComm.CommandType = CommandType.StoredProcedure;

                sqlComm.Parameters.AddWithValue("@EmpID", empID);
                sqlComm.Parameters.AddWithValue("@CompID", compID);
                sqlComm.Parameters.AddWithValue("@PayableAmt", PayableAmt);
                sqlComm.Parameters.AddWithValue("@AppMonth", AppMonth);
                sqlComm.Parameters.AddWithValue("@AppYear", AppYear);
                sqlComm.Parameters.AddWithValue("@LoanSq", LoanSq);

                sqlComm.ExecuteNonQuery();
                sqlComm.Connection.Close();
                msg = "Schedule Added Successfully";
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                SqlConnection.ClearAllPools();
            }
            return msg;
        }
        public static string Contribution(string compID, string empID, decimal Amt, string TranDesc, string TranDate, string AppMonth, string AppYear, string EntryType, string AppMonthID, string flag)
        {
            string msg;
            try
            {
                conn = GetConnection();
                conn.Open();
                SqlCommand sqlComm = new SqlCommand("dbo.sp_Transaction", conn);
                sqlComm.CommandType = CommandType.StoredProcedure;

                sqlComm.Parameters.AddWithValue("@EmpID", empID);
                sqlComm.Parameters.AddWithValue("@CompID", compID);
                sqlComm.Parameters.AddWithValue("@Amount", Amt);
                sqlComm.Parameters.AddWithValue("@TranDesc", TranDesc);
                sqlComm.Parameters.AddWithValue("@TranDate", TranDate);
                sqlComm.Parameters.AddWithValue("@ApplicableMonth", AppMonth);
                sqlComm.Parameters.AddWithValue("@ApplicableYear", AppYear);
                sqlComm.Parameters.AddWithValue("@EntryType", EntryType);
                sqlComm.Parameters.AddWithValue("@MonthID", AppMonthID);
                sqlComm.Parameters.AddWithValue("@Flag", flag);


                sqlComm.ExecuteNonQuery();
                sqlComm.Connection.Close();
                msg = "Transaction Successful";

            }
            catch (Exception x)
            {

                msg = x.Message.ToString();

            }
            finally
            {
                conn.Close();
                conn.Dispose();
                SqlConnection.ClearAllPools();
            }

            return msg;
        }
        public static string ContributionSaveList(string compID, string empID, decimal Amt, string TranDesc, string TranDate, string AppMonth, string AppYear, string EntryType, string AppMonthID, string flag, int LoanSq)
        {
            string msg;
            try
            {
                conn = GetConnection();
                conn.Open();
                SqlCommand sqlComm = new SqlCommand("dbo.sp_Transaction_SaveList", conn);
                sqlComm.CommandType = CommandType.StoredProcedure;

                sqlComm.Parameters.AddWithValue("@EmpID", empID);
                sqlComm.Parameters.AddWithValue("@CompID", compID);
                sqlComm.Parameters.AddWithValue("@Amount", Amt);
                sqlComm.Parameters.AddWithValue("@TranDesc", TranDesc);
                sqlComm.Parameters.AddWithValue("@TranDate", TranDate);
                sqlComm.Parameters.AddWithValue("@ApplicableMonth", AppMonth);
                sqlComm.Parameters.AddWithValue("@ApplicableYear", AppYear);
                sqlComm.Parameters.AddWithValue("@EntryType", EntryType);
                sqlComm.Parameters.AddWithValue("@MonthID", AppMonthID);
                sqlComm.Parameters.AddWithValue("@Flag", flag);
                sqlComm.Parameters.AddWithValue("@LoanSequence", LoanSq);


                sqlComm.ExecuteNonQuery();
                sqlComm.Connection.Close();
                msg = "Transaction Successful";

            }
            catch (Exception x)
            {

                msg = x.Message.ToString();

            }
            finally
            {
                conn.Close();
                conn.Dispose();
                SqlConnection.ClearAllPools();
            }

            return msg;
        }
        public static string FiscalClosing(string compID, decimal Amt, string closingPeriod, string openingPeriod, string TranDesc, string TranDate)
        {
            string msg;
            try
            {
                conn = GetConnection();
                conn.Open();
                SqlCommand sqlComm = new SqlCommand("dbo.sp_Closing_Process", conn);
                sqlComm.CommandType = CommandType.StoredProcedure;

                sqlComm.Parameters.AddWithValue("@CompID", compID);
                sqlComm.Parameters.AddWithValue("@totalProfit", Amt);
                sqlComm.Parameters.AddWithValue("@closingPeriod", closingPeriod);
                sqlComm.Parameters.AddWithValue("@openingPeriod", openingPeriod);
                sqlComm.Parameters.AddWithValue("@TranDesc", TranDesc);
                sqlComm.Parameters.AddWithValue("@TranDate", TranDate);               

                sqlComm.ExecuteNonQuery();
                sqlComm.Connection.Close();
                msg = "Closing Added Successfull";
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                SqlConnection.ClearAllPools();
            }
            return msg;
        }
        public static string TempLoan(string compid, string empid, decimal amt, string tranDesc, string tranDate, string appMonth, string appYear, string MonthID, int LoanSq)
        {
            string msg;
            try
            {
                conn = GetConnection();
                conn.Open();
                SqlCommand sqlComm = new SqlCommand("dbo.sp_tempLoan", conn);
                sqlComm.CommandType = CommandType.StoredProcedure;

                sqlComm.Parameters.AddWithValue("@CompID", compid);
                sqlComm.Parameters.AddWithValue("@EmpID", empid);
                sqlComm.Parameters.AddWithValue("@Amount", amt);
                sqlComm.Parameters.AddWithValue("@TranDesc", tranDesc);
                sqlComm.Parameters.AddWithValue("@TranDate", tranDate);
                sqlComm.Parameters.AddWithValue("@AppMonth", appMonth);
                sqlComm.Parameters.AddWithValue("@AppYear", appYear);
                //sqlComm.Parameters.AddWithValue("@Period", period);
                sqlComm.Parameters.AddWithValue("@MonthID", MonthID);
                sqlComm.Parameters.AddWithValue("@LoanSequence", LoanSq);

                sqlComm.ExecuteNonQuery();
                sqlComm.Connection.Close();
                msg = "Updated to Loan Repayment";
              
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                SqlConnection.ClearAllPools();
            }
            return msg;
        }

        //Selection Methods
        public static DataTable GetCompany()
        {
            //string query = "Select * from Company";
            DataTable dt = GetTable(compQuery);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }

        }
        public static void DDLFillCompany(DropDownList ddl)
        {
            //string query = "Select * from Company";
            DataTable dt = GetTable(compQuery);
            if (dt.Rows.Count > 0)
            {
                ddl.DataSource = dt;
                ddl.DataTextField = "CompName";
                ddl.DataValueField = "CompID";
                ddl.DataBind();
            }
        }
        public static DataTable GetCompany(string CompName, string CompID)
        {
            string query = compQuery + " where CompName='" + CompName + "' AND CompID!='" + CompID + "'";
            DataTable dt = GetTable(query);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return dt;
            }

        }
        public static void DDLFillCompany(DropDownList ddl, DataTable dt)
        {
            ddl.DataSource = dt;
            ddl.DataTextField = "CompName";
            ddl.DataValueField = "CompID";
            ddl.DataBind();
        }
        public static DataTable GetCompany(string CompID)
        {
            string query = compQuery + " where CompID = '" + CompID + "'";
            DataTable dt = GetTable(query);
            return dt;
        }

        public static DataTable GetDepartment()
        {
            //string query = "Select * from Department";
            DataTable dt = GetTable(deptQuery);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }

        }
        public static void DDLFillDepartment(DropDownList ddl)
        {
            //string query = "Select * from Department";
            try
            {
                DataTable dt = GetDepartment();
                if (dt.Rows.Count > 0)
                {
                    ddl.DataSource = dt;
                    ddl.DataTextField = "DepartmentName";
                    ddl.DataValueField = "DepID";
                    ddl.DataBind();
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        public static DataTable GetDepartment(string DeptName, string DeptID)
        {
            string query = deptQuery + " where DepartmentName = '" + DeptName + "' AND DepID!='" + DeptID + "'";
            DataTable dt = GetTable(query);
            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return dt;
            }
        }

        public static DataTable GetEmployee()
        {
            //string query = "Select * from Employee";
            DataTable dt = GetTable(empQuery);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        public static void DDLFillEmployee(DropDownList ddl, int CompID)
        {
            string query = "Select [EmpID],[EmployeeComCode],[CompID],[EmployeeName],[CNIC],convert (varchar(12),BirthDate,106) BirthDate,convert (varchar(12),JoiningDate,106) JoiningDate,convert (varchar(12),ConfirmationDate,106) ConfirmationDate,convert (varchar(12),ExitDate,106) ExitDate,[Designation],[DepartmentID],[EmployeementStatus],[EmployeePercentage],[IsActive],[EmployerPercentage]  from Employee where CompID='" + CompID + "' and IsActive = 'Yes' order by EmpID ";
            DataTable dt = GetTable(query);
            if (dt.Rows.Count > 0)
            {
                ddl.DataSource = dt;
                ddl.DataTextField = "EmployeeName";
                ddl.DataValueField = "EmpID";
                ddl.DataBind();
            }
            else
            {
                ddl.DataSource = dt;
                ddl.DataTextField = "EmployeeName";
                ddl.DataValueField = "EmpID";
                ddl.DataBind();
            }
        }
        public static DataTable GetEmployee(int CompanyID)
        {
            string query = "Select [EmpID],[EmployeeComCode],[CompID],[EmployeeName],[CNIC],convert (varchar(12),BirthDate,106) BirthDate,convert (varchar(12),JoiningDate,106) JoiningDate,convert (varchar(12),ConfirmationDate,106) ConfirmationDate,convert (varchar(12),ExitDate,106) ExitDate,[Designation],[DepartmentID],[EmployeementStatus],[EmployeePercentage],[IsActive],[EmployerPercentage]  from Employee where CompID='" + CompanyID + "' order by EmpID ";
            DataTable dt = GetTable(query);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetUser(string userName, string passWord)
        {
            string query = userQuery + " where userid='" + userName + "' AND password='" + passWord + "' ";
            DataTable dt = GetTable(query);
            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return dt;
            }
        }

        public static double GetBalanceAmount(string emp_id, string comp_id)
        {
            DataTable dt = new DataTable();

            //con.Open();

            SqlCommand sqlComm = new SqlCommand("dbo.sp_bal_amount", GetConnection());
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.Parameters.AddWithValue("@empid", emp_id);
            sqlComm.Parameters.AddWithValue("@compid", comp_id);

            SqlDataAdapter adapter = new SqlDataAdapter(sqlComm);
            adapter.Fill(dt);

            double balance = 0;
            if (dt.Rows.Count > 0)
            {
                balance = Convert.ToDouble(dt.Rows[0]["BalanceAmount"].ToString());
            }
            return balance;
        }

        public static DataTable GetLoans(string emp_id)
        {
            string query = loanQuery + " where EmpID= '" + emp_id + "'";
            DataTable dt = GetTable(query);
            return dt;
        }
        public static DataTable GetLoans(string emp_id, int LoanSq)
        {
            string query = loanQuery + " where EmpID= " + emp_id + " and LoanSequence= " + LoanSq;
            DataTable dt = GetTable(query);
            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

        public static string GetLoanSequence(string EmpID)
        {
            DataTable loanTable = new DataTable();
            loanTable = GetLoans(EmpID);
            if (loanTable.Rows.Count > 0)
            {

                DataTable dt = new DataTable();
                SqlCommand sqlComm = new SqlCommand("dbo.sp_s_loanSquence", GetConnection());
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.Parameters.AddWithValue("@empID", EmpID);

                SqlDataAdapter adapter = new SqlDataAdapter(sqlComm);
                adapter.Fill(dt);

                string loanSq = "";
                loanSq = dt.Rows[0]["LoanSeq"].ToString();
                return loanSq;

            }
            else
            {
                return "1";
            }


        }
        public static DataTable GetLoanSequenceForGrid(string emp_id)
        {
            string query = loanSequence + " where EmpID ='" + emp_id + "' and LoanSequence != '0' order by LoanSequence asc ";
            DataTable dt = GetTable(query);
            return dt;

        }

        public static DataTable GetLoanSchedule(string emp_id, string loan_sq)
        {
            DataTable dt = new DataTable();
            SqlCommand sqlComm = new SqlCommand("dbo.sp_s_LoanSchedule", GetConnection());
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.Parameters.AddWithValue("@empID", emp_id);
            sqlComm.Parameters.AddWithValue("@LoanSq", loan_sq);

            SqlDataAdapter adapter = new SqlDataAdapter(sqlComm);
            adapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        public static DataTable GetLoanScheduleForRepayment(string empid, string appMonth, string appYear)
        {

            string query = "Select * from [LoanSchedule] where EmpID = '" + empid + "' and ApplicableMonth = '" + appMonth + "' and ApplicableYear = '" + appYear + "' and PayableAmount is not null and (IsActive is null or IsActive =1) ";
            DataTable dt = GetTable(query);
            if (dt.Rows.Count > 0)
            {
                return dt;
               
            }
            else
            {
                return null;
            }
        }
        public static DataTable GetLoanSchedule(string empid, string appMonth, string appYear, string LoanSq)
        {

            string query = "Select * from [LoanSchedule] where EmpID = '" + empid + "' and ApplicableMonth = '" + appMonth + "' and ApplicableYear = '" + appYear + "' and LoanSequence = '"+LoanSq+"' ";
            DataTable dt = GetTable(query);
            if (dt.Rows.Count > 0)
            {
                return dt;

            }
            else
            {
                return null;
            }
        }

        public static DataTable GetMonths()
        {
            string query = "Select * from ApplicableMonth where MonthName != 'Opening'";
            DataTable dt = GetTable(query);
            if (dt.Rows.Count != 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        public static void DDLFillMonth(DropDownList ddl)
        {
            DataTable dt = GetMonths();
            ddl.DataSource = dt;
            ddl.DataTextField = "MonthName";
            ddl.DataValueField = "ID";
            ddl.DataBind();
        }

        public static DataTable GetLoanDetails(string emp_id)
        {
            string vw_query = "Select * from vw_lonaDetail where EmpID='" + emp_id + "' ";

            DataTable dt = GetTable(vw_query);
            if (dt.Rows.Count != 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetContribution(string compid, string empid, string fromDate,string toDate)
        {
            DataTable compDT =  GetCompany(compid);
            string comp_opening_date = compDT.Rows[0]["OpeningDate"].ToString();
          //  string vw_query = "Select * from vw_contribution_saveAll where CompID = '"+compid+"'AND EmpID = '"+empid+"' AND TranDate BETWEEN '"+fromDate+"' AND '"+toDate+"' order by TranID";
            string vw_query = "Select * from vw_contribution_saveAll where CompID = '" + compid + "' order by TranID";
            DataTable dt = GetTable(vw_query);
            if (dt.Rows.Count != 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        public static DataTable GetReviewList(string compid, string empid, string dateFrom, string dateTo)
        {
            if (dateTo == "")
            {
                dateTo = "2099-01-01";
            }
            string query = "Select * from vw_reviewList where CompID = '" + compid + "' AND EmpID = '" + empid + "' and TranDate between '"+dateFrom+"' and '"+dateTo+"' order by TranID ";
            DataTable dt = GetTable(query);
            if (dt.Rows.Count > 0)
            { return dt; }
            else
            { return null; }
        }
        public static string UpdateContribution(string tranID)
        {
            string msg;
            string post_time = DateTime.Now.ToString();
            string query = "Update [Transaction] set Flag = '1', PostTime = '"+post_time+"' where TranID = '" + tranID + "' ";
            try
            {
                ExecuteQuery(query);
                msg = "Successfully Updated!";
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
            return msg;

        }        


        public static DataTable GetClosingSaveList(string period, string comp_id)
        {
            string query = QueryClosingSaveList + " where Period = '"+period+"' and CompID = '"+comp_id+"' ";
            DataTable dt = GetTable(query);
            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        public static string UpdateClosingSaveList(decimal amt, string TranID)
        {
            string msg;
            string post_time = DateTime.Now.ToString();
            string query = "update [Transaction] set Flag=1, Amount="+amt+", PostTime = '"+post_time+"' where TranID= '"+TranID+"' ";
            try
            {
                ExecuteQuery(query);
                msg = "Closing list saved successfully";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }
        public static string SP_Opening(string empid, string closing_period)
        {
            string msg = string.Empty;
            try
            {
                SqlCommand sqlcmd = new SqlCommand("dbo.sp_opening", GetConnection());
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@EmpID", empid);
                sqlcmd.Parameters.AddWithValue("@closingPeriod", closing_period);

                sqlcmd.ExecuteNonQuery();
                msg = "Success";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

        public static DataTable GetRepaymentSaveList(string compid, string month, string year)
        {
            DataTable dt = new DataTable();
            SqlCommand sqlComm = new SqlCommand("dbo.sp_repayment_savelist", GetConnection());
            sqlComm.CommandType = CommandType.StoredProcedure;
            //sqlComm.Parameters.AddWithValue("@month", month);
            //sqlComm.Parameters.AddWithValue("@year", year);
            sqlComm.Parameters.AddWithValue("@compid", compid);

            SqlDataAdapter adapter = new SqlDataAdapter(sqlComm);
            adapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

        public static string UpdateLoanSchedule(string empid, string month, string year)
        {
            string msg;
            string query = "Update LoanSchedule set IsActive = '0' where EmpID = '" + empid + "' AND ApplicableMonth = '" + month + "' AND ApplicableYear = '" + year + "' ";
            try
            {
                ExecuteQuery(query);
                msg = "Saved Successfully";
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
            return msg;
        }

        public static string UpdateTempLoan(string empid, string month, string year, int serial)
        {
            string msg;
            string query = "Update Contribultion_temp_loan set flag = '1' where EmpID = '" + empid + "' AND ApplicableMonth = '"+month+"' AND ApplicableYear = '"+year+"' and Serial = "+serial;
            try
            {
                ExecuteQuery(query);
                msg = "Saved Successfully";
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
            return msg;
        }
        public static void DeleteTempLoan(string empid, string month, string year)
        {
           
                SqlCommand sqlComm = new SqlCommand("dbo.sp_del_temp_loan", GetConnection());
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.Parameters.AddWithValue("@EmpID", empid);
                sqlComm.Parameters.AddWithValue("@Month", month);
                sqlComm.Parameters.AddWithValue("@Year", year);
                sqlComm.ExecuteNonQuery();                
        }
        public static void DeleteTransaction(string TranID)
        {
            SqlCommand sqlComm = new SqlCommand("dbo.sp_del_Tran", GetConnection());
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.Parameters.AddWithValue("@TranID", TranID);            
            sqlComm.ExecuteNonQuery();        
        }
        public static void DeleteEmployee(string EmpID)
        {
            
                SqlCommand sqlComm = new SqlCommand("dbo.sp_del_Employee", GetConnection());
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.Parameters.AddWithValue("@EmpID", EmpID);
                sqlComm.ExecuteNonQuery();                
           
        }
        
        public static string UpdateTran_Repay(string compID, string empID, decimal Amt, string TranDate, string AppMonth, string AppYear, string EntryType, string AppMonthID, int LoanSq, int LoanID)
        {
            string msg;
            try
            {

                SqlCommand sqlComm = new SqlCommand("dbo.sp_tran_repayment", GetConnection());
                sqlComm.CommandType = CommandType.StoredProcedure;

                sqlComm.Parameters.AddWithValue("@EmpID", empID);
                sqlComm.Parameters.AddWithValue("@CompID", compID);
                sqlComm.Parameters.AddWithValue("@Amount", Amt);
                //sqlComm.Parameters.AddWithValue("@TranDesc", TranDesc);
                sqlComm.Parameters.AddWithValue("@TranDate", TranDate);
                sqlComm.Parameters.AddWithValue("@ApplicableMonth", AppMonth);
                sqlComm.Parameters.AddWithValue("@ApplicableYear", AppYear);
                sqlComm.Parameters.AddWithValue("@EntryType", EntryType);
                sqlComm.Parameters.AddWithValue("@MonthID", AppMonthID);
                sqlComm.Parameters.AddWithValue("@LoanSequence", LoanSq);
                sqlComm.Parameters.AddWithValue("@LoanID", LoanID);

                sqlComm.ExecuteNonQuery();

                msg = "Transaction Successful";

            }
            catch (Exception x)
            {

                msg = x.Message.ToString();

            }

            return msg;
        }    
        public static void  DDLFillPeriod(DropDownList ddl)
        {
            string query = "select * from Period order by StartDate";
            DataTable dt = GetTable(query);
            ddl.DataSource = dt;
            ddl.DataTextField = "Period";
            ddl.DataBind();
        }        
    }

}
