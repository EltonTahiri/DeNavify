using System;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace DeNavify
{
    public partial class SplashScreen : MaterialForm
    {
        // Use fully qualified name for the Timer class
        private System.Windows.Forms.Timer splashTimer;
        private readonly MaterialSkinManager materialSkinManager;

        public SplashScreen()
        {
            InitializeComponent();

            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT; // or DARK
            // Define color scheme
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Blue500, Accent.LightBlue200, TextShade.WHITE);

            // Initialize and configure the splash timer
            splashTimer = new System.Windows.Forms.Timer();
            splashTimer.Interval = 3000; // Set the duration for the splash screen (e.g., 3000 milliseconds for 3 seconds)
            splashTimer.Tick += SplashTimer_Tick;
            splashTimer.Start();
        }

        private void SplashTimer_Tick(object sender, EventArgs e)
        {
            // Stop the timer
            splashTimer.Stop();

            // Close the splash screen
            this.Close();

            // Open the main form (MainForm)
            MainForm mainForm = new MainForm();
            mainForm.Show();
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {

        }
    }
}
