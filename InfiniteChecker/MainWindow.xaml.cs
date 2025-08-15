using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using InfiniteChecker.WindowEvents;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reactive.Linq;
using Rectangle = System.Drawing.Rectangle;
using Color = System.Drawing.Color;
using Firebase.Database;
using Google.Apis.Auth.OAuth2;
using System.Reflection;
using Firebase.Database.Query;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfiniteChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <param name="handle">ウィンドウハンドル。</param>
        public MainWindow(IntPtr handle)
        {
            this.Handle = handle;
        }

        PixelColor[,] mapPixelArray { get; set; }
        public IntPtr Handle { get; set; }
        public FirebaseClient firebaseClient { get; set; }
        DateTime lastChatFetchDate = DateTime.Now;
        public Int64 serverResetTime = 0;
        public const int
        SRCPAINT = 0x00EE0086;
        private WindowEvent WindowEvent = new WindowEvent();
        BitmapSource mapSource { get; set; }
        private Keys[] keyNums = new Keys[] { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 };
        private Keys[] tenkeyNums = new Keys[] { Keys.A, Keys.S, Keys.D, Keys.F, Keys.G, Keys.C };
        private Keys[] otherKeys = new Keys[] { Keys.Delete, Keys.Left, Keys.Right };
        private long lastSCTime = DateTimeOffset.Now.ToUnixTimeSeconds();
        string lastPushedKey = "";
        async void HotKeyManager_HotKeyPressed(object sender, HotKeyEventArgs e)
        {
            long current = DateTimeOffset.Now.ToUnixTimeSeconds();
            bool timeEnough = (lastSCTime + 0.5) < current;
            bool differentKey = lastPushedKey != e.Key.ToString() && (lastSCTime + 0.2) < current;
            if (logined && (timeEnough || differentKey))
            {
                lastSCTime = current;
                try
                {
                    lastPushedKey = e.Key.ToString();
                    outputMessage(e.Key.ToString());
                    switch (e.Key.ToString())
                    {
                        case "Delete":
                            sendReset();
                            break;
                        case "Left":
                            string floor = await firebaseClient
                                .Child(refference + "/floor")
                                .OnceSingleAsync<string>();
                            if (1 < Int64.Parse(floor))
                            {
                                Int64 prev = Int64.Parse(floor) - 1;
                                await firebaseClient.Child(refference).PatchAsync("{ \"/floor\":" + prev.ToString() + "}");
                            }
                            break;
                        case "Right":
                            string floorl = await firebaseClient
                                .Child(refference + "/floor")
                                .OnceSingleAsync<string>();
                            if (Int64.Parse(floorl) < 50)
                            {
                                Int64 next = Int64.Parse(floorl) + 1;
                                await firebaseClient.Child(refference).PatchAsync("{ \"/floor\":" + next.ToString() + "}");
                            }
                            break;
                        case "D1":
                            await firebaseClient.Child(refference + "/commandTrigger").PatchAsync(new CommandTrriger
                            {
                                id = 0
                            });
                            break;
                        case "D2":
                            await firebaseClient.Child(refference + "/commandTrigger").PatchAsync(new CommandTrriger
                            {
                                id = 1
                            });
                            break;
                        case "D3":
                            await firebaseClient.Child(refference + "/commandTrigger").PatchAsync(new CommandTrriger
                            {
                                id = 2
                            });
                            break;
                        case "D4":
                            await firebaseClient.Child(refference + "/commandTrigger").PatchAsync(new CommandTrriger
                            {
                                id = 3
                            });
                            break;
                        case "D5":
                            await firebaseClient.Child(refference + "/commandTrigger").PatchAsync(new CommandTrriger
                            {
                                id = 4
                            });
                            break;
                        case "D6":
                            await firebaseClient.Child(refference + "/commandTrigger").PatchAsync(new CommandTrriger
                            {
                                id = 5
                            });
                            break;
                        case "D7":
                            await firebaseClient.Child(refference + "/commandTrigger").PatchAsync(new CommandTrriger
                            {
                                id = 6
                            });
                            break;
                        case "D8":
                            await firebaseClient.Child(refference + "/commandTrigger").PatchAsync(new CommandTrriger
                            {
                                id = 7
                            });
                            break;
                        case "D9":
                            await firebaseClient.Child(refference + "/commandTrigger").PatchAsync(new CommandTrriger
                            {
                                id = 8
                            });
                            break;
                        case "D0":
                            await firebaseClient.Child(refference + "/commandTrigger").PatchAsync(new CommandTrriger
                            {
                                id = 9
                            });
                            break;
                        case "A":
                            await firebaseClient.Child(refference).PatchAsync("{ \"/map\": \"111111111101000101101111101101000101101111101101000101101111101101000101101111101100010001101111101101000101101111101101000101101111101101000101101111101101000101111111111\"}");
                            break;
                        case "S":
                            await firebaseClient.Child(refference).PatchAsync("{ \"/map\": \"111111111101010101101010101101010101101010101101010101101010101101010101101010101101010101101111101101010101101010101101010101101010101101010101101010101101010101111111111\"}");
                            break;
                        case "D":
                            await firebaseClient.Child(refference).PatchAsync("{ \"/map\": \"101010101101010101101010101101010101101010101101010101111111111101010101101010101101010101101010101101010101111111111101010101101010101101010101101010101101010101101010101\"}");
                            break;
                        case "F":
                            await firebaseClient.Child(refference).PatchAsync("{ \"/map\": \"101010101101010101101010101101010101111111111000010000111010111101010101101010101101010101101111101101010101111010111000010000111111111101010101101010101101010101101010101\"}");
                            break;
                        case "G":
                            await firebaseClient.Child(refference).PatchAsync("{ \"/map\": \"111111111100010001101010101101010101111010111101010101101010101101010101101010101101000101101111101101000101111010111101010101101010101101010101101010101100010001111111111\"}");
                            break;
                        case "C":
                            startCooltime();
                            break;
                    }
                }
                catch
                {

                }
            } else
            {
                if (logined)
                {
                    outputMessage("SCのCTは0.2秒、同一操作の場合は0.5秒です");
                } else
                {
                    outputMessage("未ログイン");
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            foreach (Keys key in keyNums)
            {
                HotKeyManager.RegisterHotKey(key);
            }
            foreach (Keys key in tenkeyNums)
            {
                HotKeyManager.RegisterHotKey(key);
            }
            foreach (Keys key in otherKeys)
            {
                HotKeyManager.RegisterHotKey(key);
            }
            HotKeyManager.HotKeyPressed += new EventHandler<HotKeyEventArgs>(HotKeyManager_HotKeyPressed);
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("InfiniteChecker.Resources.mapcolors.bmp"))
            {
                var bmp = new Bitmap(stream);
                mapSource = Convert(bmp);
                mapPixelArray = GetPixels(mapSource);
            }
            chatFolderInfo.Text = Properties.Settings.Default.chatFolder;
            sheetName.Text = Properties.Settings.Default.sheetName;
            userName.Text = Properties.Settings.Default.userName;
            try
            {
                WindowEvent.WindowEventOccurrence += WindowEvent_WindowEventOccurrence;
                WindowEvent.Hook(
                    HookWindowEventType.Create
                    | HookWindowEventType.Destroy
                    | HookWindowEventType.Foreground
                    | HookWindowEventType.Hide
                    | HookWindowEventType.LocationChange
                    //                    | HookWindowEventType.MinimizeEnd
                    //                    | HookWindowEventType.MinimizeStart
                    //                    | HookWindowEventType.MoveSizeEnd
                    //                    | HookWindowEventType.MoveSizeStart
                    //                    | HookWindowEventType.NameChange
                    | HookWindowEventType.Show
                    );

                Closed += MainWindow_Closed;
            }
            catch
            {
            }
            string appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            if (File.Exists(appSettingsPath))
            {
                string jsonContent = File.ReadAllText(appSettingsPath);
                var config = JObject.Parse(jsonContent);
                string databaseUrl = config["Firebase"]["database_url"].ToString();
                firebaseClient = new FirebaseClient(databaseUrl, new FirebaseOptions { AuthTokenAsyncFactory = () => GetAccessToken(), AsAccessToken = true });
            }
            else
            {
                System.Windows.MessageBox.Show("appsettings.json file not found. Please copy appsettings.example.json to appsettings.json and configure it.", "Configuration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Windows.Application.Current.Shutdown();
                return;
            }
            InitDB();
            _heartbeat = new DispatcherTimer();
            _heartbeat.Interval = new TimeSpan(0, 0, 10);
            _heartbeat.Tick += new EventHandler(sendHeartbeat);
            _heartbeat.Start();
            GetRagnarok();
        }
        bool usefull = false;
        bool logined = false;
        private async void InitDB()
        {
            var minv = await firebaseClient
              .Child("Infinity/minclientversion")
              .OnceSingleAsync<ClientOption>();
            if (minv.version > 1)
            {
                usefull = false;
                outputMessage("新バージョンが出ています");
            }
            else
            {
                usefull = true;
            }
            this.Dispatcher.Invoke(() =>
            {
                startButton.IsEnabled = usefull && logined;
                resetButton.IsEnabled = usefull && logined;
                invoiceButton.IsEnabled = usefull && logined;
            });
            if (usefull && logined)
            {
                outputMessage("ログインしました");
            }
        }
        private void outputMessage(string message)
        {
            Dispatcher.Invoke(() => {
                debugOutText.Text = message;
            });

        }
        private async Task<string> GetAccessToken()
        {
            string appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            if (File.Exists(appSettingsPath))
            {
                string jsonContent = File.ReadAllText(appSettingsPath);
                return await GoogleCredential.FromJson(jsonContent).CreateScoped(new string[] {
                    "https://www.googleapis.com/auth/firebase.database",
                    "https://www.googleapis.com/auth/userinfo.email",
                }).UnderlyingCredential.GetAccessTokenForRequestAsync().ConfigureAwait(false);
            }
            else
            {
                var assembly = Assembly.GetExecutingAssembly();
                return await GoogleCredential.FromStream(assembly.GetManifestResourceStream("InfiniteChecker.Resources.firebase.json")).CreateScoped(new string[] {
                    "https://www.googleapis.com/auth/firebase.database",
                    "https://www.googleapis.com/auth/userinfo.email",
                }).UnderlyingCredential.GetAccessTokenForRequestAsync().ConfigureAwait(false);
            }
        }
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closed(
            object sender,
            System.EventArgs e
            )
        {
            try
            {
                if (WindowEvent != null)
                {
                    WindowEvent.Unhook();
                }
            }
            catch
            {
            }
        }
        bool ragnarok = false;
        /// <summary>
        /// 「WindowEventOccurrence」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowEvent_WindowEventOccurrence(
            object sender,
            WindowEventArgs e
            )
        {
            try
            {
                this.Handle = e.Hwnd;
                switch (e.WindowEventType)
                {
                    case WindowEventType.Foreground:
                        ragnarok = true;
                        break;
                    case WindowEventType.MoveSizeStart:
                        ragnarok = true;
                        break;
                    case WindowEventType.MoveSizeEnd:
                        ragnarok = true;
                        break;
                    case WindowEventType.MinimizeStart:
                        ragnarok = false;
                        break;
                    case WindowEventType.MinimizeEnd:
                        ragnarok = true;
                        break;
                    case WindowEventType.Create:
                        ragnarok = true;
                        break;
                    case WindowEventType.Destroy:
                        ragnarok = false;
                        break;
                    case WindowEventType.Show:
                        ragnarok = true;
                        break;
                    case WindowEventType.Hide:
                        ragnarok = false;
                        break;
                    case WindowEventType.LocationChange:
                        ragnarok = true;
                        break;
                    case WindowEventType.NameChange:
                        ragnarok = true;
                        break;
                }
            }
            catch
            {
            }
        }
        public BitmapSource GetClientBitmap()
        {
            if (!NativeMethods.GetClientRect(this.Handle, out var rect)) return null;
            var width = rect.right - rect.left;
            var height = rect.bottom - rect.top;
            if (width <= 200 || height <= 200) return null;
            var pt = new POINT { x = rect.left, y = rect.top };
            return CaptureWin32(pt, width, height);
        }
        private BitmapSource CaptureWin32(POINT pt, int width, int height)
        {
            IntPtr screenDC;
            IntPtr compatibleDC;
            IntPtr bmp;
            IntPtr oldBmp;

            var hdc = this.Handle;
            screenDC = NativeMethods.GetDC(hdc);
            try
            {
                compatibleDC = NativeMethods.CreateCompatibleDC(screenDC);
                try
                {
                    bmp = NativeMethods.CreateCompatibleBitmap(screenDC, 128, 138);
                    try
                    {
                        oldBmp = NativeMethods.SelectObject(compatibleDC, bmp);
                        NativeMethods.BitBlt(compatibleDC, 0, 0, 128, 138, screenDC, width - 144, 17, SRCPAINT);
                        NativeMethods.SelectObject(compatibleDC, oldBmp);

                        var image = Imaging.CreateBitmapSourceFromHBitmap(bmp, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                        image.Freeze();
                        return image;
                    }
                    finally
                    {
                        NativeMethods.DeleteObject(bmp);
                    }
                }
                finally
                {
                    NativeMethods.DeleteDC(compatibleDC);
                }
            }
            finally
            {
                NativeMethods.ReleaseDC(hdc, screenDC);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var cofd = new CommonOpenFileDialog()
            {
                Title = "チャットフォルダを選択してください",
                InitialDirectory = @"C:\",
                RestoreDirectory = true,
                IsFolderPicker = true,
            })
            {
                if (cofd.ShowDialog() != CommonFileDialogResult.Ok)
                {
                    return;
                }
                this.chatFolderInfo.Text = cofd.FileName;
                Properties.Settings.Default.chatFolder = cofd.FileName;
                Properties.Settings.Default.Save();
            };
        }

        private bool operating = false;
        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.operating)
            {
                StopTimerHand();
                outputMessage("Stop");
                startButton.Content = "動作開始";
            }
            else
            {
                outputMessage("Start");
                reset();
                questing = 0;
                SetupTimer();
                startButton.Content = "動作停止";
            }
            operating = !operating;
        }
        private DispatcherTimer _timer;
        private DispatcherTimer _heartbeat;
        private void SetupTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 150);
            _timer.Tick += new EventHandler(scanChat);
            _timer.Tick += new EventHandler(scanRagnarok);
            _timer.Start();
            this.Closing += new CancelEventHandler(StopTimer);
        }

        private void StopTimer(object sender, CancelEventArgs e)
        {
            _heartbeat.Stop();
            _timer.Stop();
        }
        private void StopTimerHand()
        {
            _timer.Stop();
        }
        string one = "0A3278";
        string two = "007878";
        string three = "000000";
        string four = "780000";
        string five = "007800";
        string six = "787800";
        string seven = "780078";
        string eight = "783200";
        string nine = "787878";
        string ten = "000078";
        string eleven = "003232";
        string twelve = "320000";
        string mine = "D60000";
        string guild = "FFA6D2";
        int[] memberInMap = new int[14];
        bool[] mapChecks = new bool[50];
        int questing = 0;
        int scanCount = 0;
        int scanned = 0;
        private void scanRagnarok(object sender, EventArgs e)
        {
            scanCount += 1;
            scan();
            if ((scanCount % 16) == 0)
            {
                checkReset();
                scanCount = 0;
                busy = false; // fallback
            }

        }
        List<int> checkedMaps = new List<int>();
        bool busy = false;
        private void scan()
        {
            //outputMessage(deb;
            if (!busy && ragnarok && logined)
            {
                busy = true;
                BitmapSource source = GetClientBitmap();
                if (source != null)
                {
                    PixelColor[,] pixelArray = GetPixels(source);
                    bool notLoad = (pixelArray[52, 129].rgb() == "FFFFFF" || pixelArray[58, 129].rgb() == "FFFFFF") && pixelArray[22, 0].rgb() != "000000" && pixelArray[48, 0].rgb() != "000000";
                    if (!notLoad)
                    {
                        return;
                    }
                    bool exam = true;
                    for (int y = 129; y < 138; y++)
                    {
                        for (int x = 24; x < 60; x++)
                        {
                            PixelColor dot = pixelArray[x, y];
                            PixelColor mapDot = mapPixelArray[x, y];
                            if (exam && mapDot.Blue > 100)
                            {
                                if (dot.Red == 255 && dot.Green == 255 && dot.Blue == 255)
                                {
                                    exam = false;
                                    x = 60;
                                    y = 138;
                                }
                            }
                            else if (exam)
                            {
                                if (dot.Red == 255 && dot.Green == 255 && dot.Blue == 255)
                                {
                                }
                                else
                                {
                                    exam = false;
                                    x = 60;
                                    y = 138;
                                }
                            }
                            if (exam && x == 59 && y == 137)
                            {
                                questing += 1;
                                if (questing == 2)
                                {
                                    reset();
                                    if (autoResetEnabled)
                                    {
                                        outputMessage("Auto reset");
                                        firebaseClient.Child(refference).PatchAsync(new ResetRooms());
                                    }
                                }
                            }
                        }
                    }
                    List<string> members = new List<string> { one, two, three, four, five, six, seven, eight, nine, ten, eleven, twelve, mine, guild };
                    bool emptyMember = memberInMap.All(ma => ma == 0);
                    for (int y = 1; y < 126; y++)
                    {
                        for (int x = 1; x < 128; x++)
                        {
                            PixelColor dot = pixelArray[x, y];
                            PixelColor mapDot = mapPixelArray[x, y];
                            if (mapDot.Red > 0 && mapDot.Green == 0 && mapDot.Blue == 0)
                            {
                                if (questing > 0 && mapDot.Red > 5)
                                {
                                    questing = 0;
                                }
                                else
                                {
                                    int map = mapDot.Red / 5;
                                    if (!mapChecks[map - 1] && !(map == 1 && emptyMember))
                                    {

                                        bool mineObject = false;
                                        bool isMemberDot = members.Contains(dot.rgb()) && pixelArray[x + 1, y + 1].rgb() == dot.rgb();
                                        if (!isMemberDot)
                                        {
                                            mineObject = members.Contains(mine) && dot.reddish() && pixelArray[x - 1, y].reddish() && pixelArray[x + 1, y].reddish() && pixelArray[x, y - 1].reddish() && pixelArray[x, y + 1].reddish() && dot.Green < (pixelArray[x, y + 1].Green - 50);
                                        }
                                        if (isMemberDot || mineObject)
                                        {

                                            int memberIndex = 99;
                                            if (mineObject)
                                            {
                                                memberIndex = 12;
                                                members.Remove(mine);
                                            }
                                            else
                                            {
                                                switch (dot.rgb())
                                                {
                                                    case "0A3278":
                                                        memberIndex = 0;
                                                        members.Remove(one);
                                                        break;
                                                    case "007878":
                                                        memberIndex = 1;
                                                        members.Remove(two);
                                                        break;
                                                    case "000000":
                                                        if (pixelArray[x - 1, y - 1].rgb() == "000000")
                                                        {
                                                            memberIndex = 2;
                                                            members.Remove(three);
                                                        }
                                                        break;
                                                    case "780000":
                                                        memberIndex = 3;
                                                        members.Remove(four);
                                                        break;
                                                    case "007800":
                                                        memberIndex = 4;
                                                        members.Remove(five);
                                                        break;
                                                    case "787800":
                                                        memberIndex = 5;
                                                        members.Remove(six);
                                                        break;
                                                    case "780078":
                                                        memberIndex = 6;
                                                        members.Remove(seven);
                                                        break;
                                                    case "783200":
                                                        memberIndex = 7;
                                                        members.Remove(eight);
                                                        break;
                                                    case "787878":
                                                        if (pixelArray[x + 1, y].rgb() == "787878" && pixelArray[x, y + 1].rgb() == "787878")
                                                        {
                                                            memberIndex = 8;
                                                            members.Remove(nine);
                                                        }
                                                        break;
                                                    case "000078":
                                                        memberIndex = 9;
                                                        members.Remove(ten);
                                                        break;
                                                    case "003232":
                                                        memberIndex = 10;
                                                        members.Remove(eleven);
                                                        break;
                                                    case "320000":
                                                        memberIndex = 11;
                                                        members.Remove(twelve);
                                                        break;
                                                    case "D60000":
                                                        memberIndex = 12;
                                                        members.Remove(mine);
                                                        break;
                                                    case "FFA6D2":
                                                        if (2 < y && dot.rgb() == pixelArray[x, y - 2].rgb() && dot.rgb() == pixelArray[x, y + 2].rgb())
                                                        {
                                                            memberIndex = 13;
                                                            members.Remove(guild);
                                                        }
                                                        break;
                                                }
                                            }
                                            if (memberIndex < 15)
                                            {
                                                if (memberInMap[memberIndex] != map)
                                                {
                                                    if (!checkedMaps.Contains(map - 1))
                                                    {
                                                        checkedMaps.Add(map - 1);
                                                    }
                                                    mapChecks.SetValue(true, map - 1);
                                                    memberInMap.SetValue(map, memberIndex);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                busy = false;
                scanned += 1;
                //outputMessage(scanned.ToString();
            }
        }
        EncodingProvider provider = CodePagesEncodingProvider.Instance;
        Regex itemInfoRegex = new Regex(@"[^\n]+[0-9]+ 個獲得");
        Regex nameRegex = new Regex(@"([^\n]+) [0-9]+ 個獲得");
        Regex amountRegex = new Regex(@"([0-9]+) 個獲得");
        private async void scanItem(object sender, RoutedEventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(this.chatFolderInfo.Text);
            List<FileSystemInfo> fsi = di.GetFileSystemInfos().OrderByDescending(x => x.CreationTime).ToList();
            var encoding = provider.GetEncoding("shift-jis");
            List<string> newFileList = new List<string>();
            var latest = fsi.First().CreationTime;
            if (fsi.Count > 0)
            {
                List<ItemAmount> items = new List<ItemAmount>();
                foreach (FileSystemInfo info in fsi)
                { 
                    if ((latest - info.CreationTime).TotalSeconds < 2)
                    {
                        newFileList.Add(info.FullName);
                    }
                }
                foreach (string filePath in newFileList)
                {
                    if (items.Count == 0)
                    {
                        StreamReader reader = new StreamReader(filePath, encoding);
                        string text = await reader.ReadToEndAsync();
                        foreach (Match match in itemInfoRegex.Matches(text))
                        {
                            string item = match.Value;
                            string name = nameRegex.Match(item).Groups[1].Value;
                            string amount = amountRegex.Match(item).Groups[1].Value;
                            if (int.TryParse(amount, out int intAmount))
                            {
                                int index = items.FindIndex(i => i.name == name);
                                if (index != -1)
                                {
                                    items[index].amount = intAmount + items[index].amount;
                                }
                                else
                                {
                                    items.Add(new ItemAmount() { name = name, amount = intAmount });
                                }
                            }
                        }
                    }
                }
                Dictionary<int, ItemAmount> dict = items.Select((i, ind) => new { i, ind }).ToDictionary(i => i.ind, i => i.i);
                try
                {
                    await firebaseClient.Child(refference).Child("invoice").Child("items").DeleteAsync();
                    await firebaseClient.Child(refference).Child("invoice").Child("items").PatchAsync(dict);
                    await firebaseClient.Child(refference).Child("invoice").PatchAsync(new AuctionPrice());
                    await firebaseClient.Child(refference).Child("invoice").PatchAsync(new DateTimeStamp());
                    await firebaseClient.Child(refference).Child("invoice").PatchAsync(new InvoiceNotice());
                    await firebaseClient.Child(refference).Child("invoice").PatchAsync(new MemberCount());
                    outputMessage("Invoice issued");
                }
                catch
                {
                    outputMessage("Invoice update failed");
                }
            }
        }
        Regex rgx = new Regex(@"無謀な探検家 : [1０１２３４５６７８９　](0?[◇―｜■]{9})");
        string mapString = "";
        private async void scanChat(object sender, EventArgs e)
        {
            if ((scanCount % 3) == 0)
            {
                DirectoryInfo di = new DirectoryInfo(this.chatFolderInfo.Text);
                FileSystemInfo[] fsi = di.GetFileSystemInfos();
                var encoding = provider.GetEncoding("shift-jis");
                var updated = false;
                List<string> newFileList = new List<string>();
                foreach (FileSystemInfo info in fsi)
                    if (this.lastChatFetchDate < info.CreationTime)
                    {
                        updated = true;
                        newFileList.Add(info.FullName);
                    }
                if (updated)
                {
                    this.lastChatFetchDate = DateTime.Now;
                    foreach (string filePath in newFileList)
                    {
                        StreamReader reader = new StreamReader(filePath, encoding);
                        string text = await reader.ReadToEndAsync();
                        foreach (Match match in rgx.Matches(text))
                        {
                            string map = match.Groups[1].Value;
                            foreach (char ch in map)
                            {
                                string str = ch.ToString();
                                if (str == "0") // 新しいマップ
                                {
                                    mapString = "";
                                }
                                else if (str == "◇") // 部屋
                                {
                                    mapString += "1";
                                }
                                else if (str == "―" || str == "｜") // 通路
                                {
                                    mapString += "1";

                                }
                                else if (str == "■") // 壁
                                    mapString += "0";
                            }
                        }
                    }
                    if (logined && mapString.ToString().Length == 171)
                    {
                        try
                        {
                            await firebaseClient.Child(refference).PatchAsync<Map>(new Map() { map = mapString });
                            outputMessage("Updated the map");
                        }
                        catch
                        {
                            outputMessage("Map update failed.");
                        }
                    }
                }
            }
        }

        public struct PixelColor
        {
            public byte Blue;
            public byte Green;
            public byte Red;
            public byte Alpha;


            public PixelColor(byte r, byte g, byte b, byte a)
            {
                Red = r;
                Green = g;
                Blue = b;
                Alpha = a;
            }

            public Color ToColor()
            {
                return Color.FromArgb(Alpha, Red, Green, Blue);
            }

            public override string ToString()
            {
                return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", Red, Green, Blue, Alpha);
            }
            public string rgb()
            {
                return string.Format("{0:X2}{1:X2}{2:X2}", Red, Green, Blue);
            }
            public bool reddish()
            {
                return Green == Blue && Green < (Red - 25) && 100 < Red;
            }
        }
        public PixelColor[,] GetPixels(BitmapSource source)
        {
            if (source.Format != PixelFormats.Bgra32)
                source = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0);

            int width = source.PixelWidth;
            int height = source.PixelHeight;
            PixelColor[,] result = new PixelColor[width, height];

            CopyPixels(source, result, width * 4, 0);

            return result;
        }

        private void CopyPixels(BitmapSource source, PixelColor[,] pixels, int stride, int offset)
        {
            var height = source.PixelHeight;
            var width = source.PixelWidth;

            var pixelBytes = new byte[height * width * 4];
            source.CopyPixels(pixelBytes, stride, 0);

            int y0 = offset / width;
            int x0 = offset - width * y0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var b = pixelBytes[(y * width + x) * 4 + 0];
                    var g = pixelBytes[(y * width + x) * 4 + 1];
                    var r = pixelBytes[(y * width + x) * 4 + 2];
                    var a = pixelBytes[(y * width + x) * 4 + 3];

                    pixels[x + x0, y + y0] = new PixelColor(r, g, b, a);
                }
            }
        }
        public static BitmapSource Convert(Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                bitmap.HorizontalResolution, bitmap.VerticalResolution,
                PixelFormats.Bgr24, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);

            return bitmapSource;
        }

        string refference = "";
        string inputedUserName = "";
        int lastResetTime = 0;
        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                loginButton.IsEnabled = false;
                startButton.IsEnabled = false;
                resetButton.IsEnabled = false;
                invoiceButton.IsEnabled = false;
            });
            logined = false;
            outputMessage("ログイン処理中…");
            refference = sheetName.Text;
            refference += "/";
            refference += password.Text;
            inputedUserName = userName.Text;
            var mapString = await firebaseClient
                .Child(refference + "/map")
                .OnceSingleAsync<string>();
            var sheetVersion = await firebaseClient
                .Child(refference + "/version")
                .OnceSingleAsync<string>();
            if (mapString != null && mapString.Length > 100)
            {
                if (sheetVersion == '2'.ToString())
                {
                    logined = true;
                    Properties.Settings.Default.sheetName = sheetName.Text;
                    Properties.Settings.Default.userName = userName.Text;
                    Properties.Settings.Default.Save();
                } else
                {
                    outputMessage("先にブラウザからシートを開いて下さい");
                    logined = false;
                    this.Dispatcher.Invoke(() =>
                    {
                        loginButton.IsEnabled = true;
                    });
                }
            }
            else
            {
                logined = false;
                this.Dispatcher.Invoke(() =>
                {
                    loginButton.IsEnabled = true;
                });
                outputMessage("シートが見付かりません");
            }
            if (usefull && logined)
            {
                outputMessage("ログインしました");
                string resetTime = await firebaseClient
                    .Child(refference + "/resetTime")
                    .OnceSingleAsync<string>();
                serverResetTime = Int64.Parse(resetTime);
            }
            this.Dispatcher.Invoke(() =>
            {
                startButton.IsEnabled = usefull && logined;
                resetButton.IsEnabled = usefull && logined;
                invoiceButton.IsEnabled = usefull && logined;
            });
        }
        int heartbeatCount = 0;
        private async void sendHeartbeat(object sender, EventArgs e)
        {
            if (logined)
            {
                try
                {
                    await firebaseClient.Child(refference + "/users/" + inputedUserName + "-Win").PatchAsync(new ServerTimeStamp());
                }
                catch
                {

                }
            }
        }
        int machineSeed = new Random().Next();
        private void sheetName_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                loginButton.IsEnabled = sheetName.Text.Length > 0 && password.Text.Length > 0 && userName.Text.Length > 0;
            });
        }

        private void password_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                loginButton.IsEnabled = sheetName.Text.Length > 0 && password.Text.Length > 0 && userName.Text.Length > 0;
            });
        }

        private void userName_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                loginButton.IsEnabled = sheetName.Text.Length > 0 && password.Text.Length > 0 && userName.Text.Length > 0;
            });
        }

        bool autoResetEnabled = false;
        private void GetRagnarok()
        {
            NativeMethods.EnumWindows(delegate (IntPtr hwnd, IntPtr param)
            {
                StringBuilder sb = new StringBuilder();
                NativeMethods.GetWindowText(hwnd, sb, 9);
                if (sb.ToString().Contains("Ragnarok"))
                {
                    this.Handle = hwnd;
                    ragnarok = true;
                    return false;
                }
                else
                {
                    return true;
                }
            }, IntPtr.Zero);
        }
        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            sendReset();
        }
        private async void sendReset()
        {
            if (usefull && logined)
            {
                reset();
                var resetRoom = new ResetRooms();
                try
                {
                    await firebaseClient.Child(refference).PatchAsync(resetRoom);
                    serverResetTime = resetRoom.resetTime;
                    outputMessage("Reset all checks");
                }
                catch
                {
                    outputMessage("Reset failed");
                }
            }
        }
        private async void startCooltime()
        {
            if (usefull && logined)
            {
                try
                {
                    var entry = await firebaseClient
                        .Child(refference + "/entries/" + inputedUserName)
                        .OnceSingleAsync<EntryUser>();
                    if (entry != null)
                    {
                        outputMessage("Patch cooltime");
                        await firebaseClient.Child(refference + "/entries/" + inputedUserName).PatchAsync(new CoolTimeStamp());
                    }
                }
                catch
                {
                    outputMessage("Patch failed - cooltime");
                }
            }
        }
        private async void checkReset()
        {
            try
            {
                string resetTime = await firebaseClient
                    .Child(refference + "/resetTime")
                    .OnceSingleAsync<string>();
                if (serverResetTime != Int64.Parse(resetTime))
                {
                    outputMessage("Reset received: Seed - " + resetTime);
                    serverResetTime = Int64.Parse(resetTime);
                    lastResetTime = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    reset();
                    questing = 10;
                }
                else
                {
                    writeRooms();
                }
            }
            catch
            {
                outputMessage("Database connection failed");
            }
        }
        private void reset()
        {
            for (int i = 0; i < 14; i++)
            {
                memberInMap[i] = 0;
            }
            for (int i = 0; i < 50; i++)
            {
                mapChecks[i] = false;
            }
            checkedMaps = new List<int>();
        }
        private void writeRooms()
        {
            try
            {
                foreach (int newChecked in checkedMaps)
                {
                    switch (newChecked)
                    {
                        case int n when 0 <= n && n < 10:
                            firebaseClient.Child(refference + "/rooms1").PatchAsync("{ \"" + (n % 10).ToString() + "\": true }");
                            break;
                        case int n when 10 <= n && n < 20:
                            firebaseClient.Child(refference + "/rooms2").PatchAsync("{ \"" + (n % 10).ToString() + "\": true }");
                            break;
                        case int n when 20 <= n && n < 30:
                            firebaseClient.Child(refference + "/rooms3").PatchAsync("{ \"" + (n % 10).ToString() + "\": true }");
                            break;
                        case int n when 30 <= n && n < 40:
                            firebaseClient.Child(refference + "/rooms4").PatchAsync("{ \"" + (n % 10).ToString() + "\": true }");
                            break;
                        case int n when 40 <= n && n < 50:
                            firebaseClient.Child(refference + "/rooms5").PatchAsync("{ \"" + (n % 10).ToString() + "\": true }");
                            break;
                    }
                }
            }
            catch
            {
                outputMessage("Database connection failed");
            }
            checkedMaps = new List<int>();
        }
    }
    public class ClientOption
    {
        public int version { get; set; }
    }
    public class EntryUser
    {
        public int root { get; set; }
    }
    public class UserHeartbeat
    {
        public long joined { get; set; }
    }
    public class Map
    {
        public string map { get; set; }
    }

    public class AllRooms
    {
        [JsonProperty("rooms1/0")]
        public bool? room1 { get; set; }
        [JsonProperty("rooms1/1")]
        public bool? room2 { get; set; }
        [JsonProperty("rooms1/2")]
        public bool? room3 { get; set; }
        [JsonProperty("rooms1/3")]
        public bool? room4 { get; set; }
        [JsonProperty("rooms1/4")]
        public bool? room5 { get; set; }
        [JsonProperty("rooms1/5")]
        public bool? room6 { get; set; }
        [JsonProperty("rooms1/6")]
        public bool? room7 { get; set; }
        [JsonProperty("rooms1/7")]
        public bool? room8 { get; set; }
        [JsonProperty("rooms1/8")]
        public bool? room9 { get; set; }
        [JsonProperty("rooms1/9")]
        public bool? room10 { get; set; }
        [JsonProperty("rooms2/0")]
        public bool? room11 { get; set; }
        [JsonProperty("rooms2/1")]
        public bool? room12 { get; set; }
        [JsonProperty("rooms2/2")]
        public bool? room13 { get; set; }
        [JsonProperty("rooms2/3")]
        public bool? room14 { get; set; }
        [JsonProperty("rooms2/4")]
        public bool? room15 { get; set; }
        [JsonProperty("rooms2/5")]
        public bool? room16 { get; set; }
        [JsonProperty("rooms2/6")]
        public bool? room17 { get; set; }
        [JsonProperty("rooms2/7")]
        public bool? room18 { get; set; }
        [JsonProperty("rooms2/8")]
        public bool? room19 { get; set; }
        [JsonProperty("rooms2/9")]
        public bool? room20 { get; set; }
        [JsonProperty("rooms3/0")]
        public bool? room21 { get; set; }
        [JsonProperty("rooms3/1")]
        public bool? room22 { get; set; }
        [JsonProperty("rooms3/2")]
        public bool? room23 { get; set; }
        [JsonProperty("rooms3/3")]
        public bool? room24 { get; set; }
        [JsonProperty("rooms3/4")]
        public bool? room25 { get; set; }
        [JsonProperty("rooms3/5")]
        public bool? room26 { get; set; }
        [JsonProperty("rooms3/6")]
        public bool? room27 { get; set; }
        [JsonProperty("rooms3/7")]
        public bool? room28 { get; set; }
        [JsonProperty("rooms3/8")]
        public bool? room29 { get; set; }
        [JsonProperty("rooms3/9")]
        public bool? room30 { get; set; }
        [JsonProperty("rooms4/0")]
        public bool? room31 { get; set; }
        [JsonProperty("rooms4/1")]
        public bool? room32 { get; set; }
        [JsonProperty("rooms4/2")]
        public bool? room33 { get; set; }
        [JsonProperty("rooms4/3")]
        public bool? room34 { get; set; }
        [JsonProperty("rooms4/4")]
        public bool? room35 { get; set; }
        [JsonProperty("rooms4/5")]
        public bool? room36 { get; set; }
        [JsonProperty("rooms4/6")]
        public bool? room37 { get; set; }
        [JsonProperty("rooms4/7")]
        public bool? room38 { get; set; }
        [JsonProperty("rooms4/8")]
        public bool? room39 { get; set; }
        [JsonProperty("rooms4/9")]
        public bool? room40 { get; set; }
        [JsonProperty("rooms5/0")]
        public bool? room41 { get; set; }
        [JsonProperty("rooms5/1")]
        public bool? room42 { get; set; }
        [JsonProperty("rooms5/2")]
        public bool? room43 { get; set; }
        [JsonProperty("rooms5/3")]
        public bool? room44 { get; set; }
        [JsonProperty("rooms5/4")]
        public bool? room45 { get; set; }
        [JsonProperty("rooms5/5")]
        public bool? room46 { get; set; }
        [JsonProperty("rooms5/6")]
        public bool? room47 { get; set; }
        [JsonProperty("rooms5/7")]
        public bool? room48 { get; set; }
        [JsonProperty("rooms5/8")]
        public bool? room49 { get; set; }
        [JsonProperty("rooms5/9")]
        public bool? room50 { get; set; }
    }
    public class ResetRooms
    {
        public bool[] rooms1 { get; set; } = new bool[10];
        public bool[] rooms2 { get; set; } = new bool[10];
        public bool[] rooms3 { get; set; } = new bool[10];
        public bool[] rooms4 { get; set; } = new bool[10];
        public bool[] rooms5 { get; set; } = new bool[10];
        public int resetTime { get; set; } = new Random().Next();
    }
    public class CommandTrriger
    {
        public int id { get; set; }
        public string seed { get; set; } = new Random().Next().ToString();
    }
    public class Machine
    {
        public int seed { get; set; }
        public long joined { get; set; }
    }
    public class ClientTimeStamp
    {
        public long joined { get; set; }
    }
    public class CoolTimeStamp
    {
        [JsonProperty("cooltime")]
        public TimeStamp cooltime { get; } = new TimeStamp();
    }
    public class ServerTimeStamp
    {
        [JsonProperty("joined")]
        public TimeStamp joined { get; } = new TimeStamp();
    }
    public class TimeStamp
    {
        [JsonProperty(".sv")]
        public string TimestampPlaceholder { get; } = "timestamp";
    }
    public class ItemAmount
    {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("amount")]
        public int amount { get; set; } = 0;
        [JsonProperty("price")]
        public int price { get; set; } = -1;
        [JsonProperty("split")]
        public bool split { get; set; } = true;
    }
    public class AuctionPrice
    {
        [JsonProperty("auction")]
        public int auction { get; set; } = 0;
    }
    public class DateTimeStamp
    {
        [JsonProperty("date")]
        public TimeStamp date { get; } = new TimeStamp();
    }
    public class InvoiceNotice
    {
        [JsonProperty("notice")]
        public bool notice { get; } = true;
    }
    public class MemberCount
    {
        [JsonProperty("memberCount")]
        public int memberCount { get; } = 12;
    }
}
