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
    public class FacultyDAL
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(FacultyDAL));

        public FacultyDAL()
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

        public bool InsertFaculty(COM.Faculty requestSetFaculty)
        {
            logger.Info("InsertFaculty");
            EntityConnection entityConnection = new EntityConnection();

            try
            {
                var insertFaculty = from faculty in entityConnection.dbclayOMSDataContext.InsertFaculty(requestSetFaculty.code,requestSetFaculty.facultyName,requestSetFaculty.yearOfEstablishment,requestSetFaculty.dean, requestSetFaculty.activated, requestSetFaculty.addUser)
                                             select faculty;

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

        public bool UpdateFaculty(COM.Faculty requestSetFaculty)
        {
            logger.Info("UpdateFaculty");
            EntityConnection entityConnection = new EntityConnection();

            try
            {
                var updateFaculty = from faculty in entityConnection.dbclayOMSDataContext.UpdateFaculty(requestSetFaculty.facultyID,requestSetFaculty.code, requestSetFaculty.facultyName, requestSetFaculty.yearOfEstablishment, requestSetFaculty.dean, requestSetFaculty.activated, requestSetFaculty.updateUser)
                                             select faculty;

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

        public COM.Faculty FetchFaculty(COM.Faculty requestSetFaculty)
        {
            logger.Info("FetchFaculty");
            EntityConnection entityConnection = new EntityConnection();
            COM.Faculty responseGetFaculty = new COM.Faculty();

            try
            {
                var fetchFaculty = from faculty in entityConnection.dbclayOMSDataContext.FetchFaculty(requestSetFaculty.facultyID)
                                            select faculty;

                foreach (var response in fetchFaculty)
                {
                    responseGetFaculty.facultyName = response.FacultyName;
                    responseGetFaculty.facultyID = response.FacultyID;
                    responseGetFaculty.yearOfEstablishment = response.YearOfEstablishment;
                    responseGetFaculty.dean = response.Dean;
                    responseGetFaculty.updateUser = response.UpdateUser;
                    responseGetFaculty.activated = response.Activated;
                    responseGetFaculty.updateDate = response.UpdateDate;                    
                }

                return responseGetFaculty;
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
                return responseGetFaculty;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetFaculty;
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

        public List<COM.Faculty> GetFaculty(COM.Faculty requestSetFaculty)
        {
            logger.Info("GetFaculty");
            EntityConnection entityConnection = new EntityConnection();
            List<COM.Faculty> responseGetFaculty = new List<COM.Faculty>();

            try
            {
                var getFaculty = from faculty in entityConnection.dbclayOMSDataContext.GetFaculty(requestSetFaculty.facultyName, requestSetFaculty.dean, requestSetFaculty.activated)
                                          select faculty;

                foreach (var response in getFaculty)
                {
                    responseGetFaculty.Add(new COM.Faculty
                    {
                        facultyName = response.FacultyName,
                        facultyID = response.FacultyID,
                        dean = response.Dean,
                        yearOfEstablishment = response.YearOfEstablishment,
                        activated = response.Activated,
                        updateDate = response.UpdateDate,
                        updateUser = response.UpdateUser
                    });
                }

                return responseGetFaculty;
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
                return responseGetFaculty;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetFaculty;
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
