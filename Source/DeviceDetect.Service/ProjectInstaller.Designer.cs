
namespace DeviceDetect.Service
{
	partial class ProjectInstaller
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.DeviceDetectServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
			this.DeviceDetectServiceInstaller = new System.ServiceProcess.ServiceInstaller();
			// 
			// DeviceDetectServiceProcessInstaller
			// 
			this.DeviceDetectServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
			this.DeviceDetectServiceProcessInstaller.Password = null;
			this.DeviceDetectServiceProcessInstaller.Username = null;
			// 
			// DeviceDetectServiceInstaller
			// 
			this.DeviceDetectServiceInstaller.Description = "Detects device events";
			this.DeviceDetectServiceInstaller.DisplayName = "Device Detect Services";
			this.DeviceDetectServiceInstaller.ServiceName = "DeviceDetectService";
			// 
			// ProjectInstaller
			// 
			this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.DeviceDetectServiceProcessInstaller,
            this.DeviceDetectServiceInstaller});

		}

		#endregion

		private System.ServiceProcess.ServiceProcessInstaller DeviceDetectServiceProcessInstaller;
		private System.ServiceProcess.ServiceInstaller DeviceDetectServiceInstaller;
	}
}