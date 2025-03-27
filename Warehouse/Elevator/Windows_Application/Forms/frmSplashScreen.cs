using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using NWGrain;

namespace SplashScreen
{
	// The SplashScreen class definition.  AKO Form
	public partial class SplashScreen : Form
	{
		#region Member Variables
		// Threading
		private static SplashScreen ms_frmSplash = null;
		private static Thread ms_oThread = null;

		// Fade in and out.
		private double m_dblOpacityIncrement = .05;
		private double m_dblOpacityDecrement = .08;
		private const int TIMER_INTERVAL = 50;

		// Status and progress bar
		private string m_sStatus;
		
		public double m_dblCompletionFraction = 100.0;
		

		// Progress smoothing
		private double m_dblLastCompletionFraction = 0.0;
		

		// Self-calibration support
		private int m_iIndex = 1;
		private int m_iActualTicks = 0;
		
		private ArrayList m_alActualTimes = new ArrayList();
		private DateTime m_dtStart;
		
		private bool m_bDTSet = false;

		#endregion Member Variables

		/// <summary>
		/// Constructor
		/// </summary>
		public SplashScreen()
		{
			InitializeComponent();
            this.lblApp.Text = string.Format("ELEVATOR Vers {0}", Application.ProductVersion.ToString());
			this.Opacity = 0.0;
			UpdateTimer.Interval = TIMER_INTERVAL;
			UpdateTimer.Start();
		//	this.ClientSize = this.BackgroundImage.Size;
		}

		#region Public Static Methods
		// A static method to create the thread and 
		// launch the SplashScreen.
		static public void ShowSplashScreen()
		{
			// Make sure it's only launched once.
			if (ms_frmSplash != null)
				return;
			ms_oThread = new Thread(new ThreadStart(SplashScreen.ShowForm));
			ms_oThread.IsBackground = true;
			ms_oThread.SetApartmentState(ApartmentState.STA);
			ms_oThread.Start();
			while (ms_frmSplash == null || ms_frmSplash.IsHandleCreated == false)
			{
				System.Threading.Thread.Sleep(TIMER_INTERVAL);
			}
		}

		// Close the form without setting the parent.
		static public void CloseForm()
		{
			if (ms_frmSplash != null && ms_frmSplash.IsDisposed == false)
			{
				// Make it start going away.
				ms_frmSplash.m_dblOpacityIncrement = -ms_frmSplash.m_dblOpacityDecrement;
			}
			ms_oThread = null;	// we don't need these any more.
			ms_frmSplash = null;
		}

		// A static method to set the status and update the reference.
		static public void SetStatus(string newStatus)
		{
			SetStatus(newStatus, true);
		}

		// A static method to set the status and optionally update the reference.
		// This is useful if you are in a section of code that has a variable
		// set of status string updates.  In that case, don't set the reference.
		static public void SetStatus(string newStatus, bool setReference)
		{
			if (ms_frmSplash == null)
				return;

			ms_frmSplash.m_sStatus = newStatus;

			if (setReference)
				ms_frmSplash.SetReferenceInternal();
		}

		// Static method called from the initializing application to 
		// give the splash screen reference points.  Not needed if
		// you are using a lot of status strings.
		static public void SetReferencePoint()
		{
			if (ms_frmSplash == null)
				return;
			ms_frmSplash.SetReferenceInternal();

		}
		#endregion Public Static Methods

		#region Private Methods

		// A private entry point for the thread.
		static private void ShowForm()
		{
			ms_frmSplash = new SplashScreen();
			Application.Run(ms_frmSplash);
		}

		// Internal method for setting reference points.
		private void SetReferenceInternal()
		{
			if (m_bDTSet == false)
			{
				m_bDTSet = true;
				m_dtStart = DateTime.Now;
                //ReadIncrements();
			}
			double dblMilliseconds = ElapsedMilliSeconds();
			m_alActualTimes.Add(dblMilliseconds);
			m_dblLastCompletionFraction = m_dblCompletionFraction;
			m_dblCompletionFraction = (m_iIndex > 0) ? 1 : 0;
		}

		// Utility function to return elapsed Milliseconds since the 
		// SplashScreen was launched.
		private double ElapsedMilliSeconds()
		{
			TimeSpan ts = DateTime.Now - m_dtStart;
			return ts.TotalMilliseconds;
		}

		

		public static SplashScreen GetSplashScreen()
		{
			return ms_frmSplash;
		}

		#endregion Private Methods

		#region Event Handlers
		// Tick Event handler for the Timer control.  Handle fade in and fade out and paint progress bar. 
		private void UpdateTimer_Tick(object sender, System.EventArgs e)
		{
			//lblStatus.Text = m_sStatus;

			// Calculate opacity
			if (m_dblOpacityIncrement > 0)		// Starting up splash screen
			{
				m_iActualTicks++;
				if (this.Opacity < 1)
					this.Opacity += m_dblOpacityIncrement;
			}
			else // Closing down splash screen
			{
				if (this.Opacity > 0)
					this.Opacity += m_dblOpacityIncrement;
				else
				{
                    //StoreIncrements();
					UpdateTimer.Stop();
					this.Close();
				}
			}

		}


        public void ResetProgressBar(string ProgressItem)
        {
            m_dblLastCompletionFraction = 0;
            lblTimeRemaining.Text = ProgressItem;

        }


		// Close the form if they double click on it.
		private void SplashScreen_DoubleClick(object sender, System.EventArgs e)
		{
			// Use the overload that doesn't set the parent form to this very window.
			//CloseForm();
		}
		#endregion Event Handlers

        private void SplashScreen_Load(object sender, EventArgs e)
        {

        }

        private void SplashScreen_FormClosed(object sender, FormClosedEventArgs e)
        {
			
        }
    }

   
}

