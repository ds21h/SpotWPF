using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace SpotWPF {
    internal sealed class Data {
        private static Data mInstance = new Data();

        static Data() { }
        private Data() { }

        internal static Data getInstance {
            get {
                return mInstance; 
            } 
        }

        private const string cConnStr = "Persist Security Info=False;Integrated Security=SSPI;database=Spotz;server=LAPTOP-E6GEPR5E";

        internal int xGetNumberSpots() {
            SqlDataReader lRdr;
            int lNumberSpots = 0;

            using (var lComm = new SqlCommand())
            using (var lConn = new SqlConnection(cConnStr)) {
                lConn.Open();
                lComm.Connection = lConn;
                lComm.CommandText = "GetNumberSpots";
                lComm.CommandType = CommandType.StoredProcedure;
                try {
                    lRdr = lComm.ExecuteReader();
                    if (lRdr.HasRows) {
                        if (lRdr.Read()) {
                            lNumberSpots = lRdr.GetInt32(0);
                        }
                    }

                    lRdr.Close();
                } catch (Exception) {
                }
            }

            return lNumberSpots;
        }

        internal List<SpotData> xGetSpots(long pMaxSpotNumber) {
            SqlParameter lParMax;
            SqlDataReader lRdr;
            List<SpotData> lSpots;
            SpotData lSpot;
            string lArticleId;
            long lArticleNumber;
            DateTime lCreated;
            string lPoster;
            string lTag;
            string lTitle;
            long lSize;
            int lCategory;
            string lSubCategories;

            lSpots = new List<SpotData>();
            lParMax = SQLHulp.gParInit("@MaxSpotNumber", pMaxSpotNumber);
            using (var lComm = new SqlCommand())
            using (var lConn = new SqlConnection(cConnStr)) {
                lComm.Parameters.Add(lParMax);
                lConn.Open();
                lComm.Connection = lConn;
                lComm.CommandText = "GetSpotsLimited";
                lComm.CommandType = CommandType.StoredProcedure;
                try {
                    lRdr = lComm.ExecuteReader();
                    if (lRdr.HasRows) {
                        while (lRdr.Read()) {
                            lArticleId = lRdr.GetString(0);
                            lArticleNumber = lRdr.GetInt64(1);
                            lCreated = lRdr.GetDateTime(2);
                            lPoster = lRdr.GetString(3);
                            lTag = lRdr.GetString(4);
                            lTitle = lRdr.GetString(5);
                            lSize = lRdr.GetInt64(6);
                            lCategory = lRdr.GetInt32(7);
                            lSubCategories = lRdr.GetString(8);

                            lSpot = new SpotData(lArticleId, lArticleNumber, lCreated, lPoster, lTag, lTitle, lSize, lCategory, lSubCategories);
                            lSpots.Add(lSpot);
                        }
                    }

                    lRdr.Close();
                } catch (Exception pExc) {
                    lSpots = null;
                }
            }

            return lSpots;
        }
        internal List<SpotData> xGetSpotsBlock(int pOffset, int pLimit) {
            SqlParameter lParOffset;
            SqlParameter lParLimit;
            SqlDataReader lRdr;
            List<SpotData> lSpots;
            SpotData lSpot;
            string lArticleId;
            long lArticleNumber;
            DateTime lCreated;
            string lPoster;
            string lTag;
            string lTitle;
            long lSize;
            int lCategory;
            string lSubCategories;

            lSpots = new List<SpotData>();
            lParOffset = SQLHulp.gParInit("@Offset", pOffset);
            lParLimit = SQLHulp.gParInit("@Limit", pLimit);
            using (var lComm = new SqlCommand())
            using (var lConn = new SqlConnection(cConnStr)) {
                lComm.Parameters.Add(lParOffset);
                lComm.Parameters.Add(lParLimit);
                lConn.Open();
                lComm.Connection = lConn;
                lComm.CommandText = "GetSpots";
                lComm.CommandType = CommandType.StoredProcedure;
                try {
                    lRdr = lComm.ExecuteReader();
                    if (lRdr.HasRows) {
                        while (lRdr.Read()) {
                            lArticleId = lRdr.GetString(0);
                            lArticleNumber = lRdr.GetInt64(1);
                            lCreated = lRdr.GetDateTime(2);
                            lPoster = lRdr.GetString(3);
                            lTag = lRdr.GetString(4);
                            lTitle = lRdr.GetString(5);
                            lSize = lRdr.GetInt64(6);
                            lCategory = lRdr.GetInt32(7);
                            lSubCategories = lRdr.GetString(8);

                            lSpot = new SpotData(lArticleId, lArticleNumber, lCreated, lPoster, lTag, lTitle, lSize, lCategory, lSubCategories);
                            lSpots.Add(lSpot);
                        }
                    }

                    lRdr.Close();
                } catch (Exception pExc) {
                    lSpots = null;
                }
            }

            return lSpots;
        }

        internal Server xGetServer(string pName) {
            SqlParameter lParName;
            SqlDataReader lRdr;
            Server lServer = null;
            int lId;
            string lName;
            string lReader;
            int lPort;
            bool lSSL;
            string lUserId;
            string lPassWord;
            long lHighSpotId;

            lParName = SQLHulp.gParInit("@Name", pName);
            using (var lComm = new SqlCommand())
            using (var lConn = new SqlConnection(cConnStr)) {
                lComm.Parameters.Add(lParName);
                lConn.Open();
                lComm.Connection = lConn;
                lComm.CommandText = "GetServer";
                lComm.CommandType = CommandType.StoredProcedure;
                try {
                    lRdr = lComm.ExecuteReader();
                    if (lRdr.HasRows) {
                        if (lRdr.Read()) {
                            lId = lRdr.GetInt32(0);
                            lName = lRdr.GetString(1);
                            lReader = lRdr.GetString(2);
                            lPort = lRdr.GetInt32(3);
                            lSSL = lRdr.GetBoolean(4);
                            lUserId = lRdr.GetString(5);
                            lPassWord = lRdr.GetString(6);
                            lHighSpotId = lRdr.GetInt64(7);

                            lServer = new Server(lId, lName, lReader, lPort, lSSL, lUserId, lPassWord, lHighSpotId);
                        }
                    }

                    lRdr.Close();
                } catch (Exception) {
                }
            }

            return lServer;
        }

        internal int xUpdateServer(Server pServer) {
            SqlParameter lParId;
            SqlParameter lParName;
            SqlParameter lParReader;
            SqlParameter lParPort;
            SqlParameter lParSSL;
            SqlParameter lParUserId;
            SqlParameter lParPassword;
            SqlParameter lParHighSpotId;
            int lResult;

            lParId = SQLHulp.gParInit("@Id", pServer.xId);
            lParName = SQLHulp.gParInit("@Name", pServer.xName);
            lParReader = SQLHulp.gParInit("@Reader", pServer.xReader);
            lParPort = SQLHulp.gParInit("@Port", pServer.xPort);
            lParSSL = SQLHulp.gParInit("@SSL", pServer.xSSL);
            lParUserId = SQLHulp.gParInit("@UserId", pServer.xUserId);
            lParPassword = SQLHulp.gParInit("@Password", pServer.xPassWord);
            lParHighSpotId = SQLHulp.gParInit("@HighSpotId", pServer.xHighSpotId);
            using (var lComm = new SqlCommand())
            using (var lConn = new SqlConnection(cConnStr)) {
                lComm.Parameters.Add(lParId);
                lComm.Parameters.Add(lParName);
                lComm.Parameters.Add(lParReader);
                lComm.Parameters.Add(lParPort);
                lComm.Parameters.Add(lParSSL);
                lComm.Parameters.Add(lParUserId);
                lComm.Parameters.Add(lParPassword);
                lComm.Parameters.Add(lParHighSpotId);
                lConn.Open();
                lComm.Connection = lConn;
                lComm.CommandText = "UpdateServer";
                lComm.CommandType = CommandType.StoredProcedure;
                try {
                    lComm.ExecuteNonQuery();
                    lResult = 0;
                } catch (Exception) {
                    lResult = 9;
                }
            }

            return lResult;
        }

        internal int xStoreSpot(SpotData pSpot) {
            SqlParameter lParArticleId;
            SqlParameter lParArticleNumber;
            SqlParameter lParCreated;
            SqlParameter lParPoster;
            SqlParameter lParTag;
            SqlParameter lParTitle;
            SqlParameter lParSize;
            SqlParameter lParCategory;
            SqlParameter lParSubCategories;
            int lResult;

            lParArticleId = SQLHulp.gParInit("@ArticleId", pSpot.xArticleId);
            lParArticleNumber = SQLHulp.gParInit("@ArticleNumber", pSpot.xArticleNumber);
            lParCreated = SQLHulp.gParInit("@Created", pSpot.xCreated);
            lParPoster = SQLHulp.gParInit("@Poster", pSpot.xPoster);
            lParTag = SQLHulp.gParInit("@Tag", pSpot.xTag);
            lParTitle = SQLHulp.gParInit("@Title", pSpot.xTitle);
            lParSize = SQLHulp.gParInit("@Size", pSpot.xSize);
            lParCategory = SQLHulp.gParInit("@Category", pSpot.xCategory);
            lParSubCategories = SQLHulp.gParInit("@SubCategories", pSpot.xSubCategories);
            using (var lComm = new SqlCommand())
            using (var lConn = new SqlConnection(cConnStr)) {
                lComm.Parameters.Add(lParArticleId);
                lComm.Parameters.Add(lParArticleNumber);
                lComm.Parameters.Add(lParCreated);
                lComm.Parameters.Add(lParPoster);
                lComm.Parameters.Add(lParTag);
                lComm.Parameters.Add(lParTitle);
                lComm.Parameters.Add(lParSize);
                lComm.Parameters.Add(lParCategory);
                lComm.Parameters.Add(lParSubCategories);
                lConn.Open();
                lComm.Connection = lConn;
                lComm.CommandText = "StoreSpot";
                lComm.CommandType = CommandType.StoredProcedure;
                lResult = 0;
                try {
                    lComm.ExecuteNonQuery();
                } catch (Exception pExc) {
                    if (pExc.HResult == -2146232060) {
                        lResult = 1;
                    } else {
                        lResult = 9;
                    }
                }
            }

            return lResult;

        }

        internal int xStoreComments(string pFileName, string pFormatFileName) {
            SqlParameter lParFileName;
            SqlParameter lParFormatFileName;
            int lResult;

            lParFileName = SQLHulp.gParInit("@File", pFileName);
            lParFormatFileName = SQLHulp.gParInit("@Format", pFormatFileName);
            using (var lComm = new SqlCommand())
            using (var lConn = new SqlConnection(cConnStr)) {
                lComm.Parameters.Add(lParFileName);
                lComm.Parameters.Add(lParFormatFileName);
                lConn.Open();
                lComm.Connection = lConn;
                lComm.CommandText = "BulkInsertComments";
                lComm.CommandType = CommandType.StoredProcedure;
                lResult = 0;
                try {
                    lComm.ExecuteNonQuery();
                } catch (Exception pExc) {
                    if (pExc.HResult == -2146232060) {
                        lResult = 1;
                    } else {
                        lResult = 9;
                    }
                }
            }

            return lResult;

        }
    }
}
