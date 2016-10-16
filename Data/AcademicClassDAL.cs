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
    public class AcademicClassDAL
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(AcademicClassDAL));

        public AcademicClassDAL()
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

        public bool InsertClass(COM.AcademicClass requestSetAcademicClass)
        {
            logger.Info("InsertClass");
            EntityConnection entityConnection = new EntityConnection();
            string error;
            try
            {
                var insertAcademicClass = from academicClass in entityConnection.dbclayOMSDataContext.InsertClass(requestSetAcademicClass.classCode, requestSetAcademicClass.academicClass , requestSetAcademicClass.programmeID , requestSetAcademicClass.yearPosition, requestSetAcademicClass.section, requestSetAcademicClass.activated, requestSetAcademicClass.addUser)
                                          select academicClass;

                foreach (var response in insertAcademicClass)
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

        public bool UpdateClass(COM.AcademicClass requestSetAcademicClass)
        {
            logger.Info("UpdateClass");
            EntityConnection entityConnection = new EntityConnection();

            try
            {
                var updateAcademicClass = from academicClass in entityConnection.dbclayOMSDataContext.UpdateClass(requestSetAcademicClass.classID,requestSetAcademicClass.classCode, requestSetAcademicClass.academicClass, requestSetAcademicClass.programmeID, requestSetAcademicClass.yearPosition, requestSetAcademicClass.section, requestSetAcademicClass.activated, requestSetAcademicClass.updateUser )
                                          select academicClass;

                foreach (var response in updateAcademicClass)
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

        public COM.AcademicClass FetchClass(COM.AcademicClass requestSetAcademicClass)
        {
            logger.Info("FetchClass");
            EntityConnection entityConnection = new EntityConnection();
            COM.AcademicClass responseGetAcademicClass = new COM.AcademicClass();

            try
            {
                var fetchAcademicClass = from academicClass in entityConnection.dbclayOMSDataContext.FetchClass(requestSetAcademicClass.classID)
                                         select academicClass;

                foreach (var response in fetchAcademicClass)
                {
                    responseGetAcademicClass.classID = response.ClassID;
                    responseGetAcademicClass.academicClass = response.Class;
                    responseGetAcademicClass.classCode = response.ClassCode;
                    responseGetAcademicClass.section = response.Section;
                    responseGetAcademicClass.yearPosition = response.YearPosition;
                    responseGetAcademicClass.programme = response.Programme;
                    responseGetAcademicClass.programmeID = response.ProgrammeID;
                    responseGetAcademicClass.updateUser = response.UpdateUser;
                    responseGetAcademicClass.activated = response.Activated;
                    responseGetAcademicClass.updateDate = response.UpdateDate;
                }

                return responseGetAcademicClass;
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
                return responseGetAcademicClass;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetAcademicClass;
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

        public List<COM.AcademicClass> GetClass(COM.AcademicClass requestSetAcademicClass)
        {
            logger.Info("GetClass");
            EntityConnection entityConnection = new EntityConnection();
            List<COM.AcademicClass> responseGetAcademicClass = new List<COM.AcademicClass>();

            try
            {
                var getAcademicClass = from academicClass in entityConnection.dbclayOMSDataContext.GetClass(requestSetAcademicClass.academicClass,requestSetAcademicClass.activated )
                                       select academicClass;

                foreach (var response in getAcademicClass)
                {
                    responseGetAcademicClass.Add(new COM.AcademicClass
                    {

                        academicClass = response.Class ,
                        classCode = response.ClassCode ,
                        classID = response.ClassID,
                        section = response.Section ,
                        yearPosition = response.YearPosition ,
                        programmeID = response.ProgrammeID ,
                        programme = response.Programme ,
                        activated = response.Activated,
                        updateDate = response.UpdateDate,
                        updateUser = response.UpdateUser
                    });
                }

                return responseGetAcademicClass;
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
                return responseGetAcademicClass;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetAcademicClass;
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
