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
    public class ProgrammeTypeTypeDAL
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ProgrammeTypeTypeDAL));

        public ProgrammeTypeTypeDAL()
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

        public bool InsertProgrammeType(COM.ProgrammeType requestSetProgrammeType)
        {
            logger.Info("InsertProgrammeType");
            EntityConnection entityConnection = new EntityConnection();
            string error;
            try
            {
                var insertProgrammeType = from programme in entityConnection.dbclayOMSDataContext.InsertProgrammeType(requestSetProgrammeType.programmeType, requestSetProgrammeType.activated, requestSetProgrammeType.addUser)
                                      select programme;

                foreach (var response in insertProgrammeType)
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

        public bool UpdateProgrammeType(COM.ProgrammeType requestSetProgrammeType)
        {
            logger.Info("UpdateProgrammeType");
            EntityConnection entityConnection = new EntityConnection();

            try
            {
                var updateProgrammeType = from programme in entityConnection.dbclayOMSDataContext.UpdateProgrammeType(requestSetProgrammeType.programmeTypeID, requestSetProgrammeType.programmeType, requestSetProgrammeType.activated, requestSetProgrammeType.updateUser)
                                      select programme;

                foreach (var response in updateProgrammeType)
                {
                    if (response.Result != 0)
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
                    entityConnection.dbclayOMSDataContext.Transaction.Dispose();
                    entityConnection.dbclayOMSDataContext.Connection.Dispose();
                    entityConnection.dbclayOMSDataContext.Connection.Close();
                    entityConnection.dbclayOMSDataContext.Dispose();
                }
            }
        }

        public COM.ProgrammeType FetchProgrammeType(COM.ProgrammeType requestSetProgrammeType)
        {
            logger.Info("FetchProgrammeType");
            EntityConnection entityConnection = new EntityConnection();
            COM.ProgrammeType responseGetProgrammeType = new COM.ProgrammeType();

            try
            {
                var fetchProgrammeType = from programme in entityConnection.dbclayOMSDataContext.FetchProgrammeType(requestSetProgrammeType.programmeTypeID)
                                     select programme;

                foreach (var response in fetchProgrammeType)
                {
                    responseGetProgrammeType.programmeTypeID = response.ProgrammeTypeID;
                    responseGetProgrammeType.programmeType = response.ProgrammeType;                   
                    responseGetProgrammeType.updateUser = response.UpdateUser;
                    responseGetProgrammeType.activated = response.Activated;
                    responseGetProgrammeType.updateDate = response.UpdateDate;
                }

                return responseGetProgrammeType;
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
                return responseGetProgrammeType;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetProgrammeType;
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

        public List<COM.ProgrammeType> GetProgrammeType(COM.ProgrammeType requestSetProgrammeType)
        {
            logger.Info("GetProgrammeType");
            EntityConnection entityConnection = new EntityConnection();
            List<COM.ProgrammeType> responseGetProgrammeType = new List<COM.ProgrammeType>();

            try
            {
                var getProgrammeType = from programme in entityConnection.dbclayOMSDataContext.GetProgrammeType(requestSetProgrammeType.programmeType)
                                   select programme;

                foreach (var response in getProgrammeType)
                {
                    responseGetProgrammeType.Add(new COM.ProgrammeType
                    {
                        
                        programmeType = response.ProgrammeType,
                        programmeTypeID = response.ProgrammeTypeID,
                        
                        activated = response.Activated,
                        updateDate = response.UpdateDate,
                        updateUser = response.UpdateUser
                    });
                }

                return responseGetProgrammeType;
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
                return responseGetProgrammeType;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetProgrammeType;
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
