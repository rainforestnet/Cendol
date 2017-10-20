using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Widget;
using System;
using System.IO;
using System.Timers;

namespace Cendol
{
    [Activity(Label = "Cendol", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        const string TAG = "CendolMain";

        //Controls
        Timer _timer;
        EditText _edtSerial;
        Button _btnSave;
        string _imei;

        // Filesystem related
        string _directory = Path.Combine((string)Android.OS.Environment.ExternalStorageDirectory, "Cendol");
        string _dataFileToday = "";
        const string LASTRECORDFILE = "lastjedi.txt";

        private void SetDataFilename()
        {
            var todayDate = DateTime.Now.ToString("yyyyMMdd");
            var filename = Path.Combine(_directory, $"{todayDate}.txt");
            _dataFileToday = filename;
        }

        private void SaveAction()
        {
            SetDataFilename();
            InputRecord record = ProcessInput();
            var fs = File.Open(_dataFileToday, FileMode.Append, FileAccess.Write, FileShare.Read);

            try
            {
                if (IsInputValid(record))
                {
                    using (var streamWriter = new StreamWriter(fs))
                    {
                        streamWriter.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}", record.DeviceId, DateTime.Now, record.RecordId, record.ItemNumber));
                    }
                }
            }
            finally
            {
                fs.Close();
            }

            record = null;
            _edtSerial.Text = "";
        }

        private bool IsInputValid(InputRecord record)
        {
            if (record.ItemNumber.Trim().Length > 0)
                return true;
            else
                return false;
        }

        private InputRecord ProcessInput()
        {
            var serialNo = "";
            serialNo = _edtSerial.Text.Trim();

            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            var recordId = span.TotalMilliseconds;
            return new InputRecord { RecordId = recordId, DeviceId = _imei, ItemNumber = serialNo };
        }

        private void StartTimer()
        {
            _timer = new Timer(5000);
            _timer.Elapsed += InvokeUploadSrv;
            _timer.Enabled = true;
        }

        private void StopTimer()
        {
            _timer.Enabled = false;
        }

        private void InvokeUploadSrv(object sender, ElapsedEventArgs e)
        {
            Log.Debug(TAG, "Intent service invoked");
            if (_dataFileToday.Length > 0)
            {
                Intent sendDataIntent = new Intent(this, typeof(UpdateServerService));
                sendDataIntent.PutExtra("DataFile", _dataFileToday);
                sendDataIntent.PutExtra("LastRecordFile", Path.Combine(_directory, LASTRECORDFILE));
                StartService(sendDataIntent);
            }
        }

        //Android Activity Lifecycle
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            _edtSerial = FindViewById<EditText>(Resource.Id.edtSerial);
            _btnSave = FindViewById<Button>(Resource.Id.btnSave);
        }

        protected override void OnStart()
        {
            base.OnStart();
            //bind save button
            _btnSave.Click += (sender, e) =>
            {
                SaveAction();
            };

            //Get device imei number
            var telephonyManager = (Android.Telephony.TelephonyManager)GetSystemService(TelephonyService);
            _imei = telephonyManager.Imei;

            //Create Working directory in file system if it doesn't exist
            if (!Directory.Exists(_directory))
                Directory.CreateDirectory(_directory);
        }

        protected override void OnResume()
        {
            base.OnResume();
            SetDataFilename();
            //Start the timer
            StartTimer();
        }

        protected override void OnStop()
        {
            base.OnStop();
            StopTimer();
        }
    }
}

