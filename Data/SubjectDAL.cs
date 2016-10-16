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
    public class SubjectDAL
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SubjectDAL));

        public SubjectDAL()
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

        public bool InsertSubject(COM.Subject requestSetSubject)
        {
            logger.Info("InsertSubject");
            EntityConnection entityConnection = new EntityConnection();
            string error;
            try
            {
                var insertSubject = from academicSubject in entityConnection.dbclayOMSDataContext.InsertSubject(requestSetSubject.subjectCode, requestSetSubject.subject, requestSetSubject.departmentID, requestSetSubject.programmeTypeID, requestSetSubject.subjectTypeID, requestSetSubject.totalMark, requestSetSubject.batchFrom, requestSetSubject.batchNo, requestSetSubject.subjectMode, requestSetSubject.subjectDescription, requestSetSubject.activated, requestSetSubject.addUser)
                                          select academicSubject;

                foreach (var response in insertSubject)
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

        public bool UpdateSubject(COM.Subject requestSetSubject)
        {
            logger.Info("UpdateSubject");
            EntityConnection entityConnection = new EntityConnection();

            try
            {
                var updateSubject = from subject in entityConnection.dbclayOMSDataContext.UpdateSubject(requestSetSubject.subjectID,requestSetSubject.subjectCode, requestSetSubject.subject, requestSetSubject.departmentID, requestSetSubject.programmeTypeID, requestSetSubject.subjectTypeID, requestSetSubject.totalMark, requestSetSubject.batchFrom, requestSetSubject.batchNo, requestSetSubject.subjectMode, requestSetSubject.subjectDescription, requestSetSubject.activated, requestSetSubject.updateUser)
                                          select subject;

                foreach (var response in updateSubject)
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

        public COM.Subject FetchSubject(COM.Subject requestSetSubject)
        {
            logger.Info("FetchSubject");
            EntityConnection entityConnection = new EntityConnection();
            COM.Subject responseGetSubject = new COM.Subject();

            try
            {
                var fetchSubject = from academicSubject in entityConnection.dbclayOMSDataContext.FetchSubject(requestSetSubject.subjectID)
                                         select academicSubject;

                foreach (var response in fetchSubject)
                {
                    responseGetSubject.subject = response.Subject;
                    responseGetSubject.subjectCode = response.SubjectCode;
                    responseGetSubject.subjectDescription = response.SubjectCode;
                    responseGetSubject.subjectID = response.SubjectID;
                    responseGetSubject.subjectMode = response.SubjectMode;
                    responseGetSubject.subjectType = response.SubjectType;
                    responseGetSubject.subjectTypeID = response.SubjectTypeID;
                    responseGetSubject.totalMark = response.TotalMark;
                    responseGetSubject.batchFrom = response.BatchFrom;
                    responseGetSubject.batchNo = response.BatchNo;
                    responseGetSubject.department = response.Department;
                    responseGetSubject.departmentID = response.DepartmentID;
                    responseGetSubject.updateUser = response.UpdateUser;
                    responseGetSubject.activated = response.Activated;
                    responseGetSubject.updateDate = response.UpdateDate;
                }

                return responseGetSubject;
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
                return responseGetSubject;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetSubject;
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

        public List<COM.Subject> GetSubject(COM.Subject requestSetSubject)
        {
            logger.Info("GetSubject");
            EntityConnection entityConnection = new EntityConnection();
            List<COM.Subject> responseGetSubject = new List<COM.Subject>();

            try
            {
                var getSubject = from subject in entityConnection.dbclayOMSDataContext.GetSubject(requestSetSubject.subject, requestSetSubject.department, requestSetSubject.programmeType, requestSetSubject.subjectType, requestSetSubject.activated)
                                 select subject;

                foreach (var response in getSubject)
                {
                    responseGetSubject.Add(new COM.Subject
                    {
                        subject = response.Subject,
                        subjectCode = response.SubjectCode,
                        subjectDescription = response.SubjectDescription,
                        subjectID = response.SubjectID,
                        subjectTypeID = response.SubjectTypeID,
                        subjectType = response.SubjectType,
                        subjectMode = response.SubjectMode,
                        programmeType = response.ProgrammeType,
                        programmeTypeID = response.ProgrammeTypeID,
                        department = response.Department,
                        departmentID = response.DepartmentID,
                        activated = response.Activated,
                        updateDate = response.UpdateDate,
                        updateUser = response.UpdateUser
                    });
                }

                return responseGetSubject;
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
                return responseGetSubject;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetSubject;
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
