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
    public class ExaminationGradeDAL
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ExaminationGradeDAL));

        public ExaminationGradeDAL()
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

        public bool InsertExaminationGrade(COM.ExaminationGrade requestSetExaminationGrade)
        {
            logger.Info("InsertExaminationGrade");
            EntityConnection entityConnection = new EntityConnection();

            try
            {
                var insertExaminationGrade = from examinationGrade in entityConnection.dbclayOMSDataContext.InsertExaminationGrade(requestSetExaminationGrade.gradeID, requestSetExaminationGrade.grade, requestSetExaminationGrade.percentageFrom, requestSetExaminationGrade.percentageTo, requestSetExaminationGrade.description, requestSetExaminationGrade.academicYear, requestSetExaminationGrade.programmeTypeID, requestSetExaminationGrade.activated, requestSetExaminationGrade.addUser)
                                   select examinationGrade;

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

        public bool UpdateExaminationGrade(COM.ExaminationGrade requestSetExaminationGrade)
        {
            logger.Info("UpdateExaminationGrade");
            EntityConnection entityConnection = new EntityConnection();

            try
            {
                var updateExaminationGrade = from examinationGrade in entityConnection.dbclayOMSDataContext.UpdateExaminationGrade(requestSetExaminationGrade.gradeID, requestSetExaminationGrade.grade, requestSetExaminationGrade.percentageFrom, requestSetExaminationGrade.percentageTo, requestSetExaminationGrade.description,requestSetExaminationGrade.academicYear,requestSetExaminationGrade.programmeTypeID,requestSetExaminationGrade.activated,requestSetExaminationGrade.updateUser )
                                   select examinationGrade;

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

        public COM.ExaminationGrade FetchExaminationGrade(COM.ExaminationGrade requestSetExaminationGrade)
        {
            logger.Info("FetchExaminationGrade");
            EntityConnection entityConnection = new EntityConnection();
            COM.ExaminationGrade responseGetExaminationGrade = new COM.ExaminationGrade();

            try
            {
                var fetchExaminationGrade = from examinationGrade in entityConnection.dbclayOMSDataContext.FetchExaminationGrade(requestSetExaminationGrade.examinationGradeID)
                                  select examinationGrade;

                foreach (var response in fetchExaminationGrade)
                {
                    responseGetExaminationGrade.grade = response.Grade;
                    responseGetExaminationGrade.gradeID = response.GradeID;
                    responseGetExaminationGrade.programmeType = response.ProgrammeType;
                    responseGetExaminationGrade.programmeTypeID = response.ProgrammeTypeID;
                    responseGetExaminationGrade.updateUser = response.UpdateUser;
                    responseGetExaminationGrade.activated = response.Activated;
                    responseGetExaminationGrade.updateDate = response.UpdateDate;
                    requestSetExaminationGrade.percentageFrom = response.PercentageFrom;
                    responseGetExaminationGrade.percentageTo = response.PercentageTo;
                    responseGetExaminationGrade.description = response.Description;
                    responseGetExaminationGrade.academicYear = response.AcademicYear
                }

                return responseGetExaminationGrade;
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
                return responseGetExaminationGrade;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetExaminationGrade;
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

        public List<COM.ExaminationGrade> GetExaminationGrade(COM.ExaminationGrade requestSetExaminationGrade)
        {
            logger.Info("GetExaminationGrade");
            EntityConnection entityConnection = new EntityConnection();
            List<COM.ExaminationGrade> responseGetExaminationGrade = new List<COM.ExaminationGrade>();

            try
            {
                var getExaminationGrade = from examinationGrade in entityConnection.dbclayOMSDataContext.GetExaminationGrade(requestSetExaminationGrade.examinationGrade, requestSetExaminationGrade.state, requestSetExaminationGrade.country)
                                  select examinationGrade;

                foreach (var response in getExaminationGrade)
                {
                    responseGetExaminationGrade.Add(new COM.ExaminationGrade
                    {
                        grade = response.Grade,
                        gradeID = response.GradeID,
                        academicYear = response.AcademicYear,
                        description = response.Description,
                        percentageFrom = response.PercentageFrom,
                        percentageTo = response.PercentageTo,
                        programmeType = response.ProgrammeType,
                        programmeTypeID = response.ProgrammeTypeID
                    });
                }

                return responseGetExaminationGrade;
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
                return responseGetExaminationGrade;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetExaminationGrade;
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
