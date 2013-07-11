using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Reader
{
    class DatabaseWriter
    {
        public DatabaseWriter(string tableName, string inputFilePath)
        {
            Microsoft.Win32.SaveFileDialog sdg = new Microsoft.Win32.SaveFileDialog();
            sdg.FileName = "databaseName";
            sdg.DefaultExt = ".sqlite";
            sdg.Filter = "SQLite3 Database File (.sqlite)|*sqlite";

            string tableMeta = "(Timestamp INTEGER PRIMARY KEY, LHS INTEGER NOT NULL, LHX REAL, LHY REAL, LHZ REAL, LKS INTEGER NOT NULL, LKX REAL, LKY REAL, LKZ REAL, LAS INTEGER NOT NULL, LAX REAL, LAY REAL, LAZ REAL, LFS INTEGER NOT NULL, LFX REAL, LFY REAL, LFZ REAL, RHS INTEGER NOT NULL, RHX REAL, RHY REAL, RHZ REAL, RKS INTEGER NOT NULL, RKX REAL, RKY REAL, RKZ REAL, RAS INTEGER NOT NULL, RAX REAL, RAY REAL, RAZ REAL, RFS INTEGER NOT NULL, LKFX REAL, LKVG REAL, RKFX REAL, RKVG REAL)";
            Nullable<bool> saveResult = sdg.ShowDialog();
            if (saveResult == true)
            {
                string databaseName = sdg.FileName;
                string cs = string.Format("URI=file:{0}", databaseName);
                #region create table
                using (SQLiteConnection con = new SQLiteConnection(cs))
                {
                    con.Open();
                    using (SQLiteTransaction tr = con.BeginTransaction())
                    {
                        using (SQLiteCommand cmd = con.CreateCommand())
                        {
                            cmd.Transaction = tr;
                            cmd.CommandText = String.Format("CREATE TABLE {0}{1}", tableName, tableMeta);
                            cmd.ExecuteNonQuery();
                        }
                        tr.Commit();
                    }

                    con.Close();
                }
                #endregion

                #region create DataTable
                using (SQLiteConnection con = new SQLiteConnection(cs))
                {
                    con.Open();
                    using (SQLiteTransaction tr2 = con.BeginTransaction())
                    {
                        using (SQLiteCommand cmd = con.CreateCommand())
                        {
                            cmd.Transaction = tr2;
                            DataTable table = new DataTable(tableName);

                            #region create columns
                            table.Columns.Add("Timestamp", System.Type.GetType("System.Int32"));
                            //------------------------------------------------------------------
                            table.Columns.Add("LHS", System.Type.GetType("System.Int32"));
                            table.Columns.Add("LHX", System.Type.GetType("System.Double"));
                            table.Columns.Add("LHY", System.Type.GetType("System.Double"));
                            table.Columns.Add("LHZ", System.Type.GetType("System.Double"));
                            table.Columns.Add("LKS", System.Type.GetType("System.Int32"));
                            table.Columns.Add("LKX", System.Type.GetType("System.Double"));
                            table.Columns.Add("LKY", System.Type.GetType("System.Double"));
                            table.Columns.Add("LKZ", System.Type.GetType("System.Double"));
                            table.Columns.Add("LAS", System.Type.GetType("System.Int32"));
                            table.Columns.Add("LAX", System.Type.GetType("System.Double"));
                            table.Columns.Add("LAY", System.Type.GetType("System.Double"));
                            table.Columns.Add("LAZ", System.Type.GetType("System.Double"));
                            table.Columns.Add("LFS", System.Type.GetType("System.Int32"));
                            table.Columns.Add("LFX", System.Type.GetType("System.Double"));
                            table.Columns.Add("LFY", System.Type.GetType("System.Double"));
                            table.Columns.Add("LFZ", System.Type.GetType("System.Double"));
                            //-------------------------------------------------------------------
                            table.Columns.Add("RHS", System.Type.GetType("System.Int32"));
                            table.Columns.Add("RHX", System.Type.GetType("System.Double"));
                            table.Columns.Add("RHY", System.Type.GetType("System.Double"));
                            table.Columns.Add("RHZ", System.Type.GetType("System.Double"));
                            table.Columns.Add("RKS", System.Type.GetType("System.Int32"));
                            table.Columns.Add("RKX", System.Type.GetType("System.Double"));
                            table.Columns.Add("RKY", System.Type.GetType("System.Double"));
                            table.Columns.Add("RKZ", System.Type.GetType("System.Double"));
                            table.Columns.Add("RAS", System.Type.GetType("System.Int32"));
                            table.Columns.Add("RAX", System.Type.GetType("System.Double"));
                            table.Columns.Add("RAY", System.Type.GetType("System.Double"));
                            table.Columns.Add("RAZ", System.Type.GetType("System.Double"));
                            table.Columns.Add("RFS", System.Type.GetType("System.Int32"));
                            table.Columns.Add("RFX", System.Type.GetType("System.Double"));
                            table.Columns.Add("RFY", System.Type.GetType("System.Double"));
                            table.Columns.Add("RFZ", System.Type.GetType("System.Double"));
                            //-------------------------------------------------------------------
                            table.Columns.Add("LKFX", System.Type.GetType("System.Double"));
                            table.Columns.Add("LKVG", System.Type.GetType("System.Double"));
                            table.Columns.Add("RKFX", System.Type.GetType("System.Double"));
                            table.Columns.Add("RKVG", System.Type.GetType("System.Double"));

                            #endregion

                            var output = new List<DataList>();

                            foreach (var line in File.ReadAllLines(inputFilePath))
                            {
                                if (line.Length == 0)
                                {
                                    continue;
                                }
                                output.Add(new DataList(line.Split(',')));
                                Console.WriteLine("Line parsed");
                            }
                            
                            foreach (DataList outputLine in output)
                            {
                                DataRow row = table.NewRow();
                                row["Timestamp"] = outputLine.Frame;
                                row["LHS"] = outputLine.HlState;
                                row["LHX"] = outputLine.HLX;
                                row["LHY"] = outputLine.HLY;
                                row["LHZ"] = outputLine.HLZ;
                                row["LKS"] = outputLine.KlState;
                                row["LKX"] = outputLine.KLX;
                                row["LKY"] = outputLine.KLY;
                                row["LKZ"] = outputLine.KLZ;
                                row["LAS"] = outputLine.AlState;
                                row["LAX"] = outputLine.ALX;
                                row["LAY"] = outputLine.ALY;
                                row["LAZ"] = outputLine.ALZ;
                                row["LFS"] = outputLine.FlState;
                                row["LFX"] = outputLine.FLX;
                                row["LFY"] = outputLine.FLY;
                                row["LFZ"] = outputLine.FLZ;
                                row["RHS"] = outputLine.HrState;
                                row["RHX"] = outputLine.HRX;
                                row["RHY"] = outputLine.HRY;
                                row["RHZ"] = outputLine.HRZ;
                                row["RKS"] = outputLine.KrState;
                                row["RKX"] = outputLine.KRX;
                                row["RKY"] = outputLine.KRY;
                                row["RKZ"] = outputLine.KRZ;
                                row["RAS"] = outputLine.ArState;
                                row["RAX"] = outputLine.ARX;
                                row["RAY"] = outputLine.ARY;
                                row["RAZ"] = outputLine.ARZ;
                                row["RFS"] = outputLine.FrState;
                                row["RFX"] = outputLine.FRX;
                                row["RFY"] = outputLine.FRY;
                                row["RFZ"] = outputLine.FRZ;
                                row["LKFX"] = outputLine.LKFX;
                                row["LKVG"] = outputLine.LKVG;
                                row["RKFX"] = outputLine.RKFX;
                                row["RKVG"] = outputLine.RKVG;

                                table.Rows.Add(row);
                            }

                            string sql = string.Format("SELECT * FROM {0}", tableName);

                            using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, con))
                            {
                                using (new SQLiteCommandBuilder(da))
                                {
                                    da.Update(table);
                                }
                            }
                            tr2.Commit();

                        }
                        con.Close();
                    }
                }
                #endregion
            }
        }
    }
}
