using System;
using System.Text;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;

using log4net;
using log4net.Appender;
using log4net.Core;

using COM = Clay.OMS.Message;

namespace Clay.OMS.Data
{
    public class DepartmentDAL
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DepartmentDAL));

        public DepartmentDAL()
        {
            InitializeLog4Net();
        }

        private void InitializeLog4Net()
        {
            RollingFileAppender appender = new RollingFileAppender();
            appender.AppendToFile = true;
            appender.Name = "ServiceLogger";
            string path = "C:\\ClayIMS"; //System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            appender.File = path + "\\ClayIMS_" + DateTime.Now.ToString("dd-MM-yyyy") + ".log";

            log4net.Layout.PatternLayout layout = new log4net.Layout.PatternLayout
            {
                ConversionPattern = "%date{{yyyy-MM-dd-HH:mm:ss.fff}} [%thread] %-5level %logger - %message%newline"
            };
            appender.Layout = layout;
            layout.ActivateOptions();
            appender.ActivateOptions();
            //appender.MaxFileSize = 10240;
            //appender.MaxSizeRollBackups = 5;

            log4net.Repository.Hierarchy.Hierarchy repository = LogManager.GetRepository() as log4net.Repository.Hierarchy.Hierarchy;
            repository.Root.AddAppender(appender);
            repository.Root.Level = Level.All;
            repository.Configured = true;
            repository.RaiseConfigurationChanged(EventArgs.Empty);

            //logger.Info("Log4NET initialized successfully.");
        }

        public bool InsertDepartment(COM.Department requestSetDepartment)
        {
            logger.Info("InsertDepartment");
            EntityConnection entityConnection = new EntityConnection();

            try
            {
                var insertDepartment = from department in entityConnection.dbclayOMSDataContext.InsertDepartment(requestSetDepartment.code, requestSetDepartment.department, requestSetDepartment.departmentType, requestSetDepartment.departmentOrder, requestSetDepartment.yearOfEstablishment, requestSetDepartment.facultyID, requestSetDepartment.activated, requestSetDepartment.addUser)
                                    select department;

                return true;
            }
            //Resolve Concurrency Conflicts by Retaining Database Values (LINQ to SQL)
            catch (ChangeConflictException ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    //Console.WriteLine(ex.Message);
                    foreach (ObjectChangeConflict occ in entityConnection.dbclayOMSDataContext.ChangeConflicts)
                    {
                        // All database values overwrite current values.
                        occ.Resolve(RefreshMode.OverwriteCurrentValues);
                    }
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return false;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return false;
            }
            finally
            {
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Dispose();
                    entityConnection.dbclayOMSDataContext.Connection.Dispose();
                    entityConnection.dbclayOMSDataContext.Connection.Close();
                    entityConnection.dbclayOMSDataContext.Dispose();
                }
            }
        }

        public bool UpdateDepartment(COM.Department requestSetDepartment)
        {
            logger.Info("UpdateDepartment");
            EntityConnection entityConnection = new EntityConnection();

            try
            {
                var updateDepartment = from department in entityConnection.dbclayOMSDataContext.UpdateDepartment(requestSetDepartment.departmentID, requestSetDepartment.code, requestSetDepartment.department,requestSetDepartment.departmentType ,requestSetDepartment.departmentOrder, requestSetDepartment.yearOfEstablishment, requestSetDepartment.facultyID, requestSetDepartment.activated, requestSetDepartment.updateUser)
                                    select department;

                return true;
            }
            //Resolve Concurrency Conflicts by Retaining Database Values (LINQ to SQL)
            catch (ChangeConflictException ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    //Console.WriteLine(ex.Message);
                    foreach (ObjectChangeConflict occ in entityConnection.dbclayOMSDataContext.ChangeConflicts)
                    {
                        // All database values overwrite current values.
                        occ.Resolve(RefreshMode.OverwriteCurrentValues);
                    }
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return false;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return false;
            }
            finally
            {
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Dispose();
                    entityConnection.dbclayOMSDataContext.Connection.Dispose();
                    entityConnection.dbclayOMSDataContext.Connection.Close();
                    entityConnection.dbclayOMSDataContext.Dispose();
                }
            }
        }

        public COM.Department FetchDepartment(COM.Department requestSetDepartment)
        {
            logger.Info("FetchDepartment");
            EntityConnection entityConnection = new EntityConnection();
            COM.Department responseGetDepartment = new COM.Department();

            try
            {
                var fetchDepartment = from department in entityConnection.dbclayOMSDataContext.FetchDepartment(requestSetDepartment.departmentID)
                                   select department;

                foreach (var response in fetchDepartment)
                {
                    responseGetDepartment.department = response.Department;
                    responseGetDepartment.departmentOrder = response.DepartmentOrder;
                    responseGetDepartment.departmentType = response.DepartmentType;
                    responseGetDepartment.departmentID = response.DepartmentID;
                    responseGetDepartment.yearOfEstablishment = response.YearOfEstablishment;
                    responseGetDepartment.facultyID = response.FacultyID;
                    responseGetDepartment.facultyName = response.FacultyName;
                    responseGetDepartment.updateUser = response.UpdateUser;
                    responseGetDepartment.activated = response.Activated;
                    responseGetDepartment.updateDate = response.UpdateDate;
                }

                return responseGetDepartment;
            }
            //Resolve Concurrency Conflicts by Retaining Database Values (LINQ to SQL)
            catch (ChangeConflictException ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    //Console.WriteLine(ex.Message);
                    foreach (ObjectChangeConflict occ in entityConnection.dbclayOMSDataContext.ChangeConflicts)
                    {
                        // All database values overwrite current values.
                        occ.Resolve(RefreshMode.OverwriteCurrentValues);
                    }
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetDepartment;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetDepartment;
            }
            finally
            {
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Dispose();
                    entityConnection.dbclayOMSDataContext.Connection.Dispose();
                    entityConnection.dbclayOMSDataContext.Connection.Close();
                    entityConnection.dbclayOMSDataContext.Dispose();
                }
            }
        }

        public List<COM.Department> GetDepartment(COM.Department requestSetDepartment)
        {
            logger.Info("GetDepartment");
            EntityConnection entityConnection = new EntityConnection();
            List<COM.Department> responseGetDepartment = new List<COM.Department>();

            try
            {
                var getDepartment = from department in entityConnection.dbclayOMSDataContext.GetDepartment(requestSetDepartment.department,requestSetDepartment.facultyName, requestSetDepartment.activated)
                                 select department;

                foreach (var response in getDepartment)
                {
                    responseGetDepartment.Add(new COM.Department
                    {
                        departmentOrder = response.DepartmentOrder ,
                        departmentType = response.DepartmentType ,
                        facultyName = response.FacultyName,
                        departmentID = response.DepartmentID,
                        department = response.Department,
                        facultyID = response.FacultyID ,
                        yearOfEstablishment = response.YearOfEstablishment,
                        activated = response.Activated,
                        updateDate = response.UpdateDate,
                        updateUser = response.UpdateUser
                    });
                }

                return responseGetDepartment;
            }
            //Resolve Concurrency Conflicts by Retaining Database Values (LINQ to SQL)
            catch (ChangeConflictException ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    //Console.WriteLine(ex.Message);
                    foreach (ObjectChangeConflict occ in entityConnection.dbclayOMSDataContext.ChangeConflicts)
                    {
                        // All database values overwrite current values.
                        occ.Resolve(RefreshMode.OverwriteCurrentValues);
                    }
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetDepartment;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetDepartment;
            }
            finally
            {
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Dispose();
                    entityConnection.dbclayOMSDataContext.Connection.Dispose();
                    entityConnection.dbclayOMSDataContext.Connection.Close();
                    entityConnection.dbclayOMSDataContext.Dispose();
                }
            }
        }

    }
}
