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
    public class ConcessionTypeDAL
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ConcessionTypeDAL));

        public ConcessionTypeDAL()
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

        public COM.ConcessionType FetchConcessionType(COM.ConcessionType requestSetConcessionType)
        {
            logger.Info("FetchConcessionType");
            EntityConnection entityConnection = new EntityConnection();
            COM.ConcessionType responseGetConcessionType = new COM.ConcessionType();

            try
            {
                var fetchConcessionType = from ConcessionType in entityConnection.dbclayOMSDataContext.FecthConcessionType(requestSetConcessionType.concessionTypeID)
                                       select ConcessionType;

                foreach (var response in fetchConcessionType)
                {
                    responseGetConcessionType.concessionType = response.ConcessionType;
                    responseGetConcessionType.concessionTypeID = response.ConcessionTypeID;
                    //responseGetConcessionType.updateUser = response.UpdateUser;
                    //responseGetConcessionType.activated = response.Activated;
                    //responseGetConcessionType.updateDate = response.UpdateDate;
                }

                return responseGetConcessionType;
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
                return responseGetConcessionType;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetConcessionType;
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


        public List<COM.ConcessionType> GetConcessionType(COM.ConcessionType requestSetConcessionType)
        {
            logger.Info("GetConcessionType");
            EntityConnection entityConnection = new EntityConnection();
            List<COM.ConcessionType> responseGetConcessionType = new List<COM.ConcessionType>();

            try
            {
                var getConcessionType = from ConcessionType in entityConnection.dbclayOMSDataContext.GetConcessionType(requestSetConcessionType.concessionType)
                                     select ConcessionType;

                foreach (var response in getConcessionType)
                {
                    responseGetConcessionType.Add(new COM.ConcessionType
                    {
                        concessionType = response.ConcessionType,
                        concessionTypeID = response.ConcessionTypeID,
                        //updateUser = response.UpdateUser,
                        //activated = response.Activated,
                        //updateDate = response.UpdateDate
                    });
                }

                return responseGetConcessionType;
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
                return responseGetConcessionType;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetConcessionType;
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
