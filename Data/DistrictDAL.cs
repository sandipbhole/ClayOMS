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
    public class DistrictDAL
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DistrictDAL));

        public DistrictDAL()
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

        public List<COM.District> GetDistrict(COM.District requestSetDistrict)
        {
            logger.Info("GetDistrict");
            EntityConnection entityConnection = new EntityConnection();
            List<COM.District> responseGetDistrict = new List<COM.District>();

            try
            {
                var getDistrict = from district in entityConnection.dbclayOMSDataContext.GetDistrict(requestSetDistrict.district, requestSetDistrict.state, requestSetDistrict.country)
                                    select district;

                foreach (var response in getDistrict)
                {
                    responseGetDistrict.Add(new COM.District
                    {
                       district = response.District ,
                       districtID = response.DistrictID,
                       state = response.State ,
                       stateID = response.StateID                        
                    });
                }

                return responseGetDistrict;
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
                return responseGetDistrict;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetDistrict;
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

        public List<COM.State> GetState(COM.State requestSetState)
        {
            logger.Info("GetState");
            EntityConnection entityConnection = new EntityConnection();
            List<COM.State> responseGetState = new List<COM.State>();

            try
            {
                var getState = from state in entityConnection.dbclayOMSDataContext.GetState( requestSetState.state, requestSetState.country)
                                  select state;

                foreach (var response in getState)
                {
                    responseGetState.Add(new COM.State
                    {
                        stateID = response.StateID,
                        state = response.State ,
                        country = response.Country ,
                        countryID = response.CountryID 
                    });
                }

                return responseGetState;
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
                return responseGetState;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetState;
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

        public List<COM.Country> GetCountry(COM.Country requestSetCountry)
        {
            logger.Info("GetCountry");
            EntityConnection entityConnection = new EntityConnection();
            List<COM.Country> responseGetCountry = new List<COM.Country>();

            try
            {
                var getCountry = from country in entityConnection.dbclayOMSDataContext.GetCountry(requestSetCountry.country)
                               select country;

                foreach (var response in getCountry)
                {
                    responseGetCountry.Add(new COM.Country
                    {
                        country = response.Country,
                        countryID = response.CountryID 
                    });
                }

                return responseGetCountry;
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
                return responseGetCountry;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (entityConnection.dbclayOMSDataContext.Connection.State == ConnectionState.Open)
                {
                    entityConnection.dbclayOMSDataContext.Transaction.Rollback();
                }
                return responseGetCountry;
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
