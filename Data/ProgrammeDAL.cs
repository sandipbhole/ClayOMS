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
    public class ProgrammeDAL
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ProgrammeDAL));

        public ProgrammeDAL()
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

        public bool InsertProgramme(COM.Programme requestSetProgramme)
        {
            logger.Info("InsertProgramme");
            EntityConnection entityConnection = new EntityConnection();
            string error;
            try
            {
                var insertProgramme = from programme in entityConnection.dbclayOMSDataContext.InsertProgramme(requestSetProgramme.degreeID, requestSetProgramme.programmeName, requestSetProgramme.facultyID, requestSetProgramme.programmeTypeID, requestSetProgramme.programmeDuration, requestSetProgramme.coordinator, requestSetProgramme.stipulatedPeriod, requestSetProgramme.programmeDescription, requestSetProgramme.programmeOrder, requestSetProgramme.activated, requestSetProgramme.addUser)
                                    select programme;

                foreach (var response in insertProgramme)
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

        public bool UpdateProgramme(COM.Programme requestSetProgramme)
        {
            logger.Info("UpdateProgramme");
            EntityConnection entityConnection = new EntityConnection();

            try
            {
                var updateProgramme = from programme in entityConnection.dbclayOMSDataContext.UpdateProgramme(requestSetProgramme.programmeID,requestSetProgramme.degreeID, requestSetProgramme.programmeName, requestSetProgramme.facultyID, requestSetProgramme.programmeTypeID, requestSetProgramme.programmeDuration, requestSetProgramme.coordinator, requestSetProgramme.stipulatedPeriod, requestSetProgramme.programmeDescription, requestSetProgramme.programmeOrder, requestSetProgramme.activated, requestSetProgramme.updateUser)
                                    select programme;

                foreach (var response in updateProgramme)
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

        public COM.Programme FetchProgramme(COM.Programme requestSetProgramme)
        {
            logger.Info("FetchProgramme");
            EntityConnection entityConnection = new EntityConnection();
            COM.Programme responseGetProgramme = new COM.Programme();

            try
            {
                var fetchProgramme = from programme in entityConnection.dbclayOMSDataContext.FetchProgramme(requestSetProgramme.programmeID)
                                   select programme;

                foreach (var response in fetchProgramme)
                {
                    responseGetProgramme.programmeName = response.Programme;
                    responseGetProgramme.programmeID = response.ProgrammeID;
                    responseGetProgramme.programmeTypeID = response.ProgrammeTypeID;
                    responseGetProgramme.programmeType = response.ProgrammeType;
                    responseGetProgramme.programmeDescription = response.ProgrammeDescription;
                    responseGetProgramme.programmeDuration = response.ProgrammeDuration;
                    responseGetProgramme.programmeOrder = response.ProgrammeOrder;
                    responseGetProgramme.stipulatedPeriod = response.StipulatedPeriod;                  
                    responseGetProgramme.updateUser = response.UpdateUser;
                    responseGetProgramme.activated = response.Activated;
                    responseGetProgramme.updateDate = response.UpdateDate;
                }

                return responseGetProgramme;
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
                return responseGetProgramme;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetProgramme;
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

        public List<COM.Programme> GetProgramme(COM.Programme requestSetProgramme)
        {
            logger.Info("GetProgramme");
            EntityConnection entityConnection = new EntityConnection();
            List<COM.Programme> responseGetProgramme = new List<COM.Programme>();

            try
            {
                var getProgramme = from programme in entityConnection.dbclayOMSDataContext.GetProgramme(requestSetProgramme.programmeName, requestSetProgramme.degree, requestSetProgramme.programmeType, requestSetProgramme.faculty, requestSetProgramme.activated)
                                 select programme;

                foreach (var response in getProgramme)
                {
                    responseGetProgramme.Add(new COM.Programme
                    {
                        programmeName = response.Programme,
                        programmeID = response.ProgrammeID,
                        degree = response.Degree ,
                        degreeID = response.DegreeID ,
                        programmeType = response.ProgrammeType ,
                        programmeTypeID = response.ProgrammeTypeID ,
                        programmeDescription = response.ProgrammeDescription ,
                        programmeDuration = response.ProgrammeDuration ,
                        programmeOrder = response.ProgrammeOrder ,
                        activated = response.Activated,
                        updateDate = response.UpdateDate,
                        updateUser = response.UpdateUser
                    });
                }

                return responseGetProgramme;
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
                return responseGetProgramme;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetProgramme;
            }
            finally
            {
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    // entityConnection.dbclayOMSDataContext.Transaction.Dispose();
                    entityConnection.dbclayOMSDataContext.Connection.Dispose();
                    entityConnection.dbclayOMSDataContext.Connection.Close();
                    entityConnection.dbclayOMSDataContext.Dispose();
                }
            }
        }

    }
}
