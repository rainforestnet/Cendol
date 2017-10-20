using Android.App;
using Android.Util;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Cendol
{
    [Service]
    public class UpdateServerService : IntentService
    {
        const string TAG = "UpdateServer";

        public UpdateServerService() : base("UpdateServerService")
        {
        }

        private double CreateItem(double recordid, string itemNumber)
        {
            HttpClient client;
            client = new HttpClient
            {
                MaxResponseContentBufferSize = 256000
            };

            var uri = new Uri("http://cendol.hrforte.com/api/item");
            var item = new { Item = itemNumber, CreatedOn = DateTime.Now, Status = "N" };
            var data = JsonConvert.SerializeObject(item);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = client.PostAsync(uri, content).Result;
            client = null;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return recordid;
            }
            else
                return 0d;
        }

        private double GetLastIdFromDisk(string lastRecordfn)
        {
            double lastRecFromFile = 0d;
            if (File.Exists(lastRecordfn))
            {
                using (StreamReader srlast = new StreamReader(lastRecordfn))
                {
                    string ln = srlast.ReadLine();
                    if (ln != null && ln.Trim().Length > 1)
                    {
                        if (Double.TryParse(ln.Trim(), out lastRecFromFile))
                        {
                            Log.Debug(TAG, $"Last ID from Disk {lastRecFromFile}");
                        }
                    }
                }
            }
            return lastRecFromFile;
        }

        private void PostToServer(string datafn, string lastRecfn,  double lastid)
        {
            var datafs = File.Open(datafn, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (StreamReader sr = new StreamReader(datafs))
            {
                string line;
                double recId = 0d;
                while ((line = sr.ReadLine()) != null)
                {
                    var s = line.Split('\t');
                    if (Double.TryParse(s[2], out recId))
                    {
                        if (recId > lastid)
                        {
                            var newRecId = CreateItem(recId, s[3]);
                            using (StreamWriter sw = new StreamWriter(lastRecfn))
                            {
                                sw.Write(newRecId);
                                Log.Debug(TAG, $"newly inserted record in database {newRecId}   {s[3]}");
                            }

                        }
                    }
                }
            }
        }

        protected override void OnHandleIntent(Android.Content.Intent intent)
        {
            string datafn = intent.GetStringExtra("DataFile");
            string lastRecfn = intent.GetStringExtra("LastRecordFile");
            Log.Debug(TAG, $"DataFile: {datafn}.");

            try
            {
                var lastid = GetLastIdFromDisk(lastRecfn);
                PostToServer(datafn, lastRecfn, lastid);
            }
            catch (Exception ex)
            {
                Log.Debug(TAG, ex.Message);
            }
        }
    }
}