using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace SpotWPF {
    internal static class SQLHulp {
        internal static SqlParameter gParInit(string pNaam, int pWaarde) {
            SqlParameter gParInitRet = default;
            var lPar = new SqlParameter();
            lPar.ParameterName = pNaam;
            lPar.SqlDbType = SqlDbType.Int;
            lPar.Value = pWaarde;
            gParInitRet = lPar;
            return gParInitRet;
        }

        internal static SqlParameter gParInit(string pNaam, int pWaarde, int pNullWaarde) {
            SqlParameter gParInitRet = default;
            var lPar = new SqlParameter();
            lPar.ParameterName = pNaam;
            lPar.SqlDbType = SqlDbType.Int;
            if (pWaarde == pNullWaarde) {
                lPar.Value = DBNull.Value;
            } else {
                lPar.Value = pWaarde;
            }

            gParInitRet = lPar;
            return gParInitRet;
        }

        internal static SqlParameter gParInit(string pNaam, long pWaarde) {
            SqlParameter gParInitRet = default;
            var lPar = new SqlParameter();
            lPar.ParameterName = pNaam;
            lPar.SqlDbType = SqlDbType.BigInt;
            lPar.Value = pWaarde;
            gParInitRet = lPar;
            return gParInitRet;
        }

        internal static SqlParameter gParInit(string pNaam, string pWaarde) {
            SqlParameter gParInitRet = default;
            var lPar = new SqlParameter();
            lPar.ParameterName = pNaam;
            lPar.SqlDbType = SqlDbType.NVarChar;
            lPar.Value = pWaarde;
            gParInitRet = lPar;
            return gParInitRet;
        }

        internal static SqlParameter gParInit(string pNaam, string pWaarde, string pNullWaarde) {
            SqlParameter gParInitRet = default;
            var lPar = new SqlParameter();
            lPar.ParameterName = pNaam;
            lPar.SqlDbType = SqlDbType.NVarChar;
            if ((pWaarde ?? "") == (pNullWaarde ?? "")) {
                lPar.Value = DBNull.Value;
            } else {
                lPar.Value = pWaarde;
            }

            gParInitRet = lPar;
            return gParInitRet;
        }

        internal static SqlParameter gParInit(string pNaam, bool pWaarde) {
            SqlParameter gParInitRet = default;
            var lPar = new SqlParameter();
            lPar.ParameterName = pNaam;
            lPar.SqlDbType = SqlDbType.Bit;
            lPar.Value = pWaarde;
            gParInitRet = lPar;
            return gParInitRet;
        }

        internal static SqlParameter gParInit(string pNaam, DateTime pWaarde) {
            SqlParameter gParInitRet = default;
            var lPar = new SqlParameter();
            lPar.ParameterName = pNaam;
            lPar.SqlDbType = SqlDbType.SmallDateTime;
            lPar.Value = pWaarde;
            gParInitRet = lPar;
            return gParInitRet;
        }

        internal static SqlParameter gParInit(string pNaam, DateTime pWaarde, bool pNull) {
            SqlParameter gParInitRet = default;
            var lPar = new SqlParameter();
            lPar.ParameterName = pNaam;
            lPar.SqlDbType = SqlDbType.DateTime;
            if (pNull) {
                lPar.Value = DBNull.Value;
            } else {
                lPar.Value = pWaarde;
            }

            gParInitRet = lPar;
            return gParInitRet;
        }
    }
}
