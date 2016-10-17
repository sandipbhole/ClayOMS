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
    public class StaffTypeDAL
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(StaffTypeDAL));

        public StaffTypeDAL()
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

        public bool InsertStaffType(COM.StaffType requestSetStaffType)
        {
            logger.Info("InsertStaffType");
            EntityConnection entityConnection = new EntityConnection();
            string error;
            try
            {
                var insertStaffType = from staffType in entityConnection.dbclayOMSDataContext.InsertStaffType(requestSetStaffType.staffType, requestSetStaffType.activated, requestSetStaffType.addUser)
                                    select staffType;

                foreach (var response in insertStaffType)
                {
                    error = response.ErrorMessage;
                    logger.Error(error);
                    return false;
                }

                entityConnection.dbclayOMSDataContext.SubmitChanges(ConflictMode.ContinueOnConflict);
                entityConnection.dbclayOMSDataContext.Transaction.Commit();
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
                    //entityConnection.dbclayOMSDataContext.Transaction.Dispose();
                    entityConnection.dbclayOMSDataContext.Connection.Dispose();
                    entityConnection.dbclayOMSDataContext.Connection.Close();
                    entityConnection.dbclayOMSDataContext.Dispose();
                }
            }
        }

        public bool UpdateStaffType(COM.StaffType requestSetStaffType)
        {
            logger.Info("UpdateStaffType");
            EntityConnection entityConnection = new EntityConnection();

            try
            {
                var updateStaffType = from staffType in entityConnection.dbclayOMSDataContext.UpdateStaffType(requestSetStaffType.staffTypeID, requestSetStaffType.staffType,requestSetStaffType.activated, requestSetStaffType.updateUser)
                                    select staffType;

                foreach (var response in updateStaffType)
                {
                    if (response.ErrorMessage != "")
                    {
                        logger.Error(response.ErrorMessage);
                        return false;
                    }
                }
                entityConnection.dbclayOMSDataContext.SubmitChanges(ConflictMode.ContinueOnConflict);
                entityConnection.dbclayOMSDataContext.Transaction.Commit();
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

        public COM.StaffType FetchStaffType(COM.StaffType requestSetStaffType)
        {
            logger.Info("FetchStaffType");
            EntityConnection entityConnection = new EntityConnection();
            COM.StaffType responseGetStaffType = new COM.StaffType();

            try
            {
                var fetchStaffType = from academicStaffType in entityConnection.dbclayOMSDataContext.FetchStaffType(requestSetStaffType.staffTypeID, requestSetStaffType.activated)
                                   select academicStaffType;

                foreach (var response in fetchStaffType)
                {
                    responseGetStaffType.staffType = response.StaffType;
                    responseGetStaffType.staffTypeID = response.StaffTypeID;                   
                    responseGetStaffType.updateUser = response.UpdateUser;
                    responseGetStaffType.activated = response.Activated;
                    responseGetStaffType.updateDate = response.UpdateDate;
                }

                return responseGetStaffType;
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
                return responseGetStaffType;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetStaffType;
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

        public List<COM.StaffType> GetStaffType(COM.StaffType requestSetStaffType)
        {
            logger.Info("GetStaffType");
            EntityConnection entityConnection = new EntityConnection();
            List<COM.StaffType> responseGetStaffType = new List<COM.StaffType>();

            try
            {
                var getStaffType = from staffType in entityConnection.dbclayOMSDataContext.GetStaffType(requestSetStaffType.staffType, requestSetStaffType.activated)
                                 select staffType;

                foreach (var response in getStaffType)
                {
                    responseGetStaffType.Add(new COM.StaffType
                    {
                        staffType = response.StaffType,
                        staffTypeID = response.StaffTypeID,                       
                        activated = response.Activated,
                        updateDate = response.UpdateDate,
                        updateUser = response.UpdateUser
                    });
                }

                return responseGetStaffType;
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
                return responseGetStaffType;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetStaffType;
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
