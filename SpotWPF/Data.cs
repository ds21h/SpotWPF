using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace SpotWPF {
    internal sealed class Data {
        private static readonly Data mInstance = new();

        static Data() { }
        private Data() { }

        internal static Data getInstance {
            get {
                return mInstance; 
            } 
        }

        private const string cConnStr = "Persist Security Info=False;Integrated Security=SSPI;database=Spotz;server=LAPTOP-E6GEPR5E";

        internal async Task<int> xGetNumberSpotsAsync() {
            SqlDataReader lRdr;
            int lNumberSpots = 0;

            using (var lComm = new SqlCommand())
            using (var lConn = new SqlConnection(cConnStr)) {
                await lConn.OpenAsync().ConfigureAwait(false);
                lComm.Connection = lConn;
                lComm.CommandText = "GetNumberSpots";
                lComm.CommandType = CommandType.StoredProcedure;
                try {
                    lRdr = await lComm.ExecuteReaderAsync().ConfigureAwait(false);
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

        internal int xSetView(string pFilter) {
            SqlParameter lFilter;
            int lResult;

            lFilter = SQLHulp.gParInit("@Filter", pFilter);
            using (var lComm = new SqlCommand())
            using (var lConn = new SqlConnection(cConnStr)) {
                lComm.Parameters.Add(lFilter);
                lConn.Open();
                lComm.Connection = lConn;
                lComm.CommandText = "SetView";
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
            bool lEro;

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
                            if (lRdr.IsDBNull(9)) {
                                lEro = false;
                            } else {
                                lEro = lRdr.GetBoolean(9);
                            }

                            lSpot = new SpotData(lArticleId, lArticleNumber, lCreated, lPoster, lTag, lTitle, lSize, lCategory, lSubCategories, lEro);
                            lSpots.Add(lSpot);
                        }
                    }

                    lRdr.Close();
                } catch (Exception) {
                    lSpots = null;
                }
            }

            return lSpots;
        }

        internal int xDeleteSpot(string pArticleId) {
            SqlParameter lArticleId;
            int lResult;

            lArticleId = SQLHulp.gParInit("@ArticleId", pArticleId);
            using (var lComm = new SqlCommand())
            using (var lConn = new SqlConnection(cConnStr)) {
                lComm.Parameters.Add(lArticleId);
                lConn.Open();
                lComm.Connection = lConn;
                lComm.CommandText = "DeleteSpot";
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
            long lHighSeenId;
            long lHighCommentId;

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
                            lHighSeenId = lRdr.GetInt64(8);
                            lHighCommentId = lRdr.GetInt64(9);

                            lServer = new Server(lId, lName, lReader, lPort, lSSL, lUserId, lPassWord, lHighSpotId, lHighSeenId, lHighCommentId);
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
            SqlParameter lParHighSeenId;
            SqlParameter lParHighCommentId;
            int lResult;

            lParId = SQLHulp.gParInit("@Id", pServer.xId);
            lParName = SQLHulp.gParInit("@Name", pServer.xName);
            lParReader = SQLHulp.gParInit("@Reader", pServer.xReader);
            lParPort = SQLHulp.gParInit("@Port", pServer.xPort);
            lParSSL = SQLHulp.gParInit("@SSL", pServer.xSSL);
            lParUserId = SQLHulp.gParInit("@UserId", pServer.xUserId);
            lParPassword = SQLHulp.gParInit("@Password", pServer.xPassWord);
            lParHighSpotId = SQLHulp.gParInit("@HighSpotId", pServer.xHighSpotId);
            lParHighSeenId = SQLHulp.gParInit("@HighSeenId", pServer.xHighSeenId);
            lParHighCommentId = SQLHulp.gParInit("@HighCommentId", pServer.xHighCommentId);
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
                lComm.Parameters.Add(lParHighSeenId);
                lComm.Parameters.Add(lParHighCommentId);
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
            SqlParameter lParEro;
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
            lParEro = SQLHulp.gParInit("@Ero", pSpot.xEro);
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
                lComm.Parameters.Add(lParEro);
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

        internal int xStoreComment(long pArticleNumber, string pSpotId, DateTime pCreated) {
            SqlParameter lParArticleNumber;
            SqlParameter lParSpotId;
            SqlParameter lParCreated;
            int lResult;

            lParArticleNumber = SQLHulp.gParInit("@ArticleNumber", pArticleNumber);
            lParSpotId = SQLHulp.gParInit("@SpotId", pSpotId);
            lParCreated = SQLHulp.gParInit("@Created", pCreated);
            using (var lComm = new SqlCommand())
            using (var lConn = new SqlConnection(cConnStr)) {
                lComm.Parameters.Add(lParArticleNumber);
                lComm.Parameters.Add(lParSpotId);
                lComm.Parameters.Add(lParCreated);
                lConn.Open();
                lComm.Connection = lConn;
                lComm.CommandText = "InsertComment";
                lComm.CommandType = CommandType.StoredProcedure;
                lResult = 0;
                try {
                    lComm.ExecuteNonQuery();
                } catch (Exception) {
                    lResult = 9;
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

        internal async Task<List<long>> xGetCommentsAsync(string pSpotId) {
            SqlParameter lParSpotId;
            SqlDataReader lRdr;
            List<long> lComments;
            long lCommentNumber;

            lComments = new List<long>();
            lParSpotId = SQLHulp.gParInit("@SpotId", pSpotId);
            using (var lComm = new SqlCommand())
            using (var lConn = new SqlConnection(cConnStr)) {
                lComm.Parameters.Add(lParSpotId);
                await lConn.OpenAsync().ConfigureAwait(false);
                lComm.Connection = lConn;
                lComm.CommandText = "GetComments";
                lComm.CommandType = CommandType.StoredProcedure;
                try {
                    lRdr = await lComm.ExecuteReaderAsync().ConfigureAwait(false);
                    if (lRdr.HasRows) {
                        while (lRdr.Read()) {
                            lCommentNumber = lRdr.GetInt64(0);

                            lComments.Add(lCommentNumber);
                        }
                    }

                    lRdr.Close();
                } catch (Exception) {
                    lComments = null;
                }
            }

            return lComments;
        }

        internal List<FilterEntry> xGetFilters() {
            SqlDataReader lRdr;
            List<FilterEntry> lFilters;
            FilterEntry lFilter;
            int lSeq;
            string lTitle;
            string lFilterString;

            lFilters = new List<FilterEntry>();
            using (var lComm = new SqlCommand())
            using (var lConn = new SqlConnection(cConnStr)) {
                lConn.Open();
                lComm.Connection = lConn;
                lComm.CommandText = "GetFilters";
                lComm.CommandType = CommandType.StoredProcedure;
                try {
                    lRdr = lComm.ExecuteReader();
                    if (lRdr.HasRows) {
                        while (lRdr.Read()) {
                            lSeq = lRdr.GetInt32(0);
                            lTitle = lRdr.GetString(1);
                            if (lRdr.IsDBNull(2)) {
                                lFilterString = "";
                            } else { 
                                lFilterString = lRdr.GetString(2);
                            }

                            lFilter = new FilterEntry(lSeq, lTitle, lFilterString);
                            lFilters.Add(lFilter);
                        }
                    }

                    lRdr.Close();
                } catch (Exception) {
                    lFilters = null;
                }
            }

            return lFilters;
        }
    }
}
